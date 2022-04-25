using System.IO;
using System.Text.Json;

namespace KrahmerSoft.MediaTesterLib
{
	public class Options
	{
		public bool StopProcessingOnFailure { get; set; } = true;
		public bool QuickTestAfterEachFile { get; set; } = true;
		public string TestDirectory { get; set; }
		public bool QuickFirstFailingByteMethod { get; set; } = true;
		public bool RemoveTempDataFilesUponCompletion { get; set; } = true;
		public bool SaveTestResultsFileToMedia { get; set; } = true;
		public long MaxBytesToTest { get; set; } = -1;
		public string LanguageCode { get; set; }

		public const string CONFIG_FILENAME = "MediaTesterOptions.json";

		public void Serialize(string filePath = CONFIG_FILENAME)
		{
			File.WriteAllText(filePath, JsonSerializer.Serialize(this));
		}

		public static Options Deserialize(string filePath = CONFIG_FILENAME)
		{
			Options options;
			try
			{
				options = JsonSerializer.Deserialize<Options>(File.ReadAllText(filePath));
			}
			catch //(Exception ex)
			{
				// If anything goes wrong just create a default object
				options = new Options();
			}

			return options;
		}
	}
}
