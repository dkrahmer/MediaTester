using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTesterLib
{
	public class Options
	{
		public bool StopProcessingOnFailure { get; set; } = true;
		public bool QuickTestAfterEachFile { get; set; } = true;
		public string TestDirectory { get; set; }
		public bool QuickFirstFailingByteMethod { get; set; } = true;
		public bool RemoveTempDataFilesUponSuccess { get; set; } = true;	
		public long MaxBytesToTest { get; set; } = -1;

		public const string CONFIG_FILENAME = "MediaTesterOptions.json";

		public void Serialize(string filePath = CONFIG_FILENAME)
		{
			File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
		}

		static public Options Deserialize(string filePath = CONFIG_FILENAME)
		{
			Options options;
			try
			{
				options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(filePath));
			}
			catch (Exception ex)
			{
				// If anything goes wrong just create a default object
				options = new Options();
			}

			return options;
		}
	}
}
