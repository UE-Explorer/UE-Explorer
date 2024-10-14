using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Storm.TabControl;
using UEExplorer.Properties;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Nodes;

namespace UEExplorer.UI.Tabs
{
    using Dialogs;

    using UELib;
    using UELib.Core;
    using UELib.Flags;

    [System.Runtime.InteropServices.ComVisible( false )]
    public partial class UC_PackageExplorer : UserControl_Tab
    {
        public string FileName{ get; set; }

        /// <summary>
        /// My Unreal file package!
        /// </summary>
        private UnrealPackage _UnrealPackage;

        public UC_PackageExplorer()
        {
            InitializeComponent();
        }

        private void UC_PackageExplorer_Load(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = Settings.Default.PackageExplorer_SplitterDistance;

            // Fold all { } blocks
            var foldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(TextEditorPanel.TextEditor.TextArea);
            var foldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingManager, TextEditorPanel.TextEditor.Document);

            var langPath = Path.Combine(Application.StartupPath, "Config", "UnrealScript.xshd");
            if (File.Exists(langPath))
            {
                using (var stream = new System.Xml.XmlTextReader(langPath))
                {
                    TextEditorPanel.TextEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
                        stream,
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance
                    );
                }
            }

            TextEditorPanel.searchWiki.Click += SearchWiki_Click;
            TextEditorPanel.searchDocument.Click += SearchDocument_Click;
            TextEditorPanel.searchPackage.Click += SearchClasses_Click;
            TextEditorPanel.searchObject.Click += SearchObject_Click;
            TextEditorPanel.TextEditor.ContextMenuOpening += ContextMenu_ContextMenuOpening;
            TextEditorPanel.copy.Click += Copy_Click;
            _State = Program.Options.GetState( FileName );
        }

        void Copy_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            TextEditorPanel.TextEditor.Copy();
        }

        string GetSelection()
        {
            return TextEditorPanel.TextEditor.TextArea.Selection.GetText();
        }

        void ContextMenu_ContextMenuOpening( object sender, System.Windows.Controls.ContextMenuEventArgs e )
        {
            if( TextEditorPanel.TextEditor.TextArea.Selection.Length == 0 )
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                TextEditorPanel.searchDocument.Visibility = System.Windows.Visibility.Collapsed;
                TextEditorPanel.searchObject.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            var selection = GetSelection();
            if( selection.IndexOf( '\n' ) != -1 )
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            
            TextEditorPanel.searchDocument.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchObject.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchWiki.Header = String.Format( Resources.SEARCH_WIKI_ITEM, selection );

            TextEditorPanel.searchDocument.Header = findInDocumentToolStripMenuItem.Text;
            TextEditorPanel.searchPackage.Header = findInClassesToolStripMenuItem.Text;
            TextEditorPanel.searchObject.Header = Resources.SEARCH_AS_OBJECT;
        }

        void SearchWiki_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            Process.Start( String.Format( Resources.URL_UnrealWikiSearch, GetSelection() ) );
        }

        void SearchDocument_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            SearchBox.Text = GetSelection();
            FindButton.PerformClick();
        }

        void SearchClasses_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            SearchBox.Text = GetSelection();
            findInClassesToolStripMenuItem.PerformClick();
        }

        void SearchObject_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            DoSearchObjectByGroup( GetSelection() );
        }

        private XMLSettings.State _State;

        public void InitializeFromFile(string filePath)
        {
            ProgressStatus.SaveStatus();
            
            try
            {
                LoadPackage();
            }
            catch (Exception exception)
            {
                throw new UnrealException("Couldn't load or initialize package", exception);
            }
            finally
            {
                ProgressStatus.ResetStatus();
            }
        }

        private void LoadPackage()
        {
            SuspendLayout();
            if (Program.Options.bForceLicenseeMode)
            {
                UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;
            }

            if (Program.Options.bForceVersion)
            {
                UnrealPackage.OverrideVersion = Program.Options.Version;
            }

        reload:
            ProgressStatus.SetStatus(Resources.PACKAGE_LOADING);
            // Open the file.
            try
            {
                // HACK: temporary workaround for legacy UE Explorer code, in order to suppress foreign file signatures without having to modify UELib.
                var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4];
                int read = stream.Read(buffer, 0, 4);
                stream.Close();

                uint signature = BitConverter.ToUInt32(buffer, 0);

                if (read == 4 && (
                        // 0x9E2A83C1
                        signature != UnrealPackage.Signature &&
                        signature != UnrealPackage.Signature_BigEndian
                        // Killing Floor
                        && signature != 0x9E2A83C2
                        // Hawken
                        && signature != 0xEA31928C
                    ))
                {
                    if (MessageBox.Show(
                            Resources.PACKAGE_UNKNOWN_SIGNATURE,
                            Resources.Warning, MessageBoxButtons.YesNo
                        ) == DialogResult.No
                       )
                    {
                        ((ProgramForm)ParentForm).Tabs.CloseTab((TabStripItem)Parent);
                        return;
                    }

                }

                UnrealConfig.SuppressSignature = true;
                _UnrealPackage = UnrealLoader.LoadPackage(FileName);
                UnrealConfig.SuppressSignature = false;

                if (_UnrealPackage.Summary.CompressedChunks != null && _UnrealPackage.Summary.CompressedChunks.Any())
                {
                    if (MessageBox.Show(Resources.PACKAGE_IS_COMPRESSED,
                            Resources.NOTICE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question
                        ) == DialogResult.OK)
                    {
                        Process.Start("http://www.gildor.org/downloads");
                        MessageBox.Show(Resources.COMPRESSED_HOWTO,
                            Resources.NOTICE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information
                        );
                    }

                    TabControl_General.Selected -= TabControl_General_Selected;
                    TabControl_General.TabPages.Remove(TabPage_Objects);
                    TabControl_General.TabPages.Remove(TabPage_Tables);
                    InitializeMetaInfo();
                    InitializeUI();
                    return;
                }

                TabControl_General.TabPages.Remove(TabPage_Chunks);
            }
            catch (Exception e)
            {
                throw new UnrealException(e.Message, e);
            }

            string ntlPath = Path.Combine(Application.StartupPath, "Native Tables", Program.Options.NTLPath);
            if (File.Exists(ntlPath + NativesTablePackage.Extension))
            {
                // Load the native names.
                try
                {
                    _UnrealPackage.NTLPackage = new NativesTablePackage();
                    _UnrealPackage.NTLPackage.LoadPackage(ntlPath);
                }
                catch (Exception e)
                {
                    _UnrealPackage.NTLPackage = null;
                    throw new UnrealException
                    (
                        String.Format("Couldn't load {0}! \r\nEvent:Loading Package", ntlPath), e
                    );
                }
            }

            InitializeMetaInfo();
            InitializePackage();
            ResumeLayout();
        }

        private void InitializeMetaInfo()
        {
            // Section 1
            VersionValue.Text 	= (_UnrealPackage.Version.ToString( CultureInfo.InvariantCulture ));
            FlagsValue.Text 	= UnrealMethods.FlagToString( (_UnrealPackage.PackageFlags & ~(uint)PackageFlags.Protected) );
            LicenseeValue.Text 	= _UnrealPackage.LicenseeVersion.ToString( CultureInfo.InvariantCulture );
            Label_GUID.Text		= _UnrealPackage.GUID;

            // Section 2	
            if( _UnrealPackage.Version >= 245 )
            {
                if( _UnrealPackage.EngineVersion > 0 )
                {
                    Label_EngineVersion.Visible		= true;
                    EngineValue.Visible				= true;
                    var v = _UnrealPackage.EngineVersion & 0x0000FFFF;
                    var l = _UnrealPackage.EngineVersion >> 16;
                    EngineValue.Text =  l > 0 
                        ? String.Format( "{0}/{1}", v, l.ToString( CultureInfo.InvariantCulture ).PadLeft( v.ToString( CultureInfo.InvariantCulture ).Length, '0' ) )
                        : v.ToString( CultureInfo.InvariantCulture );
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
                    var v = _UnrealPackage.CookerVersion & 0x0000FFFF;
                    var l = _UnrealPackage.CookerVersion >> 16;
                    CookerValue.Text = l > 0 
                        ? String.Format( "{0}/{1}", v, l.ToString( CultureInfo.InvariantCulture ).PadLeft( v.ToString( CultureInfo.InvariantCulture ).Length, '0' ) )
                        : v.ToString( CultureInfo.InvariantCulture );
                }
            }

            BuildValue.Text = _UnrealPackage.Build.Name.ToString();

            // Automatic iterate through all package flags and return them as a string list
            var flags = new List<string>
            {
                "AllowDownload " + _UnrealPackage.HasPackageFlag( PackageFlags.AllowDownload ),
                "ClientOptional " + _UnrealPackage.HasPackageFlag( PackageFlags.ClientOptional ),
                "ServerSideOnly " + _UnrealPackage.HasPackageFlag( PackageFlags.ServerSideOnly )
            };

            if( _UnrealPackage.Version >= UnrealPackage.VCOOKEDPACKAGES )
            {
                flags.Add( "Cooked " + _UnrealPackage.IsCooked() );
                flags.Add( "Compressed " + _UnrealPackage.HasPackageFlag( PackageFlags.Compressed ) );
                flags.Add( "FullyCompressed " + _UnrealPackage.HasPackageFlag( PackageFlags.FullyCompressed ) );	
                flags.Add( "Debug " + _UnrealPackage.IsDebug() );
                flags.Add( "Script " + _UnrealPackage.IsScript() );
                flags.Add( "Stripped " + _UnrealPackage.IsStripped() );			
                flags.Add( "Map " + _UnrealPackage.IsMap() );
                flags.Add( "Console " + _UnrealPackage.IsBigEndianEncoded );
            }
            else if( _UnrealPackage.Version > 61 && _UnrealPackage.Version <= 69 )		// <= UT99
            {
                flags.Add( "Encrypted " + _UnrealPackage.HasPackageFlag( PackageFlags.Encrypted ) );
            }

            foreach( var flag in flags )
            {
                var r = new DataGridViewRow();
                r.CreateCells( DataGridView_Flags );
                var vals = flag.Split( new[]{ ' ' } );
                r.SetValues( vals[0], vals[1] );
                DataGridView_Flags.Rows.Add( r );
            }
        }

        private void InitializePackage()
        {
            _ClassesList = new List<UClass>();
            _UnrealPackage.NotifyPackageEvent += _OnNotifyPackageEvent;
            _UnrealPackage.NotifyObjectAdded += _OnNotifyObjectAdded;

            ProgressStatus.ResetValue();
            int max = Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Construct ) 
                ? _UnrealPackage.Exports.Count + _UnrealPackage.Imports.Count 
                : 0;

            if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Deserialize ) )
                max += _UnrealPackage.Exports.Count;
                                
            // Importing objects has been disabled FIXME:
            /*if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Import ) )
                max += _UnrealPackage.ImportTableList.Count;*/

            if( Program.Options.InitFlags.HasFlag( UnrealPackage.InitFlags.Link ) )
                max += _UnrealPackage.Exports.Count;

            ProgressStatus.SetMaxProgress( max );
            ProgressStatus.Loading.Visible = true;

            try
            {
                Refresh();

                // False if set to 'Auto', by not assigning to CookerPlatform we let UELib runs its auto-detect course.
                if (Enum.TryParse(((ProgramForm)ParentForm).platformMenuItem.Text, out BuildPlatform platform))
                {
                    _UnrealPackage.CookerPlatform = platform;
                };
                
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
            finally
            {
                _UnrealPackage.NotifyObjectAdded -= _OnNotifyObjectAdded;
                _UnrealPackage.NotifyPackageEvent -= _OnNotifyPackageEvent;    
                ProgressStatus.ResetValue();
            }

            InitializeUI();

            if( ONLOAD_MESSAGE != null )
            {
                MessageBox.Show( ONLOAD_MESSAGE, Resources.PACKAGE_METAINFO, MessageBoxButtons.OK );
            }
        }

        private string ONLOAD_MESSAGE;

        private void ReadMetaInfo()
        {
            foreach( var obj in _UnrealPackage.Objects.Where( o => o is UConst && o.Name.StartsWith( "META_DECOMPILER" ) ) )
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

        private void ToolStripButton1_Click( object sender, EventArgs e )
        {
            using( var sfd = new SaveFileDialog{
                DefaultExt = "uc",
                Filter = String.Format( "{0}(*.uc)|*.uc", Resources.UnrealClassFilter ),
                FilterIndex = 1,
                Title = Resources.ExportTextTitle,
                FileName = Label_ObjectName.Text + UnrealExtensions.UnrealCodeExt
            } )
            {
                if( sfd.ShowDialog() == DialogResult.OK )
                {
                    File.WriteAllText( sfd.FileName, TextEditorPanel.TextEditor.Text );
                }
            }
        }
        
        private List<UClass> _ClassesList;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            Console.WriteLine( "Disposing UC_PackageExplorer " + disposing );

            ProgressStatus.ResetStatus();
            ProgressStatus.ResetValue();
            
            if ( disposing )
            {
                _BorderPen.Dispose();
                _LinePen.Dispose();

                WPFHost.Dispose();

                if( _UnrealPackage != null )
                {
                    _UnrealPackage.Dispose();
                    _UnrealPackage = null;
                } 

                if( components != null )
                {
                    components.Dispose();
                }
            }
            
            base.Dispose( disposing );
        }

        private void _OnNotifyPackageEvent( object sender, UnrealPackage.PackageEventArgs e )
        {
            switch( e.EventId )
            {
                case UnrealPackage.PackageEventArgs.Id.Construct:
                    ProgressStatus.SetStatus( Resources.CONSTRUCTING_OBJECTS );
                    break;

                case UnrealPackage.PackageEventArgs.Id.Deserialize:
                    ProgressStatus.SetStatus( Resources.DESERIALIZING_OBJECTS );
                    break;

                case UnrealPackage.PackageEventArgs.Id.Import:
                    ProgressStatus.SetStatus( Resources.IMPORTING_OBJECTS );
                    break;

                case UnrealPackage.PackageEventArgs.Id.Link:
                    ProgressStatus.SetStatus( Resources.LINKING_OBJECTS );
                    break;
                
                case UnrealPackage.PackageEventArgs.Id.Object:
                    ProgressStatus.IncrementValue();
                    break;
            }
        }

        private void InitializeUI()
        {
            ProgressStatus.SetStatus( Resources.INITIALIZING_UI );
            exportDecompiledClassesToolStripMenuItem.Click += _OnExportClassesClick;
            exportScriptClassesToolStripMenuItem.Click += _OnExportScriptsClick;
            if( _UnrealPackage.Objects == null )
            {
                PackageIsCompressed();
            }
            InitializeTabs();

            SearchObjectTextBox.Text = _State.SearchObjectValue;
            DoSearchObjectByGroup( SearchObjectTextBox.Text );

            SearchObjectTextBox.TextChanged += (e, sender) =>
            {
                _State.SearchObjectValue = SearchObjectTextBox.Text;
                _State.Update();
            };
        }

        private void InitializeTabs()
        {
            ValidateTabs();
            if( _UnrealPackage.Generations != null )
            {
                CreateGenerationsList();
            }
        }

        private void ValidateTabs()
        {
            if( _ClassesList == null || _ClassesList.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Classes );
                exportDecompiledClassesToolStripMenuItem.Enabled = false;
                exportScriptClassesToolStripMenuItem.Enabled = false;
            } 
            else
            {
                findInClassesToolStripMenuItem.Enabled = true;
            }

            if( _UnrealPackage.Imports == null || _UnrealPackage.Imports.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Deps );  
            }

            if( _UnrealPackage.Generations == null || _UnrealPackage.Generations.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Generations );
            }

            if( _UnrealPackage.Exports == null || _UnrealPackage.Exports.Count == 0 
                || !_UnrealPackage.Exports.Any( obj => obj.Outer == null && obj.Class?.ObjectName != "Class") )
            {
                TabControl_Objects.Controls.Remove( TabPage_Content );   
            }
        }

        private void PackageIsCompressed()
        {
            Num_ObjectIndex.Enabled = false;
            Num_NameIndex.Enabled = false;

            if( _UnrealPackage.CompressedChunks == null ) 
                return;

            foreach( var chunk in _UnrealPackage.CompressedChunks )
            {
                DataGridView_Chunks.Rows.Add( 
                    chunk.UncompressedOffset, 
                    chunk.UncompressedSize, 
                    chunk.CompressedOffset, 
                    chunk.CompressedSize 
                    );
            }
        }

        private delegate void AddNodeDelegate( object table );

        private void AddNameNodeAsync( object nameTable )
        {
            if( DataGridView_NameTable.InvokeRequired )
            {
                var del = new AddNodeDelegate( AddNameNodeAsync );
                foreach( var t in _UnrealPackage.Names )
                {
                    Invoke( del, t );
                }
            }
            else
            {
                DataGridView_NameTable.Rows.Add
                ( 
                    ((UNameTableItem)nameTable).Name, 
                    String.Format( "{0:X4}", ((UNameTableItem)nameTable).Flags ) 
                );
            }
        }

        private TreeNode[]  _ThreadingNodes;
        private int         _ThreadingNodeIndex;

        private void AddExportNodeAsync( object exportTable )
        {
            if( TreeView_Exports.InvokeRequired )
            {
                var del = new AddNodeDelegate( AddExportNodeAsync );
                foreach( var t in _UnrealPackage.Exports )
                {
                    TreeView_Exports.Invoke( del, t );
                }
            }
            else
            {
                if( _ThreadingNodeIndex == 0 )
                {
                    _ThreadingNodes = new TreeNode[_UnrealPackage.Exports.Count];
                }

                var exp = exportTable as UExportTableItem;
                var node = new UExportNode {Table = exp, Text = exp.ObjectName};
                InitializeObjectNode( exp, node );

                _ThreadingNodes[_ThreadingNodeIndex ++] = node;

                if( _ThreadingNodeIndex != _UnrealPackage.Exports.Count ) 
                    return;

                TreeView_Exports.Nodes.AddRange( _ThreadingNodes );
                _ThreadingNodes = null;
                _ThreadingNodeIndex = 0;
            }
        }

        private void AddImportNodeAsync( object importTable )
        {
            if( TreeView_Imports.InvokeRequired )
            {
                AddNodeDelegate del = AddImportNodeAsync;
                foreach( var importItem in _UnrealPackage.Imports )
                {
                    Invoke( del, importItem );
                }
            }
            else
            {
                if( _ThreadingNodeIndex == 0 )
                {
                    _ThreadingNodes = new TreeNode[_UnrealPackage.Imports.Count];
                }

                var imp = (importTable as UImportTableItem);
                var node = new UImportNode
                {
                    Table = imp,
                    Text = imp.ObjectName
                };
                InitializeObjectNode( imp, node );
                _ThreadingNodes[_ThreadingNodeIndex ++] = node;
                if( _ThreadingNodeIndex != _UnrealPackage.Imports.Count ) 
                    return;

                TreeView_Imports.Nodes.AddRange( _ThreadingNodes );
                _ThreadingNodes = null;
                _ThreadingNodeIndex = 0;
            }
        }	

        private static void _OnImportNodeExpand( object sender, TreeViewCancelEventArgs e )
        {
            var importNode = e.Node as UTableNode;
            if( importNode != null )
            {
                importNode.Expanded();
            }
        }

        private static void _OnExportNodeExpand( object sender, TreeViewCancelEventArgs e )
        {
            var exportNode = e.Node as UTableNode;
            if( exportNode != null )
            {
                exportNode.Expanded();
            }
        }

        private void _OnNotifyObjectAdded( object sender, ObjectEventArgs e )
        {
            if( e.ObjectRef.ExportTable != null && e.ObjectRef.Table.ClassIndex == 0 && e.ObjectRef.Name.ToLower() != "none" )
            {
                _ClassesList.Add( (UClass)e.ObjectRef );
            }
        }

        private void CreateClassesList()
        {
            _ClassesList.Sort( (cl, cl2) => String.Compare( cl.Name, cl2.Name, StringComparison.Ordinal ) );

            TreeView_Classes.BeginUpdate();
            foreach( var Object in _ClassesList )
            {			
                if( Object == null )
                {
                    continue;
                }
  
                var imageKey = Object.GetImageName();
                var node = new ObjectNode( Object )
                {
                    ImageKey = imageKey, 
                    SelectedImageKey = imageKey, 
                    Text = Object.Name
                };
                node.Nodes.Add( "DUMMYNODE", "" );

                if( Object.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                {
                    node.ForeColor = Color.Red;
                }
                TreeView_Classes.Nodes.Add( node );	
            }
            // HACK:Add a MetaData object to the classes tree(hack because MetaData is not an actual class)
            var metobj = _UnrealPackage.FindObject<UMetaData>( "MetaData" );
            if( metobj != null )
            {
                var node = new ObjectNode( metobj )
                {
                    ImageKey = "Info", 
                    SelectedImageKey = "Info", 
                    Text = metobj.Name
                };
                node.Nodes.Add( "DUMMYNODE", "" );
                if( metobj.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                {
                    node.ForeColor = Color.Red;
                }
                TreeView_Classes.Nodes.Add( node );
            }
            TreeView_Classes.EndUpdate();

            TreeView_Classes.AfterSelect += _OnClassesNodeSelected;
            TreeView_Classes.BeforeExpand += _OnClassesNodeExpand;
        }

        private void TabControl_General_Selecting( object sender, TabControlCancelEventArgs e )
        {
            if( e.Action != TabControlAction.Selecting )
                return;

            if( e.TabPage != TabPage_Objects ) 
                return;

            if( TreeView_Classes.Nodes.Count == 0 )
                CreateClassesList();

            if( TreeView_Deps.Nodes.Count == 0 )
                CreateDependenciesList();  

            if( TreeView_Content.Nodes.Count == 0 )
                CreateContentList();
        }

        private void CreateDependenciesList()
        {
            foreach( var importItem in _UnrealPackage.Imports.Where( table => table.OuterIndex == 0 && table.ClassName == "Package" ) )
            {
                GetDependencyOn( importItem, TreeView_Deps.Nodes.Add( importItem.ObjectName ) );
            }
        }

        private void GetDependencyOn( UImportTableItem parentImport, TreeNode node )
        {
            if( node == null )
                return;

            foreach( var importItem in _UnrealPackage.Imports.Where( table => table != parentImport 
                && table.OuterTable == parentImport ) )
            {
                GetDependencyOn( importItem, node.Nodes.Add( importItem.ObjectName ) );
            }

            node.ToolTipText = parentImport.ClassName;
            InitializeObjectNode( parentImport, node );
        }

        protected void InitializeObjectNode( UObjectTableItem item, TreeNode node )
        {
            if( item.Object != null )
            {
                node.ImageKey = item.Object.GetImageName();	
                node.SelectedImageKey = node.ImageKey;

                if( item.Object.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                {
                    InitializeNodeError( node, item.Object );
                }
            }

            if( _UnrealPackage.HasClassType( item.ClassName ) )
            {
                node.ForeColor = Color.DarkOrange;
                node.ToolTipText = String.Format( Resources.CLASS_ISNT_SUPPORTED, item.ClassName );   
            }
        }

        private ObjectNode CreateObjectNode( UObjectTableItem item )
        {
            var objectNode = new ObjectNode( item.Object )
            {
                Text = item.ObjectName, 
                Tag = item
            };
            InitializeObjectNode( item, objectNode );  
            return objectNode;
        }

        private void CreateGenerationsList()
        {
            foreach( var gen in _UnrealPackage.Generations )
            {
                DataGridView_GenerationsTable.Rows.Add( gen.NamesCount, gen.ExportsCount, gen.NetObjectsCount );
            }
        }

        private void TreeView_Content_BeforeExpand( object sender, TreeViewCancelEventArgs e )
        {
            var objectNode = e.Node as ObjectNode;
            if( objectNode == null )
                return;

            var item = objectNode.Tag as UObjectTableItem;
            if( item == null )
                return;

            // Kill dummies.
            objectNode.Nodes.Clear();
            TreeView_Content.BeginUpdate();
            CreateContentNodesFor( item, objectNode.Nodes, true );
            TreeView_Content.EndUpdate();
        }

        // Lazy recursive.
        private void CreateContentNodesFor( UObjectTableItem item, TreeNodeCollection nodeContainer, bool recursive = false )
        {
            if( !recursive )
            {
                var objectNode = CreateObjectNode( item );
                nodeContainer.Add( objectNode );
                nodeContainer = objectNode.Nodes;
            }

            foreach( var obj in _UnrealPackage.Exports )
            {
                if( obj.OuterTable == null || obj.OuterTable != item )
                    continue;

                if( !recursive )
                {
                    nodeContainer.Add( "DUMMYNODE" );
                    break;
                }
                CreateContentNodesFor( obj, nodeContainer );
            }
        }

        private void CreateContentList()
        {
            TreeView_Content.BeginUpdate();
            foreach( var obj in _UnrealPackage.Exports )
            {
                if( obj.OuterTable == null && obj.ClassName != "Class" )
                {
                    CreateContentNodesFor( obj, TreeView_Content.Nodes );
                }
            }
            TreeView_Content.EndUpdate();
            TreeView_Content.Sort();
        }

        private void _OnExportClassesClick( object sender, EventArgs e )
        {
            DoExportPackageClasses();
        }

        private void _OnExportScriptsClick( object sender, EventArgs e )
        {	
            DoExportPackageClasses( true );	
        }

        private void DoExportPackageClasses( bool exportScripts = false )
        {
            var exportPath = _UnrealPackage.ExportPackageClasses( exportScripts );
            var dialogResult = MessageBox.Show( 
                String.Format( 
                    Resources.EXPORTED_ALL_PACKAGE_CLASSES, 
                    _UnrealPackage.PackageName, 
                    exportPath 
                ), 
                Application.ProductName,
                MessageBoxButtons.YesNo 
            );
            if( dialogResult == DialogResult.Yes )
            {
                Process.Start( exportPath );
            }
        }

        internal void ReloadPackage()
        {
            string filePath = _UnrealPackage.FullPackageName;

            var form = ((ProgramForm)ParentForm);
            form.Tabs.CloseTab((TabStripItem)Parent);
            form.LoadFromFile(filePath);
        }

        private void OutputNodeObject( TreeNode treeNode )
        {
            try
            {
                var unrealDecompilable = treeNode as IUnrealDecompilable;
                if( unrealDecompilable == null )
                    return;

                SetContentText( unrealDecompilable, unrealDecompilable.Decompile() );
                // Assemble a title
                string newTitle;
                var decompilableObject = treeNode as IDecompilableObject;
                if( decompilableObject != null )
                {
                    UObject obj;
                    if( (obj = decompilableObject.Object as UObject) != null )
                    {
                        newTitle = obj.GetOuterGroup();
                        SetContentTitle( newTitle );
                        if( obj.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                        {
                            InitializeNodeError( treeNode, obj );
                        }
                    }
                    else
                    {
                        newTitle = treeNode.Text;
                        SetContentTitle( newTitle, false );
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
                    SetContentTitle( newTitle, false );
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

        private static void InitializeNodeError( TreeNode node, UObject errorObject )
        {
            node.ForeColor = Color.Red;
            node.ToolTipText = GetExceptionMessage( errorObject );
        }

        private static string GetExceptionMessage( UObject errorObject )
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
                if( node != null && !((UObject)node.Object).HasInitializedNodes )
                {
                    TreeView_Classes.BeginUpdate();
                    node.Nodes.RemoveByKey( "DUMMYNODE" );
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

        #region Node-ContextMenu Methods
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

        private bool _SuppressNodeSelect;
        private void TreeView_Content_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            switch( e.Button )
            {
                case MouseButtons.Left:	
                    //PerformNodeAction( e.Node as IDecompileableObjectNode, "OBJECT" );
                    break;

                case MouseButtons.Right:
                    _SuppressNodeSelect = true;
                    ShowNodeContextMenuStrip( TreeView_Content, e, _OnContentItemClicked );
                    break;
            }
        }

        private static void ShowNodeContextMenuStrip( TreeView tree, TreeNodeMouseClickEventArgs e, 
            ToolStripItemClickedEventHandler itemClicked )
        {
            tree.SelectedNode = e.Node;

            var viewToolsContextMenu = new ContextMenuStrip();
            BuildItemNodes( e.Node, viewToolsContextMenu.Items, itemClicked );
            if( viewToolsContextMenu.Items.Count == 0 )
            {
                viewToolsContextMenu.Dispose();
                return;
            }

            viewToolsContextMenu.ItemClicked += itemClicked;
            viewToolsContextMenu.Closed += (eSender, eEvent) =>
            {
                ((ContextMenuStrip)eSender).ItemClicked -= itemClicked;
                //((ContextMenuStrip)eSender).Dispose();
            };
            viewToolsContextMenu.Show( tree, e.Location );	
        }

        private static void BuildItemNodes( object target, ToolStripItemCollection itemCollection, ToolStripItemClickedEventHandler itemClickEvent = null )
        {
            itemCollection.Clear();

            var obj = target as UObject;
            if( obj == null && target is IDecompilableObject )
            {
                obj = (target as IDecompilableObject).Object as UObject;
            }

            var addItem = (Action<string, string>)((title, id) =>
            {
                var item = itemCollection.Add( title );	
                item.Name = id;
            });

            if( target is IUnrealDecompilable )
            {
                addItem.Invoke( Resources.NodeItem_ViewObject, "OBJECT" );  
            }

            if( obj == null )
            {
                return;
            }

            if( obj.Outer != null )
            {
                addItem( Resources.NodeItem_ViewOuter, "VIEW_OUTER" );
            }

            if( obj is IUnrealViewable )
            { 
                if( File.Exists( Program.Options.UEModelAppPath	) )
                {
                    addItem( Resources.NodeItem_OpenInUEModelViewer, "OPEN_UEMODELVIEWER" );
#if DEBUG
                    addItem( Resources.NodeItem_ExportWithUEModelViewer, "EXPORT_UEMODELVIEWER" );
#endif
                }
#if DEBUG
                addItem( Resources.NodeItem_ViewContent, "CONTENT" );
#endif	
            }

            if( obj is UMetaData )
            {
                addItem( Resources.NodeItem_ViewUsedTags, "USED_TAGS" );	
            }

            var uStruct = (obj as UStruct); 
            if( uStruct != null )
            {
                if( uStruct.Super != null )
                {
                    addItem( Resources.NodeItem_ViewParent, "VIEW_SUPER" );
                }

                var @class = obj as UClass;
                if( @class != null )
                {
                    if( @class.IsClassWithin() )
                    {
                        addItem( Resources.NodeItem_ViewOuter, "VIEW_WITHIN" );
                    }
                }

                if( uStruct.ByteCodeManager != null )
                {
                    if( @class != null )
                    {
                        addItem( Resources.NodeItem_ViewReplication, "REPLICATION" );	
                    }
                    addItem( Resources.NodeItem_ViewTokens, "TOKENS" );
                    addItem( Resources.NodeItem_ViewDisassembledTokens, "TOKENS_DISASSEMBLE" );
                }

                if( uStruct.ScriptText != null )
                {
                    addItem( Resources.NodeItem_ViewScript, "SCRIPT" );
                }

                if( uStruct.ProcessedText != null )
                {
                    addItem( Resources.NodeItem_ViewProcessedScript, "PROCESSEDSCRIPT" );
                }
                            
                if( uStruct.CppText != null )
                {
                    addItem( Resources.NodeItem_ViewCPPText, "CPPSCRIPT" );
                }

                if( uStruct.Properties != null && uStruct.Properties.Any() )
                {
                    addItem( Resources.NodeItem_ViewDefaultProperties, "DEFAULTPROPERTIES" );	
                }
            }

            var bufferedObject = obj as IBuffered;
            if( bufferedObject.GetBuffer() != null )
            {
                var bufferedItem = new ToolStripMenuItem 
                {
                    Text = Resources.NodeItem_ViewBuffer,
                    Name = "BUFFER"
                };

                bool shouldAddBufferItem = bufferedObject.GetBufferSize() > 0;

                var tableNode = obj as IContainsTable;
                if( tableNode.Table != null )
                { 
                    var tableBufferItem = bufferedItem.DropDownItems.Add( Resources.NodeItem_ViewTableBuffer );
                    tableBufferItem.Name = "TABLEBUFFER";
                    shouldAddBufferItem = true;
                }

                if( obj.Default != null && obj.Default != obj )
                {
                    var defaultBufferItem = bufferedItem.DropDownItems.Add( Resources.NodeItem_DefaultBuffer );
                    defaultBufferItem.Name = "DEFAULTBUFFER";  
                    shouldAddBufferItem = true;
                }

                if( shouldAddBufferItem )
                {
                    bufferedItem.DropDownItemClicked += itemClickEvent;
                    itemCollection.Add( bufferedItem );
                }
            }

            if( obj.ThrownException != null )
            {
                itemCollection.Add( new ToolStripSeparator() );
                addItem( Resources.NodeItem_ViewException, "EXCEPTION" );		
            }
            itemCollection.Add( new ToolStripSeparator() );
            addItem( Resources.NodeItem_ManagedProperties, "MANAGED_PROPERTIES" );
#if DEBUG
            addItem( "Force Deserialize", "FORCE_DESERIALIZE" );
#endif	
        }

        private void _OnImportsItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Imports.SelectedNode as IDecompilableObject, e.ClickedItem.Name );
        }

        private void TreeView_Imports_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            if( e.Button != MouseButtons.Right ) 
                return;

            ShowNodeContextMenuStrip( TreeView_Imports, e, _OnImportsItemClicked );
        }

        private void ViewTools_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            if( _LastNodeContent == null )
                return;

            PerformNodeAction( _LastNodeContent, e.ClickedItem.Name );
        }

        private void _OnClassesItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Classes.SelectedNode, e.ClickedItem.Name );	
        }

        private void _OnExportsItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Exports.SelectedNode, e.ClickedItem.Name );
        }

        private void _OnContentItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Content.SelectedNode, e.ClickedItem.Name );
        }

        private static string LegacyFormatTokenHeader( UStruct.UByteCodeDecompiler.Token token )
        {
            string name = token.GetType().Name;
            string abbrName = string.Concat(name.Where(char.IsUpper));
            return $"{abbrName}({token.Size}/{token.StorageSize})";
        }

        private static string _DisassembleTokensTemplate;
        private static void LegacyDisassembleTokens( UStruct container, UStruct.UByteCodeDecompiler decompiler, int tokenCount, StringBuilder content )
        {
            for( var i = 0; i + 1 < tokenCount; ++ i )
            {
                var token = decompiler.NextToken;
                var firstTokenIndex = decompiler.CurrentTokenIndex;
                int lastTokenIndex;
                int subTokensCount;

                string value;
                try
                {
                    value = token.Decompile();
                }
                catch( Exception e )
                {
                    value = "Exception occurred while decompiling token: " + e;
                }
                finally
                {
                    lastTokenIndex = decompiler.CurrentTokenIndex;
                    subTokensCount = lastTokenIndex - firstTokenIndex;
                    decompiler.CurrentTokenIndex = firstTokenIndex;
                }

                var buffer = new byte[token.StorageSize];
                container.Package.Stream.Position = container.ExportTable.SerialOffset + container.ScriptOffset + token.StoragePosition;
                container.Package.Stream.Read( buffer, 0, buffer.Length );

                var header = LegacyFormatTokenHeader( token );
                var bytes = BitConverter.ToString( buffer ).Replace( '-', ' ' );

                content.Append(String.Format( _DisassembleTokensTemplate.Replace( "%INDENTATION%", UDecompilingState.Tabs ), 
                    token.Position, token.StoragePosition, 
                    header, bytes,
                    value != String.Empty ? value + "\r\n" : value, firstTokenIndex, lastTokenIndex
                ));

                if( subTokensCount > 0 )
                {
                    UDecompilingState.AddTab();
                    try
                    {
                        LegacyDisassembleTokens(container, decompiler, subTokensCount + 1, content);
                    }
                    catch (Exception ex)
                    {
                        content.Append($"/*Disassemble error: {ex}*/");
                    }
                    finally
                    {
                        i += subTokensCount;
                        UDecompilingState.RemoveTab();
                    }
                }
            }
        }

        private void PerformNodeAction( object target, string action )
        {
            if( target == null )
                return;

            var obj = target as UObject;
            if( obj == null && target is IDecompilableObject )
            {
                obj = ((IDecompilableObject) target).Object as UObject; 
            }

            try
            {
                switch( action )
                {
                    case "USED_TAGS":
                    {
                        var n = target as ObjectNode;
                        if( n != null )
                        {
                            var metaObj = n.Object as UMetaData;
                            if( metaObj != null )
                            {
                                SetContentTitle( metaObj.GetOuterGroup() );
                                SetContentText( target as TreeNode, metaObj.GetUniqueMetas() );
                            }
                        }
                        break;
                    }

                    case "OPEN_UEMODELVIEWER":
                    {
                        Process.Start
                        ( 
                            Program.Options.UEModelAppPath, 
                            "-path=" + _UnrealPackage.PackageDirectory
                            + " " + _UnrealPackage.PackageName
                            + " " + ((TreeNode)target).Text
                        );
                        break;
                    }

                    case "EXPORT_UEMODELVIEWER":
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
                            + " " + ((TreeNode)target).Text;
                        var appInfo = new ProcessStartInfo( Program.Options.UEModelAppPath, appArguments )
                        {
                            UseShellExecute = false, 
                            RedirectStandardOutput = true, 
                            CreateNoWindow = false
                        };
                        var app = Process.Start( appInfo );
                        var log = String.Empty;
                        app.OutputDataReceived += (sender, e) => log += e.Data;
                        //app.WaitForExit();

                        if( Directory.GetFiles( contentDir ).Length > 0 )
                        {
                            if( MessageBox.Show( 
                                Resources.UC_PackageExplorer_PerformNodeAction_QUESTIONEXPORTFOLDER, 
                                Application.ProductName,
                                MessageBoxButtons.YesNo 
                                ) == DialogResult.Yes )
                            {
                                Process.Start( contentDir );
                            }
                        }
                        else
                        {
                            MessageBox.Show
                            ( 
                                String.Format
                                ( 
                                    "The object was not exported.\r\n\r\nArguments:{0}\r\n\r\nLog:{1}", 
                                    appArguments, 
                                    log 
                                ),
                                Application.ProductName 
                            );
                        }
                        break;
                    }						

                    case "OBJECT":
                    {
                        if( obj != null )
                        {
                            SetContentTitle( obj.GetOuterGroup() );
                            SetContentText( obj, obj.Decompile() );   
                        }
                        else if( target is IUnrealDecompilable )
                        {
                            var node = target as TreeNode;
                            SetContentTitle( node.Text );
                            SetContentText( node, (target as IUnrealDecompilable).Decompile() );
                        }
                        break;
                    }

                    case "VIEW_OUTER":
                        if( obj != null && obj.Outer != null )
                        {
                            SetContentTitle( obj.Outer.GetOuterGroup() );
                            SetContentText( obj.Outer, obj.Outer.Decompile() );   
                        }
                        break;

                    case "VIEW_SUPER":
                        if( obj is UStruct && ((UStruct)obj).Super != null )
                        {
                            var super = ((UStruct)obj).Super;
                            SetContentTitle( super.GetOuterGroup() );
                            SetContentText( super, super.Decompile() );   
                        }
                        break;

                    case "VIEW_WITHIN":
                        if( obj is UClass && ((UClass)obj).Within != null )
                        {
                            var within = ((UClass)obj).Within;
                            SetContentTitle( within.GetOuterGroup() );
                            SetContentText( within, within.Decompile() );   
                        }
                        break;

                    case "MANAGED_PROPERTIES":
                        using( var propDialog = new PropertiesDialog{
                                ObjectLabel = {Text = ((TreeNode)target).Text},
                                ObjectPropertiesGrid = {SelectedObject = obj}
                            } )
                        {
                            propDialog.ShowDialog( this );
                        }
                        break;

                    case "REPLICATION":
                    {
                        var unClass = obj as UClass;
                        if( unClass != null )
                        {
                            SetContentTitle( unClass.Name, true, "Replication" );
                            SetContentText( unClass, unClass.FormatReplication() );
                        }
                        break;
                    }

                    case "SCRIPT":
                    {
                        var str = obj as UStruct;
                        if( str != null && str.ScriptText != null )
                        {
                            SetContentTitle( str.ScriptText.GetOuterGroup() );
                            SetContentText( str.ScriptText, str.ScriptText.Decompile() );
                        }
                        break;
                    }

                    case "CPPSCRIPT":
                    {
                        var str = obj as UStruct;
                        if( str != null && str.CppText != null )
                        {
                            SetContentTitle( str.CppText.GetOuterGroup() );
                            SetContentText( str.CppText, str.CppText.Decompile() );
                        }
                        break;
                    }

                    case "PROCESSEDSCRIPT":
                    {
                        var str = obj as UStruct;
                        if( str != null && str.ProcessedText != null )
                        {
                            SetContentTitle( str.ProcessedText.GetOuterGroup() );
                            SetContentText( str.ProcessedText, str.ProcessedText.Decompile() );
                        }
                        break;
                    }

                    case "DEFAULTPROPERTIES":
                    {
                        var unStruct = obj as UStruct;
                        if( unStruct != null )
                        {
                            SetContentTitle( unStruct.Default.GetOuterGroup(), true, "Default-Properties" );
                            SetContentText( unStruct, unStruct.FormatDefaultProperties() );
                        }
                        break;
                    }
                        
                    case "TOKENS_DISASSEMBLE":
                    {
                        var unStruct = obj as UStruct;
                        if( unStruct != null && unStruct.ByteCodeManager != null )
                        {                           
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            _DisassembleTokensTemplate = LoadTemplate("struct.tokens-disassembled");
                            var content = new StringBuilder(codeDec.DeserializedTokens.Count);
                            LegacyDisassembleTokens( unStruct, codeDec, codeDec.DeserializedTokens.Count, content );
                            SetContentTitle( unStruct.GetOuterGroup(), true, "Tokens-Disassembled" );
                            SetContentText( unStruct, content.ToString() );
                        }
                        break;
                    }

                    case "TOKENS":
                    {
                        var unStruct = obj as UStruct;
                        if( unStruct != null && unStruct.ByteCodeManager != null )
                        {                   
                            var tokensTemplate = LoadTemplate("struct.tokens");
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            var content = new StringBuilder(codeDec.DeserializedTokens.Count);
                            while ( codeDec.CurrentTokenIndex + 1 < codeDec.DeserializedTokens.Count )
                            {
                                string output;
                                var breakOut = false;

                                var t = codeDec.NextToken;
                                int orgIndex = codeDec.CurrentTokenIndex;
                                try
                                {
                                    output = t.Decompile();
                                }
                                catch
                                {
                                    output = "Exception occurred while decompiling token: " + t.GetType().Name;
                                    breakOut = true;
                                }

                                string chain = LegacyFormatTokenHeader( t );
                                int endTokenIndex = codeDec.CurrentTokenIndex;
                                if( endTokenIndex < codeDec.DeserializedTokens.Count ) // sanity check
                                {
                                    for( int i = orgIndex + 1; i < endTokenIndex; ++ i )
                                    {
                                        chain += " -> " + LegacyFormatTokenHeader( codeDec.DeserializedTokens[i] );
                                    }
                                }
                                
                                var buffer = new byte[t.StorageSize];
                                _UnrealPackage.Stream.Position = unStruct.ExportTable.SerialOffset + unStruct.ScriptOffset + t.StoragePosition;
                                _UnrealPackage.Stream.Read( buffer, 0, buffer.Length );

                                content.Append(String.Format( tokensTemplate, 
                                    t.Position, t.StoragePosition, 
                                    chain, BitConverter.ToString( buffer ).Replace( '-', ' ' ),
                                    output != String.Empty ? output + "\r\n" : output
                                ));

                                if( breakOut )
                                    break;
                            }
                            SetContentTitle( unStruct.GetOuterGroup(), true, "Tokens" );
                            SetContentText( unStruct, content.ToString() );
                        }
                        break;
                    }

                    case "BUFFER":
                    {
                        var bufferObject = (IBuffered)obj;
                        if( bufferObject.GetBufferSize() > 0 )
                        {
                            ViewBufferFor( bufferObject );
                        }
                        break;
                    }

                    case "TABLEBUFFER":
                    {
                        var tableObject = target as IContainsTable ?? obj;
                        ViewBufferFor( tableObject.Table );
                        break;
                    }

                    case "DEFAULTBUFFER":
                    {
                        var unObject = obj;
                        if( unObject != null )
                        {
                            ViewBufferFor( unObject.Default );
                        }
                        break;
                    }

                    case "EXCEPTION":
                    {
                        var oNode = target as ObjectNode;
                        if( oNode != null )
                        {
                            SetContentText( oNode, GetExceptionMessage( ((UObject)oNode.Object) ) );
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

        private static string LoadTemplate( string name )
        {
            return File.ReadAllText( Path.Combine(Program.s_templateDir, name + ".txt" ), System.Text.Encoding.ASCII );
        }
        #endregion

        private void ToolStripButton_Find_Click( object sender, EventArgs e )
        {
            if( TextEditorPanel == null )
            {
                return;
            }
            EditorUtil.FindText( TextEditorPanel, SearchBox.Text );
        }

        private void SearchBox_KeyPress_1( object sender, KeyPressEventArgs e )
        {
            if( TextEditorPanel == null )
            {
                return;
            }

            if( e.KeyChar != '\r' ) 
                return;

            EditorUtil.FindText( TextEditorPanel, SearchBox.Text );	  
            e.Handled = true;
        }

        public override void TabFind()
        {
            using( var findDialog = new FindDialog( TextEditorPanel ) )
            {
                findDialog.ShowDialog();
            }
        }

        private struct BufferData
        {
            public string Text;
            public string Label;
            public object Node;

            public double Y, X;
        }
        private readonly List<BufferData> _ContentBuffer = new List<BufferData>();
        private int _BufferIndex = -1;
        private object _LastNodeContent;

        public void SetContentTitle( string title, bool isSearchable = true, string sub = "" )
        {
            Label_ObjectName.Text = title;
            if( sub != "" )
            {
                Label_ObjectName.Text += " -> " + sub.Replace( '-', ' ' );
            }

            if( !isSearchable ) 
                return;

            SearchObjectTextBox.Text = title;
            if( sub != "" )
            {
                SearchObjectTextBox.Text += ":" + sub;
            }
            SearchObjectTextBox.SelectAll();
        }

        public void SetContentText( object node, string content, bool skip = false, bool resetView = true )
        {
            if( _LastNodeContent != node )
            {
                BuildItemNodes( node, ViewTools.DropDownItems );
            }
            ViewTools.Enabled = ViewTools.DropDownItems.Count > 0 && node != null;
            _LastNodeContent = node;

            content = content.TrimStart( '\r', '\n' ).TrimEnd( '\r', '\n' );
            if( content.Length > 0 )
            {
                SearchBox.Enabled = true;
                ExportButton.Enabled = true;
                WPFHost.Enabled = true;
                ViewTools.Enabled = true;
                findInDocumentToolStripMenuItem.Enabled = true;
            }

            TextEditorPanel.TextEditor.Text = content;
            if( resetView )
            {
                TextEditorPanel.TextEditor.ScrollToHome();
            }

            if( skip )
                return;

            if( _ContentBuffer.Count > 0 )
            {
                // No need to buffer the same content
                if( _BufferIndex > -1 && _BufferIndex < _ContentBuffer.Count && _ContentBuffer[_BufferIndex].Node == node )
                {
                    return;
                }

                StoreViewForBuffer( _BufferIndex );
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

        private void StoreViewForBuffer( int bufferIndex )
        {
            var content = _ContentBuffer[bufferIndex];
            content.X = TextEditorPanel.TextEditor.HorizontalOffset;
            content.Y = TextEditorPanel.TextEditor.VerticalOffset;
            _ContentBuffer[bufferIndex] = content;
        }

        private void RestoreBufferedContent( int bufferIndex )
        {
            RestoreBufferedContent(_ContentBuffer[bufferIndex]);
        }

        private void RestoreBufferedContent(BufferData bufferData)
        {
            SetContentTitle(bufferData.Label, false);
            SetContentText(bufferData.Node, bufferData.Text, true);
            SelectNode(bufferData.Node as TreeNode);

            TextEditorPanel.TextEditor.ScrollToVerticalOffset(bufferData.Y);
            TextEditorPanel.TextEditor.ScrollToHorizontalOffset(bufferData.X);
        }

        private void ToolStripButton_Backward_Click( object sender, EventArgs e )
        {
            if( _BufferIndex - 1 <= -1 ) 
                return;

            FilterText.Text = String.Empty;
            StoreViewForBuffer( _BufferIndex );
            RestoreBufferedContent( -- _BufferIndex );

            if( _BufferIndex == 0 )
            {
                PrevButton.Enabled = false;
            }
            NextButton.Enabled = true;
        }

        private void ToolStripButton_Forward_Click( object sender, EventArgs e )
        {
            if( _BufferIndex + 1 >= _ContentBuffer.Count ) 
                return;

            FilterText.Text = String.Empty;
            StoreViewForBuffer( _BufferIndex );
            RestoreBufferedContent( ++ _BufferIndex );

            if( _BufferIndex == _ContentBuffer.Count-1 )
            {
                NextButton.Enabled = false;
            }
            PrevButton.Enabled = true;
        }

        private void SelectNode( TreeNode node )
        {
            if( node == null ) 
                return;

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

        private void TabControl_General_Selected( object sender, TabControlEventArgs e )
        {
            if( e.TabPage == TabPage_Tables )
            {
                if( DataGridView_NameTable.Rows.Count != 0 ) 
                    return;

                new Thread( () => AddNameNodeAsync( null ) ).Start();
                TabControl_General.Selected -= TabControl_General_Selected;
            }
        }

        private void TabControl_Tables_Selected( object sender, TabControlEventArgs e )
        {
            if( e.TabPage == TabPage_Exports )
            {
                if( TreeView_Exports.Nodes.Count != 0 ) 
                    return;

                new Thread( () => AddExportNodeAsync( null ) ).Start();
                TreeView_Exports.BeforeExpand += _OnExportNodeExpand;
                return;
            }

            if( e.TabPage == TabPage_Imports )
            {
                if( TreeView_Imports.Nodes.Count != 0 ) 
                    return;

                new Thread( () => AddImportNodeAsync( null ) ).Start();
                TreeView_Imports.BeforeExpand += _OnImportNodeExpand;
            }
        }

        private void ViewBufferToolStripMenuItem_Click( object sender, EventArgs e )
        {
            ViewBufferFor( _UnrealPackage );
        }

        private void ViewBufferFor( IBuffered target )
        {
            if( target == null )
            {
                MessageBox.Show( Resources.NoTargetForViewBuffer );
                return;
            }

            var hexDialog = new HexViewerForm( target, FileName );
            hexDialog.Show( ParentForm );
        }

        private System.Windows.Forms.Timer _FilterTextChangedTimer = null;
        private readonly List<TreeNode> _FilteredNodes = new List<TreeNode>();

        private void FilterText_TextChanged( object sender, EventArgs e )
        {
            if( _FilterTextChangedTimer != null && _FilterTextChangedTimer.Enabled )
            {
                _FilterTextChangedTimer.Stop();
                _FilterTextChangedTimer.Dispose();
                _FilterTextChangedTimer = null;
            }

            if( _FilterTextChangedTimer == null )
            {
                _FilterTextChangedTimer = new System.Windows.Forms.Timer();
                _FilterTextChangedTimer.Interval = 350;
                _FilterTextChangedTimer.Tick += _FilterTextChangedTimer_Tick;
                _FilterTextChangedTimer.Start();
            }
        }

        private void _FilterTextChangedTimer_Tick( object sender, EventArgs e )
        {
            _FilterTextChangedTimer.Stop();
            _FilterTextChangedTimer.Dispose();
            _FilterTextChangedTimer = null;

            for ( int i = 0; i < TreeView_Classes.Nodes.Count; ++ i )
            {
                if( TreeView_Classes.Nodes[i].Text.IndexOf( FilterText.Text, StringComparison.OrdinalIgnoreCase ) != -1 )
                    continue;

                _FilteredNodes.Add( TreeView_Classes.Nodes[i] );
                TreeView_Classes.Nodes[i].Remove();
                -- i;
            }

            for( int i = 0; i < _FilteredNodes.Count; ++ i )
            {
                if( FilterText.Text != String.Empty &&
                    _FilteredNodes[i].Text.IndexOf( FilterText.Text, StringComparison.OrdinalIgnoreCase ) < 0 )
                    continue;

                TreeView_Classes.Nodes.Add( _FilteredNodes[i] );
                _FilteredNodes.Remove( _FilteredNodes[i] ); 
                -- i;
            }

            TreeView_Classes.Sort();
        }

        private void ReloadButton_Click( object sender, EventArgs e )
        {
            ReloadPackage();
        }

        private readonly Pen _BorderPen = new Pen( Color.FromArgb( 237, 237, 237 ) );
        private readonly Pen _LinePen = new Pen( Color.White );

        private void Panel4_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.DrawRectangle( _BorderPen, 0, 0, panel4.Width-1, panel4.Height-1 );
        }

        private void Panel1_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.DrawRectangle( _BorderPen, 0, 0, panel1.Width-1, panel1.Height-1 );
        }

        private void ToolStripSeparator1_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.FillRectangle( _LinePen.Brush, 2, 0, panel1.Width-4, panel1.Height );
            e.Graphics.DrawLine( _BorderPen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Bottom );
            e.Graphics.DrawLine( _BorderPen, e.ClipRectangle.Right-1, e.ClipRectangle.Top, e.ClipRectangle.Right-1, e.ClipRectangle.Bottom );
        }

        private void ToolStrip_Content_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.DrawRectangle( _BorderPen, 0, 0, ((Control)sender).Width-1, panel1.Height-1 );
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
                var removedNodes = new List<TreeNode>();
                for( int i = 0; i < TreeView_Exports.Nodes.Count; ++ i )
                {
                    if( TreeView_Exports.Nodes[i].ImageKey != checkBox.ImageKey )
                        continue;

                    removedNodes.Add( TreeView_Exports.Nodes[i] );
                    TreeView_Exports.Nodes.RemoveAt( i );
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
                TreeView_Exports.Nodes.AddRange( ((List<TreeNode>)checkBox.Tag).ToArray() );
            }		
        }

        private void Button_Export_Click( object sender, EventArgs e )
        {
            var exportableObject = ((ObjectNode)TreeView_Content.SelectedNode).Object as IUnrealExportable; 
            if( (UObject)exportableObject == null )
            {
                return;
            }
            ((UObject)exportableObject).BeginDeserializing();

            string extensions = String.Empty;
            foreach( var ext in exportableObject.ExportableExtensions )
            {
                extensions += string.Format( "{0}(*" + ".{0})|*.{0}", ext );
                if( ext != exportableObject.ExportableExtensions.Last() )
                {
                    extensions += "|";
                }
            }
            var dialog = new SaveFileDialog{Filter = extensions, FileName = ((UObject)exportableObject).Name};
            if( dialog.ShowDialog() != DialogResult.OK )
                return;

            using( var stream = new FileStream( dialog.FileName, FileMode.Create, FileAccess.Write ) )
            {
                exportableObject.SerializeExport(
                    exportableObject.ExportableExtensions.ElementAt( dialog.FilterIndex - 1 ), stream );
                stream.Flush();
            }
        }

        private void CheckIfNodeIsExportable( TreeNode node )
        {
            bool exportable = false;

            var soundObject = ((ObjectNode)node).Object as IUnrealExportable;
            if( soundObject != null )
            {
                exportable = soundObject.CompatableExport();	
        
                Button_Export.Text = String.Format( Resources.EXPORT_AS, node.Text );	
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
            PerformNodeAction( e.Node as IDecompilableObject, "OBJECT" );
            CheckIfNodeIsExportable( e.Node );
        }

        private void Num_ObjectIndex_ValueChanged( object sender, EventArgs e )
        {
            int objectIndexToFind = (int)Num_ObjectIndex.Value;
            try
            {
                var foundObject = _UnrealPackage.GetIndexTable( objectIndexToFind );
                LObjectIndex.Text = foundObject != null 
                    ? String.Format( Resources.OBJECT_IS, foundObject.ObjectName ) 
                    : Resources.NO_OBJECT_WAS_FOUND;
            }
            catch( ArgumentOutOfRangeException exc )
            {
                LObjectIndex.Text = String.Format( Resources.INVALID_OBJECT_INDEX, exc.ActualValue );
            }	
        }

        private void Num_NameIndex_ValueChanged( object sender, EventArgs e )
        {
            try
            {
                LNameIndex.Text = String.Format( Resources.NAME_IS, 
                    _UnrealPackage.Names[(int)Num_NameIndex.Value].Name 
                );
            }
            catch( ArgumentOutOfRangeException exc )
            {
                LNameIndex.Text = String.Format( Resources.INVALID_NAME_INDEX, exc.ActualValue );
            }	
        }    

        private int _FindCount;
        private TabPage _FindTab;
        private void FindInClassesToolStripMenuItem_Click( object sender, EventArgs e )
        {
            string findText;
            using( var findDialog = new FindDialog() )
            {
                findDialog.FindInput.Text = SearchBox.Text;
                if( findDialog.ShowDialog() != DialogResult.OK )
                {
                    return;
                }
                findText = findDialog.FindInput.Text;
            }

            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus( Resources.SEARCHING_CLASSES_STATUS );

            var documentResults = new List<TextSearchHelpers.DocumentResult>();
            foreach( var content in _ClassesList )
            {
                var	findContent = content.Decompile();
                var findResults = TextSearchHelpers.FindText( findContent, findText );
                if( !findResults.Any() )
                {
                    continue;
                }

                var document = new TextSearchHelpers.DocumentResult
                {
                    Results = findResults,
                    Document = content
                };
                documentResults.Add( document );
            }
            ProgressStatus.Reset();

            if( documentResults.Count == 0 )
            {
                MessageBox.Show( String.Format( Resources.NO_FIND_RESULTS, findText ) );
                return;
            }

            if( _FindTab != null )
            {
                TabControl_General.TabPages.Remove( _FindTab );	
            }

            _FindTab = new TabPage
            {
                Text = String.Format( Resources.FIND_RESULTS_TITLE, ++ _FindCount )
            }; 

            var treeResults = new TreeView{ Dock = DockStyle.Fill };
            _FindTab.Controls.Add( treeResults );
            TabControl_General.TabPages.Add( _FindTab );

            TabControl_General.SelectTab( _FindTab );

            foreach( var documentResult in documentResults )
            {
                var documentNode = treeResults.Nodes.Add( ((UClass)documentResult.Document).Name );
                documentNode.Tag = documentResult;
                foreach( var result in documentResult.Results )
                {
                    var resultNode = documentNode.Nodes.Add( result.ToString() );
                    resultNode.Tag = result;
                }
            }

            treeResults.AfterSelect += (nodeSender, nodeEvent) =>
            {
                var findResult = nodeEvent.Node.Tag as TextSearchHelpers.FindResult;
                if( findResult == null )
                {
                    return;
                }

                var documentResult = nodeEvent.Node.Parent.Tag as TextSearchHelpers.DocumentResult;
                if( documentResult != null )
                {
                    var unClass = ((UClass)documentResult.Document);
                    SetContentTitle( 
                        String.Format( "{0}: {1}, {2}", 
                            unClass.Name, 
                            findResult.TextLine, 
                            findResult.TextColumn 
                        ), 
                        false 
                    ); 
                    SetContentText( nodeEvent.Node, unClass.Decompile(), false, false );
                }

                TextEditorPanel.TextEditor.ScrollTo( findResult.TextLine, findResult.TextColumn );
                TextEditorPanel.TextEditor.Select( findResult.TextIndex, findText.Length );
            };
        }

        private void FindInDocumentToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SearchBox.Focus();
        }

        private void SearchBox_TextChanged( object sender, EventArgs e )
        {
            FindButton.Enabled = SearchBox.Text.Length > 0;
            findNextToolStripMenuItem.Enabled = FindButton.Enabled;
        }

        private void FindNextToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if( TextEditorPanel == null )
            {
                return;
            }
            EditorUtil.FindText( TextEditorPanel, SearchBox.Text );
        }

        private void SplitContainer1_SplitterMoved( object sender, SplitterEventArgs e )
        {
            Settings.Default.PackageExplorer_SplitterDistance = splitContainer1.SplitterDistance;
            Settings.Default.Save();
        }

        private bool DoSearchObjectByGroup( string objectGroup )
        {
            var protocol = String.Empty;
            var page = String.Empty;
            if( objectGroup.Contains( ':' ) )
            {
                protocol = objectGroup.Substring( 0, objectGroup.IndexOf( ':' ) ).ToLower();    
                page = objectGroup.Substring( protocol.Length + 1 ).ToLower();
            }

            var obj = _UnrealPackage.FindObjectByGroup( protocol == "" ? objectGroup : protocol );
            if( obj != null )
            {
                if( page != "" )
                {
                    switch( page )
                    {
                        case "replication":
                            if( obj is UClass )
                            {
                                PerformNodeAction( obj, "REPLICATION" );
                                return true;
                            }
                            break;

                        case "tokens":
                            if( obj is UStruct )
                            {
                                PerformNodeAction( obj, "TOKENS" );
                                return true;
                            }
                            break;

                        case "tokens-disassembled":
                            if( obj is UStruct )
                            {
                                PerformNodeAction( obj, "TOKENS_DISASSEMBLE" );
                                return true;
                            }
                            break;

                        case "default-properties":
                            if( obj is UStruct )
                            {
                                PerformNodeAction( obj, "DEFAULTPROPERTIES" );
                                return true;
                            }
                            break;
                    }
                }

                var content = obj.ImportTable == null 
                    ? obj.Decompile() 
                    : String.Format( "// No decompilable data available for {0}", obj.GetOuterGroup() );

                SetContentTitle( obj.GetOuterGroup() );
                SetContentText( obj, content );
                return true;
            }    

            switch( protocol )
            {
                case "about":
                    switch( page )
                    {
                        case "stats":
                            var classesCount = _ClassesList.Count;
                            var exportsCount = _UnrealPackage.Exports.Count;
                            var importsCount = _UnrealPackage.Imports.Count;
                            var namesCount = _UnrealPackage.Names.Count;
                            var output = String.Format( "Number of classes: {0}\r\n" +
                                                        "Number of exports: {1}\r\n" +
                                                        "Number of imports: {2}\r\n" +
                                                        "Number of names: {3}\r\n", classesCount, exportsCount, importsCount, namesCount );
                            SetContentTitle( String.Format( "{0} - {1}", protocol, page ), false );
                            SetContentText( null, output, true );
                            return true;
                    }
                    break;
            }
            return false;
        }

        private void SearchObjectTextBox_KeyPress( object sender, KeyPressEventArgs e )
        {
            switch( e.KeyChar )
            {
                case '\r':
                    e.Handled = DoSearchObjectByGroup( SearchObjectTextBox.Text );
                    break;
            }
        }

        private void SearchObjectButton_Click( object sender, EventArgs e )
        {
            DoSearchObjectByGroup( SearchObjectTextBox.Text );
        }

        private void ToggleClassesHierachy( object sender, EventArgs e )
        {
            if( !_CheckBox_ToggleHierachy.Checked )
                return;

            _CheckBox_ToggleHierachy.Enabled = false;
            TreeView_Classes.BeginUpdate();
            var nodes = TreeView_Classes.Nodes;
            for( var i = 0; i < nodes.Count; ++ i )
            {
                var node = TreeView_Classes.Nodes[i] as ObjectNode;
                if( node == null )
                {
                    continue;
                }

                var classObject = node.Object as UClass;
                if( classObject == null )
                {
                    continue;
                }

                string parentNodeName;
                if( classObject.Super != null 
                    && classObject.Super.ImportTable != null
                    && !classObject.IsClassWithin() )
                {
                    var name = classObject.Super.GetOuterGroup();
                    var col = nodes.Find( name, false );
                    var groupNode = col.Length == 0 ? nodes.Add( name, name, "Diagram", "Diagram" ) : col.First();
                    nodes.RemoveAt( i ); -- i;
                    groupNode.Nodes.Add( node );
                    continue;
                }

                TreeNode otherNode;
                if( classObject.ImportTable != null )
                {
                    var name = classObject.GetOuterGroup();
                    parentNodeName = name;
                }
                else
                {
                    if( classObject.Super == null )
                        continue;

                    parentNodeName = ((classObject.IsClassWithin() 
                        && String.Compare( classObject.Super.Name, "Object", StringComparison.OrdinalIgnoreCase ) == 0)
                        ? classObject.Within : classObject.Super).Name;
                }

                // Search a new parent for the current looped node.
                if( !FindNodeRecursive( nodes, out otherNode, parentNodeName ) ) 
                    continue;

                nodes.RemoveAt( i ); -- i;
                var classesNode = otherNode.Nodes["CLASSES"] 
                    ?? otherNode.Nodes.Add( "CLASSES", "Classes", "UClass-Within", "UClass-Within" );
                classesNode.Nodes.Add( node );
            }
            //TreeView_Classes.Sort();
            TreeView_Classes.EndUpdate();
        }

        private static bool FindNodeRecursive( IEnumerable collection, out TreeNode node, string nodeText )
        {
            foreach( var otherNode in collection.OfType<TreeNode>() )
            {
                if( otherNode.Text == nodeText )
                {
                    node = otherNode;
                    return true;
                }
                if( FindNodeRecursive( otherNode.Nodes, out node, nodeText ) )
                {
                    return true;
                }
            }
            node = null;
            return false;
        }

        private void UC_PackageExplorer_Leave(object sender, EventArgs e)
        {
            _Tools_StripDropDownButton.Enabled = false;
        }

        private void RecentToolStripDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            recentToolStripDropDownButton.DropDownItems.Clear();

            if (_BufferIndex == -1)
            {
                return;
            }

            var recentItems = _ContentBuffer
                //.GetRange(0, _BufferIndex + 1)
                .Select((d, i) => new ToolStripMenuItem
                {
                    Text = d.Node?.ToString() ?? d.Label,
                    Tag = i,
                    Checked = i == _BufferIndex
                })
                .Reverse()
                .ToArray<ToolStripMenuItem>();

            recentToolStripDropDownButton.DropDownItems.AddRange(recentItems);
        }

        private void RecentToolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int bufferIndex = (int)e.ClickedItem.Tag;
            RestoreBufferedContent(bufferIndex);
            _BufferIndex = bufferIndex;
        }
    }
}
