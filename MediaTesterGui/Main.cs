using KrahmerSoft.MediaTesterLib;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace KrahmerSoft.MediaTesterGui
{
	public partial class Main : Form
	{
		private Options _mediaTesterOptions;
		private readonly bool[] _restartAfterClose;
		private bool _isInitializing = false;
		private MediaTester _mediaTester;
		private Thread _mediaTesterThread;
		private const string PLACEHOLDER_VALUE = "---";
		private const string TEST_RESULTS_FILENAME_DATETIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";
		private DateTime? _startDateTime;
		private Font _languageComboBoxClosedFont;
		private Font _languageComboBoxOpenFont;

		public Main(Options mediaTesterOptions = null, bool[] restartAfterClose = null)
		{
			_isInitializing = true;
			_mediaTesterOptions = mediaTesterOptions ?? Options.Deserialize();
			_restartAfterClose = restartAfterClose;

			Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
			InitializeComponent();
			InitializeValues();
			UpdateUiFromOptions();
			EnableControls();

			FormClosing += Main_FormClosing;
			_isInitializing = false;
		}

		private void InitializeValues()
		{
			_languageComboBoxOpenFont = LanguageComboBox.Font;
			_languageComboBoxClosedFont = new Font("Courier New", _languageComboBoxOpenFont.Size, FontStyle.Regular);
			LanguageComboBox.Font = _languageComboBoxClosedFont;
			LanguageComboBox.Items.AddRange(Languages.Options);
		}

		private void UpdateUiFromOptions()
		{
			if (_mediaTesterOptions.MaxBytesToTest < 0)
			{
				MaxBytesToTestComboBox.SelectedIndex = 0;
			}
			else
			{
				MaxBytesToTestComboBox.Text = _mediaTesterOptions.MaxBytesToTest.ToString("n0");
			}
			TargetTextBox.Text = _mediaTesterOptions.TestDirectory;
			StopProcessingOnFailureCheckBox.Checked = _mediaTesterOptions.StopProcessingOnFailure;
			QuickTestAfterEachFileCheckBox.Checked = _mediaTesterOptions.QuickTestAfterEachFile;
			QuickFirstFailingByteMethodCheckBox.Checked = _mediaTesterOptions.QuickFirstFailingByteMethod;
			RemoveTempDataFilesUponCompletionCheckBox.Checked = _mediaTesterOptions.RemoveTempDataFilesUponCompletion;
			SaveTestResultsFileToMediaCheckBox.Checked = _mediaTesterOptions.SaveTestResultsFileToMedia;
			SelectCurrentLanguageCode();

			UpdateTargetInformation();
		}

		private void SelectCurrentLanguageCode()
		{
			string selectCode = Thread.CurrentThread.CurrentCulture.Name;
			if (!SelectLanguageCode(selectCode))
				SelectLanguageCode(Languages.Default);
		}

		private bool SelectLanguageCode(string selectCode)
		{
			_isInitializing = true;
			try
			{
				for (int i = 0; i < Languages.Options.Length; i++)
				{
					if (Languages.Options[i].Code == selectCode)
					{
						LanguageComboBox.SelectedIndex = i;
						return true;
					}
				}

				return false;
			}
			finally
			{
				_isInitializing = false;
			}
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
					long lTargetAvailableBytes = MediaTester.GetAvailableBytes(targetDirectory, out long lTargetTotalBytes, actual: true);
					targetTotalBytes = $"{lTargetTotalBytes:n0} {Strings.Bytes}";
					targetAvailableBytes = $"{lTargetAvailableBytes:n0} {Strings.Bytes}";
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
				Invoke(d, new object[] { enable });
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
			SaveOptions();
			InitializeMediaTester();
			_mediaTesterThread = new Thread(new ThreadStart(RemoveTempDataFilesGui));
			_mediaTesterThread.Start();
		}

		private void WriteAndVerifyButton_Click(object sender, EventArgs e)
		{
			if (!ValidateGui())
				return;

			if (MessageBox.Show(Strings.ContinueWithTest, Strings.StartTest, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
				MessageBox.Show(Strings.TheSelectedTargetIsInvalid, Strings.InvalidTarget, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				WriteLog(_mediaTester, $"{Strings.AverageWriteSpeed}: {_averageWriteBytesPerSecond:n0} {Strings.BytesPerSecond}");

			if (_averageReadBytesPerSecond > 0)
				WriteLog(_mediaTester, $"{Strings.AverageReadSpeed}: {_averageReadBytesPerSecond:n0} {Strings.BytesPerSecond}");

			if (_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
			{
				RemoveTempDataFiles();
			}

			if (success)
			{
				long lTargetAvailableBytes = MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out long lTargetTotalBytes, actual: true);
				WriteLog(_mediaTester, Strings.VerifiedXBytesOfXTotal
					.Replace("{TotalBytesVerified}", $"{_mediaTester.TotalBytesVerified:n0}")
					.Replace("{TargetTotalBytes}", $"{lTargetTotalBytes:n0}"));
				WriteLog(_mediaTester, Strings.MediaTestPassed);
				// WriteLog(_mediaTester, $"Information: Not all bytes are directly testable because directory and filenames take up additional space on the media.");

				if (!_mediaTesterOptions.RemoveTempDataFilesUponCompletion)
				{
					WriteLog(_mediaTester, $"{Strings.BeSureToDeleteTemporaryDirectory} '{_mediaTester.GetTestDirectory()}'");
				}
			}
			else
			{
				WriteLog(_mediaTester, Strings.MediaTestFailed
					.Replace("{FirstFailingByteIndex}", $"{_mediaTester.FirstFailingByteIndex:n0}")
					.Replace("{TotalBytesVerified}", $"{_mediaTester.TotalBytesVerified:n0}"));
			}

			if (_mediaTesterOptions.SaveTestResultsFileToMedia && (success || _mediaTesterOptions.RemoveTempDataFilesUponCompletion))
			{
				string testResultsFilePath = Path.Combine(_mediaTester.Options.TestDirectory,
					Strings.ResultsFilename
						.Replace("{DateTime}", DateTime.Now.ToString(TEST_RESULTS_FILENAME_DATETIME_FORMAT))
						.Replace("{PassFail}", success ? Strings.Pass : Strings.Fail));
				string testResultsLog = ActivityLogTextBox.Text;
				long lTargetAvailableBytes = MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out _, actual: true);

				int spaceNeeded = testResultsLog.Length + (int) Math.Pow(2, 16);
				bool enoughSpace = true;
				while (spaceNeeded > MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out _, actual: true))
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
					WriteLog(_mediaTester, $"{Strings.WroteTestResultsFile} '{testResultsFilePath}'");
				}
				else
				{
					WriteLog(_mediaTester, $"{Strings.NotEnoughFreeSpaceToWrite} '{testResultsFilePath}'");
				}
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

			long lTargetAvailableBytes = MediaTester.GetAvailableBytes(_mediaTester.GetTestDirectory(), out long lTargetTotalBytes, actual: true);

			ClearLog(null, null);
			WriteLog(_mediaTester, $"MediaTester v{Assembly.GetEntryAssembly().GetName().Version}");
			WriteLog(_mediaTester, $"{Strings.TotalReportedMediaSize}: {lTargetTotalBytes:n0} {Strings.Bytes}");
			WriteLog(_mediaTester, $"{Strings.TotalReportedAvailableSpace}: {lTargetAvailableBytes:n0} {Strings.Bytes}");
			WriteLog(_mediaTester, $"{Strings.TemporaryDataPath}: '{_mediaTester.GetTestDirectory()}'");
			_startDateTime = DateTime.Now;
		}

		private void ThreadCompleteEnableControls()
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
				WriteLog(_mediaTester, Strings.FailedWritingBlock
					.Replace("{AbsoluteDataBlockIndex}", $"{absoluteDataBlockIndex:n0}")
					.Replace("{AbsoluteDataByteIndex}", $"{absoluteDataByteIndex:n0}"));
			}
		}

		private void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, verifyBytesPerSecond, false);
		}

		private void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond, bool isQuickTest = false)
		{
			if (!isQuickTest)
				UpdateStatus(readBytesPerSecond: readBytesPerSecond,
					verifyBytesPerSecond: verifyBytesPerSecond,
					writeBytesRemaining: 0,
					readBytesRemaining: mediaTester.Options.MaxBytesToTest - mediaTester.TotalBytesVerified - mediaTester.TotalBytesFailed);

			if (bytesFailed == 0)
			{
				if (isQuickTest)
				{
					string verifiedBlockByteIndex = Strings.VerifiedBlockByteIndex.Replace("{AbsoluteDataBlockIndex}", $"{absoluteDataBlockIndex:n0}").Replace("{AbsoluteDataByteIndex}", $"{absoluteDataByteIndex:n0}");
					WriteLog(mediaTester, $"{(isQuickTest ? $"{Strings.QuickTest}: " : string.Empty)}{verifiedBlockByteIndex}");
				}
			}
			else
			{
				string failedBlockByteIndex = Strings.FailedBlockByteIndex.Replace("{AbsoluteDataBlockIndex}", $"{absoluteDataBlockIndex:n0}").Replace("{AbsoluteDataByteIndex}", $"{absoluteDataByteIndex:n0}");
				WriteLog(mediaTester, $"{(isQuickTest ? $"{Strings.QuickTest}: " : string.Empty)}{failedBlockByteIndex}");
				if (isQuickTest)
				{
					WriteLog(mediaTester, Strings.IdentifyingFirstFailingByte);
				}
			}
		}

		private void AfterQuickTest(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, verifyBytesPerSecond, true);
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
				Invoke(d, new object[] { mediaTester, message });
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

		private delegate void UpdateStatusDelegate(long readBytesPerSecond, long writeBytesPerSecond, long writeBytesRemaining, long readBytesRemaining, long verifyBytesPerSecond);
		private void UpdateStatus(long readBytesPerSecond = -1, long writeBytesPerSecond = -1, long writeBytesRemaining = 0, long readBytesRemaining = 0, long verifyBytesPerSecond = 0)
		{
			const decimal EstimatedReadVsWriteSpeedRatio = 2M;
			if (ActivityLogTextBox.InvokeRequired)
			{
				UpdateStatusDelegate d = new UpdateStatusDelegate(UpdateStatus);
				Invoke(d, new object[] { readBytesPerSecond, writeBytesPerSecond, writeBytesRemaining, readBytesRemaining, verifyBytesPerSecond });
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
				WrittenBytesLabel.Text = $"{_mediaTester?.TotalBytesWritten.ToString("n0") ?? PLACEHOLDER_VALUE} {Strings.Bytes}";
				VerifiedBytesLabel.Text = $"{_mediaTester?.TotalBytesVerified.ToString("n0") ?? PLACEHOLDER_VALUE} {Strings.Bytes}";
				FailedBytesLabel.Text = $"{_mediaTester?.TotalBytesFailed.ToString("n0") ?? PLACEHOLDER_VALUE} {Strings.Bytes}";
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
				WriteBytesPerSecondStatusLabel.Text = $"{Strings.Write}: {writeBytesPerSecond:n0} {Strings.BytesPerSecond}";
				Helpers.UpdateAverage(ref _averageWriteBytesPerSecond, ref _totalWriteSpeedSamples, ref writeBytesPerSecond);
				WriteSpeedLabel.Text = $"{_averageWriteBytesPerSecond:n0} {Strings.BytesPerSecond}";
			}
			else
			{
				WriteBytesPerSecondStatusLabel.Text = string.Empty;
				_totalWriteSpeedSamples = 0;
			}

			if (readBytesPerSecond > 0)
			{
				ReadBytesPerSecondStatusLabel.Text = $"{Strings.Read}: {readBytesPerSecond:n0} {Strings.BytesPerSecond}";
				Helpers.UpdateAverage(ref _averageReadBytesPerSecond, ref _totalReadSpeedSamples, ref readBytesPerSecond);
				ReadSpeedLabel.Text = $"{_averageReadBytesPerSecond:n0} {Strings.BytesPerSecond}";
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
				_mediaTesterThread?.Abort();
				_startDateTime = null;
			}
			catch //(Exception ex)
			{
			}
		}

		private void AboutButton_Click(object sender, EventArgs e)
		{
			string aboutMessage = string.Join("\n",
				$"MediaTester v{Assembly.GetEntryAssembly().GetName().Version}",
				$"Copyright © 2022  Doug Krahmer",
				$"",
				$"MediaTester can test any media (SD, microSD, thumb, etc)",
				$"and verify it stores the expected number of bytes.",
				$"If you buy any storage media, you should use MediaTester",
				$"to verify it or risk losing your data.",
				$"",
				$"https://github.com/dkrahmer/MediaTester (Click 'Help' button to view)",
				$"",
				$"This program is free software: you can redistribute it and/or modify",
				$"it under the terms of the GNU General Public License as published by",
				$"the Free Software Foundation, either version 3 of the License, or",
				$"(at your option) any later version.",
				$"",
				$"This program is distributed in the hope that it will be useful,",
				$"but WITHOUT ANY WARRANTY; without even the implied warranty of",
				$"MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the",
				$"GNU General Public License for more details.",
				$"",
				$"You should have received a copy of the GNU General Public License",
				$"along with this program.  If not, see <http://www.gnu.org/licenses/>."
			);

			MessageBox.Show(aboutMessage, "About MediaTester", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, "https://github.com/dkrahmer/MediaTester/releases");
		}

		private void Main_Load(object sender, EventArgs e)
		{
			Text += $" v{Assembly.GetEntryAssembly().GetName().Version}";
		}
		private void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				_mediaTesterThread?.Interrupt();
				_mediaTesterThread?.Join();
			}
			catch
			{
			}
		}

		private void LanguageComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (_isInitializing)
				return;

			string languageCode = (LanguageComboBox.SelectedItem as Languages.LanguageCode)?.Code;
			if (languageCode == Thread.CurrentThread.CurrentCulture.Name)
				return;

			var requestedCulture = new System.Globalization.CultureInfo(languageCode);
			string changeLanguageRestartDetails = Strings.ResourceManager.GetString(nameof(Strings.ChangeLanguageRestartDetails), requestedCulture)
				+ $"\n\n{Strings.ChangeLanguageRestartDetails}";
			string changeLanguageRestart = Strings.ResourceManager.GetString(nameof(Strings.ChangeLanguageRestart), requestedCulture);

			if (MessageBox.Show(changeLanguageRestartDetails, changeLanguageRestart, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				_mediaTesterOptions.LanguageCode = languageCode;

				if (_restartAfterClose != null && _restartAfterClose.Length > 0)
					_restartAfterClose[0] = true;
				Close();
			}
			else
			{
				SelectCurrentLanguageCode();
			}
		}

		private void LanguageComboBox_DropDownClosed(object sender, EventArgs e)
		{
			LanguageComboBox.Font = _languageComboBoxClosedFont;
		}

		private void LanguageComboBox_DropDown(object sender, EventArgs e)
		{
			LanguageComboBox.Font = _languageComboBoxOpenFont;
		}
	}
}
