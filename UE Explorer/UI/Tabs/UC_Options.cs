using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UEExplorer.Properties;
using UELib;
using System.Drawing;
using UELib.Types;

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
			PreBeginBracket.Text = Program.Options.PreBeginBracket;
			PreEndBracket.Text = Program.Options.PreEndBracket;

			PathText.Text = Program.Options.UEModelAppPath;
			IndentionNumeric.Value = Program.Options.Indention;

			foreach( var enumElement in Enum.GetNames( typeof(PropertyType) ) )
			{
				if( enumElement == "StructOffset" )
					continue;

				VariableType.Items.Add( enumElement );	
			}

			foreach( var type in Program.Options.VariableTypes )
			{
				var node = new TreeNode( type.VFullName ) 
				{
					Tag = type
				};
				VariableTypesTree.Nodes.Add( node );	
			}

			base.TabCreated();
		}

		public static IEnumerable<string> GetNativeTables()
		{
			return Directory.GetFiles( Path.Combine( Application.StartupPath, "Native Tables" ), "*" 
				+ NativesTablePackage.Extension );
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

			Program.Options.InitFlags |= UnrealPackage.InitFlags.RegisterClasses;

			Program.Options.bForceVersion = CheckBox_Version.Checked;
			Program.Options.Version = (ushort)NumericUpDown_Version.Value;

			Program.Options.bForceLicenseeMode = CheckBox_LicenseeMode.Checked;
			Program.Options.LicenseeMode = (ushort)NumericUpDown_LicenseeMode.Value;

			Program.Options.bSuppressComments = SuppressComments.Checked;   
			UnrealConfig.SuppressComments = SuppressComments.Checked; 
			Program.Options.UEModelAppPath = PathText.Text;

			Program.Options.PreBeginBracket = PreBeginBracket.Text;
			Program.Options.PreEndBracket = PreEndBracket.Text;
			UnrealConfig.PreBeginBracket = Program.ParseFormatOption( Program.Options.PreBeginBracket );
			UnrealConfig.PreEndBracket = Program.ParseFormatOption( Program.Options.PreEndBracket );

			Program.Options.Indention = (int)IndentionNumeric.Value;
			Program.UpdateIndention();

			Program.Options.VariableTypes.Clear();
			foreach( TreeNode node in VariableTypesTree.Nodes )
			{
				Program.Options.VariableTypes.Add( node.Tag as UnrealConfig.VariableType );
			}
			UnrealConfig.VariableTypes = Program.Options.VariableTypes;

			Program.SaveConfig();
			MessageBox.Show( Resources.SAVE_SUCCESS, Resources.SAVED, 
				MessageBoxButtons.OK, MessageBoxIcon.Information 
			);
		}

		private void PathButton_Click( object sender, EventArgs e )
		{
			var dialog = new OpenFileDialog
			{
				Filter = "UE Model View (umodel.exe)|umodel.exe"
			};

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

		private void VariableTypesTree_AfterSelect( object sender, TreeViewEventArgs e )
		{
			var vType = (UnrealConfig.VariableType)e.Node.Tag;

			VariableTypeGroup.Text = vType.VFullName;
			VariableType.SelectedIndex = VariableType.Items.IndexOf( vType.VType );
		}

		private void VariableType_SelectedIndexChanged( object sender, EventArgs e )
		{
			var vType = (UnrealConfig.VariableType)VariableTypesTree.SelectedNode.Tag;
			vType.VType = (string)VariableType.SelectedItem;
		}

		private void VariableTypeGroup_TextChanged( object sender, EventArgs e )
		{
			var vType = (UnrealConfig.VariableType)VariableTypesTree.SelectedNode.Tag;
			vType.VFullName = VariableTypeGroup.Text;

			VariableTypesTree.SelectedNode.Text = vType.VFullName;
		}

		private void NewArrayType_Click( object sender, EventArgs e )
		{
			if( VariableTypesTree.Nodes.Count == 0 )
			{
				VariableTypeGroup.Enabled = true;
				VariableType.Enabled = true;	
			}

			var node = new TreeNode( "Package.Class.Property" )
			{
				Tag = new UnrealConfig.VariableType
				{
					VFullName = "Package.Class.Property",
					VType = "ObjectProperty"
				}
			};

			VariableTypesTree.Nodes.Add( node );
			VariableTypesTree.SelectedNode = node;
		}

		private void DeleteArrayType_Click( object sender, EventArgs e )
		{
			if( VariableTypesTree.SelectedNode == null )
				return;

			VariableTypesTree.Nodes.Remove( VariableTypesTree.SelectedNode );

			if( VariableTypesTree.Nodes.Count == 0 )
			{
				//VariableTypeGroup.Text = String.Empty;
				VariableTypeGroup.Enabled = false;
				VariableType.Enabled = false;
			}
		}
	}
}
