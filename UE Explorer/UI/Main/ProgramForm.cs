using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Eliot.Utilities;
using Storm.TabControl;
using UEExplorer.Properties;

namespace UEExplorer.UI
{
	using Development;
	using Dialogs;
	using Tabs;

	using UELib;

    public partial class ProgramForm : Form
    {		
		public readonly TabsManager TManager;
		private MRUManager _MRUManager;

		private void InitializeUI()
		{		 
			ProgressStatus.Status = ProgressLabel;
			ProgressStatus.Loading = LoadingProgress;

			foreach( string filePath in UC_Options.GetNativeTables() )
			{
				SelectedNativeTable.DropDown.Items.Add( Path.GetFileNameWithoutExtension( filePath ) );
			}

			SelectedNativeTable.Text = Program.Options.NTLPath;
			Platform.Text = Program.Options.Platform;

#if DEBUG
			_CacheExtractorItem.Enabled = true;
#endif

			MRUManager.SetStoragePath( Application.StartupPath );
			_MRUManager = MRUManager.Load();
			_MRUManager.RefreshEvent += RefreshMRUEvent;
			RefreshMRUEvent();
		}

		private void RefreshMRUEvent()
		{
			_ROF.MenuItems.Clear();
			for( int i = _MRUManager.Files.Count - 1; i >= 0; -- i )
			{
				if( !File.Exists( _MRUManager.Files[i] ) )
				{
					_MRUManager.Files.RemoveAt( i );
					-- i;
					continue;	
				}

				var item = _ROF.MenuItems.Add
				( 
					(_MRUManager.Files.Count - i) + " " + Path.GetFileName( _MRUManager.Files[i] ) 
				);		
				item.Tag = _MRUManager.Files[i];
				item.Click += _ROF_ItemClicked; 
			}

			_ROF.Enabled = _ROF.MenuItems.Count > 0;
			_MRUManager.Save();
		}

		private void _ROF_ItemClicked( object sender, EventArgs e )
		{
			var item = sender as MenuItem;
			LoadFile( item.Tag as string );
			RefreshMRUEvent();
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

					var assembly = Assembly.LoadFile( file );
					Type[] types = assembly.GetExportedTypes();
					foreach( Type t in types )
					{
						Type i = t.GetInterface( "IExtension" );
						if( i != null )
						{
							string extensionname = "Extension";

							object[] attribs = t.GetCustomAttributes( typeof(ExtensionTitleAttribute), false );
							if( attribs.Length > 0 )
							{
								extensionname = ((ExtensionTitleAttribute)attribs[0]).Title;
							}
				 
							var item = menuItem13.MenuItems.Add( extensionname );
							var ext = Activator.CreateInstance( t ) as IExtension;
							ext.Initialize( this );
							item.Click += ext.OnActivate;

							menuItem13.Enabled = true;
						}
					}
				}
			}
		}

		private void ToolsToolStripMenuItem_DropDownOpening( object sender, EventArgs e )
		{
			menuItem20.Checked = Program.AreFileTypesRegistered();

			if( menuItem13.Enabled )
				return;

			InitializeExtensions();
		}

    	public static string Version
    	{
    		get{ return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    	}

		internal ProgramForm()
        {
			Program.LogManager.StartLogStream();
			Text = string.Format( "{0} {1}", Application.ProductName, Version );

            InitializeComponent();	
			InitializeConfig();
			InitializeUI();

			TManager = new TabsManager( this, TabComponentsStrip );
		}
		
		public void LoadFile( string fileName )
		{
			ITabComponent tabComponent = null;

			ProgressStatus.SaveStatus();
			ProgressStatus.SetStatus( string.Format( 
					Resources.ProgramForm_LoadFile_Loading_file, 
					Path.GetFileName( fileName ) 
				) 
			);

			try
			{
				switch( Path.GetExtension( fileName ) )
				{
					case ".uc": case ".uci":
						tabComponent = TManager.AddTabComponent( typeof(UC_UClassFile), 
							Path.GetFileName( fileName ) 
						);
						var classFile = (UC_UClassFile)tabComponent;
						if( classFile == null )
						{
							return;
						}

						classFile.FileName = fileName;
						classFile.PostInitialize();
						break;

					default:
						tabComponent = TManager.AddTabComponent( typeof(UC_PackageExplorer), 
							Path.GetFileName( fileName ) 
						);
						var unrealFile = (UC_PackageExplorer)tabComponent;
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
				ExceptionDialog.Show( string.Format( Resources.ProgramForm_LoadFile_Failed_loading_package, 
					fileName ), e 
				);		
			}
			finally
			{
				ProgressStatus.Reset();

			   _MRUManager.AddFile( fileName );
			}
		}

		#region Events
		private void AboutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var about = new AboutForm();
			about.ShowDialog();
		}

		private void OpenFileToolStripMenuItem_Click( object sender, EventArgs e )
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

		private void UnrealColorGeneratorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			// open a tool dialog!
			var cgf = new ColorGeneratorForm();
			cgf.Show();
		}

		private void UnrealCacheExtractorToolStripMenuItem_Click( object sender, EventArgs e )
		{
			TManager.AddTabComponent( typeof(CacheExtractorTabComponent), 
				Resources.ProgramForm_Cache_Extractor 
			);
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
			foreach( var tc in TManager.Tabs )
			{
				if( tc.Tab == e.Item )
				{
					tc.TabClosing();
					TManager.Tabs.Remove( tc );
					break;
				}
			}	
		}

		private void TabComponentsStrip_TabStripItemClosed( object sender, EventArgs e )
		{
			TabComponentsStrip.Visible = TabComponentsStrip.Items.Count > 0;
			HomepageButton.Visible = !TabComponentsStrip.Visible;
		}

		private void ExitToolStripMenuItem_Click( object sender, EventArgs e )
		{
   			Application.Exit();
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
				TManager.AddTabComponent( typeof(UC_Default), Resources.Homepage );
			}
		}

		private void ToggleUEExplorerFileIconsToolStripMenuItem_Click( object sender, EventArgs e )
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
			Program.ToggleRegisterFileTypes( menuItem20.Checked );
		}

		private void UEExplorer_Form_DragEnter( object sender, DragEventArgs e )
		{
			e.Effect = e.Data.GetDataPresent( DataFormats.FileDrop ) 
				? DragDropEffects.Move 
				: DragDropEffects.None;
		}

		private void UEExplorer_Form_DragDrop( object sender, DragEventArgs e )
		{
			var allowedExtensions = UnrealExtensions.FormatUnrealExtensionsAsFilter().Replace( 
				"*.u;", 
				"*.u;*.uc;*.uci;" 
			);

			if( !e.Data.GetDataPresent( DataFormats.FileDrop ) ) 
				return;

			var files = (string[])e.Data.GetData( DataFormats.FileDrop );
			foreach( var filePath in files )
			{
				if( allowedExtensions.Contains( Path.GetExtension( filePath ) ) )
				{
					LoadFile( filePath );
				}
			}
		}

		private void SaveFileToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( TManager.SelectedComponent != null )
			{
				TManager.SelectedComponent.TabSave();
			}
		}

		private void FindToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( TManager.SelectedComponent != null )
			{
				TManager.SelectedComponent.TabFind();
			}
		}
		#endregion

		private void DonateToolStripMenuItem1_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.Donate_URL );
		}

		private void CheckForUpdates( object sender, EventArgs e )
		{
			ProgressStatus.SaveStatus();
			ProgressStatus.SetStatus( Resources.ProgramForm_Check_for_Updates_Status );

			var web = new WebClient();
			web.Headers["Content-Type"] = "application/x-www-form-urlencoded";
			web.UploadStringCompleted += (stringSender, stringEvent) =>
 			{
				try
				{
					var result = stringEvent.Result.Trim();
					if( result != Version )
					{
						if( MessageBox.Show(
							String.Format
							(
								Resources.NEW_VERSION_AVAILABLE_MESSAGE,
								Version,
								result
							),
							Resources.NEW_VERSION_AVAILABLE_TITLE,
							MessageBoxButtons.YesNo ) == DialogResult.Yes )
						{
							System.Diagnostics.Process.Start( Program.Program_URL );
						}
					}
					else
					{
						MessageBox.Show
						(
							String.Format
							(
								Resources.NO_NEW_VERSION_AVAILABLE_MESSAGE,
								Application.ProductName
							)
						);
					}
				}
				catch( Exception exc )
				{
					MessageBox.Show
					(
						String.Format
						(
							Resources.CHECKFORUPDATES_FAILED_MESSAGE
								+ "\r\n\r\n{0}",
							exc
						),
						Resources.Error,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				}
				finally
				{ 
					ProgressStatus.ResetStatus();
				}
			};
			web.UploadStringAsync( new Uri( Program.Version_URL ), "Post", Program.Program_Parm_ID );
		}

	    private void MenuItem7_Click( object sender, EventArgs e )
		{
			TManager.AddTabComponent( typeof(UC_Options), Resources.Options );
		}

		private void MenuItem24_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.Forum_URL );
		}

		private void MenuItem26_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.WEBSITE_URL );
		}

		private void Platform_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			Platform.Text = e.ClickedItem.Text;
			Program.Options.Platform = Platform.Text;
			Program.SaveConfig();
		}

		private void MenuItem4_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( Program.Contact_URL );
		}

		private void ProgramForm_FormClosed( object sender, FormClosedEventArgs e )
		{
			Program.LogManager.EndLogStream();
		}

		private void OpenHome_Click( object sender, EventArgs e )
		{
			TManager.AddTabComponent( typeof(UC_Default), Resources.Homepage );
		}

		private void SocialMenuItem_Click( object sender, EventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.facebook.com/UE.Explorer" );
		}

		private void ProgramForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			Properties.Settings.Default.Save();
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