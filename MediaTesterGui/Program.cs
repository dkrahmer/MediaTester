using KrahmerSoft.MediaTesterLib;
using System;
using System.Threading;
using System.Windows.Forms;

namespace KrahmerSoft.MediaTesterGui
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Options mediaTesterOptions = null;
			try
			{
				mediaTesterOptions = Options.Deserialize();
			}
			catch
			{
				MessageBox.Show(Strings.ErrorLoadingOptionsDetails, Strings.ErrorLoadingOptions, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool[] restartAfterClose = new bool[] { false };
			do
			{
				string language = mediaTesterOptions.LanguageCode;

				try
				{
					language = mediaTesterOptions.LanguageCode;
					if (string.IsNullOrEmpty(language))
						language = Thread.CurrentThread.CurrentCulture.Name;

					if (language?.Length > 2)
						language = language.Substring(0, 2);

					if (Thread.CurrentThread.CurrentCulture.Name != language)
					{
						Thread.CurrentThread.CurrentCulture =
							Thread.CurrentThread.CurrentUICulture =
							new System.Globalization.CultureInfo(language);
					}
				}
				catch
				{
					string invalidLanguageDetails = Strings.InvalidLanguageDetails
						.Replace("{InvalidLanguageName}", language)
						.Replace("{DefaultLanguageName}", Thread.CurrentThread.CurrentCulture.Name);
					MessageBox.Show(invalidLanguageDetails, Strings.InvalidLanguage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

				MessageBoxManager.OK = Strings.ButtonOk;
				MessageBoxManager.Yes = Strings.ButtonYes;
				MessageBoxManager.No = Strings.ButtonNo;
				MessageBoxManager.Register();

				restartAfterClose[0] = false;
				Application.Run(new Main(mediaTesterOptions, restartAfterClose));
			} while (restartAfterClose[0]);
		}
	}
}
