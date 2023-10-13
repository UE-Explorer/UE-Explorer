using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Eliot.Utilities.Net;
using UEExplorer.Properties;

namespace UEExplorer.UI.Dialogs
{
    public partial class AboutDialog : Form
    {
        private const string DonatorsUrl = "https://eliotvu.com/files/donators.txt";

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
            ProductNameLabel.Text = Application.ProductName;
            VersionLabel.Text = string.Format(Resources.Version, Assembly.GetExecutingAssembly().GetName().Version);
            CopyrightLabel.Text = AssemblyCopyright;
            DonateLink.Text = Program.DonateUrl;
        }

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            InitializeDonators();
        }

        private void InitializeDonators()
        {
            Refresh();

            using (var buffer = new StreamReader(WebRequest.Create(DonatorsUrl).Get()))
            {
                buffer.BaseStream.Position = 0;
                DonatorsSet.ReadXml(buffer);
            }

            DonatorsGrid.AutoGenerateColumns = true;
            DonatorsGrid.DataSource = DonatorsSet;
            DonatorsGrid.DataMember = "Donators";
        }

        private void LicenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"file:///{Path.Combine(Application.StartupPath, "LICENSE-3RD-PARTY.html")}");
        }
    }
}
