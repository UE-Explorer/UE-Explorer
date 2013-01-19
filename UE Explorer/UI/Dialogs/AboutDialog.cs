using System;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Eliot.Utilities.Net;
using UEExplorer.Properties;

namespace UEExplorer.UI.Dialogs
{
	public partial class AboutDialog : Form
	{
		public AboutDialog()
		{	
			InitializeComponent();
			Text += " " + Application.ProductName;
		}	

		private void AboutForm_Load( object sender, EventArgs e )
		{
			label4.Text = Application.ProductName;
		 	VersionLabel.Text = String.Format( Resources.Version, ProgramForm.Version );
			CopyrightLabel.Text = AssemblyCopyright;
			LinkLabel.Text = Program.WEBSITE_URL;		
		}

		private void AboutForm_Shown( object sender, EventArgs e )
		{
			InitializeDonators();
		}

		private const string DONATORS_URL = "http://eliotvu.com/files/donators.txt"; 
		private void InitializeDonators()
		{
			Refresh();

			//var web = new WebClient();
			//web.DownloadFileAsync( )

			using( var buffer = new StreamReader( WebRequest.Create( DONATORS_URL ).Get() ) )
			{ 
				buffer.BaseStream.Position = 0;
				DonatorsSet.ReadXml( buffer );
			}
			DonatorsGrid.AutoGenerateColumns = true;
			DonatorsGrid.DataSource = DonatorsSet;
			DonatorsGrid.DataMember = "Donators";

			DonateLink.Text = Program.Donate_URL;		
		}

		private string AssemblyCopyright
		{
			get
			{
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false );
				return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		private void LicenseLink_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "file:///" + Path.Combine( Application.StartupPath, "license.html" ) );
		}
	}
}
