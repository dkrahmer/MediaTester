using MediaTesterLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTesterGui
{
	public partial class Main : Form
	{
		MediaTesterLib.Options _mediaTesterOptions = Options.Deserialize();
		MediaTester _mediaTester;
		private Thread _mediaTesterThread;
		const string PALCEHOLDER_VALUE = "---";
		const string BYTES = " Bytes";
		const string BYTES_PER_SECOND = BYTES + "/sec";

		DateTime? _startDateTime;

		public Main()
		{
			InitializeComponent();
			UpdateUiFromOptions();
			EnableControls();
		}

		private void UpdateUiFromOptions()
		{
			if (_mediaTesterOptions.MaxBytesToTest < 0)
			{
				MaxBytesToTestComboBox.SelectedIndex = 0;
			}
			else
			{
				MaxBytesToTestComboBox.Text = _mediaTesterOptions.MaxBytesToTest.ToString("#,##0");
			}
			TargetTextBox.Text = _mediaTesterOptions.TestDirectory;
			StopProcessingOnFailureCheckBox.Checked = _mediaTesterOptions.StopProcessingOnFailure;
			QuickTestAfterEachFileCheckBox.Checked = _mediaTesterOptions.QuickTestAfterEachFile;
			QuickFirstFailingByteMethodCheckBox.Checked = _mediaTesterOptions.QuickFirstFailingByteMethod;

			UpdateTargetInformation();
		}

		private void UpdateOptionsFromUi()
		{
			_mediaTesterOptions.TestDirectory = TargetTextBox.Text;
			_mediaTesterOptions.StopProcessingOnFailure = StopProcessingOnFailureCheckBox.Checked;
			_mediaTesterOptions.QuickTestAfterEachFile = QuickTestAfterEachFileCheckBox.Checked;
			_mediaTesterOptions.QuickFirstFailingByteMethod = QuickFirstFailingByteMethodCheckBox.Checked;

			long lMaxBytesToTest;
			if (long.TryParse(MaxBytesToTestComboBox.Text.Replace(",", string.Empty).Replace(".", string.Empty), out lMaxBytesToTest))
			{
				_mediaTesterOptions.MaxBytesToTest = lMaxBytesToTest;
			}
			else
			{
				_mediaTesterOptions.MaxBytesToTest = -1;
				MaxBytesToTestComboBox.SelectedIndex = 0;
			}
		}

		private void ChooseTargetButton_Click(object sender, EventArgs e)
		{
			using (var folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
				DialogResult result = folderBrowserDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
				{
					TargetTextBox.Text = folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void TargetTextBox_TextChanged(object sender, EventArgs e)
		{
			UpdateTargetInformation();
		}

		private void UpdateTargetInformation()
		{
			string targetTotalBytes = PALCEHOLDER_VALUE;
			string targetAvailableBytes = PALCEHOLDER_VALUE;
			string targetDirectory = TargetTextBox.Text;

			if (!string.IsNullOrWhiteSpace(targetDirectory))
			{
				if (Directory.Exists(targetDirectory))
				{
					long lTargetTotalBytes;
					long lTargetAvailableBytes = MediaTester.GetAvailableBytes(targetDirectory, out lTargetTotalBytes);
					targetTotalBytes = $"{lTargetTotalBytes.ToString("#,##0")}{BYTES}";
					targetAvailableBytes = $"{lTargetAvailableBytes.ToString("#,##0")}{BYTES}";
				}
			}

			TargetTotalBytesLabel.Text = targetTotalBytes;
			TargetAvailableBytesLabel.Text = targetAvailableBytes;
		}

		private void SaveOptions_Click(object sender, EventArgs e)
		{
			SaveOptions();
		}

		private void SaveOptions()
		{
			UpdateOptionsFromUi();
			_mediaTesterOptions.Serialize();
		}

		private void DefaultOptionsButton_Click(object sender, EventArgs e)
		{
			_mediaTesterOptions = new Options();
			UpdateUiFromOptions();
		}

		private delegate void EnableControlsDelegate(bool enable);
		private void EnableControls(bool enable = true)
		{
			if (TestOptionsGgroupBox.InvokeRequired)
			{
				EnableControlsDelegate d = new EnableControlsDelegate(EnableControls);
				this.Invoke(d, new object[] { enable });
				return;
			}

			UpdateStatus(0, 0);

			TestOptionsGgroupBox.Enabled = enable;
			SaveOptionsButton.Enabled = enable;
			DefaultOptionsButton.Enabled = enable;
			WriteAndVerifyButton.Visible = enable;
			VerifyOnlyButton.Visible = enable;
			AbortButton.Visible = !enable;
			ProgressBar.Enabled = !enable;

			if (!enable)
			{
				WriteSpeedLabel.Text = PALCEHOLDER_VALUE;
				ReadSpeedLabel.Text = PALCEHOLDER_VALUE;
			}
		}

		private void DisableControls()
		{
			EnableControls(false);
		}

		private void WriteAndVerifyButton_Click(object sender, EventArgs e)
		{
			DisableControls();
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(MediaTesterFullTest));
			_mediaTesterThread.Start();
		}

		private void VerifyOnlyButton_Click(object sender, EventArgs e)
		{
			DisableControls();
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(MediaTesterVerifyTestFiles));
			_mediaTesterThread.Start();
		}

		private void MediaTesterFullTest()
		{
			try
			{
				bool result = _mediaTester.FullTest();
			}
			finally
			{
				FinishMediaTesterRun();
			}
		}

		private void MediaTesterVerifyTestFiles()
		{
			try
			{
				bool result = _mediaTester.VerifyTestFiles();
			}
			finally
			{
				FinishMediaTesterRun();
			}
		}

		private void InitializeMediaTester()
		{
			UpdateOptionsFromUi();
			_mediaTester = new MediaTester(_mediaTesterOptions);
			_mediaTester.OnException += OnMediaTesterException;
			_mediaTester.AfterQuickTest += AfterQuickTest;
			_mediaTester.AfterVerifyBlock += AfterVerifyBlock;
			_mediaTester.AfterWriteBlock += AfterWriteBlock;

			ClearLog(null, null);
			WriteLog(_mediaTester, $"Test data path: '{_mediaTester.GetTestDirectory()}'...");
			_startDateTime = DateTime.Now;
		}

		private void FinishMediaTesterRun()
		{
			_mediaTesterThread = null;
			_mediaTester = null;

			EnableControls();
		}

		private void AfterWriteBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long writeBytesPerSecond, int bytesWritten, int bytesFailedWrite)
		{
			UpdateStatus(writeBytesPerSecond: writeBytesPerSecond,
				writeBytesRemaining: mediaTester.Options.MaxBytesToTest - mediaTester.TotalBytesWritten,
				readBytesRemaining: mediaTester.Options.MaxBytesToTest);

			if (bytesFailedWrite == 0)
			{
				//WriteLog(mediaTester, $"Successfully wrote block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")}.");
			}
			else
			{
				WriteLog(mediaTester, $"FAILED writing block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")}.");
			}
		}

		private void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, false);
		}

		private void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, bool isQuickTest = false)
		{
			if (!isQuickTest)
				UpdateStatus(readBytesPerSecond: readBytesPerSecond,
					writeBytesRemaining: 0,
					readBytesRemaining: mediaTester.Options.MaxBytesToTest);

			if (bytesFailed == 0)
			{
				if (isQuickTest)
					WriteLog(mediaTester, $"{(isQuickTest ? "Quick test: " : string.Empty)}Verified block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")}");
			}
			else
			{
				WriteLog(mediaTester, $"{(isQuickTest ? "Quick test: " : string.Empty)}FAILED block {absoluteDataBlockIndex.ToString("#,##0")}! Byte index: {absoluteDataByteIndex.ToString("#,##0")}");
			}
		}

		private void AfterQuickTest(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, true);
		}

		private void OnMediaTesterException(MediaTester mediaTester, Exception exception)
		{
			WriteLog(mediaTester, $"{exception.Message}");
			if (exception.InnerException != null)
			{
				OnMediaTesterException(mediaTester, exception.InnerException);
			}
		}

		private delegate void WriteLogDelegate(MediaTester mediaTester, string message);
		private void WriteLog(MediaTester mediaTester, string message)
		{
			if (ActivityLogTextBox.InvokeRequired)
			{
				WriteLogDelegate d = new WriteLogDelegate(WriteLog);
				this.Invoke(d, new object[] { mediaTester, message });
				return;
			}

			if (message == null)
			{
				ActivityLogTextBox.Text = string.Empty;
			}
			else
			{
				ActivityLogTextBox.AppendText((ActivityLogTextBox.Text.Length == 0 ? string.Empty : "\r\n") + message);
			}
		}
		private void ClearLog(object p1, object p2)
		{
			WriteLog(null, null);
		}

		private delegate void UpdateStatusDelegate(long readBytesPerSecond, long writeBytesPerSecond, long writeBytesRemaining, long readBytesRemaining);
		private void UpdateStatus(long readBytesPerSecond = -1, long writeBytesPerSecond = -1, long writeBytesRemaining = 0, long readBytesRemaining = 0)
		{
			if (ActivityLogTextBox.InvokeRequired)
			{
				UpdateStatusDelegate d = new UpdateStatusDelegate(UpdateStatus);
				this.Invoke(d, new object[] { readBytesPerSecond, writeBytesPerSecond, writeBytesRemaining, readBytesRemaining });
				return;
			}

			UpdateSpeedAverage(readBytesPerSecond, writeBytesPerSecond);

			decimal bytesPerSecond = 0;

			if (writeBytesPerSecond > 0)
			{
				bytesPerSecond = _averageWriteBytesPerSecond;
			}
			else if (readBytesPerSecond > 0)
			{
				bytesPerSecond = _averageReadBytesPerSecond;
			}

			TimeSpan? elapsedTime = null;
			TimeSpan? writeTimeRemaining = null;
			TimeSpan? readTimeRemaining = null;
			TimeSpan? totalTimeRemaining = null;

			if (_startDateTime != null)
			{
				elapsedTime = new TimeSpan(0, 0, (int)((DateTime.Now - _startDateTime.Value).TotalSeconds));
				writeTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int)((decimal)writeBytesRemaining / bytesPerSecond));
				readTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int)((decimal)readBytesRemaining / bytesPerSecond)); // Assume read speed is the same as write speed since we do not know for sure.
				totalTimeRemaining = writeTimeRemaining + readTimeRemaining;
			}

			// TODO: display to the user...
			ElapsedTimeLabel.Text = (elapsedTime?.ToString() ?? PALCEHOLDER_VALUE);
			TotalTimeRemainingLabel.Text = (totalTimeRemaining?.ToString() ?? PALCEHOLDER_VALUE);

			if (_mediaTester != null)
			{
				WrittenBytesLabel.Text = (_mediaTester?.TotalBytesWritten.ToString("#,##0") ?? PALCEHOLDER_VALUE) + BYTES;
				VerifiedBytesLabel.Text = (_mediaTester?.TotalBytesVerified.ToString("#,##0") ?? PALCEHOLDER_VALUE) + BYTES;
				FailedBytesLabel.Text = (_mediaTester?.TotalBytesFailed.ToString("#,##0") ?? PALCEHOLDER_VALUE) + BYTES;
			}

			ProgressBar.Value = _mediaTester == null ? 0 : (int)(10M * _mediaTester.ProgressPercent);
		}

		long _totalReadSpeedSamples = 0;
		long _totalWriteSpeedSamples = 0;
		decimal _averageReadBytesPerSecond = 0;
		decimal _averageWriteBytesPerSecond = 0;

		private void UpdateSpeedAverage(long readBytesPerSecond = -1, long writeBytesPerSecond = -1)
		{
			if (writeBytesPerSecond > 0)
			{
				WriteBytesPerSecondStatusLabel.Text = "Write: " + writeBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
				UpdateAverage(ref _averageWriteBytesPerSecond, ref _totalWriteSpeedSamples, ref writeBytesPerSecond);
				WriteSpeedLabel.Text = _averageWriteBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
			}
			else
			{
				WriteBytesPerSecondStatusLabel.Text = string.Empty;
				_totalWriteSpeedSamples = 0;
			}

			if (readBytesPerSecond > 0)
			{
				ReadBytesPerSecondStatusLabel.Text = "Read: " + readBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
				UpdateAverage(ref _averageReadBytesPerSecond, ref _totalReadSpeedSamples, ref readBytesPerSecond);
				ReadSpeedLabel.Text = _averageReadBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
			}
			else
			{
				ReadBytesPerSecondStatusLabel.Text = string.Empty;
				_totalReadSpeedSamples = 0;
			}
		}

		private void UpdateAverage(ref decimal runningAverage, ref long totalSamples, ref long newValue)
		{
			if (totalSamples > 0)
			{
				runningAverage = ((decimal)totalSamples * runningAverage + (decimal)newValue) / (decimal)(++totalSamples);
			}
			else
			{
				runningAverage = newValue;
				totalSamples++;
			}
		}

		private void AbortButton_Click(object sender, EventArgs e)
		{
			try
			{
				_mediaTesterThread?.Abort();
				_startDateTime = null;
			}
			catch (Exception ex)
			{
			}
		}
	}
}
