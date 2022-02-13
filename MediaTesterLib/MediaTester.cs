using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace KrahmerSoft.MediaTesterLib
{
	public readonly record struct WrittenBlock(long AbsoluteDataBlockIndex, long AbsoluteDataByteIndex, string TestFilePath, long WriteBytesPerSecond, int BytesWritten, int BytesFailedWrite);
	public readonly record struct VerifiedBlock(long AbsoluteDataBlockIndex, long AbsoluteDataByteIndex, string TestFilePath, long VerifyBytesPerSecond, int BytesVerified, int BytesFailed);

	public class VerifiedBlockEventArgs : EventArgs
	{
		public VerifiedBlockEventArgs(VerifiedBlock block)
		{
			AbsoluteDataBlockIndex = block.AbsoluteDataBlockIndex;
			AbsoluteDataByteIndex = block.AbsoluteDataByteIndex;
			TestFilePath = block.TestFilePath;
			VerifyBytesPerSecond = block.VerifyBytesPerSecond;
			BytesVerified = block.BytesVerified;
			BytesFailed = block.BytesFailed;
		}

		public long AbsoluteDataBlockIndex;
		public long AbsoluteDataByteIndex;
		public string TestFilePath;
		public long VerifyBytesPerSecond;
		public int BytesVerified;
		public int BytesFailed;
	}

	public class WritedBlockEventArgs : EventArgs
	{
		public WritedBlockEventArgs(WrittenBlock block)
		{
			AbsoluteDataBlockIndex = block.AbsoluteDataBlockIndex;
			AbsoluteDataByteIndex = block.AbsoluteDataByteIndex;
			TestFilePath = block.TestFilePath;
			WriteBytesPerSecond = block.WriteBytesPerSecond;
			BytesWritten = block.BytesWritten;
			BytesFailedWrite = block.BytesFailedWrite;
		}

		public long AbsoluteDataBlockIndex;
		public long AbsoluteDataByteIndex;
		public string TestFilePath;
		public long WriteBytesPerSecond;
		public int BytesWritten;
		public int BytesFailedWrite;
	}

	public class ExceptionEventArgs : EventArgs
	{
		public ExceptionEventArgs(Exception e)
		{
			Exception = e;
		}

		public Exception Exception;
	}

	public class MediaTester
	{
		/// <summary>
		/// Specify the size of each block, in bytes.
		/// </summary>
		public const int DATA_BLOCK_SIZE = 8 * 1024 * 1024;

		/// <summary>
		/// Specify the number of blocks per files.
		/// </summary>
		public const int DATA_BLOCKS_PER_FILE = 128;

		/// <summary>
		/// The total size of a test file, in bytes.
		/// </summary>
		public const int FILE_SIZE = DATA_BLOCK_SIZE * DATA_BLOCKS_PER_FILE;

		public const string TempSubDirectoryName = "MediaTester";

		public event EventHandler<VerifiedBlockEventArgs> QuickTestCompleted;

		public event EventHandler<VerifiedBlockEventArgs> BlockVerified;

		public event EventHandler<WritedBlockEventArgs> BlockWritten;

		public event EventHandler<ExceptionEventArgs> ExceptionThrown;

		private const FileOptions FileFlagNoBuffering = (FileOptions) 0x20000000;

		/// <summary>
		/// Say if write+verify or only-verify mode
		/// </summary>
		private bool _isBatchMode = false;

		private Options _options;
		private decimal _averageVerifyBytesPerSecond;
		private long _totalVerifySpeedSamples;
		public int _totalTargetFiles = 0;

		public Options Options
		{
			get
			{
				if (_options == null)
				{
					_options = new Options();
				}
				return _options;
			}
			set
			{
				_options = value;
			}
		}

		/// <summary>
		/// Store the result of the test.
		/// </summary>
		public bool IsSuccess { get; protected set; } = true;

		public decimal ProgressPercent { get; protected set; }
		public long TotalBytesWritten { get; protected set; }
		public long TotalBytesVerified { get; protected set; }
		public long TotalBytesFailed { get; protected set; }
		public long TotalGeneratedTestFileBytes { get; protected set; }
		public decimal FirstFailingByteIndex { get; protected set; } = -1;

		// Number of bytes to write or read. It is used to compute the progress percentage and stats.
		public long TotalTargetBytes { get; protected set; } = 0;

		/// <summary>
		/// Initialize MediaTester with user-defined options.
		/// </summary>
		/// <param name="options"></param>
		public MediaTester(Options options)
		{
			Options = options;
		}

		/// <summary>
		/// Initialize MediaTester to perform the test on the given path with default options.
		/// </summary>
		/// <param name="testDirectory"></param>
		public MediaTester(string testDirectory)
		{
			Options.TestDirectory = testDirectory;
		}

		/// <summary>
		/// Start a complete test.
		/// </summary>
		/// <returns></returns>
		public bool FullTest()
		{
			bool success;
			_isBatchMode = true;

			if (_options.MaxBytesToTest == -1)
			{
				TotalTargetBytes = GetAvailableBytes();
			}
			else
			{
				TotalTargetBytes = _options.MaxBytesToTest;
			}

			success = GenerateTestFiles();
			if (Options.StopProcessingOnFailure && !success && Options.QuickFirstFailingByteMethod)
			{
				return false;
			}

			success = VerifyTestFiles();
			if (Options.StopProcessingOnFailure && !success)
			{
				return false;
			}

			if (TotalGeneratedTestFileBytes != TotalBytesVerified)
			{
				success = false;
				IsSuccess &= false;
				OnExceptionThrown(new Exception($"Total bytes verified does not match total bytes written. Total Bytes Written: {TotalGeneratedTestFileBytes.ToString("#,##0")} ; Total Bytes Verified: {TotalBytesVerified.ToString("#,##0")}"));
			}
			if (Options.StopProcessingOnFailure && !success)
			{
				return false;
			}

			_isBatchMode = false;
			return IsSuccess;
		}

		/// <summary>
		/// Verify all the test files.
		/// </summary>
		/// <returns></returns>
		public bool VerifyTestFiles()
		{
			TotalBytesVerified = 0;
			TotalBytesFailed = 0;

			if (_options.MaxBytesToTest == -1)
			{
				TotalTargetBytes = GetAvailableBytes();
			}
			else
			{
				TotalTargetBytes = _options.MaxBytesToTest;
			}

			if (!_isBatchMode)
				IsSuccess = true;

			bool allFilesSuccess = true;

			for (int testFileIndex = 0; ; testFileIndex++)
			{
				string testFilePath = GetTestFilePath(testFileIndex);
				if (!File.Exists(testFilePath))
					break;

				bool success = VerifyTestFile(testFileIndex, testFilePath, out _, out _, true);
				allFilesSuccess &= success;
				IsSuccess &= success;

				if (Options.StopProcessingOnFailure && !success)
				{
					return success;
				}

				if (TotalBytesVerified + TotalBytesFailed >= TotalTargetBytes)
				{
					// The requested number of bytes has been verified
					return success;
				}
			}

			return allFilesSuccess;
		}

		/// <summary>
		/// Remove temporary files in reverse index order.
		/// </summary>
		/// <param name="filesToRemove">The number of files to delete.</param>
		/// <returns>The number of deleted files.</returns>
		public int RemoveTempDataFiles(int filesToRemove = int.MaxValue)
		{
			// Find the last file index that exists
			int fileCount = 0;
			while (true)
			{
				string testFilePath = GetTestFilePath(fileCount);
				if (!File.Exists(testFilePath))
					break;

				fileCount++;
			}

			// Delete files in reverse order
			int filesRemoved = 0;
			string testDirectory = GetTestDirectory();
			for (int testFileIndex = fileCount - 1; testFileIndex >= 0; testFileIndex--)
			{
				if (filesToRemove <= 0)
					break;

				string testFilePath = GetTestFilePath(testFileIndex);

				File.Delete(testFilePath);
				filesToRemove--;
				filesRemoved++;
			}

			if (Directory.Exists(testDirectory)
				&& Directory.EnumerateFiles(testDirectory).FirstOrDefault() == null
				&& Directory.EnumerateDirectories(testDirectory).FirstOrDefault() == null)
			{
				// Delete empty directory
				Directory.Delete(testDirectory);
			}

			return filesRemoved;
		}

		/// <summary>
		/// Get the target directory where test files are. This path always ends with the "TempSubDirectoryName".
		/// </summary>
		/// <returns></returns>
		public string GetTestDirectory()
		{
			string testDirectory = Options.TestDirectory;
			if (testDirectory.EndsWith(":"))
			{
				testDirectory += "\\";
			}

			if (!testDirectory.TrimEnd('\\').EndsWith(TempSubDirectoryName))
			{
				testDirectory = Path.Combine(testDirectory, TempSubDirectoryName);
			}

			return testDirectory;
		}

		/// <summary>
		/// Get available space in a drive.
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="totalSize">Total capacity of drive.</param>
		/// <param name="actual">If true get the concrete free space, otherwise get the current free space summing up test files (if present).</param>
		/// <returns></returns>
		public static long GetAvailableBytes(string directory, out long totalSize, bool actual = false)
		{
			// TODO catch exception if disk is removed (System.IO.DriveNotFoundException)
			DriveInfo driveInfo = new(directory);
			totalSize = driveInfo.TotalSize;
			long freeSpace = driveInfo.AvailableFreeSpace;
			if (actual)
				return freeSpace;

			freeSpace += GetTestFilesSize(directory);

			return freeSpace;
		}

		/// <summary>
		/// Get free bytes on disk.
		/// </summary>
		/// <param name="actual">if true return the concrete free space, otherwise return the concrete space and ignore test files.</param>
		/// <returns></returns>
		private long GetAvailableBytes(bool actual = false)
		{
			return GetAvailableBytes(GetTestDirectory(), out _, actual);
		}

		/// <summary>
		/// Get the total size of test files in the given directory.
		/// </summary>
		/// <param name="directory">The directory where the test files are.</param>
		/// <returns></returns>
		private static long GetTestFilesSize(string directory)
		{
			int skippedFiles = 0;

			long size = 0;

			// Add the space taken by existing TestMedia files
			for (int testFileIndex = 0; skippedFiles < 100; testFileIndex++)
			{
				string testFilePath = GetTestFilePath(directory, testFileIndex);
				if (!File.Exists(testFilePath))
				{
					skippedFiles++;
					continue;
				}
				var fileInfo = new FileInfo(testFilePath);

				size += fileInfo.Length;
			}

			return size;
		}

		/// <summary>
		/// Return the highest relative block ID.
		/// </summary>
		/// <param name="testFileSize"></param>
		/// <returns></returns>
		private static int GetLastDataBlockIndex(long testFileSize)
		{
			return (int) ((testFileSize + DATA_BLOCK_SIZE - 1) / DATA_BLOCK_SIZE) - 1;
		}

		/// <summary>
		/// Return the highest file ID.
		/// </summary>
		/// <param name="testFileSize">The number of bytes to test. If negative, consider all the free space (including past test file).</param>
		/// <returns></returns>
		private int GetLastTestFileIndex(long testFileSize)
		{
			if (testFileSize < 0)
			{
				testFileSize = GetAvailableBytes();
			}
			return (int) ((testFileSize + FILE_SIZE - 1) / FILE_SIZE) - 1;
		}

		/// <summary>
		/// Creates and writes all the test files.
		/// </summary>
		/// <returns></returns>
		private bool GenerateTestFiles()
		{
			IsSuccess = true;
			TotalGeneratedTestFileBytes = 0;
			try
			{
				int lastFileIndex = GetLastTestFileIndex(TotalTargetBytes);
				int _totalFileNumber = lastFileIndex + 1;

				for (int testFileIndex = 0; testFileIndex <= lastFileIndex; testFileIndex++)
				{
					long desideredBytes = ComputeDesideredTestFileSize(testFileIndex, _totalFileNumber, TotalTargetBytes);
					string testFilePath = GenerateTestFile(testFileIndex, desideredBytes, out long actualTestFileSize);
					TotalGeneratedTestFileBytes += actualTestFileSize;
					if (Options.QuickTestAfterEachFile && testFilePath != null)
					{
						int checkIndex = 0;
						long absoluteDataBlockIndex = GetAbsoluteDataBlockIndex(testFileIndex, checkIndex);
						long absoluteDataByteIndex = GetAbsoluteDataByteIndex(testFileIndex, checkIndex);
						SetProgressPercent(absoluteDataByteIndex + DATA_BLOCK_SIZE, 1);
						bool success = VerifyTestFileDataBlock(testFileIndex, testFilePath, checkIndex, out int bytesVerified, out int bytesFailed, out long readBytesPerSecond);
						IsSuccess &= success;
						OnQuickTestCompleted(new VerifiedBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed));
						if (bytesFailed > 0 && Options.QuickFirstFailingByteMethod)
						{
							success = false;
							VerifyTestFile(testFileIndex, testFilePath, out bytesVerified, out bytesFailed);
						}

						if (success)
						{
							checkIndex = GetLastDataBlockIndex(actualTestFileSize);
							absoluteDataBlockIndex = GetAbsoluteDataBlockIndex(testFileIndex, checkIndex);
							absoluteDataByteIndex = GetAbsoluteDataByteIndex(testFileIndex, checkIndex);
							SetProgressPercent(absoluteDataByteIndex + DATA_BLOCK_SIZE, 1);
							success = VerifyTestFileDataBlock(testFileIndex, testFilePath, checkIndex, out bytesVerified, out bytesFailed, out readBytesPerSecond);
							IsSuccess &= success;
							OnQuickTestCompleted(new VerifiedBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, readBytesPerSecond, bytesVerified, bytesFailed));
							if (bytesFailed > 0 && Options.QuickFirstFailingByteMethod)
							{
								success = false;
								VerifyTestFile(testFileIndex, testFilePath, out bytesVerified, out bytesFailed);
							}
						}

						if (Options.StopProcessingOnFailure && !success)
							return success;
					}
				}

				return IsSuccess;
			}
			catch (Exception ex)
			{
				IsSuccess = false;
				OnExceptionThrown(new Exception("An unhandled exception occurred while writing test data.", ex));
				throw;
			}
		}

		/// <summary>
		/// Get the number of byte to write for the current file.
		/// </summary>
		/// <param name="fileIndex"></param>
		/// <param name="totalFileNumber"></param>
		/// <param name="totalBytesToTest"></param>
		/// <returns></returns>
		private long ComputeDesideredTestFileSize(int fileIndex, int totalFileNumber, long totalBytesToTest)
		{
			if (fileIndex < GetLastTestFileIndex(totalBytesToTest))
			{
				return FILE_SIZE;
			}
			return totalBytesToTest - (totalFileNumber - 1) * FILE_SIZE;
		}

		/// <summary>
		/// Create a file and write it. If the file is already on disk, leave it.
		/// </summary>
		/// <param name="testFileIndex"></param>
		/// <param name="desideredTestFileSize">The desired size of the test file.</param>
		/// <param name="actualTestFileSize"></param>
		/// <returns>The absolute path to test file.</returns>
		private string GenerateTestFile(int testFileIndex, long desideredTestFileSize, out long actualTestFileSize)
		{
			long freeSpace = GetAvailableBytes(true);
			actualTestFileSize = desideredTestFileSize;
			if (actualTestFileSize > freeSpace)
			{
				actualTestFileSize = (int) freeSpace;
			}

			string testFilePath = GetTestFilePath(testFileIndex);
			Directory.CreateDirectory(GetTestDirectory());

			// If the file already exist, try to reuse it
			if (File.Exists(testFilePath))
			{
				var fileInfo = new FileInfo(testFilePath);
				if (fileInfo.Length <= FILE_SIZE)
				{
					if (fileInfo.Length == FILE_SIZE || actualTestFileSize == 0)
					{
						// File already exists. Leaving in place.
						actualTestFileSize = (int) fileInfo.Length;
						TotalBytesWritten += fileInfo.Length;
						return testFilePath;
					}

					if (actualTestFileSize <= fileInfo.Length)
					{
						// File already exists but needs to be bigger.
						if (freeSpace < FILE_SIZE)
						{
							actualTestFileSize += (int) freeSpace;
							if (actualTestFileSize > FILE_SIZE)
							{
								actualTestFileSize = FILE_SIZE;
							}
						}
						if (actualTestFileSize > FILE_SIZE || freeSpace > FILE_SIZE)
						{
							actualTestFileSize = FILE_SIZE;
						}
					}
				}
			}

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			try
			{
				// Create file and write
				//https://docs.microsoft.com/en-us/windows/win32/api/fileapi/ns-fileapi-createfile2_extended_parameters
				// Known current issues in .NET 6:
				// - https://github.com/dotnet/runtime/issues/62851
				// - https://github.com/dotnet/runtime/issues/27408
				using var file = new FileStream(testFilePath, FileMode.Create, FileAccess.Write, FileShare.Read,
												DATA_BLOCK_SIZE, FileFlagNoBuffering | FileOptions.WriteThrough);
				using var fileWriter = new BinaryWriter(file);

				// Check if the free space changed after creating the file since
				// adding a directory or file to the FAT can decrease available space
				freeSpace = GetAvailableBytes(true);
				if (actualTestFileSize > freeSpace)
				{
					actualTestFileSize = (int) freeSpace;
				}

				long lastWriteBytesPerSecond = 0;
				int lastDataBlockIndex = GetLastDataBlockIndex(actualTestFileSize);
				double lastTimestamp = 0;
				for (int dataBlockIndex = 0; dataBlockIndex <= lastDataBlockIndex; dataBlockIndex++)
				{
					int dataBlockSize = (dataBlockIndex == lastDataBlockIndex && actualTestFileSize % DATA_BLOCK_SIZE != 0)
						? (int) (actualTestFileSize % DATA_BLOCK_SIZE) : DATA_BLOCK_SIZE;
					long absoluteDataBlockIndex = GetAbsoluteDataBlockIndex(testFileIndex, dataBlockIndex);
					long absoluteDataByteIndex = GetAbsoluteDataByteIndex(testFileIndex, dataBlockIndex);

					try
					{
						var dataBlock = GenerateDataBlock(testFileIndex, dataBlockIndex, dataBlockSize);

						fileWriter.Write(dataBlock);
						//fileWriter.Flush(); // Force the data to finish writing to the device
						//file.Flush(true);   // Clear all intermediate file buffers (OS I/O cache, etc)

						long writeBytesPerSecond;

						if (dataBlockSize == DATA_BLOCK_SIZE)
						{
							double now = stopwatch.Elapsed.TotalSeconds;
							double time = now - lastTimestamp;
							lastTimestamp = now;
							writeBytesPerSecond = (long) (dataBlockSize / time);
							lastWriteBytesPerSecond = writeBytesPerSecond;
						}
						else
						{
							// prevent an artificial rate spike on the last block
							writeBytesPerSecond = lastWriteBytesPerSecond;
						}

						TotalBytesWritten += dataBlockSize;
						SetProgressPercent(TotalBytesWritten, 1);
						OnBlockWritten(new WrittenBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, writeBytesPerSecond, dataBlockSize, 0));
					}
					catch (Exception ex)
					{
						IsSuccess = false;
						OnBlockWritten(new WrittenBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, 0, 0, dataBlockSize));
						OnExceptionThrown(new Exception($"Unable to write block to file '{testFilePath}'.", ex));
						throw;
					}

					// yields once in a while to allow thread interruption
					Thread.Sleep(0);
				}
			}
			catch (Exception ex)
			{
				testFilePath = null;
				IsSuccess = false;
				OnExceptionThrown(new Exception($"Unable to open file '{testFilePath}' for writing.", ex));
				throw;
			}

			return testFilePath;
		}

		/// <summary>
		/// Verify a test file. TODO this function can be split and simplified.
		/// </summary>
		/// <param name="testFileIndex"></param>
		/// <param name="testFilePath"></param>
		/// <param name="bytesVerified"></param>
		/// <param name="bytesFailed"></param>
		/// <param name="updateTotalBytes"></param>
		/// <returns></returns>
		private bool VerifyTestFile(int testFileIndex, string testFilePath, out int bytesVerified, out int bytesFailed, bool updateTotalBytes = false)
		{
			bool success = true;
			bytesVerified = 0;
			bytesFailed = 0;

			try
			{
				using var fileReader = new FileStream(testFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, DATA_BLOCK_SIZE,
					FileFlagNoBuffering | FileOptions.SequentialScan);
				int lastDataBlockIndex = GetLastDataBlockIndex((int) fileReader.Length);
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				double lastElapsedSeconds = 0;
				for (int dataBlockIndex = 0; dataBlockIndex <= lastDataBlockIndex; dataBlockIndex++)
				{
					long absoluteDataBlockIndex = GetAbsoluteDataBlockIndex(testFileIndex, dataBlockIndex);
					long absoluteDataByteIndex = GetAbsoluteDataByteIndex(testFileIndex, dataBlockIndex);
					try
					{
						bool blockSuccess = VerifyTestFileDataBlock(fileReader, testFileIndex, dataBlockIndex, out int blockBytesVerified, out int blockBytesFailed, out _);
						success &= blockSuccess;
						IsSuccess &= success;
						bytesVerified += blockBytesVerified;
						bytesFailed += blockBytesFailed;
						if (updateTotalBytes)
						{
							TotalBytesVerified += blockBytesVerified;
							TotalBytesFailed += blockBytesFailed;
						}

						int dataBlockSize = blockBytesVerified + blockBytesFailed;
						SetProgressPercent(absoluteDataByteIndex + dataBlockSize, 2);

						double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
						long verifiedBytesPerSecond = (long) (dataBlockSize / (elapsedSeconds - lastElapsedSeconds));
						lastElapsedSeconds = elapsedSeconds;

						OnBlockVerified(new VerifiedBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, verifiedBytesPerSecond, blockBytesVerified, blockBytesFailed));
					}
					catch
					{
						success = false;
						IsSuccess &= success;
						long exceptionBlockBytesFailed = fileReader.Length - (dataBlockIndex * DATA_BLOCK_SIZE);
						if (exceptionBlockBytesFailed > DATA_BLOCK_SIZE)
							exceptionBlockBytesFailed = DATA_BLOCK_SIZE;

						OnBlockVerified(new VerifiedBlock(absoluteDataBlockIndex, absoluteDataByteIndex, testFilePath, 0, 0, (int) exceptionBlockBytesFailed));
						throw;
					}

					if (Options.StopProcessingOnFailure && !success)
						return success;

					if (updateTotalBytes)
					{
						if (TotalBytesVerified + TotalBytesFailed >= TotalTargetBytes)
						{
							// The requested number of bytes has been verified
							return success;
						}
					}
				}
			}
			catch (Exception ex)
			{
				OnExceptionThrown(new Exception($"Unable to open '{testFilePath}' for reading.", ex));
				success = false;
				IsSuccess &= success;
				throw;
			}

			return success;
		}

		private const int BatchPhaseCount = 2;

		/// <summary>
		/// Calculate and set the progress percentage, considering the batch id.
		/// </summary>
		/// <param name="percent">Actual percentage of current batch.</param>
		/// <param name="batchPhaseNumber">Current batch number.</param>
		private void SetProgressPercent(long totalBytes, int batchPhaseNumber)
		{
			decimal percent = 100M * totalBytes / TotalTargetBytes;

			if (_isBatchMode)
			{
				percent = percent / BatchPhaseCount + (100M * (batchPhaseNumber - 1) / BatchPhaseCount);
			}

			if (percent < 0)
				percent = 0;
			else if (percent > 100)
				percent = 100;

			ProgressPercent = percent;
		}

		/// <summary>
		/// Return the total size of test files on disk.
		/// </summary>
		/// <returns></returns>
		private long GetTotalDataFileBytes()
		{
			long totalBytes = 0;
			for (int testFileIndex = 0; ; testFileIndex++)
			{
				string testFilePath = GetTestFilePath(testFileIndex);
				if (!File.Exists(testFilePath))
					break;

				FileInfo fileInfo = new(testFilePath);
				totalBytes += fileInfo.Length;
			}

			return totalBytes;
		}

		/// <summary>
		/// Verify a data block.
		/// </summary>
		/// <param name="testFileIndex"></param>
		/// <param name="testFilePath"></param>
		/// <param name="dataBlockIndex"></param>
		/// <param name="bytesVerified"></param>
		/// <param name="bytesFailed"></param>
		/// <param name="readBytesPerSecond"></param>
		/// <returns></returns>
		private bool VerifyTestFileDataBlock(int testFileIndex, string testFilePath, int dataBlockIndex, out int bytesVerified, out int bytesFailed, out long readBytesPerSecond)
		{
			using var file = File.OpenRead(testFilePath);
			return VerifyTestFileDataBlock(file, testFileIndex, dataBlockIndex, out bytesVerified, out bytesFailed, out readBytesPerSecond);
		}

		/// <summary>
		/// Verify a data block.
		/// </summary>
		/// <param name="fileReader"></param>
		/// <param name="fileIndex"></param>
		/// <param name="dataBlockIndex"></param>
		/// <param name="bytesVerified"></param>
		/// <param name="bytesFailed"></param>
		/// <param name="readBytesPerSecond"></param>
		/// <returns></returns>
		private bool VerifyTestFileDataBlock(FileStream fileReader, int fileIndex, int dataBlockIndex, out int bytesVerified, out int bytesFailed, out long readBytesPerSecond)
		{
			byte[] knownGoodDataBlock = null;
			var thread = new Thread(() =>
			{
				knownGoodDataBlock = GenerateDataBlock(fileIndex, dataBlockIndex, DATA_BLOCK_SIZE);
			})
			{
				IsBackground = true
			};
			thread.Start();

			var dataBlock = ReadDataBlock(fileReader, dataBlockIndex, out readBytesPerSecond);

			thread.Join();

			// yields once in a while to allow thread interruption
			Thread.Sleep(0);

			return VerifyDataBlock(dataBlock, fileIndex, dataBlockIndex, out bytesVerified, out bytesFailed, knownGoodDataBlock: knownGoodDataBlock);
		}

		/// <summary>
		/// Read the block from file. The length of this block depends on the remaining bytes in the files.
		/// </summary>
		/// <param name="fileReader"></param>
		/// <param name="dataBlockIndex">Relative block ID.</param>
		/// <param name="readBytesPerSecond"></param>
		/// <returns></returns>
		private static byte[] ReadDataBlock(FileStream fileReader, int dataBlockIndex, out long readBytesPerSecond)
		{
			int dataBlockStartIndex = dataBlockIndex * DATA_BLOCK_SIZE;
			long dataBlockSize = fileReader.Length - dataBlockStartIndex;
			if (dataBlockSize > DATA_BLOCK_SIZE)
				dataBlockSize = DATA_BLOCK_SIZE;

			var dataBlock = new byte[(int) dataBlockSize];

			if (fileReader.Position != dataBlockStartIndex)
				fileReader.Seek(dataBlockStartIndex, SeekOrigin.Begin);

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			int readBytes = fileReader.Read(dataBlock, 0, dataBlock.Length);
			stopwatch.Stop();
			readBytesPerSecond = (long) ((double) readBytes / stopwatch.Elapsed.TotalSeconds);

			if (readBytes != dataBlock.Length)
			{
				Array.Resize(ref dataBlock, readBytes);
			}

			return dataBlock;
		}

		/// <summary>
		/// Verify 2 blocks against each other. If the reference block length is different (longer) from the test block, it is regenerated to match the lenght of test block. TODO refactor this piece of code.
		/// </summary>
		/// <param name="dataBlock">The block to be tested.</param>
		/// <param name="fileIndex"></param>
		/// <param name="dataBlockIndex"></param>
		/// <param name="bytesVerified"></param>
		/// <param name="bytesFailed"></param>
		/// <param name="knownGoodDataBlock">The reference block.</param>
		/// <returns></returns>
		private bool VerifyDataBlock(byte[] dataBlock, int fileIndex, int dataBlockIndex, out int bytesVerified, out int bytesFailed, byte[] knownGoodDataBlock = null)
		{
			bytesVerified = 0;
			bytesFailed = 0;
			if (knownGoodDataBlock == null || knownGoodDataBlock.Length != dataBlock.Length)
				knownGoodDataBlock = GenerateDataBlock(fileIndex, dataBlockIndex, dataBlock.Length);

			if (dataBlock.Length > knownGoodDataBlock.Length)
			{
				bytesFailed = dataBlock.Length;
				SetFirstFailingByteIndex(GetAbsoluteDataByteIndex(fileIndex, dataBlockIndex));
				return false;
			}

			// Now dataBlock and knownGoodDataBlock have the same length.
			for (int i = 0; i < dataBlock.Length; i++)
			{
				if (dataBlock[i] == knownGoodDataBlock[i])
				{
					bytesVerified++;
				}
				else
				{
					bytesFailed++;
					SetFirstFailingByteIndex(GetAbsoluteDataByteIndex(fileIndex, dataBlockIndex) + i);
				}
			}

			return bytesFailed == 0;
		}

		/// <summary>
		/// Set FirstFailingByteIndex. TODO: This can be inserted directly in Property FirstFailingByteIndex
		/// </summary>
		/// <param name="failingByteIndex"></param>
		private void SetFirstFailingByteIndex(long failingByteIndex)
		{
			if (FirstFailingByteIndex > failingByteIndex || FirstFailingByteIndex < 0)
			{
				FirstFailingByteIndex = failingByteIndex;
			}
		}

		/// <summary>
		/// Get the absolute file path given its index.
		/// </summary>
		/// <param name="fileIndex"></param>
		/// <returns></returns>
		private string GetTestFilePath(int fileIndex)
		{
			return GetTestFilePath(GetTestDirectory(), fileIndex);
		}

		/// <summary>
		/// Get the absolute directory of a test file.
		/// </summary>
		/// <param name="testDirectory">The target directory of test files.</param>
		/// <param name="fileIndex">The file index, from 0 to n.</param>
		/// <returns>The full path to test file, the file name index is increased by one than "fileIndex".</returns>
		private static string GetTestFilePath(string testDirectory, int fileIndex)
		{
			return Path.Combine(testDirectory, (fileIndex + 1).ToString("D8") + ".MediaTester");
		}

		/// <summary>
		/// Get the global block ID given the actual file and the relative block ID.
		/// </summary>
		/// <param name="fileIndex">Test file index (count starts from 0).</param>
		/// <param name="fileDataBlockIndex"></param>
		/// <returns></returns>
		private static long GetAbsoluteDataBlockIndex(int fileIndex, int fileDataBlockIndex)
		{
			return ((long) fileIndex * (long) DATA_BLOCKS_PER_FILE) + (long) fileDataBlockIndex;
		}

		/// <summary>
		/// Get the absolute byte for the current block
		/// </summary>
		/// <param name="fileIndex">Test file index (count starts from 0).</param>
		/// <param name="fileDataBlockIndex">Relative block ID.</param>
		/// <returns></returns>
		private static long GetAbsoluteDataByteIndex(int fileIndex, int fileDataBlockIndex)
		{
			return GetAbsoluteDataBlockIndex(fileIndex, fileDataBlockIndex) * DATA_BLOCK_SIZE;
		}

		/// <summary>
		/// Create a block of specified length initialized with "pseudo-random" data depending only from absolute block ID.
		/// </summary>
		/// <param name="fileIndex">File index (count from 0).</param>
		/// <param name="fileDataBlockIndex">Relative block ID.</param>
		/// <param name="blockSize">Block size in bytes.</param>
		/// <returns></returns>
		private static byte[] GenerateDataBlock(int fileIndex, int fileDataBlockIndex, int blockSize)
		{
			return GenerateDataBlock(GetAbsoluteDataBlockIndex(fileIndex, fileDataBlockIndex), blockSize);
		}

		/// <summary>
		/// Create a block of specified length initialized with "pseudo-random" data depending only from absolute block ID.
		/// </summary>
		/// <param name="absoluteDataBlockIndex"></param>
		/// <param name="blockSize">Block size in bytes.</param>
		/// <returns></returns>
		private static byte[] GenerateDataBlock(long absoluteDataBlockIndex, int blockSize)
		{
			var dataBlock = new byte[blockSize];
			var rand = new Random(GetDataBlockSeed(absoluteDataBlockIndex));
			rand.NextBytes(dataBlock);
			return dataBlock;
		}

		/// <summary>
		/// Get the seed for the current block.
		/// </summary>
		/// <param name="absoluteDataBlockIndex"></param>
		/// <returns></returns>
		private static int GetDataBlockSeed(long absoluteDataBlockIndex)
		{
			return (int) absoluteDataBlockIndex;
		}

		private void OnExceptionThrown(Exception ex)
		{
			ExceptionThrown?.Invoke(this, new ExceptionEventArgs(ex));
		}

		private void OnBlockWritten(WrittenBlock writtenBlock)
		{
			BlockWritten?.Invoke(this, new WritedBlockEventArgs(writtenBlock));
		}

		private void OnBlockVerified(VerifiedBlock block)
		{
			BlockVerified?.Invoke(this, new VerifiedBlockEventArgs(block));
		}

		private void OnQuickTestCompleted(VerifiedBlock block)
		{
			QuickTestCompleted?.Invoke(this, new VerifiedBlockEventArgs(block));
		}
	}
}