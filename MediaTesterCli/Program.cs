using KrahmerSoft.MediaTesterLib;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace KrahmerSoft.MediaTesterCli
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string testDirectory = null;
			if (args == null || args.Length == 0)
			{
				while (string.IsNullOrEmpty(testDirectory))
				{
					Console.WriteLine();
					Console.WriteLine();
					Console.Write("Please enter a drive letter or a full path: ");
					testDirectory = Console.ReadLine();
				}
			}
			else
			{
				int pos = Array.IndexOf(args, "-d");
				if (args.Length > pos)
				{
					testDirectory = args[pos + 1];
				}

				if (testDirectory == null)
				{
					Console.WriteLine("Path missing");
					Environment.Exit(0);
				}
			}

			// Add :\ in case of single letter (e.g. C), since windows doesn't accept the drive letter as valid path
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				if (testDirectory.Length == 1)
				{
					testDirectory += @":\";
				}
			}

			DirectoryInfo info = new(testDirectory);
			if (info.Exists)
			{
				testDirectory = info.FullName;
			}

			if (!Directory.Exists(testDirectory))
			{
				Console.WriteLine("Path doesn't exist or is invalid");
				Environment.Exit(0);
			}

			testDirectory = Path.TrimEndingDirectorySeparator(testDirectory);
			if (!testDirectory.EndsWith(MediaTester.TempSubDirectoryName))
			{
				testDirectory = Path.Combine(testDirectory, MediaTester.TempSubDirectoryName);
			}

			Console.WriteLine();
			Console.WriteLine($"Bytes per file: {MediaTester.FILE_SIZE:#,##0}");
			Console.WriteLine($"Data block size: {MediaTester.DATA_BLOCK_SIZE:#,##0}");
			Console.WriteLine($"Blocks per file: {MediaTester.DATA_BLOCKS_PER_FILE:#,##0}");
			Console.WriteLine($"Writing temp files to: {testDirectory}...");
			Console.WriteLine();

			var mediaTester = new MediaTester(testDirectory)
			{
			};

			mediaTester.BlockWritten += AfterWriteBlock;
			mediaTester.QuickTestCompleted += AfterQuickTest;
			mediaTester.BlockVerified += AfterVerifyBlock;

			bool result = false;
			try
			{
				result = mediaTester.FullTest();
			}
			catch (Exception ex)
			{
				LogException(mediaTester, ex);
			}

			Console.WriteLine();
			Console.WriteLine("Media Test Summary...");
			Console.WriteLine("------------------------------------------------");
			Console.WriteLine("Result: " + (result ? "PASS" : "FAIL"));
			Console.WriteLine($"Temp File Path: {mediaTester.Options.TestDirectory}");
			Console.WriteLine($"Total bytes attempted: {mediaTester.TotalTargetBytes:#,##0}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesVerified > 0)
				Console.WriteLine($"Verified bytes: {mediaTester.TotalBytesVerified:#,##0}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesFailed > 0)
				Console.WriteLine($"Failed bytes: {mediaTester.TotalBytesFailed:#,##0}");

			if (mediaTester.FirstFailingByteIndex > 0)
				Console.WriteLine($"First failing byte index: {mediaTester.FirstFailingByteIndex:#,##0}{(mediaTester.Options.QuickFirstFailingByteMethod ? " (quick method)" : string.Empty)}");

			Console.WriteLine(result ? "SUCCESS!" : "FAIL!");

			Console.WriteLine();
			Console.WriteLine("Press enter to continue...");
			Console.ReadLine();
		}

		private static void AfterWriteBlock(object sender, WrittenBlockEventArgs e)
		{
			MediaTester mediaTester = sender as MediaTester;
			if (e.BytesFailedWrite == 0)
			{
				WriteLog(mediaTester, $"Successfully wrote block {e.AbsoluteDataBlockIndex:#,##0}. Byte index: {e.AbsoluteDataByteIndex:#,##0} / {mediaTester.TotalTargetBytes:#,##0}. {e.WriteBytesPerSecond:#,##0} B/sec ({mediaTester.ProgressPercent:0.00}%)");
			}
			else
			{
				WriteLog(mediaTester, $"FAILED writing block {e.AbsoluteDataBlockIndex:#,##0}. Byte index: {e.AbsoluteDataByteIndex:#,##0} / {mediaTester.TotalTargetBytes:#,##0}. ({mediaTester.ProgressPercent:0.00}%)");
			}
		}

		private static void AfterVerifyBlock(object sender, VerifiedBlockEventArgs e)
		{
			MediaTester mediaTester = sender as MediaTester;
			AfterVerifyBlock(mediaTester, e.AbsoluteDataBlockIndex, e.AbsoluteDataByteIndex, e.TestFilePath, e.VerifyBytesPerSecond, e.BytesVerified, e.BytesFailed, false);
		}

		private static void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePathlong, long verifyBytesPerSecond, int bytesVerified, int bytesFailed, bool isQuickTest = false)
		{
			WriteLog(mediaTester, $"{(bytesFailed == 0 ? "Verified" : "FAILED")} {(isQuickTest ? "quick test " : string.Empty)}block {absoluteDataBlockIndex,3:#,##0}. Byte index: {absoluteDataByteIndex,14:#,##0} / {mediaTester.TotalTargetBytes:#,##0}. {(isQuickTest ? string.Empty : verifyBytesPerSecond.ToString("#,##0") + "B/sec ")}({mediaTester.ProgressPercent:0.00}%)");
		}

		private static void AfterQuickTest(object sender, VerifiedBlockEventArgs e)
		{
			MediaTester mediaTester = sender as MediaTester;
			AfterVerifyBlock(mediaTester, e.AbsoluteDataBlockIndex, e.AbsoluteDataByteIndex, e.TestFilePath, e.VerifyBytesPerSecond, e.BytesVerified, e.BytesFailed, true);
		}

		private static void LogException(MediaTester mediaTester, Exception exception)
		{
			WriteLog(mediaTester, exception.Message);
			if (exception.InnerException != null)
			{
				LogException(mediaTester, exception.InnerException);
			}
		}

		private static void WriteLog(MediaTester mediaTester, string message)
		{
			string finalMessage = (mediaTester.IsSuccess ? "No errors" : "FAILURES!") + " - " + message;
			Console.WriteLine(finalMessage);
		}
	}
}