using MediaTesterLib;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MediaTester
{
	public partial class Main : Form
	{
		MediaTesterLib.Options _mediaTesterOptions = Options.Deserialize();
		MediaTesterLib.MediaTester _mediaTester;
		private Thread _mediaTesterThread;
		const string PALCEHOLDER_VALUE = "---";
		const string BYTES = " Bytes";
		const string BYTES_PER_SECOND = BYTES + "/sec";
		const string TEST_RESULTS_FILENAME_TEMPLATE = "MediaTesterResults_{0}_{1}.txt";
		const string TEST_RESULTS_FILENAME_DATETIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";

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
			RemoveTempDataFilesUponCompletionCheckBox.Checked = _mediaTesterOptions.RemoveTempDataFilesUponCompletion;
			SaveTestResultsFileToMediaCheckBox.Checked = _mediaTesterOptions.SaveTestResultsFileToMedia;

			UpdateTargetInformation();
		}

		private void UpdateOptionsFromUi()
		{
			_mediaTesterOptions.TestDirectory = TargetTextBox.Text;
			_mediaTesterOptions.StopProcessingOnFailure = StopProcessingOnFailureCheckBox.Checked;
			_mediaTesterOptions.QuickTestAfterEachFile = QuickTestAfterEachFileCheckBox.Checked;
			_mediaTesterOptions.QuickFirstFailingByteMethod = QuickFirstFailingByteMethodCheckBox.Checked;
			_mediaTesterOptions.RemoveTempDataFilesUponCompletion = RemoveTempDataFilesUponCompletionCheckBox.Checked;
			_mediaTesterOptions.SaveTestResultsFileToMedia = SaveTestResultsFileToMediaCheckBox.Checked;

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
					long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(targetDirectory, out lTargetTotalBytes, actual: true);
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

			UpdateTargetInformation();
			UpdateStatus(0, 0);

			TestOptionsGgroupBox.Enabled = enable;
			RemoveTempDataFilesButton.Enabled = enable;
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

		private void RemoveTempDataFilesButton_Click(object sender, EventArgs e)
		{
			if (!ValidateGui())
				return;

			DisableControls();
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(RemoveTempDataFilesGui));
			_mediaTesterThread.Start();
		}

		private void WriteAndVerifyButton_Click(object sender, EventArgs e)
		{
			if (!ValidateGui())
				return;

			if (MessageBox.Show("MediaTester works best if no existing data exists on the target media.\nContinue with non-destructive test?", "Start Test?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			DisableControls();
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(MediaTesterFullTest));
			_mediaTesterThread.Start();
		}

		private void VerifyOnlyButton_Click(object sender, EventArgs e)
		{
			if (!ValidateGui())
				return;

			DisableControls();
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(MediaTesterVerifyTestFiles));
			_mediaTesterThread.Start();
		}

		private bool ValidateGui()
		{
			if (string.IsNullOrWhiteSpace(TargetTextBox.Text) || !Directory.Exists(TargetTextBox.Text))
			{
				MessageBox.Show("The selected target is invalid or does not exist!", "Invalid target", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

		private void RemoveTempDataFilesGui()
		{
			try
			{
				RemoveTempDataFiles();
			}
			finally
			{
				ThreadCompleteEnableControls();
			}
		}

		private void RemoveTempDataFiles()
		{
			WriteLog(_mediaTester, $"Removing temp data files...");
			int filesDeleted = _mediaTester.RemoveTempDataFiles();
			WriteLog(_mediaTester, $"Removed {filesDeleted} temp data file{(filesDeleted == 1 ? string.Empty : "s")}.");
		}

		private void MediaTesterFullTest()
		{
			try
			{
				bool success = _mediaTester.FullTest();
				LogTestCompletion(success);
			}
			finally
			{
				ThreadCompleteEnableControls();
			}
		}

		private void MediaTesterVerifyTestFiles()
		{
			try
			{
				bool success = _mediaTester.VerifyTestFiles();
				LogTestCompletion(success);
			}
			finally
			{
				ThreadCompleteEnableControls();
			}
		}

		private void LogTestCompletion(bool success)
		{
			if (_averageWriteBytesPerSecond > 0)
				WriteLog(_mediaTester, $"Averge write speed: {_averageWriteBytesPerSecond.ToString("#,##0")}{BYTES_PER_SECOND}");

			if (_averageReadBytesPerSecond > 0)
				WriteLog(_mediaTester, $"Averge read speed: {_averageReadBytesPerSecond.ToString("#,##0")}{BYTES_PER_SECOND}");

			if (_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
			{
				RemoveTempDataFiles();
			}

			if (success)
			{
				long lTargetTotalBytes;
				long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out lTargetTotalBytes, actual: true);
				WriteLog(_mediaTester, $"Verified {_mediaTester.TotalBytesVerified.ToString("#,##0")}{BYTES} of {lTargetTotalBytes.ToString("#,##0")}{BYTES} total.");
				WriteLog(_mediaTester, $"Media test PASSED!");
				// WriteLog(_mediaTester, $"Information: Not all bytes are directly testable because directory and filenames take up additional space on the media.");

				if (!_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
				{
					WriteLog(_mediaTester, $"Notice: Be sure to delete the temporary directory before using the media. '{_mediaTester.GetTestDirectory()}'");
				}
			}
			else
			{
				WriteLog(_mediaTester, $"Media test FAILED! First failing byte: {_mediaTester.FirstFailingByteIndex.ToString("#,##0")}. Verified {_mediaTester.TotalBytesVerified.ToString("#,##0")}{BYTES}.");
			}

			if (_mediaTesterOptions.SaveTestResultsFileToMedia && (success || _mediaTesterOptions.RemoveTempDataFilesUponCompletion))
			{
				string dateTime = DateTime.Now.ToString(TEST_RESULTS_FILENAME_DATETIME_FORMAT);
				string testResultsFilePath = Path.Combine(_mediaTester.Options.TestDirectory, string.Format(TEST_RESULTS_FILENAME_TEMPLATE, dateTime, success ? "PASS" : "FAIL"));
				string testResultsLog = ActivityLogTextBox.Text;
				long lTargetTotalBytes;
				long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out lTargetTotalBytes, actual: true);

				int spaceNeeded = testResultsLog.Length + (int)Math.Pow(2, 16);
				bool enoughSpace = true;
				while (spaceNeeded > MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out lTargetTotalBytes, actual: true))
				{
					if (_mediaTester.RemoveTempDataFiles(1) < 1)
					{
						enoughSpace = false;
						break; // No files deleted
					}
				}

				if (enoughSpace)
				{
					File.WriteAllText(testResultsFilePath, testResultsLog);
					WriteLog(_mediaTester, $"Wrote test results file '{testResultsFilePath}'");
				}
				else
				{
					WriteLog(_mediaTester, $"Not enough free space to write test results file '{testResultsFilePath}'");
				}
			}
		}

		private void InitializeMediaTester()
		{
			UpdateOptionsFromUi();
			_mediaTester = new MediaTesterLib.MediaTester(_mediaTesterOptions);
			_mediaTester.OnException += OnMediaTesterException;
			_mediaTester.AfterQuickTest += AfterQuickTest;
			_mediaTester.AfterVerifyBlock += AfterVerifyBlock;
			_mediaTester.AfterWriteBlock += AfterWriteBlock;

			ClearLog(null, null);
			WriteLog(_mediaTester, $"MediaTester v{Assembly.GetEntryAssembly().GetName().Version}");
			WriteLog(_mediaTester, $"Temporary data path: '{_mediaTester.GetTestDirectory()}'");
			_startDateTime = DateTime.Now;
		}

		private void ThreadCompleteEnableControls()
		{
			_mediaTesterThread = null;
			_mediaTester = null;

			EnableControls();
		}

		private void AfterWriteBlock(MediaTesterLib.MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long writeBytesPerSecond, int bytesWritten, int bytesFailedWrite)
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

		private void AfterVerifyBlock(MediaTesterLib.MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, false);
		}

		private void AfterVerifyBlock(MediaTesterLib.MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, bool isQuickTest = false)
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

		private void AfterQuickTest(MediaTesterLib.MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, true);
		}

		private void OnMediaTesterException(MediaTesterLib.MediaTester mediaTester, Exception exception)
		{
			WriteLog(mediaTester, $"{exception.Message}");
			if (exception.InnerException != null)
			{
				OnMediaTesterException(mediaTester, exception.InnerException);
			}
		}

		private delegate void WriteLogDelegate(MediaTesterLib.MediaTester mediaTester, string message);
		private void WriteLog(MediaTesterLib.MediaTester mediaTester, string message)
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
			const decimal EstimatedReadVsWriteSpeedRatio = 2M;
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
				readTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int)((writeBytesPerSecond > 0 ? EstimatedReadVsWriteSpeedRatio : 1M)
									* (decimal)readBytesRemaining / bytesPerSecond)); // Assume read speed is the same as write speed since we do not know for sure.
				totalTimeRemaining = writeTimeRemaining + readTimeRemaining;
			}

			// Display to the user...
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

		private void AboutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string aboutMessage = $"MediaTester will test any media, such as SD cards, \n"
								+ $"and verify that the media stores the expected number \n"
								+ $"of bytes. If you buy any storage media, you should \n"
								+ $"use MediaTested to verify it or risk losing your data.\n"
								+ $"\n"
								+ $"Written by Doug Krahmer\n"
								+ $"Released as plublic domain open source.\n"
								+ $"https://github.com/dkrahmer/MediaTester (Click Help to view)\n\n"
								+ $"\n"
								+ $"Version: v{Assembly.GetEntryAssembly().GetName().Version}";

			MessageBox.Show(aboutMessage, "About MediaTester", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, "https://github.com/dkrahmer/MediaTester/releases");
		}

		private void Main_Load(object sender, EventArgs e)
		{
			this.Text += $" v{Assembly.GetEntryAssembly().GetName().Version}";
		}
	}
}
