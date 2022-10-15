using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Eliot.Utilities.Net;
using UEExplorer.Properties;
using UEExplorer.UI.Main;

namespace UEExplorer.UI.Dialogs
{
    public partial class AboutDialog : Form
    {
        private const string DONATORS_URL = "https://eliotvu.com/files/donators.txt";

        public AboutDialog()
        {
            InitializeComponent();
        }

        private string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Text += $" {Application.ProductName}";
            label4.Text = Application.ProductName;
            VersionLabel.Text = string.Format(Resources.Version, ProgramForm.Version);
            CopyrightLabel.Text = AssemblyCopyright;
            LinkLabel.Text = Program.WEBSITE_URL;
        }

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            InitializeDonators();
        }

        private void InitializeDonators()
        {
            Refresh();

            //var web = new WebClient();
            //web.DownloadFileAsync( )

            using (var buffer = new StreamReader(WebRequest.Create(DONATORS_URL).Get()))
            {
                buffer.BaseStream.Position = 0;
                DonatorsSet.ReadXml(buffer);
            }

            DonatorsGrid.AutoGenerateColumns = true;
            DonatorsGrid.DataSource = DonatorsSet;
            DonatorsGrid.DataMember = "Donators";

            DonateLink.Text = Program.Donate_URL;
        }

        private void LicenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("file:///" + Path.Combine(Application.StartupPath, "license.html"));
        }
    }
}