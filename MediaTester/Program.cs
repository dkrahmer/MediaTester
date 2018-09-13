using MediaTesterLib;
using System;
using System.IO;

namespace MediaTesterCli
{
	class Program
	{
		static void Main(string[] args)
		{
			string testDirectory = null;
			if (args == null || args.Length < 1)
			{
				while (string.IsNullOrEmpty(testDirectory))
				{
					Console.WriteLine();
					Console.WriteLine();
					Console.Write("Please enter a drive letter or path to test: ");
					testDirectory = Console.ReadLine();
				}
				if (!testDirectory.Contains(@":\"))
				{
					testDirectory = Path.Combine(testDirectory.Substring(0, 1).ToUpper() + @":\", MediaTester.TempSubDirectoryName);
				}
			}
			else
			{
				testDirectory = args[0];
			}

			testDirectory = testDirectory.TrimEnd('\\');
			if (!testDirectory.EndsWith(MediaTester.TempSubDirectoryName))
			{
				testDirectory = Path.Combine(testDirectory, MediaTester.TempSubDirectoryName);
			}

			Console.WriteLine();
			Console.WriteLine($"Bytes per file: {MediaTester.FILE_SIZE.ToString("#,##0")}");
			Console.WriteLine($"Data block size: {MediaTester.DATA_BLOCK_SIZE.ToString("#,##0")}");
			Console.WriteLine($"Blocks per file: {MediaTester.DATA_BLOCKS_PER_FILE.ToString("#,##0")}");
			Console.WriteLine($"Writing temp files to: {testDirectory}...");
			Console.WriteLine();

			var mediaTester = new MediaTester(testDirectory)
			{
			};

			mediaTester.AfterWriteBlock += AfterWriteBlock;
			mediaTester.AfterQuickTest += AfterQuickTest;
			mediaTester.AfterVerifyBlock += AfterVerifyBlock;
			mediaTester.OnException += OnMediaTesterException;

			bool result = mediaTester.FullTest();

			Console.WriteLine();
			Console.WriteLine("Media Test Summary...");
			Console.WriteLine("------------------------------------------------");
			Console.WriteLine("Result: " + (result ? "PASS" : "FAIL"));
			Console.WriteLine($"Temp File Path: {mediaTester.Options.TestDirectory}");
			Console.WriteLine($"Total bytes attempted: {mediaTester.Options.MaxBytesToTest.ToString("#,##0")}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesVerified > 0)
				Console.WriteLine($"Verified bytes: {mediaTester.TotalBytesVerified.ToString("#,##0")}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesFailed > 0)
				Console.WriteLine($"Failed bytes: {mediaTester.TotalBytesFailed.ToString("#,##0")}");

			if (mediaTester.FirstFailingByteIndex > 0)
				Console.WriteLine($"First failing byte index: {mediaTester.FirstFailingByteIndex.ToString("#,##0")}{(mediaTester.Options.QuickFirstFailingByteMethod ? " (quick method)" : string.Empty)}");

			if (!result)
			{
				Console.WriteLine();
				for (int i = 0; i < 20; i++)
				{
					Console.Write(i % 2 == 0 ? string.Empty : "   ");
					for (int j = 0; j < 15; j++)
					{
						Console.Write("FAIL!  ");
					}
					Console.WriteLine();
				}
			}

			Console.WriteLine();
			Console.WriteLine("Press enter to continue...");
			Console.ReadLine();
		}

		private static void AfterWriteBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long writeBytesPerSecond, int bytesWritten, int bytesFailedWrite)
		{
			if (bytesFailedWrite == 0)
			{
				WriteLog(mediaTester, $"Successfully wrote block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")} / {mediaTester.Options.MaxBytesToTest.ToString("#,##0")}. {writeBytesPerSecond.ToString("#,##0")} B/sec ({mediaTester.ProgressPercent.ToString("0.00")}%)");
			}
			else
			{
				WriteLog(mediaTester, $"FAILED writing block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")} / {mediaTester.Options.MaxBytesToTest.ToString("#,##0")}. ({mediaTester.ProgressPercent.ToString("0.00")}%)");
			}
		}

		private static void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, false);
		}

		private static void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePathlong, long readBytesPerSecond, int bytesVerified, int bytesFailed, bool isQuickTest = false)
		{
			if (bytesFailed == 0)
			{
				WriteLog(mediaTester, $"Verified {(isQuickTest ? "quick test " : string.Empty)}block {absoluteDataBlockIndex.ToString("#,##0")}. Byte index: {absoluteDataByteIndex.ToString("#,##0")} / {mediaTester.Options.MaxBytesToTest.ToString("#,##0")}. {(isQuickTest ? string.Empty : readBytesPerSecond.ToString("#,##0") + "B/sec ")}({mediaTester.ProgressPercent.ToString("0.00")}%)");
			}
			else
			{
				WriteLog(mediaTester, $"FAILED {(isQuickTest ? "quick test " : string.Empty)}block {absoluteDataBlockIndex.ToString("#,##0")}! Byte index: {absoluteDataByteIndex.ToString("#,##0")} / {mediaTester.Options.MaxBytesToTest.ToString("#,##0")}. {(isQuickTest ? string.Empty : readBytesPerSecond.ToString("#,##0") + "B/sec ")}({mediaTester.ProgressPercent.ToString("0.00")}%)");
			}
		}

		private static void AfterQuickTest(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePathlong, long readBytesPerSecond, int bytesVerified, int bytesFailed)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePathlong, readBytesPerSecond, bytesVerified, bytesFailed, true);
		}

		private static void OnMediaTesterException(MediaTester mediaTester, Exception exception)
		{
			WriteLog(mediaTester, $"{exception.Message}");
			if (exception.InnerException != null)
			{
				OnMediaTesterException(mediaTester, exception.InnerException);
			}
		}

		private static void WriteLog(MediaTester mediaTester, string message)
		{
			string finalMessage = (mediaTester.IsSuccess ? "No errors" : "FAILURES!") + " - " + message;
			Console.WriteLine(finalMessage);
		}
	}
}
