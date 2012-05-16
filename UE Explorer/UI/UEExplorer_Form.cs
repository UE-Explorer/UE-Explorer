using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Storm.TabControl;
using UEExplorer.Properties;

namespace UEExplorer.UI
{
	using UEExplorer.Development;
	using UEExplorer.UI.Dialogs;
	using UEExplorer.UI.Tabs;

	using UELib;

    public partial class UEExplorer_Form : Form
    {		
		public readonly TabsManager TManager;

		private void InitializeUI()
		{		 
			ProgressStatus.Status = ProgressLabel;
			ProgressStatus.Loading = LoadingProgress;

			foreach( string filePath in UC_Options.GetNativeTables() )
			{
				SelectedNativeTable.DropDown.Items.Add( Path.GetFileNameWithoutExtension( filePath ) );
			}

			SelectedNativeTable.Text = Program.Options.NTLPath;
		}

		private void SelectedNativeTable_DropDownOpening( object sender, EventArgs e )
		{
			// In case it got changed!
			SelectedNativeTable.Text = Program.Options.NTLPath;
		}

		private void SelectedNativeTable_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			SelectedNativeTable.Text = e.ClickedItem.Text;

			Program.Options.NTLPath = SelectedNativeTable.Text;
			Program.SaveConfig();
		}

		private void InitializeConfig()
		{
			Program.LoadConfig();
		}

		private void InitializeExtensions()
		{
			string extpath = Path.Combine( Application.StartupPath, "Extensions" );
			if( Directory.Exists( extpath ) )
			{
				string[] files = Directory.GetFiles( extpath );
				foreach( string file in files )
				{
					if( Path.GetExtension( file ) != ".dll" )
						continue;

					Assembly a = System.Reflection.Assembly.LoadFile( file );
					Type[] types = a.GetExportedTypes();
					foreach( Type t in types )
					{
						Type i = t.GetInterface( "IExtension" );
						if( i != null )
						{
							string extensionname = "Extension";

							object[] attribs = t.GetCustomAttributes( typeof(ExtensionTitleAttribute), false );
							if( attribs != null && attribs.Length > 0 )
							{
								extensionname = ((ExtensionTitleAttribute)attribs[0]).Title;
							}
				 
							IExtension ext = Activator.CreateInstance( t ) as IExtension;

							var item = menuItem13.MenuItems.Add( extensionname );
							item.Click += ext.OnActivate;
							ext.Initialize( this );

							menuItem13.Enabled = true;
						}
					}
				}
			}
		}

		private void toolsToolStripMenuItem_DropDownOpening( object sender, EventArgs e )
		{
			menuItem20.Checked = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( UEAUFL ) != null;

			if( menuItem13.Enabled )
				return;

			InitializeExtensions();
		}

		private FileStream _logStream;

    	public static string Version
    	{
    		get{ return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    	}

		internal UEExplorer_Form()
        {

			Text = Application.ProductName + " " + Version;
			//this.ControlBox = false;	  

			string path = Path.Combine( Application.StartupPath, "Log.txt" );
			_logStream = new FileStream( path, FileMode.Create, FileAccess.Write  );
			var sw = new StreamWriter( _logStream );
			Console.SetOut( sw );

            InitializeComponent();	
			InitializeConfig();
			InitializeUI();

			TManager = new TabsManager( this );
		}

		private void UEExplorer_Form_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( _logStream == null ) 
				return;

			_logStream.Flush();
			_logStream.Close();
			_logStream.Dispose();
		}

		public ITabComponent AddTabComponent( Type type, string name )
		{
			// Avoid duping tabs.
			foreach( ITabComponent TC in TManager.Tabs )
			{
				if( TC.Tab.Title == name )
				{
					return null;
				}
			}

			ITabComponent newtab = (ITabComponent)Activator.CreateInstance( type );
			TabStripItem item = TManager.CreateTabPage( name );
			TManager.AddTab( newtab, item );

			TabComponentsStrip.Visible = TabComponentsStrip.Items.Count > 0;
			item.Refresh();
			return newtab;
		}

		public void LoadFile( string fileName )
		{
			ITabComponent tabComponent = null;

			ProgressStatus.SaveStatus();
			ProgressStatus.SetStatus( "Loading file " + Path.GetFileName( fileName ) + "..." );

			//Refresh();

			try
			{
				switch( Path.GetExtension( fileName ) )
				{
					case ".uc": case ".uci":
						tabComponent = AddTabComponent( typeof(UC_UClassFile), Path.GetFileName( fileName ) );
						UC_UClassFile classFile = (UC_UClassFile)tabComponent;
						if( classFile == null )
						{
							return;
						}

						classFile.FileName = fileName;
						classFile.PostInitialize();
						break;

					default:
						tabComponent = AddTabComponent( typeof(UC_PackageExplorer), Path.GetFileName( fileName ) );
						UC_PackageExplorer unrealFile = (UC_PackageExplorer)tabComponent;
						if( unrealFile == null )
						{
							return;
						}

						unrealFile.FileName = fileName;
						unrealFile.PostInitialize();
						break;
				}
			}
			catch( Exception e )
			{
				if( tabComponent != null )
				{
					TManager.RemoveTab( tabComponent );
				}
				ExceptionDialog.Show( "Failed loading package: " + fileName, e );
				
			}
			finally
			{
				ProgressStatus.Reset();
			}
		}

		public bool IsLoaded( string fileName )
		{
			return TManager.Tabs.Exists(
				delegate( ITabComponent tc ){ 
					return (IHasFileName)tc != null 
						? ((IHasFileName)tc).FileName == fileName 
						: true; 
				} 
			);
		}

		#region Events
		private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var about = new AboutForm();
			about.ShowDialog();
		}

		private void openFileToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var ofd = new OpenFileDialog
			{
			    DefaultExt = "u",
			    Filter = UnrealExtensions.FormatUnrealExtensionsAsFilter().Replace( "*.u;", "*.u;*.uc;*.uci;" ),
			    FilterIndex = 1,
			    Title = Resources.Open_File,
			    Multiselect = true
			};
			if( ofd.ShowDialog( this ) == DialogResult.OK )
			{
				foreach( string fileName in ofd.FileNames )
				{
					LoadFile( fileName );
				}
			}	
		}

		private void unrealColorGeneratorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			// open a tool dialog!
			var cgf = new ColorGeneratorForm();
			cgf.Show();
		}

		private void unrealCacheExtractorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddTabComponent( typeof(CacheExtractorTabComponent), "Cache Extractor" );
		}

		private void TabComponentsStrip_TabStripItemSelectionChanged( TabStripItemChangedEventArgs e )
		{		
			// This delegate is called when UE is still none on startup
			if( TManager.SelectedComponent == null || e.ChangeType == TabStripItemChangeTypes.Removed )
			{
				return;
			}

			TManager.SelectedComponent.TabSelected();
			var show = TManager.SelectedComponent is UC_UClassFile; 
			menuItem12.Enabled = show;
			menuItem12.Visible = show;
			menuItem9.Enabled = show;
			menuItem9.Visible = show;
			menuItem2.Enabled = show;
			menuItem2.Visible = show;
		}

		private void TabComponentsStrip_TabStripItemClosing( TabStripItemClosingEventArgs e )
		{
			// Find the owner of this TabStripItem
			foreach( var TC in TManager.Tabs )
			{
				if( TC.Tab == e.Item )
				{
					TC.TabClosing();
					TManager.Tabs.Remove( TC );
					break;
				}
			}	
		}

		private void TabComponentsStrip_TabStripItemClosed( object sender, EventArgs e )
		{
			TabComponentsStrip.Visible = TabComponentsStrip.Items.Count > 0;
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
   			Application.Exit();
		}

		private void unrealNativeTableGeneratorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddTabComponent( typeof(UnrealNativesGeneratorTab), "Unreal Natives Table Generator" );
		}

		private void Unreal_Explorer_Form_Shown( object sender, EventArgs e )
		{
			Refresh();
			var args = Environment.GetCommandLineArgs();
			for( int i = 1; i < args.Length; ++ i )
			{
				if( File.Exists( args[i] ) )
				{
					LoadFile( args[i] );
				}
			}

			if( TManager.Tabs.Count == 0 )
			{
				AddTabComponent( typeof(UC_Default), "Homepage" );
			}
		}

		private void toggleUEExplorerFileIconsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( !menuItem20.Checked )
			{
				MessageBox.Show( 
					Resources.RegistryWarning, 
					Resources.Warning, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning 
				);	
			}
			ChangeUnrealRegistry( menuItem20.Checked );
		}

		private void UEExplorer_Form_DragEnter( object sender, DragEventArgs e )
		{
			e.Effect = e.Data.GetDataPresent( DataFormats.FileDrop ) ? DragDropEffects.Move : DragDropEffects.None;
		}

		private void UEExplorer_Form_DragDrop( object sender, DragEventArgs e )
		{
			string allowedExtensions = UnrealExtensions.FormatUnrealExtensionsAsFilter().Replace( "*.u;", "*.u;*.uc;*.uci;" );
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				var files = (string[])e.Data.GetData( DataFormats.FileDrop );

				foreach( string filePath in files )
				{
					if( allowedExtensions.Contains( Path.GetExtension( filePath ) ) )
					{
						LoadFile( filePath );
					}
				}
			}
		}

		private void saveFileToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( TManager.SelectedComponent != null )
			{
				TManager.SelectedComponent.TabSave();
			}
		}

		private void findToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( TManager.SelectedComponent != null )
			{
				TManager.SelectedComponent.TabFind();
			}
		}
		#endregion

		private const string UEAUFL = "UEExplorer.AnyUnrealFile";

		private static void ChangeUnrealRegistry( bool undo = false )
		{		
			var extkeys = new List<Microsoft.Win32.RegistryKey>();
			var extensions = UnrealExtensions.FormatUnrealExtensionsAsList();
			if( undo )
			{
				Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree( UEAUFL );
				foreach( string ext in extensions )
				{
					Microsoft.Win32.RegistryKey extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext, true );
					if( extkey != null )
					{
						if( (string)extkey.GetValue( "" ) == UEAUFL )
						{
							Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree( ext );
						}
						else
						{
							extkeys.Add( extkey );
						}
					}	
				}

				foreach( Microsoft.Win32.RegistryKey key in extkeys )
				{
					var reference = (string)key.GetValue( "" );
					if( reference != null )
					{
						Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( reference, true );
						if( k != null )
						{
							EditKey( k, true );
						}
					}
				}
			}
			else
			{
				foreach( string ext in extensions )
				{
					Microsoft.Win32.RegistryKey extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext, true );
					if( extkey == null )
					{
						extkey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey( ext );
						extkey.SetValue( "", UEAUFL, Microsoft.Win32.RegistryValueKind.String );
						extkey.SetValue( "Content Type", "application", Microsoft.Win32.RegistryValueKind.String );
					}
					else if( (string)(extkey.GetValue( "" )) != UEAUFL )
					{		  
						extkeys.Add( extkey );
					}
				}

				Microsoft.Win32.RegistryKey unrealfilekey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey( UEAUFL );
				if( unrealfilekey != null )
				{
					unrealfilekey.SetValue( "", "Unreal File" );
					EditKey( unrealfilekey );
				}

				foreach( Microsoft.Win32.RegistryKey key in extkeys )
				{
					string reference = (string)key.GetValue( "" );
					if( reference != null )
					{
						Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( reference, true );
						if( k != null )
						{
							EditKey( k );
						}
					}
				}
			}
		}

		private static void EditKey( Microsoft.Win32.RegistryKey key, bool undo = false )
		{
			if( undo )
			{
				var xkey = key.OpenSubKey( "DefaultIcon", true );
				// Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
				if( xkey != null )
				{
					string curkey = (string)xkey.GetValue( "" );
					if( curkey != null )
					{
						string oldasc = (string)xkey.GetValue( "OldAssociation" );
						xkey.SetValue( "OldAssociation", curkey, Microsoft.Win32.RegistryValueKind.String );
						xkey.SetValue( "", oldasc ?? "", Microsoft.Win32.RegistryValueKind.String );
					}
				}

				var shellkey = key.OpenSubKey( "shell", true );
				if( shellkey != null )
				{
					shellkey.DeleteSubKeyTree( "open in " + Application.ProductName );
				}
			}
			else
			{
				// Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
				if( key.OpenSubKey( "DefaultIcon" ) == null )
				{
					using( var defaulticonkey = key.CreateSubKey( "DefaultIcon" ) )
					{
						if( defaulticonkey != null )
						{
							string mykey = Path.Combine( Application.StartupPath, "unrealfile.ico" );
							string oldassociation = (string)defaulticonkey.GetValue( "" );
							if( oldassociation != mykey )
							{
								if( oldassociation != null )
								{
									defaulticonkey.SetValue( "OldAssociation", oldassociation, Microsoft.Win32.RegistryValueKind.String );
								}	
								defaulticonkey.SetValue( "", mykey, Microsoft.Win32.RegistryValueKind.String );
							}
						}
					}
				}

				var shellkey = key.CreateSubKey( "shell" );
				var editkey = shellkey.CreateSubKey( "open in " + Application.ProductName );
				editkey.SetValue( "", "&Open in " + Application.ProductName );
				var cmdkey = editkey.CreateSubKey( "command" );
				cmdkey.SetValue( "", "\"" + Application.ExecutablePath + "\" \"%1\"", Microsoft.Win32.RegistryValueKind.ExpandString );
			}
		}

		private void donateToolStripMenuItem1_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.WEBSITE_URL + "donate.html" );
		}

		private void checkForUpdates( object sender, EventArgs e )
		{
			try
			{
				ProgressStatus.SaveStatus();
				ProgressStatus.SetStatus( "Checking for updates..." );
				// ID of UE Explorer
				var postData = "data[items][id]=21";
				var result = Program.Post( Program.WEBSITE_URL + "apps/version/", postData ).Trim();
				if( result != Version )
				{
					if( MessageBox.Show(
						"Clicking yes will bring you to the page with the latest version!"
						+ "\r\n\r\nYour version: " + Version
						+ "\r\nLatest version: " + result
						, "A new version is available!"
						, MessageBoxButtons.YesNo
					) == System.Windows.Forms.DialogResult.Yes )
					{
						System.Diagnostics.Process.Start( Program.WEBSITE_URL + "portfolio/view/21/UE-Explorer" );
					}
				}
				else
				{
					MessageBox.Show( "You have the latest version of " + Application.ProductName );
				}
			}
			catch( Exception exc )
			{
				MessageBox.Show( "Failed to request the latest version. Please try again later!"
					+ "\r\nException:" + exc.Message,
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error
				);
			}
			finally
			{
				ProgressStatus.ResetStatus();
			}
		}

		private void menuItem7_Click( object sender, EventArgs e )
		{
			AddTabComponent( typeof(UC_Options), Application.ProductName + " Options" );
		}

		private void menuItem24_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.WEBSITE_URL + "forum/" );
		}

		private void menuItem26_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.WEBSITE_URL );
		}
    }

	public static class ProgressStatus
	{
		public static ToolStripProgressBar Loading;
		public static ToolStripStatusLabel Status;

		private static string _SavedStatus;

		public static void SetStatus( string status )
		{
			Status.Text = status;
			Status.Owner.Refresh();
		}

		public static void Reset()
		{
			ResetStatus();
			ResetValue();
		}

		public static void ResetStatus()
		{
			SetStatus( _SavedStatus );
		}

		public static void SaveStatus()
		{
			_SavedStatus = Status.Text;
		}

		public static void IncrementValue()
		{
			++ Loading.Value;
		}

		public static void ResetValue()
		{
			Loading.Visible = false;
			Loading.Value = 0;
		}

		public static int GetProgress()
		{
			return Loading.Value;
		}

		public static void SetMaxProgress( int max )
		{
			Loading.Maximum = max;
		}
	}
}