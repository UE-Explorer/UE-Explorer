using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using UELib;
using UELib.Core;
using UELib.Flags;
using UEExplorer;
using UEExplorer.UI;
using UEExplorer.UI.Dialogs;

namespace UEExplorer.UI.Tabs
{
	public class UnrealNativesGeneratorTab : TabComponent
	{
		private UC_NativeGenerator _UTab = new UC_NativeGenerator();
		private NativesTablePackage _APkg = new NativesTablePackage();

		protected override void TabCreated()
		{
			base.TabCreated();

			_UTab.Dock = DockStyle.Fill;
			Tab.Controls.Add( _UTab );

			_APkg.NativesTableList = new List<NativeTable>();

			// Init Delegates
			_UTab.Button_Add.Click += _OnAddNatives;
			_UTab.Button_Save.Click += _OnSaveNatives;
		}

		private void _OnSaveNatives( object sender, EventArgs e )
		{
			_APkg.CreatePackage( Path.Combine( Application.StartupPath, "Native Tables" ) + "\\NativesTableList_" + _UTab.textBox_Name.Text );
		}

		private void _OnAddNatives( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = "u";
			ofd.Filter = "UnrealScript(*.u)|*.u";
			ofd.FilterIndex = 1;
			ofd.Title = "File Dialog";
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
			DialogResult dr = ofd.ShowDialog( Owner.Owner.Owner );
			if( dr != DialogResult.OK )
			{
				return;
			}

			List<UnrealPackage> Packages = new List<UnrealPackage>();

			// Load every selected file from the file dialog
			foreach( string FileName in ofd.FileNames )
			{
				Packages.Add( UnrealLoader.LoadPackage( FileName ) );
			}
	
			foreach( UnrealPackage NPkg in Packages )
			{
				NPkg.RegisterClass( "Function", typeof(UFunction) );
				NPkg.InitializeExportObjects( UnrealPackage.InitFlags.Deserialize );

				TreeNode PkgNode;
				if( NPkg.ObjectsList.Count > 0 )
				{
					PkgNode = _UTab.TreeView_Packages.Nodes.Add( Path.GetFileNameWithoutExtension( NPkg.FullPackageName ) );
				}
				else
				{
					continue;
				}

				foreach( UFunction Func in NPkg.ObjectsList.OfType<UFunction>() )
				{
					if( Func.HasFunctionFlag( FunctionFlags.Native ) && Func.NativeToken > 0 )
					{			
						NativeTable NT = new NativeTable();
						NT.ByteToken = Func.NativeToken;
						if( Func.IsOperator() )
						{				
							if( Func.IsPre() )
							{
								NT.Format =	(byte)NativeType.PreOperator;
							}
							else if( Func.IsPost() )
							{
								NT.Format =	(byte)NativeType.PostOperator;
							}
							else
							{
								NT.Format =	(byte)NativeType.Operator;
								NT.OperPrecedence = Func.OperPrecedence;
							}
						}
						else
						{
							NT.Format =	(byte)NativeType.Function;
						}
						NT.Name = Func.FriendlyName;		
						_APkg.NativesTableList.Add( NT );
						TreeNode Tree = PkgNode.Nodes.Add( NT.Name );
						Tree.Nodes.Add( "Format:" + NT.Format );
						Tree.Nodes.Add( "ByteToken:" + NT.ByteToken );
						Tree.Nodes.Add( "OperPrecedence:" + NT.OperPrecedence );
					}
				}
				NPkg.Stream.Close();
			}
		}
	}
}
