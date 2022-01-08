﻿using KrahmerSoft.MediaTesterLib;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace KrahmerSoft.MediaTester
{
	public partial class Main : Form
	{
		private Options _mediaTesterOptions = Options.Deserialize();
		private MediaTesterLib.MediaTester _mediaTester;
		private Thread _mediaTesterThread;
		private const string PLACEHOLDER_VALUE = "---";
		private const string BYTES = " Bytes";
		private const string BYTES_PER_SECOND = BYTES + "/sec";
		private const string TEST_RESULTS_FILENAME_TEMPLATE = "MediaTesterResults_{0}_{1}.txt";
		private const string TEST_RESULTS_FILENAME_DATETIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";
		private DateTime? _startDateTime;

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

			if (long.TryParse(MaxBytesToTestComboBox.Text.Replace(",", string.Empty).Replace(".", string.Empty), out long lMaxBytesToTest))
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
			string targetTotalBytes = PLACEHOLDER_VALUE;
			string targetAvailableBytes = PLACEHOLDER_VALUE;
			string targetDirectory = TargetTextBox.Text;

			if (!string.IsNullOrWhiteSpace(targetDirectory))
			{
				if (Directory.Exists(targetDirectory))
				{
					long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(targetDirectory, out long lTargetTotalBytes, actual: true);
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

		private void EnableControls(bool enable = true)
		{
			if (TestOptionsGgroupBox.InvokeRequired)
			{
				Invoke(() => EnableControls(enable));
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
			AbortButton.Enabled = true;

			if (!enable)
			{
				WriteSpeedLabel.Text = PLACEHOLDER_VALUE;
				ReadSpeedLabel.Text = PLACEHOLDER_VALUE;
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
			AbortButton.Enabled = false;
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
				try
				{
					RemoveTempDataFiles();
				}
				catch (IOException)
				{
					WriteLog("An error raised during the cancellation of test files.");
					WriteLog("Check the file permission, reconnect the drive and retry.");
				}
				
			}
			finally
			{
				ThreadCompleteEnableControls();
			}
		}

		private void RemoveTempDataFiles()
		{
			WriteLog($"Removing temp data files...");
			int filesDeleted = _mediaTester.RemoveTempDataFiles();
			WriteLog($"Removed {filesDeleted} temp data file{(filesDeleted == 1 ? string.Empty : "s")}.");
		}

		private enum ResultMediaTest
		{ PASS, FAIL, USER_CANCEL, ERROR };

		private void MediaTesterFullTest()
		{
			try
			{
				ResultMediaTest result = ResultMediaTest.FAIL;
				try
				{
					if (_mediaTester.FullTest())
					{
						result = ResultMediaTest.PASS;
					}
				}
				catch (ThreadInterruptedException)
				{
					result = ResultMediaTest.USER_CANCEL;
				}
				catch (IOException)
				{
					result = ResultMediaTest.ERROR;
				}
				ElaborateTestResult(result);
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
				ResultMediaTest result = ResultMediaTest.FAIL;
				try
				{
					if (_mediaTester.VerifyTestFiles())
					{
						result = ResultMediaTest.PASS;
					}
				}
				catch (ThreadInterruptedException)
				{
					result = ResultMediaTest.USER_CANCEL;
				}
				catch (IOException)
				{
					result = ResultMediaTest.ERROR;
				}
				ElaborateTestResult(result);
			}
			finally
			{
				ThreadCompleteEnableControls();
			}
		}

		private void ElaborateTestResult(ResultMediaTest result)
		{
			if (result == ResultMediaTest.USER_CANCEL)
			{
				ClearLog();
				WriteLog("The test was manually stopped by the user.");
			}
			else if(result == ResultMediaTest.ERROR)
			{
				ClearLog();
				WriteLog("An error raised during the test and it was interrupted.");
				WriteLog("Check the file permission, reconnect the drive and retry.");
			}
			else
			{
				LogTestCompletion(result == ResultMediaTest.PASS);
			}
		}

		private void LogTestCompletion(bool success)
		{
			if (_averageWriteBytesPerSecond > 0)
				WriteLog($"Averge write speed: {_averageWriteBytesPerSecond.ToString("#,##0")}{BYTES_PER_SECOND}");

			if (_averageReadBytesPerSecond > 0)
				WriteLog($"Averge read speed: {_averageReadBytesPerSecond.ToString("#,##0")}{BYTES_PER_SECOND}");

			if (_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
			{
				RemoveTempDataFiles();
			}

			if (success)
			{
				long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out long lTargetTotalBytes, actual: true);
				WriteLog($"Verified {_mediaTester.TotalBytesVerified.ToString("#,##0")}{BYTES} of {lTargetTotalBytes.ToString("#,##0")}{BYTES} total.");
				WriteLog($"Media test PASSED!");
				// WriteLog(_mediaTester, $"Information: Not all bytes are directly testable because directory and filenames take up additional space on the media.");

				if (!_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
				{
					WriteLog($"Notice: Be sure to delete the temporary directory before using the media. '{_mediaTester.GetTestDirectory()}'");
				}
			}
			else
			{
				WriteLog($"Media test FAILED! First failing byte: {_mediaTester.FirstFailingByteIndex.ToString("#,##0")}. Verified {_mediaTester.TotalBytesVerified.ToString("#,##0")}{BYTES}.");
			}

			if (_mediaTesterOptions.SaveTestResultsFileToMedia && (success || _mediaTesterOptions.RemoveTempDataFilesUponCompletion))
			{
				string dateTime = DateTime.Now.ToString(TEST_RESULTS_FILENAME_DATETIME_FORMAT);
				string testResultsFilePath = Path.Combine(_mediaTester.Options.TestDirectory, string.Format(TEST_RESULTS_FILENAME_TEMPLATE, dateTime, success ? "PASS" : "FAIL"));
				string testResultsLog = ActivityLogTextBox.Text;
				long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out long lTargetTotalBytes, actual: true);

				int spaceNeeded = testResultsLog.Length + (int) Math.Pow(2, 16);
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
					WriteLog($"Wrote test results file '{testResultsFilePath}'");
				}
				else
				{
					WriteLog($"Not enough free space to write test results file '{testResultsFilePath}'");
				}
			}
		}

		private void InitializeMediaTester()
		{
			UpdateOptionsFromUi();
			_mediaTester = new MediaTesterLib.MediaTester(_mediaTesterOptions);
			_mediaTester.ExceptionThrown += (s, e) => { LogException(s as MediaTesterLib.MediaTester, e.Exception); };
			_mediaTester.QuickTestCompleted += (s, e) =>
			{
				AfterVerifyBlock(s as MediaTesterLib.MediaTester, e.AbsoluteDataBlockIndex, e.AbsoluteDataByteIndex, e.TestFilePath, e.ReadBytesPerSecond, e.BytesVerified, e.BytesFailed, e.VerifyBytesPerSecond, true);
			};
			_mediaTester.BlockVerified += (s, e) =>
			{
				AfterVerifyBlock(s as MediaTesterLib.MediaTester, e.AbsoluteDataBlockIndex, e.AbsoluteDataByteIndex, e.TestFilePath, e.ReadBytesPerSecond, e.BytesVerified, e.BytesFailed, e.VerifyBytesPerSecond, false);
			};
			_mediaTester.BlockWritten += (s, e) =>
			{
				AfterWriteBlock(s as MediaTesterLib.MediaTester, e.AbsoluteDataBlockIndex, e.AbsoluteDataByteIndex, e.TestFilePath, e.WriteBytesPerSecond, e.BytesWritten, e.BytesFailedWrite);
			};

			long lTargetAvailableBytes = MediaTesterLib.MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out long lTargetTotalBytes, actual: true);

			ClearLog();
			WriteLog($"MediaTester v{Assembly.GetEntryAssembly().GetName().Version}");
			WriteLog($"Total reported media size: {lTargetTotalBytes.ToString("#,##0")}{BYTES}");
			WriteLog($"Total reported available space: {lTargetAvailableBytes.ToString("#,##0")}{BYTES}");
			WriteLog($"Temporary data path: '{_mediaTester.GetTestDirectory()}'");
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
				WriteLog($"FAILED writing block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")}.");
			}
		}

		private void AfterVerifyBlock(MediaTesterLib.MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond, bool isQuickTest = false)
		{
			if (!isQuickTest)
				UpdateStatus(readBytesPerSecond: readBytesPerSecond,
					verifyBytesPerSecond: verifyBytesPerSecond,
					writeBytesRemaining: 0,
					readBytesRemaining: mediaTester.Options.MaxBytesToTest - mediaTester.TotalBytesVerified - mediaTester.TotalBytesFailed);

			if (bytesFailed == 0)
			{
				if (isQuickTest)
					WriteLog($"{(isQuickTest ? "Quick test: " : string.Empty)}Verified block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")}");
			}
			else
			{
				WriteLog($"{(isQuickTest ? "Quick test: " : string.Empty)}FAILED block {absoluteDataBlockIndex.ToString("#,##0")}! Byte index: {absoluteDataByteIndex.ToString("#,##0")}");
				if (isQuickTest)
				{
					WriteLog("Identifying first failing byte...");
				}
			}
		}

		/// <summary>
		/// Write Exception message on log. Since exception may be nested, this function is recursive.
		/// </summary>
		/// <param name="mediaTester"></param>
		/// <param name="exception"></param>
		private void LogException(MediaTesterLib.MediaTester mediaTester, Exception exception)
		{
			WriteLog($"{exception.Message}");
			if (exception.InnerException != null)
			{
				LogException(mediaTester, exception.InnerException);
			}
		}

		private void WriteLog(string message)
		{
			if (ActivityLogTextBox.InvokeRequired)
			{
				Invoke(() => WriteLog(message));
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

		private void ClearLog()
		{
			WriteLog(null);
		}

		private void UpdateStatus(long readBytesPerSecond = -1, long writeBytesPerSecond = -1, long writeBytesRemaining = 0, long readBytesRemaining = 0, long verifyBytesPerSecond = 0)
		{
			const decimal EstimatedReadVsWriteSpeedRatio = 2M;
			if (ActivityLogTextBox.InvokeRequired)
			{
				Invoke(() => UpdateStatus(readBytesPerSecond, writeBytesPerSecond, writeBytesRemaining, readBytesRemaining, verifyBytesPerSecond));
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
				elapsedTime = new TimeSpan(0, 0, (int) (DateTime.Now - _startDateTime.Value).TotalSeconds);
				writeTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int) ((decimal) writeBytesRemaining / bytesPerSecond));
				if (verifyBytesPerSecond > 1000)
				{
					readTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int) ((decimal) readBytesRemaining / verifyBytesPerSecond)); // Assume read speed is the same as write speed since we do not know for sure.
				}
				else
				{
					readTimeRemaining = new TimeSpan(0, 0, bytesPerSecond < .01M ? 0 : (int) ((writeBytesPerSecond > 0 ? EstimatedReadVsWriteSpeedRatio : 1M)
										* (decimal) readBytesRemaining / bytesPerSecond)); // Assume read speed is the same as write speed since we do not know for sure.
				}
				totalTimeRemaining = writeTimeRemaining + readTimeRemaining;
			}

			// Display to the user...
			ElapsedTimeLabel.Text = elapsedTime?.ToString() ?? PLACEHOLDER_VALUE;
			TotalTimeRemainingLabel.Text = totalTimeRemaining?.ToString() ?? PLACEHOLDER_VALUE;

			if (_mediaTester != null)
			{
				WrittenBytesLabel.Text = (_mediaTester?.TotalBytesWritten.ToString("#,##0") ?? PLACEHOLDER_VALUE) + BYTES;
				VerifiedBytesLabel.Text = (_mediaTester?.TotalBytesVerified.ToString("#,##0") ?? PLACEHOLDER_VALUE) + BYTES;
				FailedBytesLabel.Text = (_mediaTester?.TotalBytesFailed.ToString("#,##0") ?? PLACEHOLDER_VALUE) + BYTES;
			}

			ProgressBar.Value = _mediaTester == null ? 0 : (int) (10M * _mediaTester.ProgressPercent);
		}

		private long _totalReadSpeedSamples = 0;
		private long _totalWriteSpeedSamples = 0;
		private decimal _averageReadBytesPerSecond = 0;
		private decimal _averageWriteBytesPerSecond = 0;

		private void UpdateSpeedAverage(long readBytesPerSecond = -1, long writeBytesPerSecond = -1)
		{
			if (writeBytesPerSecond > 0)
			{
				WriteBytesPerSecondStatusLabel.Text = "Write: " + writeBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
				Helpers.UpdateAverage(ref _averageWriteBytesPerSecond, ref _totalWriteSpeedSamples, ref writeBytesPerSecond);
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
				Helpers.UpdateAverage(ref _averageReadBytesPerSecond, ref _totalReadSpeedSamples, ref readBytesPerSecond);
				ReadSpeedLabel.Text = _averageReadBytesPerSecond.ToString("#,##0") + BYTES_PER_SECOND;
			}
			else
			{
				ReadBytesPerSecondStatusLabel.Text = string.Empty;
				_totalReadSpeedSamples = 0;
			}
		}

		private void AbortButton_Click(object sender, EventArgs e)
		{
			try
			{
				_mediaTesterThread?.Interrupt();
				_startDateTime = null;
			}
			catch
			{
			}
		}

		private void AboutLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string aboutMessage = $"MediaTester can test any media (SD, microSD, thumb, etc) \n"
								+ $"and verify it stores the expected number of bytes. \n"
								+ $"If you buy any storage media, you should use MediaTester \n"
								+ $"to verify it or risk losing your data.\n"
								+ $"\n"
								+ $"Written by Doug Krahmer\n"
								+ $"Released as public domain open source.\n"
								+ $"https://github.com/dkrahmer/MediaTester (Click Help to view)\n\n"
								+ $"\n"
								+ $"Version: v{Assembly.GetEntryAssembly().GetName().Version}";

			MessageBox.Show(aboutMessage, "About MediaTester", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, "https://github.com/dkrahmer/MediaTester/releases");
		}

		private void Main_Load(object sender, EventArgs e)
		{
			Text += $" v{Assembly.GetEntryAssembly().GetName().Version}";
		}
	}
}