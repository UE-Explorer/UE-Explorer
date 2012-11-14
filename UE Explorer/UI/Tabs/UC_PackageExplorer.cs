using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UEExplorer.Properties;

namespace UEExplorer.UI.Tabs
{
	using Dialogs;

	using UELib;
	using UELib.Core;
	using UELib.Engine;
	using UELib.Flags;

	[System.Runtime.InteropServices.ComVisible( false )]
	public partial class UC_PackageExplorer : UserControl_Tab, IHasFileName
	{
		public string FileName{ get; set; }

		/// <summary>
		/// My Unreal file package!
		/// </summary>
		protected UnrealPackage _UnrealPackage;

		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected override void TabCreated()
		{						
			/*string uLangPath = Path.Combine( Application.StartupPath, "Config", "UnrealScript" );
			if( File.Exists( uLangPath + ".LANG" ) )
			{
				ScriptPage.Document.LanguageFile = uLangPath;
			}*/		

			string langPath = Path.Combine( Application.StartupPath, "Config", "UnrealScript.xshd" );
			if( File.Exists( langPath ) )
			{
				try
				{
					TextEditorPanel.textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load( 
						new System.Xml.XmlTextReader( langPath ), 
						ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance 
					);
					TextEditorPanel.searchWiki.Click += searchWiki_Click;
					TextEditorPanel.textEditor.ContextMenuOpening += new System.Windows.Controls.ContextMenuEventHandler( contextMenu_ContextMenuOpening );
					TextEditorPanel.copy.Click += copy_Click;

					// Fold all { } blocks
					//var foldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(myTextEditor1.textEditor.TextArea);
					//var foldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
					//foldingStrategy.UpdateFoldings(foldingManager, myTextEditor1.textEditor.Document);
				}
				catch( Exception e )
				{
					ExceptionDialog.Show( e.GetType().Name, e ); 
				}
			}

			_Form = Owner.Owner;
			base.TabCreated();
		}

		void copy_Click( object sender, System.Windows.RoutedEventArgs e )
		{
			TextEditorPanel.textEditor.Copy();
			//System.Windows.Clipboard.SetText( GetSelection() );
		}

		string GetSelection()
		{
			return TextEditorPanel.textEditor.TextArea.Selection.GetText( TextEditorPanel.textEditor.Document );
		}

		void contextMenu_ContextMenuOpening( object sender, System.Windows.Controls.ContextMenuEventArgs e )
		{
			if( TextEditorPanel.textEditor.TextArea.Selection.Length == 0 )
			{
				TextEditorPanel.searchWiki.IsEnabled = false;
				return;
			}
			var selection = GetSelection();
			//myTextEditor1.searchWiki.IsEnabled = !selection.Contains( '\n' );
			TextEditorPanel.searchWiki.Header = "Search UnrealWiki for \"" 
				+  selection.Substring( 0, 64 )
				+ "\"";
		}

		void searchWiki_Click( object sender, System.Windows.RoutedEventArgs e )
		{
			System.Diagnostics.Process.Start( 
				String.Format( 
					"http://wiki.beyondunreal.com/?ns0=1&ns100=1&ns102=1&ns104=1&ns106=1&search={0}&title=Special%3ASearch&fulltext=Advanced+search&fulltext=Advanced+search",
					GetSelection()
				) 
			);
		}

		public void PostInitialize()
		{  
			LoadPackage();
		}

		private long _SummarySize;

		private void LoadPackage()
		{
			if( Program.Options.bForceLicenseeMode )
			{
				UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;
			}

			if( Program.Options.bForceVersion )
			{
				UnrealPackage.OverrideVersion = Program.Options.Version;
			}

			UnrealConfig.SuppressSignature = false;
			reload:
			ProgressStatus.SetStatus( "Loading Package..." );
			// Open the file.
			try
			{
				UnrealConfig.Platform = (UnrealConfig.CookedPlatform)Enum.Parse( typeof(UnrealConfig.CookedPlatform), _Form.Platform.Text, true );
				_UnrealPackage = UnrealLoader.LoadPackage( FileName );
				UnrealConfig.SuppressSignature = false;
				_SummarySize = _UnrealPackage.Stream.Position;

				if( _UnrealPackage.CompressedChunks != null && _UnrealPackage.CompressedChunks.Capacity > 0 )
				{
					if( MessageBox.Show( "This package is compressed! Compressed packages are not supported."
						+ "\r\n\r\nPlease consider decompressing the package using \"Unreal Package Decompressor\" from Gildor"
						+ "\r\n\r\nYou can download the tool from http://www.gildor.org/downloads or press OK to go there now.",
						"Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Question
					) == DialogResult.OK )
					{
						System.Diagnostics.Process.Start( "http://www.gildor.org/downloads" );
						MessageBox.Show( "To use Gildor's tool, try \"decompress.exe PACKAGENAME.EXT\" you may have to specify -lzo.", 
							"Notice", MessageBoxButtons.OK, MessageBoxIcon.Information 
						);
					}
					TabControl_General.Selected -= TabControl_General_Selected;
					TabControl_General.TabPages.Remove( TabPage_Objects );
					TabControl_General.TabPages.Remove( TabPage_Tables );
					InitializeUI();
					return;
				}

				TabControl_General.TabPages.Remove( TabPage_Chunks );
			}
			catch( System.IO.FileLoadException )
			{
				_UnrealPackage = null;
				if( MessageBox.Show(
				        "This package has an unknown signature.\r\n\r\nAre you sure you want to try to deserialize this package? Clicking Yes might lead to unexpected results!",
				        "Warning", MessageBoxButtons.YesNo
				    ) == DialogResult.No
				)
				{
					Owner.RemoveTab( this );
					return;
				}
				UnrealConfig.SuppressSignature = true;
				goto reload;
				//throw new UnrealException( "Invalid package signature!", e );

			}
			catch( Exception e )
			{
				throw new UnrealException( e.Message, e );
			}

			string NTLPath = Path.Combine( Application.StartupPath, "Native Tables", Program.Options.NTLPath );
			if( File.Exists( NTLPath + NativesTablePackage.Extension ) )
			{
				// Load the native names.
				try
				{
					_UnrealPackage.NTLPackage = new NativesTablePackage();
					_UnrealPackage.NTLPackage.LoadPackage( NTLPath ); 
				}
				catch( Exception e )
				{
					_UnrealPackage.NTLPackage = null;
					throw new UnrealException( "Couldn't load " + NTLPath + "! \r\nEvent:Loading Package", e );
				}
			}

			InitializePackage();
		}

		private void InitializePackage()
		{
			_ClassesList = new List<UClass>();
			_UnrealPackage.NotifyPackageEvent += _OnNotifyPackageEvent;
			_UnrealPackage.NotifyObjectAdded += _OnNotifyObjectAdded;

			ProgressStatus.ResetValue();
			int max = Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Construct ) ? _UnrealPackage.ExportTableList.Count + _UnrealPackage.ImportTableList.Count : 0;

			if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Deserialize ) )
				max += _UnrealPackage.ExportTableList.Count;
								
			// Importing objects has been disabled FIXME:
			/*if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Import ) )
				max += _UnrealPackage.ImportTableList.Count;*/

			if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Link ) )
				max += _UnrealPackage.ExportTableList.Count;

			ProgressStatus.SetMaxProgress( max );
			ProgressStatus.Loading.Visible = true;

			try
			{
				_UnrealPackage.InitializePackage( Program.Options.InitFlags );

				ReadMetaInfo();
			}
			catch( ImportingObjectsException e )
			{
				throw new UnrealException( "Couldn't load " + FileName + "! \r\nEvent:Importing Objects", e );
			}
			catch( LinkingObjectsException e )
			{
				throw new UnrealException( "Couldn't load " + FileName + "! \r\nEvent:Linking Objects", e );
			}
			catch( Exception e )
			{
				throw new UnrealException( "Couldn't load " + FileName + "! \r\nEvent:Initializing Package", e );
			}

			_UnrealPackage.NotifyObjectAdded -= _OnNotifyObjectAdded;
			_UnrealPackage.NotifyPackageEvent -= _OnNotifyPackageEvent;

			InitializeUI();

			if( ONLOAD_MESSAGE != null )
			{
				MessageBox.Show( ONLOAD_MESSAGE, "Package's MetaInfo", MessageBoxButtons.OK );
			}
		}

		private string ONLOAD_MESSAGE;

		private void ReadMetaInfo()
		{
			foreach( var obj in _UnrealPackage.ObjectsList.Where((o) => o is UConst && o.Name.StartsWith( "META_DECOMPILER" ) ) )
			{
				var value = ((UConst)obj).Value;
				var parms = obj.Name.Substring( 16 ).Split( '_' );
				switch( parms[0] )
				{
					case "VAR":
						switch( parms[1] )
						{
							case "AUTHOR":
								LABEL_Author.Text += value;
								LABEL_Author.Visible = true;
								break;

							case "COPYRIGHT":
								LABEL_Copyright.Text += value;
								LABEL_Copyright.Visible = true;
								break;
						}
						break;
						
					//const META_DECOMPILER_VAR_AUTHOR			= "AUTHOR";
					//const META_DECOMPILER_VAR_COPYRIGHT			= "COPYRIGHT";
					//const META_DECOMPILER_EVENT_ONLOAD_MESSAGE		= "POPUP_MESSAGE";

					case "EVENT":
						switch( parms[1] )
						{
							case "ONLOAD":
								switch( parms[2] )
								{
									case "MESSAGE":
										ONLOAD_MESSAGE = "This package contains a message from the developer.\r\n\r\n" + value;
										break;
								}
								break;
						}
						break;
				}	
			}
		}

		private void toolStripButton1_Click( object sender, EventArgs e )
		{
			var sfd = new SaveFileDialog
			{
			    DefaultExt = "uc",
			    Filter = "Unreal Class(*.uc)|*.uc",
			    FilterIndex = 1,
			    Title = "Export Text",
			    FileName = Label_ObjectName.Text + UnrealExtensions.UnrealCodeExt
			};
			if( sfd.ShowDialog() == DialogResult.OK )
			{
				File.WriteAllText( sfd.FileName, TextEditorPanel.textEditor.Text );
			}
		}	

		internal class UnrealTableNode : TreeNode 
		{
			internal UObjectTableItem Table;
			protected bool _Initialized;

			public virtual void Initialize()
			{
			}
		}

		internal class ExportNode : UnrealTableNode, IDecompilableObjectNode
		{
			public IUnrealDecompilable Object
			{
				get{ return Table.Object; }
				set{ Table.Object = value as UObject; }
 			}

			public bool CanViewBuffer
			{
				get{ return true; }
			}

			public bool AllowDecompile
			{
				get{ return true; }
			}

			public string Decompile()
			{
				return Table.Object.Decompile();
			}

			public ExportNode()
			{
				Nodes.Add( "DUMMYNODE" );
			}

			public override void Initialize()
			{
				if( _Initialized )
					return;

				_Initialized = true;

				TreeView.BeginUpdate();
				Nodes.Clear();
				var exp = Table as UExportTableItem;
				ulong objFlags = exp.ObjectFlags;
				if( objFlags != 0 )
				{
					string flagTitle = "Flags(" + UnrealMethods.FlagToString( exp.ObjectFlags ) + ")";
					TreeNode flagNode = Nodes.Add( flagTitle  );
					flagNode.ToolTipText = UnrealMethods.FlagsListToString( UnrealMethods.FlagsToList( typeof(ObjectFlagsLO), typeof(ObjectFlagsHO), exp.ObjectFlags ) );	
				}

				Nodes.Add( "Export Offset:" + exp.Offset );
				Nodes.Add( "Export Size:" + exp.Size );

				if( exp.ExportFlags != 0 )
				{
					Nodes.Add( "Export Flags:" + UnrealMethods.FlagToString( exp.ExportFlags ) );
				}
				
				if( exp.ClassIndex == 0 )
				{
					Nodes.Add( "Class:Class(" + exp.ClassIndex + ")" );
				}
				else
				{
					Nodes.Add( "Class:" + exp.ClassTable.ObjectName + "(" + exp.ClassIndex + ")" );
				}

				if( exp.SuperIndex != 0 )
				{
					Nodes.Add( "Super:" + exp.SuperTable.ObjectName + "(" + exp.SuperIndex + ")" );
				}

				if( exp.OuterIndex != 0 )
				{
					Nodes.Add( "Outer:" + exp.OuterTable.ObjectName + "(" + exp.OuterIndex + ")" );
				}

				if( exp.ObjectIndex != 0 )
				{
					Nodes.Add( "Object:" + exp.ObjectName + "(" + exp.ObjectIndex + ")" );
				}

				if( exp.ArchetypeIndex != 0 )
				{
					Nodes.Add( "Archetype:" + exp.ArchetypeName + "(" + exp.ArchetypeIndex + ")" );
				}

				if( exp.SerialSize > 0 )
				{
					Nodes.Add( "Object Size:" + exp.SerialSize );
					Nodes.Add( "Object Offset:" + exp.SerialOffset );
				}	
			
				#if DEBUG
					if( exp.Guid != Guid.Empty.ToString() )
					{
						Nodes.Add( "Guid:" + exp.Guid );
					}
				
					try
					{
						if( exp.ComponentMap != null && exp.ComponentMap.Count > 0 )
						{
							var n = Nodes.Add( "ComponentsMap" );
							foreach( var pair in exp.ComponentMap )
							{
								n.Nodes.Add( "Name:" + exp.Owner.GetIndexName( pair.Key ) + " Object:" + pair.Value );
							}
						}
					}
					catch{}

					try
					{
						if( exp.NetObjects != null && exp.NetObjects.Count > 0 )
						{
							var n = Nodes.Add( "NetObjects" );
							foreach( var obj in exp.NetObjects )
							{
								n.Nodes.Add( "Name:" + exp.Owner.GetIndexObjectName( obj ) + "(" + obj + ")" );
							}
						}
					}
					catch{}	
				#endif
				if( exp.Object != null && exp.Object.NetIndex != 0 )
				{
					Nodes.Add( "NetIndex:" + exp.Owner.GetIndexObjectName( exp.Object.NetIndex ) + "(" + exp.Object.NetIndex + ")" ); 
				}
				TreeView.EndUpdate();
			}
		}

		internal class ImportNode : UnrealTableNode
		{
			public ImportNode()
			{
				Nodes.Add( "DUMMYNODE" );
			}
			public override void Initialize()
			{
				if( _Initialized )
					return;

				_Initialized = true;

				TreeView.BeginUpdate();
				Nodes.Clear();
				var imp = Table as UImportTableItem;
				if( imp.ObjectIndex != 0 )
				{
					Nodes.Add( "Object:" + imp.ObjectName + "(" + imp.ObjectIndex + ")" );
				}

				if( imp.ClassIndex == 0 )
				{
					Nodes.Add( "Class:Class(" + imp.ClassIndex + ")" );
				}
				else
				{
					Nodes.Add( "Class:" + imp.ClassName + "(" + imp.ClassIndex + ")" );
				}

				if( imp.PackageIndex != 0 )
				{
					Nodes.Add( "ClassPackage:" + imp.PackageName + "(" + imp.PackageIndex + ")" );
				}

				if( imp.OuterIndex != 0 )
				{
					Nodes.Add( "Outer:" + imp.OuterName + "(" + imp.OuterIndex + ")" );
				}
				TreeView.EndUpdate();
			}
		}

		private ProgramForm _Form = null;

		private List<UClass> _ClassesList = null;

		public override void TabClosing()
		{
			base.TabClosing();

			ProgressStatus.ResetStatus();
			ProgressStatus.ResetValue();

			if( _UnrealPackage != null )
			{
				_UnrealPackage.Dispose();
				_UnrealPackage = null;
			}
		}

		private void _OnNotifyPackageEvent( object sender, UnrealPackage.PackageEventArgs e )
		{
			switch( e.EventId )
			{
				case UnrealPackage.PackageEventArgs.Id.Construct:
					ProgressStatus.SetStatus( "Constructing Objects..." );
					break;

				case UnrealPackage.PackageEventArgs.Id.Deserialize:
					ProgressStatus.SetStatus( "Deserializing Objects..." );
					break;

				case UnrealPackage.PackageEventArgs.Id.Import:
					ProgressStatus.SetStatus( "Importing Objects..." );
					break;

				case UnrealPackage.PackageEventArgs.Id.Link:
					ProgressStatus.SetStatus( "Linking Objects..." );
					break;
				
				case UnrealPackage.PackageEventArgs.Id.Object:
					ProgressStatus.IncrementValue();
					break;
			}
		}

		[MTAThreadAttribute]
		protected void InitializeUI()
		{
			ProgressStatus.SetStatus( "Initializing UI..." );

			// Disable misc' functionalities.
			//causesvalidation = false;
			SuspendLayout();

			exportDecompiledClassesToolStripMenuItem.Click 			+= _OnExportClassesClick;
			exportScriptClassesToolStripMenuItem.Click 				+= _OnExportScriptsClick;

			TreeView_Classes.AfterSelect 							+= _OnClassesNodeSelected;
			TreeView_Classes.BeforeExpand							+= _OnClassesNodeExpand;
			Button_FindObject.Click 								+= _OnFindObjectClicked;
			Button_FindName.Click 									+= _OnFindNameClicked;
			// Package Info

			// Section 1
			VersionValue.Text 			= (_UnrealPackage.Version.ToString());
			FlagsValue.Text 			= UnrealMethods.FlagToString( (_UnrealPackage.PackageFlags & ~(uint)PackageFlags.Protected) );
			LicenseeValue.Text 			= _UnrealPackage.LicenseeVersion.ToString();

			Label_GUID.Text				= _UnrealPackage.GUID;

			// Section 2	
			if( _UnrealPackage.Version >= 245 )
			{
				if( _UnrealPackage.EngineVersion > 0 )
				{
					Label_EngineVersion.Visible		= true;
					EngineValue.Visible				= true;
					EngineValue.Text 				=  _UnrealPackage.EngineVersion.ToString();
				}

				if( _UnrealPackage.Group != "None" )
				{
					Label_Folder.Visible			= true;
					FolderValue.Visible				= true;
					FolderValue.Text				= _UnrealPackage.Group;
				}
			}

			if( _UnrealPackage.Version >= UnrealPackage.VCOOKEDPACKAGES )
			{
				if( _UnrealPackage.CookerVersion > 0 )
				{
					Label_CookerVersion.Visible		= true;
					CookerValue.Visible				= true;
					CookerValue.Text				= _UnrealPackage.CookerVersion.ToString();
				}
			}

			BuildValue.Text = _UnrealPackage.Build.GameID.ToString();

			// Automatic iterate through all package flags and return them as a string list
			var flags = new List<string>
			{
			    "AllowDownload " + (_UnrealPackage.HasPackageFlag(PackageFlags.AllowDownload) ? "True" : "False"),
			    "ClientOptional " + (_UnrealPackage.HasPackageFlag(PackageFlags.ClientOptional) ? "True" : "False"),
			    "ServerSideOnly " + (_UnrealPackage.HasPackageFlag(PackageFlags.ServerSideOnly) ? "True" : "False")
			};

			if( _UnrealPackage.Version >= UnrealPackage.VCOOKEDPACKAGES )
			{
				flags.Add( "Cooked " + _UnrealPackage.IsCooked().ToString() );
				flags.Add( "Compressed " + _UnrealPackage.HasPackageFlag( PackageFlags.Compressed ) );
				flags.Add( "FullyCompressed " + _UnrealPackage.HasPackageFlag( PackageFlags.FullyCompressed ) );	
				flags.Add( "Debug " + _UnrealPackage.IsDebug().ToString() );
				flags.Add( "Script " + _UnrealPackage.IsScript().ToString() );
				flags.Add( "Stripped " + _UnrealPackage.IsStripped().ToString() );			
				flags.Add( "Map " + _UnrealPackage.IsMap().ToString() );
				flags.Add( "Console " + _UnrealPackage.IsBigEndian.ToString() );
			}
			else if( _UnrealPackage.Version > 61 && _UnrealPackage.Version <= 69 )		// <= UT99
			{
				//flags.Add( "Unsecure " + _UnrealPackage.HasPackageFlag( PackageFlags.Unsecure ) );
				flags.Add( "Encrypted " + _UnrealPackage.HasPackageFlag( PackageFlags.Encrypted ) );
			}

			foreach( string flag in flags )
			{
				var r = new DataGridViewRow();
	            r.CreateCells( DataGridView_Flags );
				string[] vals = flag.Split( new char[1] { ' ' } );
				r.SetValues( vals[0], vals[1] );
	            DataGridView_Flags.Rows.Add( r );
			}

			if( _UnrealPackage.ObjectsList != null )
			{
				TabPage_Objects.Text += " (" + _UnrealPackage.ObjectsList.Count + ")";

				TabPage_Tables.Text += " (" + (_UnrealPackage.ImportTableList.Count +
					_UnrealPackage.ExportTableList.Count +
					_UnrealPackage.NameTableList.Count) + ")";
				TabPage_Names.Text += " (" + _UnrealPackage.NameTableList.Count + ")";
				TabPage_Exports.Text += " (" + _UnrealPackage.ExportTableList.Count + ")";
				TabPage_Imports.Text += " (" + _UnrealPackage.ImportTableList.Count + ")";

				CreateClassesList();
				// HACK:Add a MetaData object to the classes tree(hack because MetaData is not an actual class)
				var metobj = _UnrealPackage.FindObject( "MetaData", typeof( UMetaData ), false );
				if( metobj != null )
				{
					var node = new ObjectNode( metobj ) { ImageKey = "UClass", SelectedImageKey = "UClass", Text = metobj.Name };
					node.Nodes.Add( "DUMMYNODE" );
					if( metobj.SerializationState.HasFlag( UObject.ObjectState.Errorlized ) )
					{
						node.ForeColor = Color.Red;
					}
					TreeView_Classes.Nodes.Add( node );
				}

				try
				{
					CreateDependenciesList();
				}
				catch( Exception e )
				{
					// May happen, like on games such as Medal of Honor: Airborne.
					ExceptionDialog.Show( "An exception occurred when creating the dependencies table!", e );
				}

				try
				{
					CreateContentList();
				}
				catch( Exception e )
				{
					// May happen, like on games such as Medal of Honor: Airborne.
					ExceptionDialog.Show( "An exception occurred when creating the content table!", e );
				}
			}
			else
			{
				EmptyIsPackage();
			}

			if( _UnrealPackage.GenerationsList != null )
			{
				TabPage_Generations.Text += " (" + _UnrealPackage.GenerationsList.Count + ")";
				CreateGenerationsList();
			}

			if( _UnrealPackage.CompressedChunks != null )
			{
				foreach( var chunk in _UnrealPackage.CompressedChunks )
				{
					this.DataGridView_Chunks.Rows.Add( chunk.UncompressedOffset, chunk.UncompressedSize, chunk.CompressedOffset, chunk.CompressedSize );
				}
			}

			ResumeLayout();
			CausesValidation = true;
		}

		private void EmptyIsPackage()
		{
			Button_FindName.Enabled = false;
			Button_FindObject.Enabled = false;
			exportScriptClassesToolStripMenuItem.Enabled = false;
			exportDecompiledClassesToolStripMenuItem.Enabled = false;
		}

		private delegate void CreateTableDelegate( object nameTable );

		protected void CreateNameTable( object nameTable )
		{
			if( DataGridView_NameTable.InvokeRequired )
			{
				var del = new CreateTableDelegate( CreateNameTable );
				foreach( var t in _UnrealPackage.NameTableList )
				{
					Invoke( del, t );
				}
			}
			else
			{
				DataGridView_NameTable.Rows.Add( ((UNameTableItem)nameTable).Name, String.Format( "{0:x4}", (nameTable as UNameTableItem).Flags ) );
			}
		}

		private TreeNode[] _ExportNodes = null;
		private int _NIndex = 0;
		protected void CreateExportTable( object exportTable )
		{
			if( TreeView_Exports.InvokeRequired )
			{
				var del = new CreateTableDelegate( CreateExportTable );
				foreach( var t in _UnrealPackage.ExportTableList )
				{
					TreeView_Exports.Invoke( del, t );
				}
			}
			else
			{
				if( _NIndex == 0 )
				{
				    _ExportNodes = new TreeNode[_UnrealPackage.ExportTableList.Count];
				}

				var exp = exportTable as UExportTableItem;
				var node = new ExportNode {Table = exp, Text = exp.ObjectName};
				SetImageKeyForObject( exp, node );

				node.SelectedImageKey = node.ImageKey;
				_ExportNodes[_NIndex ++] = node;

				if( exp.Object != null && exp.Object.SerializationState.HasFlag( UObject.ObjectState.Errorlized ) )
				{
				    AddSerToolTipError( node, exp.Object );
				}
				else if( !_UnrealPackage.IsRegisteredClass( exp.ClassName != String.Empty ? exp.ClassName : "Class" ) )
				{
				    node.ForeColor = Color.DarkOrange;
				    node.ToolTipText = "Class " + exp.ClassName + " isn't supported";
				}

				if( _NIndex == _UnrealPackage.ExportTableList.Count )
				{
					TreeView_Exports.BeginUpdate();
				    TreeView_Exports.Nodes.AddRange( _ExportNodes );
					TreeView_Exports.EndUpdate();
					//_ExportNodes = null;
				    _NIndex = 0;
				}
			}
		}

		private void _OnExportNodeExpand( object sender, TreeViewCancelEventArgs e )
		{
			if( e.Node is ExportNode )
			{
				((ExportNode)e.Node).Initialize();
			}
		}

		protected void CreateImportTable( object importTable )
		{
			if( TreeView_Imports.InvokeRequired )
			{
				CreateTableDelegate del = new CreateTableDelegate( CreateImportTable );
				for( int i = 0; i < _UnrealPackage.ImportTableList.Count; ++ i )
				{
					Invoke( del, _UnrealPackage.ImportTableList[i] );
				}
			}
			else
			{
				if( _NIndex == 0 )
				{
					_ExportNodes = new TreeNode[_UnrealPackage.ImportTableList.Count];
				}

				var imp = (importTable as UImportTableItem);
				ImportNode node = new ImportNode
				{
				    Table = imp,
				    Text = imp.ObjectName
				};
				_ExportNodes[_NIndex ++] = node;

				SetImageKeyForObject( imp, node );


				if( _NIndex == _UnrealPackage.ImportTableList.Count )
				{
					TreeView_Exports.BeginUpdate();
					TreeView_Imports.Nodes.AddRange( _ExportNodes );
					TreeView_Exports.EndUpdate();
					_ExportNodes = null;
					_NIndex = 0;
				}
			}
		}	

		private void _OnImportNodeExpand( object sender, TreeViewCancelEventArgs e )
		{
			if( e.Node is ImportNode )
			{
				((ImportNode)e.Node).Initialize();
			}
		}

		private void _OnNotifyObjectAdded( object sender, ObjectEventArgs e )
		{
			if( e.ObjectRef.Table.ClassIndex == 0 && e.ObjectRef.Name.ToLower() != "none" )
			{
				_ClassesList.Add( (UClass)e.ObjectRef );
				return;
			}
		}

		protected void CreateClassesList()
		{
			if( _ClassesList == null || _ClassesList.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Classes );
				exportDecompiledClassesToolStripMenuItem.Enabled = false;
				exportScriptClassesToolStripMenuItem.Enabled = false;
				return;
			}

			_ClassesList.Sort( (cl, cl2) => cl.Name.CompareTo( cl2.Name ) );

			TreeView_Classes.BeginUpdate();
			foreach( var Object in _ClassesList )
			{			
				if( Object == null )
				{
					continue;
				}

				var node = new ObjectNode( Object )
				{
					ImageKey = "UClass", 
					SelectedImageKey = "UClass", 
					Text = Object.Name
				};
				node.Nodes.Add( "DUMMYNODE" );

				if( Object.SerializationState.HasFlag( UObject.ObjectState.Errorlized ) )
				{
					node.ForeColor = Color.Red;
				}
				TreeView_Classes.Nodes.Add( node );	
			}
			TreeView_Classes.EndUpdate();

			TabPage_Classes.Text += " (" + TreeView_Classes.Nodes.Count + ")";
		}

		protected void CreateDependenciesList()
		{
			if( _UnrealPackage.ObjectsList == null || _UnrealPackage.ObjectsList.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Deps );
				return;
			}

			foreach( var table in _UnrealPackage.ImportTableList )
			{										
				// Actually a group...
				if( table.OuterIndex != 0 )
					continue;

				if( table.ClassName == "Package" )
				{
					GetDependencyOn( table, TreeView_Deps.Nodes.Add( table.ObjectName ) );
				}
			}

			if( TreeView_Deps.Nodes.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Deps );
				return;
			}

			TabPage_Deps.Text += " (" + TreeView_Deps.Nodes.Count + ")";
		}

		protected void GetDependencyOn( UImportTableItem parent, TreeNode node )
		{
			if( node == null )
				return;

			foreach( var table in _UnrealPackage.ImportTableList.Where( table => table != parent && table.OuterTable == parent ) )
			{
				GetDependencyOn( table, node.Nodes.Add( table.ObjectName ) );
			}

			node.ToolTipText = "Class:" + parent.ClassName 
				+ "\r\nDependencies:" + node.Nodes.Count;
			SetImageKeyForObject( parent, node );
		}

		protected void SetImageKeyForObject( UObjectTableItem tableObject, TreeNode node )
		{
			if( tableObject.Object != null )
			{
				if( tableObject.Object.GetType().IsSubclassOf( typeof(UProperty) ) )
				{
					node.ImageKey = typeof(UProperty).Name;
				}
				else
				{
					if( tableObject.ClassName == "Package" )
					{
						node.ImageKey = "List";
					}
					else node.ImageKey = tableObject.Object.GetType().Name;	
				}
				node.SelectedImageKey = node.ImageKey;
			}
		}

		protected void CreateGenerationsList()
		{
			if( _UnrealPackage.GenerationsList == null || _UnrealPackage.GenerationsList.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Generations );
				return;
			}

			foreach( var gen in _UnrealPackage.GenerationsList )
			{
				DataGridView_GenerationsTable.Rows.Add( gen.NameCount, gen.ExportCount, gen.NetObjectCount );
			}
		}

		protected void CreateContentList()
		{
			if( _UnrealPackage.ObjectsList == null || _UnrealPackage.ObjectsList.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Content );
				return;
			}

			var groups = new List<ObjectNode>();
			foreach( var obj in _UnrealPackage.ObjectsList.Where
				(o => (
					(o.ResistsInGroup()) || o.HasObjectFlag( ObjectFlagsLO.Automated )) 
					&& o.ExportIndex > 0 
					&& o.Outer != null
				))
			{
				var groupNode = groups.Find( n => n.Text == obj.Outer.Name );
				if( groupNode == null )
				{
					groupNode = new ObjectNode( obj.Outer ) { Text = obj.Outer.Name };
					SetImageKeyForObject( obj.Outer.Table, groupNode );
					groups.Add( groupNode );
				}
				var objectNode = new ObjectNode( obj ){Text = obj.Name};
				SetImageKeyForObject( obj.Table, objectNode );
				groupNode.Nodes.Add( objectNode );
			}

			TreeView_Content.Nodes.AddRange( groups.ToArray() );
			if( TreeView_Content.Nodes.Count == 0 )
			{
				TabControl_Objects.Controls.Remove( TabPage_Content );
				return;
			}

			TreeView_Content.Sort();
			TabPage_Content.Text += " (" + TreeView_Content.Nodes.Count + ")";
		}

		private void _OnExportClassesClick( object sender, EventArgs e )
		{
			string packagePath = Application.StartupPath + "\\Decompiled\\" + Path.GetFileNameWithoutExtension( _UnrealPackage.FullPackageName ); 
			if( Directory.Exists( packagePath ) )
			{
				string[] files = Directory.GetFiles( packagePath );
				foreach( string file in files )
				{
					File.Delete( packagePath + file );
				}			
			}
			Directory.CreateDirectory( packagePath + "\\Classes" );
			foreach( UClass Object in _ClassesList )
			{
				File.WriteAllText( packagePath + "\\Classes\\" + Object.Name + UnrealExtensions.UnrealCodeExt, Object.Decompile() );
			}	
		   	CreateFlagsFile( packagePath );
			DialogResult DR = MessageBox.Show( "Decompiled all package classes of " + _UnrealPackage.FullPackageName + " to " + packagePath +
				"\r\n\r\nClick Yes if you want to go to the decompiled classes directory.", 
				Application.ProductName,
				MessageBoxButtons.YesNo );
			if( DR == DialogResult.Yes )
			{
				System.Diagnostics.Process.Start( packagePath + "\\Classes" );
			}
		}

		private void _OnExportScriptsClick( object sender, EventArgs e )
		{
			string packagePath = Application.StartupPath + "\\Exported\\" + Path.GetFileNameWithoutExtension( _UnrealPackage.FullPackageName ); 
			if( Directory.Exists( packagePath ) )
			{
				string[] files = Directory.GetFiles( packagePath );
				foreach( var file in files )
				{
					File.Delete( packagePath + file );
				}			
			}
			Directory.CreateDirectory( packagePath + "\\Classes" );
			foreach( var Object in _ClassesList )
			{
				if( Object.ScriptBuffer == null )
				{
					continue;
				}

				string output = Object.ScriptBuffer.Decompile();

				File.WriteAllText( packagePath + "\\Classes\\" + Object.Name + UnrealExtensions.UnrealCodeExt, output );
			}				
			CreateFlagsFile( packagePath );
			DialogResult DR = MessageBox.Show( "Exported all package classes of " + _UnrealPackage.FullPackageName + " to " + packagePath +
				"\r\n\r\nClick Yes if you want to go to the exported classes directory.", 
				Application.ProductName,
				MessageBoxButtons.YesNo );
			if( DR == DialogResult.Yes )
			{
				System.Diagnostics.Process.Start( packagePath + "\\Classes" );
			}
		}

		private void CreateFlagsFile( string packagePath )
		{
			var upkgContent = new string[4];
			upkgContent[0] = "[Flags]";
			upkgContent[1] = "AllowDownload=" + (_UnrealPackage.HasPackageFlag( PackageFlags.AllowDownload) ? "True" : "False");
			upkgContent[2] = "ClientOptional=" + (_UnrealPackage.HasPackageFlag( PackageFlags.ClientOptional) ? "True" : "False");
			upkgContent[3] = "ServerSideOnly=" + (_UnrealPackage.HasPackageFlag( PackageFlags.ServerSideOnly) ? "True" : "False");
			File.WriteAllLines( packagePath + "\\Classes\\" + Path.GetFileNameWithoutExtension( _UnrealPackage.FullPackageName ) + UnrealExtensions.UnrealFlagsExt, upkgContent );
		}

		internal void ReloadPackage()
		{
			Owner.RemoveTab( this );
			Owner.Owner.LoadFile( FileName );
		}

		private void OutputNodeObject( TreeNode treeNode )
		{
			try
			{
				var newTitle = String.Empty;
				if( treeNode is IDecompilableNode )
				{
					SetContentText( treeNode, ((IDecompilableNode)treeNode).Decompile() );

					// Assemble a title
					if( treeNode is IDecompilableObjectNode )
					{
						UObject obj;
						if( (obj = ((IDecompilableObjectNode)treeNode).Object as UObject) != null )
						{
							newTitle = obj.GetOuterGroup();
							if( obj.SerializationState.HasFlag( UObject.ObjectState.Errorlized ) )
							{
								AddSerToolTipError( treeNode, obj );
							}
						}
						else
						{
							newTitle = treeNode.Text;
						}
					}
					else
					{
						if( treeNode.Parent != null )
						{
							newTitle = treeNode.Parent.Text + "." + treeNode.Text;
						}
						else
						{
							newTitle = treeNode.Text;
						}
					}
					Label_ObjectName.Text = newTitle;
				}
			}
			catch( Exception except )
			{
				ExceptionDialog.Show( "An exception occurred while attempting to display content of node: " + treeNode.Text, except );
				treeNode.ForeColor = Color.Red;
				treeNode.ToolTipText = except.Message;
			}
		}

		private void _OnClassesNodeSelected( object sender, TreeViewEventArgs e )
		{
			OutputNodeObject( e.Node );
		}

		private void AddSerToolTipError( TreeNode node, UObject errorObject )
		{
			node.ForeColor = Color.Red;
			node.ToolTipText = GetExceptionMessage( errorObject );
		}

		private string GetExceptionMessage( UObject errorObject )
		{
			return "Deserialization failed by the following exception:\n" 
				+ errorObject.ThrownException.Message 
				+ "\n\nOccurred on position:" 
					+ errorObject.ExceptionPosition + "/" + errorObject.ExportTable.SerialSize
				+ "\n\nStackTrace:" + errorObject.ThrownException.StackTrace;
		}

		private void _OnExportsNodeSelected( object sender, TreeViewEventArgs e )
		{
			OutputNodeObject( e.Node );
		}

		private void _OnClassesNodeExpand( object sender, TreeViewCancelEventArgs e )
		{		
			try
			{
				var node = e.Node as ObjectNode;
				// This makes sure that this only applies to the first Nodes of TreeView_Classes, and only the first time!
				if( node != null && !(node.Object as UObject).InitializedNodes )
				{
					TreeView_Classes.BeginUpdate();
					// Clear DUMMYNODE
					node.Nodes.Clear();
					((UObject)node.Object).InitializeNodes( node );
					TreeView_Classes.EndUpdate();
				}
			}
			catch( Exception except )
			{
				ExceptionDialog.Show( "An exception occurred while initializing this node: " + e.Node.Text, except );
				e.Node.ForeColor = Color.Red;
				e.Node.ToolTipText = except.Message;
			}
		}

		private void TreeView_Classes_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
		{
			if( e.Button != MouseButtons.Right ) 
				return;

			ShowNodeContextMenuStrip( TreeView_Classes, e, _OnClassesItemClicked );	
		}

		private void TreeView_Exports_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
		{
			if( e.Button != MouseButtons.Right ) 
				return;

			ShowNodeContextMenuStrip( TreeView_Exports, e, _OnExportsItemClicked );
		}

		private bool _SuppressNodeSelect = false;
		private void TreeView_Content_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
		{
			switch( e.Button )
			{
				case System.Windows.Forms.MouseButtons.Left:	
					//PerformNodeAction( e.Node as IDecompileableObjectNode, "View Object" );
					break;

				case System.Windows.Forms.MouseButtons.Right:
					_SuppressNodeSelect = true;
					ShowNodeContextMenuStrip( TreeView_Content, e, _OnContentItemClicked );
					break;
			}

			CheckIfNodeIsExportable( e.Node );
		}

		private void ShowNodeContextMenuStrip( TreeView tree, TreeNodeMouseClickEventArgs e, ToolStripItemClickedEventHandler itemClicked )
		{
			tree.SelectedNode = e.Node;

			var viewToolsContextMenu = new ContextMenuStrip();
			BuildItemNodes( e.Node, viewToolsContextMenu.Items );
			if( viewToolsContextMenu.Items.Count == 0 )
				return;

			viewToolsContextMenu.ItemClicked += itemClicked;
			viewToolsContextMenu.Show( tree, e.Location );	
		}

		private void BuildItemNodes( TreeNode performingNode, ToolStripItemCollection itemCollection )
		{
			itemCollection.Clear();

			var decompilableNode = performingNode as IDecompilableNode;
			if( decompilableNode != null ) 
			{ 
				itemCollection.Add( "View Object" );
				var decompilableObjectNode = decompilableNode as IDecompilableObjectNode;
				if( decompilableObjectNode != null )
				{
					if( decompilableObjectNode.Object is UContent )
					{ 
						if( File.Exists( Program.Options.UEModelAppPath	) )
						{
							itemCollection.Add( "Open in UE Model Viewer" );
		#if DEBUG
							itemCollection.Add( "Export with UE Model Viewer" );
		#endif
						}
		#if DEBUG
						itemCollection.Add( "View Content" );
		#endif	
					}

					if( decompilableObjectNode.Object is UMetaData )
					{
						itemCollection.Add( "View Used Tags" );	
					}

					if( decompilableObjectNode.Object is UStruct )
					{
						var unStruct = (decompilableObjectNode.Object as UStruct); 
						if( unStruct.ScriptSize > 0 )
						{
							if( decompilableObjectNode.Object is UClass )
							{
								itemCollection.Add( "View Replication" );	
							}
							itemCollection.Add( "View Tokens" );
						}

						if( decompilableObjectNode.Object is UClass )
						{
							itemCollection.Add( "View Script" );	
						}

						if( unStruct.Properties != null )
						{
							itemCollection.Add( "View DefaultProperties" );	
						}
					}

					if( (decompilableObjectNode.Object as ISupportsBuffer) != null )
					{ 
						itemCollection.Add( "View Buffer" );
					}

					var myObj = decompilableObjectNode.Object as UObject;
					if( myObj != null && myObj.ThrownException != null )
					{
						itemCollection.Add( new ToolStripSeparator() );
						itemCollection.Add( "View Exception" );		
					}

					if( decompilableObjectNode.Object is IUnrealDeserializableObject )
					{
						itemCollection.Add( new ToolStripSeparator() );
						itemCollection.Add( "Managed Properties" );
						#if DEBUG
							itemCollection.Add( "Force Deserialize" );
						#endif	
					}
				}
			}
		}

		private void _OnClassesItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( TreeView_Classes.SelectedNode is ObjectNode )
			{
				PerformNodeAction( TreeView_Classes.SelectedNode as ObjectNode, e.ClickedItem.Text );
			}			
		}

		private void _OnExportsItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( TreeView_Exports.SelectedNode is ExportNode )
			{
				PerformNodeAction( TreeView_Exports.SelectedNode as ExportNode, e.ClickedItem.Text );
			}	
		}

		private void _OnContentItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( TreeView_Content.SelectedNode is ObjectNode )
			{
				PerformNodeAction( TreeView_Content.SelectedNode as ObjectNode, e.ClickedItem.Text );
			}	
		}

		private void PerformNodeAction( IDecompilableObjectNode node, string action )
		{
			if( node == null )
				return;

			try
			{
				switch( action )
				{
					case "View Used Tags":
					{
						var n = node as ObjectNode;
						if( n != null )
						{
							var cnode = n.Object as UMetaData;
							if( cnode != null )
							{
								Label_ObjectName.Text = node.Text;
								SetContentText( node as TreeNode, cnode.GetUniqueMetas() );
							}
						}
						break;
					}

#if DEBUG
					case "View Content":
					{
						var n = node as ObjectNode;
						if( n != null )
						{
							var cnode = n.Object as UContent;
							if( cnode is UTexture )
							{
								var tex = ((UTexture)cnode);
								tex.BeginDeserializing();
								if( tex.MipMaps == null || tex.MipMaps.Count == 0 )
									break;

								Image picture = new Bitmap( (int)tex.MipMaps[0].Width, (int)tex.MipMaps[0].Height );
								var painter = System.Drawing.Graphics.FromImage( picture );
								painter.Clear( Color.White );

								for( int x = 0; x < tex.MipMaps[0].Width; ++ x )
								{
									for( int y = 0; y < tex.MipMaps[0].Height; ++ y )
									{
										painter.FillEllipse( new SolidBrush( Color.FromArgb( tex.MipMaps[0].Pixels[x + y] ) ), x, y, 1, 1 );
									}
								}
								painter.Dispose();
								//TexturePreview.Image = picture;
							}
						}
						break;
					}
#endif
					case "Open in UE Model Viewer":
					{
						System.Diagnostics.Process.Start( 
							Program.Options.UEModelAppPath, 
							"-path=" + _UnrealPackage.PackageDirectory
							+ " " + _UnrealPackage.PackageName
							+ " " + node.Text
						);
						break;
					}

					case "Export with UE Model Viewer":
					{
						string packagePath = Application.StartupPath 
							+ "\\Exported\\" 
							+ _UnrealPackage.PackageName; 

						string contentDir = packagePath + "\\Content"; 
						Directory.CreateDirectory( contentDir );
						var appArguments = "-path=" + _UnrealPackage.PackageDirectory
							+ " " + "-out="  + contentDir
							+ " -export"
							+ " " + _UnrealPackage.PackageName
							+ " " + node.Text;
						var appInfo = new System.Diagnostics.ProcessStartInfo(
							Program.Options.UEModelAppPath, 
							appArguments
						);
						appInfo.UseShellExecute = false;
						appInfo.RedirectStandardOutput = true;
						appInfo.CreateNoWindow = false;
						var app = System.Diagnostics.Process.Start( appInfo );
						var log = String.Empty;
						app.OutputDataReceived += delegate
						(object sender, System.Diagnostics.DataReceivedEventArgs e)
						{
							log += e.Data;
							return;
						};
						//app.WaitForExit();

						if( Directory.GetFiles( contentDir ).Length > 0 )
						{
							if( MessageBox.Show( 
								Resources.UC_PackageExplorer_PerformNodeAction_QUESTIONEXPORTFOLDER, 
								Application.ProductName,
								MessageBoxButtons.YesNo 
								) == DialogResult.Yes )
							{
								System.Diagnostics.Process.Start( contentDir );
							}
						}
						else
						{
							MessageBox.Show( 
								string.Format( 
									"The object was not exported.\r\n\r\nArguments:{0}\r\n\r\nLog:{1}", 
									appArguments, 
									log 
								),
								Application.ProductName 
							);
						}
						break;
					}						

					case "View Object":
						Label_ObjectName.Text = node.Text;
						SetContentText( node as TreeNode, node.Decompile() );
						break;

					case "Managed Properties":
						var propDialog = new PropertiesDialog();
						propDialog.ObjectLabel.Text = node.Text;
						propDialog.ObjectPropertiesGrid.SelectedObject = node.Object;
						propDialog.ShowDialog( this );
						break;

#if DEBUG
					case "Force Deserialize":
						Label_ObjectName.Text = node.Text;

						((IUnrealDeserializableObject)node.Object).BeginDeserializing();
						((UObject)node.Object).PostInitialize();
						break;
#endif

					case "View Replication":
					{
						var unClass = node.Object as UClass;
						if( unClass != null )
						{
							Label_ObjectName.Text = unClass.Name;
							SetContentText( node as TreeNode, unClass.FormatReplication() );
						}
						break;
					}

					case "View Script":
					{
						var unClass = node.Object as UClass;
						if( unClass != null && unClass.ScriptBuffer != null )
						{
							Label_ObjectName.Text = unClass.ScriptBuffer.Name;
							SetContentText( node as TreeNode, unClass.ScriptBuffer.Decompile() );
						}
						break;
					}

					case "View DefaultProperties":
					{
						var unStruct = node.Object as UStruct;
						if( unStruct != null )
						{
							Label_ObjectName.Text = unStruct.Name;
							SetContentText( node as TreeNode, unStruct.FormatDefaultProperties() );
						}
						break;
					}

					case "View Tokens":
					{
						var unStruct = node.Object as UStruct;
						if( unStruct != null && unStruct.ScriptSize > 0 )
						{
							Label_ObjectName.Text = node.Text;

							var codeDec = unStruct.ByteCodeManager;
							codeDec.Deserialize();
							codeDec.InitDecompile();

							string content = String.Empty;
							while( codeDec.CurrentTokenIndex + 1 < codeDec.DeserializedTokens.Count )
							{
								var t = codeDec.NextToken;
								int orgIndex = codeDec.CurrentTokenIndex;
								string output;
								bool breakOut = false;
								try
								{
									output = t.Decompile();
								}
								catch
								{
									output = "Exception occurred while decompiling token: " + t.GetType().Name;
									breakOut = true;
								}
								string chain = t.GetType().Name.Substring( 0, t.GetType().Name.Length - 5 ) 
									+ "(" + t.Size + ")";
								int inlinedTokens = codeDec.CurrentTokenIndex - orgIndex;
								if( inlinedTokens > 0 )
								{
									++ orgIndex;
									for( int i = 0; i < inlinedTokens; ++ i )
									{
										//var breakLine = i > 0 && i % 3 == 0;
										//if( breakLine )
										//{
										//    chain += "\r\n\t\t";
										//}
										var tokenName = codeDec.DeserializedTokens[orgIndex + i].GetType().Name; 
										chain += " -> " 
											+ tokenName.Substring( 0, tokenName.Length - 5 ) 
											+ "(" + codeDec.DeserializedTokens[orgIndex + i].Size + ")";
									}
								}

								content += "(0x" + String.Format( "{0:x3}", t.Position ).ToUpper() + ") " + chain 
									+ (output != String.Empty ? "\r\n\t" + output + "\r\n" : "\r\n");

								if( breakOut )
									break;
							}
							SetContentText( node as TreeNode, content );
						}
						break;
					}

					case "View Buffer":
					{
						var obj = node.Object as UObject;
						var HVD = new HexViewDialog( obj, this );
						HVD.Show( _Form );
						HVD.ShowInTaskbar = true;
						break;
					}

					case "View Exception":
					{
						var ONode = node as ObjectNode;
						if( ONode != null )
						{
							SetContentText( ONode, GetExceptionMessage( ((UObject)ONode.Object) ) );
						}
						break;
					}
				}
			}
			catch( Exception e )
			{
				ExceptionDialog.Show( "An exception occurred while performing: " + action, e );
			}
		}

		private void _OnFindObjectClicked( object sender, EventArgs e )
		{
			int objectIndexToFind = (int)Num_ObjectIndex.Value;
			try
			{
				var foundObject = _UnrealPackage.GetIndexObject( objectIndexToFind );
				if( foundObject != null )
				{
					MessageBox.Show( Resources.OBJECT_IS + foundObject.Name, Application.ProductName );
				}
				else
				{
					MessageBox.Show( Resources.NO_OBJECT_WAS_FOUND, Application.ProductName );
				}
			}
			catch( System.ArgumentOutOfRangeException exc )
			{
				MessageBox.Show( Resources.INVALID_OBJECT_INDEX + exc.ActualValue, Application.ProductName );
			}
		}

		private void _OnFindNameClicked( object sender, EventArgs e )
		{
			try
			{
				MessageBox.Show( Resources.NAME_IS + _UnrealPackage.NameTableList[(int)Num_NameIndex.Value].Name, Application.ProductName );
			}
			catch( System.ArgumentOutOfRangeException exc )
			{
				MessageBox.Show( Resources.INVALID_NAME_INDEX + exc.ActualValue, Application.ProductName );
			}
		}

		private void ToolStripButton_Find_Click( object sender, EventArgs e )
		{
			if( this.TextEditorPanel == null )
			{
				return;
			}
			EditorUtil.FindText( this.TextEditorPanel, SearchBox.Text );
		}

		private void SearchBox_KeyPress_1( object sender, KeyPressEventArgs e )
		{
			if( this.TextEditorPanel == null )
			{
				return;
			}

			if( e.KeyChar == '\r' )
			{
				EditorUtil.FindText( this.TextEditorPanel, SearchBox.Text );	  
				e.Handled = true;
			}
		}

		public override void TabFind()
		{
			new FindDialog( this.TextEditorPanel ).Show();
		}

		private struct BufferData
		{
			public string Text;
			public string Label;
			public TreeNode Node;
		}
		private readonly List<BufferData> _ContentBuffer = new List<BufferData>();
		private int _BufferIndex = -1;
		private TreeNode _LastNodeContent;

		private void SetContentText( TreeNode node, string content, bool skip = false )
		{
			if( _LastNodeContent != node )
			{
				BuildItemNodes( node, ViewTools.DropDownItems );
			}
			_LastNodeContent = node;

			content = content.TrimStart( '\r', '\n' ).TrimEnd( '\r', '\n' );
			if( content.Length > 0 )
			{
				FindButton.Enabled = true;
				SearchBox.Enabled = true;
				ExportButton.Enabled = true;
				WPFHost.Enabled = true;
				ViewTools.Enabled = true;
			}

			//TextEditorPanel.textEditor.Clear();
			TextEditorPanel.textEditor.Text = content;
			TextEditorPanel.textEditor.ScrollToHome();

			if( skip )
				return;

			if( _ContentBuffer.Count > 0 )
			{
				// No need to buffer the same content
				if( _BufferIndex > -1 && _BufferIndex < _ContentBuffer.Count && _ContentBuffer[_BufferIndex].Node == node )
				{
					return;
				}

				// Clean all above buffers when a new node was user-selected
				if( (_ContentBuffer.Count - 1) - _BufferIndex > 0 )
				{
					_ContentBuffer.RemoveRange( _BufferIndex, _ContentBuffer.Count - _BufferIndex );
					_BufferIndex = _ContentBuffer.Count - 1;
					NextButton.Enabled = false;
				}
			}

			var bd = new BufferData{ Text = content, Node = node, Label = Label_ObjectName.Text };
			_ContentBuffer.Add( bd );

			// Maximum 10 can be buffered; remove last one
			if( _ContentBuffer.Count > 10 )
			{
				_ContentBuffer.RemoveRange( 0, 1 );
			}
			else ++ _BufferIndex;

			if( _BufferIndex > 0 )
			{
				PrevButton.Enabled = true;
			}
		}

		private void ToolStripButton_Backward_Click( object sender, EventArgs e )
		{
			if( _BufferIndex - 1 > -1 )
			{
				FilterText.Text = String.Empty;
				-- _BufferIndex;
				Label_ObjectName.Text = _ContentBuffer[_BufferIndex].Label;
				SetContentText( _ContentBuffer[_BufferIndex].Node, _ContentBuffer[_BufferIndex].Text, true );
				SelectNode( _ContentBuffer[_BufferIndex].Node );

				if( _BufferIndex == 0 )
				{
					PrevButton.Enabled = false;
				}

				NextButton.Enabled = true;
			}
		}

		private void ToolStripButton_Forward_Click( object sender, EventArgs e )
		{
			if( _BufferIndex + 1 < _ContentBuffer.Count )
			{
				FilterText.Text = String.Empty;
				++ _BufferIndex;
				Label_ObjectName.Text = _ContentBuffer[_BufferIndex].Label;
				SetContentText( _ContentBuffer[_BufferIndex].Node, _ContentBuffer[_BufferIndex].Text, true );
				SelectNode( _ContentBuffer[_BufferIndex].Node );

				if( _BufferIndex == _ContentBuffer.Count-1 )
				{
					NextButton.Enabled = false;
				}
				PrevButton.Enabled = true;
			}
		}

		private void SelectNode( TreeNode node )
		{
			if(	node != null )
			{
				if( node.TreeView.Name == "TreeView_Classes" )
				{
					node.TreeView.AfterSelect -= _OnClassesNodeSelected;
				}
				node.TreeView.Show(); 
				node.TreeView.Select();
				node.TreeView.SelectedNode = node;
				if( node.TreeView.Name == "TreeView_Classes" )
				{
					node.TreeView.AfterSelect += _OnClassesNodeSelected;
				}
			}
		}

		private void TabControl_General_Selected( object sender, TabControlEventArgs e )
		{
			if( e.TabPage == TabPage_Tables )
			{
				if( DataGridView_NameTable.Rows.Count == 0 )
				{
					new Thread( () => CreateNameTable( null ) ).Start();
					TabControl_General.Selected -= TabControl_General_Selected;
				}
			}
		}

		private void TabControl_Tables_Selected( object sender, TabControlEventArgs e )
		{
			if( e.TabPage == TabPage_Exports )
			{
				if( TreeView_Exports.Nodes.Count == 0 )
				{
					new Thread( () => CreateExportTable( null ) ).Start();
					TreeView_Exports.BeforeExpand += _OnExportNodeExpand; 
				}
				return;
			}

			if( e.TabPage == TabPage_Imports )
			{
				if( TreeView_Imports.Nodes.Count == 0 )
				{
					new Thread( () => CreateImportTable( null ) ).Start();
					TreeView_Imports.BeforeExpand += _OnImportNodeExpand; 
				}
			}
		}

		private void viewBufferToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var obj = new UPackageObject( _UnrealPackage, _SummarySize );
			var HVD = new HexViewDialog( obj, this );
			HVD.Show( _Form );
			HVD.ShowInTaskbar = true;
		}

		private List<TreeNode> _FilteredNodes = new List<TreeNode>();

		private void FilterText_TextChanged( object sender, EventArgs e )
		{
			for( int i = 0; i < TreeView_Classes.Nodes.Count; ++ i )
			{
				if( TreeView_Classes.Nodes[i].Text.IndexOf( FilterText.Text, StringComparison.OrdinalIgnoreCase ) == -1 )
				{
					_FilteredNodes.Add( TreeView_Classes.Nodes[i] );
					TreeView_Classes.Nodes[i].Remove();
					-- i;
				}
			}

			for( int i = 0; i < _FilteredNodes.Count; ++ i )
			{
				if( FilterText.Text == String.Empty || _FilteredNodes[i].Text.IndexOf( FilterText.Text, StringComparison.OrdinalIgnoreCase  ) >= 0 )
				{
					TreeView_Classes.Nodes.Add( _FilteredNodes[i] );
					_FilteredNodes.Remove( _FilteredNodes[i] ); 
					-- i;
				}	
			}

			TreeView_Classes.Sort();
		}

		private void ReloadButton_Click( object sender, EventArgs e )
		{
			ReloadPackage();
		}

		private Pen borderPen = new Pen( Color.FromArgb( 237, 237, 237 ) );
		private Pen linePen = new Pen( Color.White );
		private void panel4_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.DrawRectangle( borderPen, 0, 0, panel4.Width-1, panel4.Height-1 );
		}

		private void panel1_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.DrawRectangle( borderPen, 0, 0, panel1.Width-1, panel1.Height-1 );
		}

		private void toolStripSeparator1_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.FillRectangle( linePen.Brush, 2, 0, panel1.Width-4, panel1.Height );
			e.Graphics.DrawLine( borderPen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Bottom );
			e.Graphics.DrawLine( borderPen, e.ClipRectangle.Right-1, e.ClipRectangle.Top, e.ClipRectangle.Right-1, e.ClipRectangle.Bottom );
		}

		private void ToolStrip_Content_Paint( object sender, PaintEventArgs e )
		{
			e.Graphics.DrawRectangle( borderPen, 0, 0, ((Control)sender).Width-1, panel1.Height-1 );
		}

		private void TreeView_Deps_DrawNode( object sender, DrawTreeNodeEventArgs e )
		{
			//int num = ((TreeNode)sender).Nodes.Count;
			//if( num == 0 )
			//{
			//    return;
			//}

			//e.Graphics.DrawString( "x" + num, 
			//    TreeView_Deps.Font, borderPen.Brush,
			//    e.Bounds.Right - 32, ((float)(e.Bounds.Top - e.Bounds.Bottom))*0.5f	
			//);
		}

		private void FilterByClassCheckBox( object sender, EventArgs e )
		{
			var checkBox = ((CheckBox)sender);
			if( !checkBox.Checked )
			{
				TreeView_Exports.BeginUpdate();
				List<TreeNode> removedNodes = new List<TreeNode>();
				for( int i = 0; i < TreeView_Exports.Nodes.Count; ++ i )
				{
					if( TreeView_Exports.Nodes[i].ImageKey == checkBox.ImageKey )
					{
						removedNodes.Add( TreeView_Exports.Nodes[i] );
						TreeView_Exports.Nodes.RemoveAt( i );
					}
				}
				checkBox.Tag = removedNodes;
				TreeView_Exports.EndUpdate();
			}
			else
			{
				if( checkBox.Tag == null )
				{
					checkBox.Checked = false;
					return;
				}
				TreeView_Exports.Nodes.AddRange( (checkBox.Tag as List<TreeNode>).ToArray() );
			}		
		}

		private void Button_Export_Click( object sender, EventArgs e )
		{
			var exportableObject = ((ObjectNode)TreeView_Content.SelectedNode).Object as IUnrealExportable; 
			((UObject)exportableObject).BeginDeserializing();

			List<string> exts = exportableObject.ExportableExtensions.ToList();
			string extensions = String.Empty;
			foreach( string ext in exts )
			{
				extensions += string.Format( "{0}(*" + ".{0})|*.{0}", ext );
				if( ext != exts.Last() )
				{
					extensions += "|";
				}
			}
			var dialog = new SaveFileDialog(){Filter = extensions, FileName = ((UObject)exportableObject).Name};
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				var stream = new FileStream( dialog.FileName, FileMode.Create, FileAccess.Write );
				exportableObject.SerializeExport( exts[dialog.FilterIndex], stream );
				stream.Flush();
				stream.Close();
			}
		}

		private void CheckIfNodeIsExportable( TreeNode node )
		{
			bool exportable = false;

			var soundObject = ((ObjectNode)node).Object as IUnrealExportable;
			if( soundObject != null )
			{
				exportable = soundObject.CompatableExport();	
		
				Button_Export.Text = string.Format( "Export {0} As...", node.Text );	
			}

			Button_Export.Enabled = exportable;
			Button_Export.Refresh();
		}

		private void TreeView_Content_AfterSelect( object sender, TreeViewEventArgs e )
		{
			// Selection shouldn't view object, for example on contextmenu selection.
			if( _SuppressNodeSelect )
			{
				_SuppressNodeSelect = false;
				return;
			}
			PerformNodeAction( e.Node as IDecompilableObjectNode, "View Object" );
		}

		private void ViewTools_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( _LastNodeContent == null )
				return;

			var decompilableObject = _LastNodeContent as IDecompilableObjectNode;
			if( decompilableObject == null )
			{
				return;
			}

			PerformNodeAction( decompilableObject, e.ClickedItem.Text );
		}
	}

	internal class UPackageObject : UObject
	{
		private readonly byte[] _MyBufferData;

		public UPackageObject( UnrealPackage package, long summarySize )
		{
			_MyBufferData = new byte[summarySize];
			package.Stream.Position = 0;
			package.Stream.Read( _MyBufferData, 0, (int)summarySize );
			//SwitchPackage( package );

			Name = package.PackageName;
		}

		/// <inheritdoc/>
		public override byte[] GetBuffer()
		{
			return _MyBufferData;
		}
	}
}
