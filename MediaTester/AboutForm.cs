using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KrahmerSoft.MediaTester
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void linkLabelRepository_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string url = "https://github.com/dkrahmer/MediaTester";

			// hack because of this: https://github.com/dotnet/corefx/issues/10361
			url = url.Replace("&", "^&");
			Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
		}
	}
}