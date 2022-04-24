namespace KrahmerSoft.MediaTesterGui
{
	internal static class Languages
	{
		public static string Default { get; } = "en";

		public static LanguageCode[] Options = new LanguageCode[]
		{
			// Use language ISO 639-1 codes: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
			// The name of the language should be the native spelling (only English should be in English)
			// Alphabetize this list by language name rather than by ISO code
			new LanguageCode("en", "English"),
			//new LanguageCode("es", "Español")
			//new LanguageCode("it", "Italiano")
		};

		public class LanguageCode
		{
			public LanguageCode(string code, string name)
			{
				Code = code;
				Name = name;
			}
			public string Code { get; }
			public string Name { get; }
			public string Description => $"{Code} - {Name}";
		}
	}
}
