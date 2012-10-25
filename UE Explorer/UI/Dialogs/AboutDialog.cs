using System;
using System.Reflection;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{	
			InitializeComponent();
			Text = "About " + Application.ProductName;
		}

		private void AboutForm_Load( object sender, EventArgs e )
		{
			label4.Text = Application.ProductName;
		 	VersionLabel.Text = "Version " + ProgramForm.Version;
			CopyrightLabel.Text = Application.ProductName + " " + AssemblyCopyright;
			LinkLabel.Text = Program.WEBSITE_URL;
		}

		private string AssemblyCopyright
		{
			get
			{
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false );
				return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}
	}
}
