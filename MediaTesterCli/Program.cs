using KrahmerSoft.MediaTesterLib;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace KrahmerSoft.MediaTesterCli
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string testDirectory = null;
			if (args == null || args.Length < 1)
			{
				while (string.IsNullOrEmpty(testDirectory))
				{
					Console.WriteLine();
					Console.WriteLine($"MediaTester CLI v{Assembly.GetEntryAssembly().GetName().Version} (lib v{Assembly.GetAssembly(typeof(MediaTester)).GetName().Version})");
					Console.WriteLine();
					Console.Write($"{Strings.EnterPathToTest}: ");
					testDirectory = Console.ReadLine();
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					if (testDirectory.IndexOf(@":\") == 1)
					{
						testDirectory = Path.Combine(testDirectory.Substring(0, 1).ToUpper() + @":\", MediaTester.TempSubDirectoryName);
					}
				}
			}
			else
			{
				testDirectory = args[0];
			}

			testDirectory = testDirectory.TrimEnd('\\').TrimEnd('/');
			if (!testDirectory.EndsWith(MediaTester.TempSubDirectoryName))
			{
				testDirectory = Path.Combine(testDirectory, MediaTester.TempSubDirectoryName);
			}

			Console.WriteLine();
			Console.WriteLine($"{Strings.BytesPerFile}: {MediaTester.FILE_SIZE:n0}");
			Console.WriteLine($"{Strings.DataBlockSize}: {MediaTester.DATA_BLOCK_SIZE:n0}");
			Console.WriteLine($"{Strings.BlocksPerFile}: {MediaTester.DATA_BLOCKS_PER_FILE:n0}");
			Console.WriteLine($"{Strings.WritingTempFilesTo}: {testDirectory}...");
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
			Console.WriteLine(Strings.MediaTestSummary);
			Console.WriteLine("------------------------------------------------");
			Console.WriteLine($"{Strings.Result}: {(result ? Strings.Pass : Strings.Fail)}");
			Console.WriteLine($"{Strings.TempFilePath}: {mediaTester.Options.TestDirectory}");
			Console.WriteLine($"{Strings.TotalBytesAttempted}: {mediaTester.Options.MaxBytesToTest:n0}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesVerified > 0)
				Console.WriteLine($"{Strings.VerifiedBytes}: {mediaTester.TotalBytesVerified:n0}");

			if (!mediaTester.Options.StopProcessingOnFailure || mediaTester.TotalBytesFailed > 0)
				Console.WriteLine($"{Strings.FailedBytes}: {mediaTester.TotalBytesFailed:n0}");

			if (mediaTester.FirstFailingByteIndex > 0)
				Console.WriteLine($"{Strings.FirstFailingByteIndex}: {mediaTester.FirstFailingByteIndex:n0}{(mediaTester.Options.QuickFirstFailingByteMethod ? $" ({Strings.QuickMethod})" : string.Empty)}");

			if (!result)
			{
				// Output a big fail block
				Console.WriteLine();
				for (int i = 0; i < 20; i++)
				{
					Console.Write(i % 2 == 0 ? string.Empty : "   ");
					for (int j = 0; j < 15; j++)
					{
						Console.Write($"{Strings.FailExclaimation}  ");
					}
					Console.WriteLine();
				}
			}

			Console.WriteLine();
			Console.WriteLine(Strings.PressEnterToContinue);
			Console.ReadLine();
		}

		private static void AfterWriteBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long writeBytesPerSecond, int bytesWritten, int bytesFailedWrite)
		{
			string byteIndex = Strings.ByteIndexOfTotal.Replace("{DataByteIndex}", $"{absoluteDataByteIndex:n0}").Replace("{MaxBytesToTest}", $"{mediaTester.Options.MaxBytesToTest:n0}");
			if (bytesFailedWrite == 0)
			{
				WriteLog(mediaTester, $"{Strings.SuccessfullyWroteBlock.Replace("{DataBlockIndex}", $"{absoluteDataBlockIndex:n0}")}. {byteIndex} {writeBytesPerSecond:n0} {Strings.BytesPerSecond} ({mediaTester.ProgressPercent:n2}%)");
			}
			else
			{
				WriteLog(mediaTester, $"{Strings.FailedWritingBlock.Replace("{DataBlockIndex}", $"{absoluteDataBlockIndex:n0}")} {byteIndex} ({mediaTester.ProgressPercent:n2}%)");
			}
		}

		private static void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePath, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed, verifyBytesPerSecond, false);
		}

		private static void AfterVerifyBlock(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePathlong, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond, bool isQuickTest = false)
		{
			string message = string.Concat((bytesFailed == 0 ? (isQuickTest ? Strings.VerifiedQuickTestBlock : Strings.VerifiedBlock) : (isQuickTest ? Strings.FailedQuickTestBlock : Strings.FailedBlock))
					.Replace("{DataBlockIndex}", $"{absoluteDataBlockIndex:n0}")
				, " ", Strings.ByteIndexOfTotal.Replace("{DataByteIndex}", $"{absoluteDataByteIndex:n0}").Replace("{MaxBytesToTest}", $"{mediaTester.Options.MaxBytesToTest:n0}")
				, " ", $"{(isQuickTest ? string.Empty : $"{readBytesPerSecond:n0} {Strings.BytesPerSecond} ")}({mediaTester.ProgressPercent:n2}%)");

			WriteLog(mediaTester, message);
		}

		private static void AfterQuickTest(MediaTester mediaTester, long absoluteDataBlockIndex, long absoluteDataByteIndex, string testFilePathlong, long readBytesPerSecond, int bytesVerified, int bytesFailed, long verifyBytesPerSecond)
		{
			AfterVerifyBlock(mediaTester, absoluteDataBlockIndex, absoluteDataByteIndex, testFilePathlong, readBytesPerSecond, bytesVerified, bytesFailed, verifyBytesPerSecond, true);
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
			string finalMessage = (mediaTester.IsSuccess ? Strings.NoErrors : Strings.FailuresExclaimation) + " - " + message;
			Console.WriteLine(finalMessage);
		}
	}
}
