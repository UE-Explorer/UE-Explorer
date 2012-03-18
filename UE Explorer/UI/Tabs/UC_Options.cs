using System;
using System.IO;
using System.Windows.Forms;
using UELib;
using System.Drawing;

namespace UEExplorer.UI.Tabs
{
	public partial class UC_Options : UserControl_Tab
	{
		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected override void TabCreated()
		{
			Program.LoadConfig();
			CheckBox_SerObj.Checked = Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Deserialize );
			CheckBox_LinkObj.Checked = Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Link );
			CheckBox_ImpObj.Checked = Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Import );

			foreach( string filePath in GetNativeTables() )
			{
				ComboBox_NativeTable.Items.Add( Path.GetFileNameWithoutExtension( filePath ) );
			}

			ComboBox_NativeTable.SelectedIndex = ComboBox_NativeTable.Items.IndexOf( Program.Options.NTLPath ); 

			CheckBox_Version.Checked = Program.Options.bForceVersion;
			NumericUpDown_Version.Value = Program.Options.Version;
			CheckBox_LicenseeMode.Checked = Program.Options.bForceLicenseeMode;
			NumericUpDown_LicenseeMode.Value = Program.Options.LicenseeMode;
			SuppressComments.Checked = Program.Options.bSuppressComments;

			PathText.Text = Program.Options.UEModelAppPath;

			base.TabCreated();
		}

		public static string[] GetNativeTables()
		{
			return Directory.GetFiles( Path.Combine( Application.StartupPath, "Native Tables" ), "*" + NativesTablePackage.Extension );
		}

		private void Button_Save_Click( object sender, EventArgs e )
		{
			Program.Options.NTLPath = ComboBox_NativeTable.SelectedItem.ToString();
			Program.Options.InitFlags = UnrealPackage.InitFlags.All;

			if( !CheckBox_LinkObj.Checked )
				Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Link;

			if( !CheckBox_ImpObj.Checked )
				Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Import;

			Program.Options.InitFlags |= UnrealPackage.InitFlags.Deserialize;

			if( !CheckBox_SerObj.Checked )
				Program.Options.InitFlags &= ~UnrealPackage.InitFlags.Deserialize;

			Program.Options.InitFlags |= UnrealPackage.InitFlags.RegisterClasses | UnrealPackage.InitFlags.Construct;

			Program.Options.bForceVersion = CheckBox_Version.Checked;
			Program.Options.Version = (ushort)NumericUpDown_Version.Value;

			Program.Options.bForceLicenseeMode = CheckBox_LicenseeMode.Checked;
			Program.Options.LicenseeMode = (ushort)NumericUpDown_LicenseeMode.Value;

			Program.Options.bSuppressComments = SuppressComments.Checked;   
			UELib.UnrealConfig.SuppressComments = SuppressComments.Checked; 
			Program.Options.UEModelAppPath = PathText.Text;

			Program.SaveConfig();
		}

		private void PathButton_Click( object sender, EventArgs e )
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "UE Model View (umodel.exe)|umodel.exe";
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				PathText.Text = dialog.FileName;
			}
		}

		private void PathText_TextChanged( object sender, EventArgs e )
		{
			PathText.BackColor = File.Exists( PathText.Text ) 
				&& Path.GetFileName( PathText.Text ) == "umodel.exe"
				? Color.Green
				: Color.Red;
		}
	}
}
