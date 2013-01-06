using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UEExplorer.Properties;
using UELib.Engine;

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

        /// <summary>
        /// Called when the Tab is added to the chain.
        /// </summary>
        protected override void TabCreated()
        {						
            var langPath = Path.Combine( Application.StartupPath, "Config", "UnrealScript.xshd" );
            if( File.Exists( langPath ) )
            {
                try
                {
                    TextEditorPanel.textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load( 
                        new System.Xml.XmlTextReader( langPath ), 
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance 
                    );
                    TextEditorPanel.searchWiki.Click += SearchWiki_Click;
                    TextEditorPanel.textEditor.ContextMenuOpening += ContextMenu_ContextMenuOpening;
                    TextEditorPanel.copy.Click += Copy_Click;

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

            _Form = Tabs.Form;
            base.TabCreated();
        }

        void Copy_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            TextEditorPanel.textEditor.Copy();
        }

        string GetSelection()
        {
            return TextEditorPanel.textEditor.TextArea.Selection.GetText( TextEditorPanel.textEditor.Document );
        }

        void ContextMenu_ContextMenuOpening( object sender, System.Windows.Controls.ContextMenuEventArgs e )
        {
            if( TextEditorPanel.textEditor.TextArea.Selection.Length == 0 )
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            var selection = GetSelection();
            if( selection.IndexOf( '\n' ) != -1 )
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            
            TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchWiki.Header = String.Format( Resources.SEARCH_WIKI_ITEM, selection );
        }

        void SearchWiki_Click( object sender, System.Windows.RoutedEventArgs e )
        {
            Process.Start
            ( 
                String.Format
                ( 
                    "http://wiki.beyondunreal.com/?ns0=1&ns100=1&ns102=1&ns104=1&ns106=1&search={0}&title=Special%3ASearch&fulltext=Advanced+search&fulltext=Advanced+search",
                    GetSelection()
                ) 
            );
        }

        public void PostInitialize()
        {  
            LoadPackage();
        }

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
            ProgressStatus.SetStatus( Resources.PACKAGE_LOADING );
            // Open the file.
            try
            {
                UnrealConfig.Platform = (UnrealConfig.CookedPlatform)Enum.Parse
                ( 
                    typeof(UnrealConfig.CookedPlatform), 
                    _Form.Platform.Text, true 
                );
                _UnrealPackage = UnrealLoader.LoadPackage( FileName );
                UnrealConfig.SuppressSignature = false;

                if( _UnrealPackage.CompressedChunks != null && _UnrealPackage.CompressedChunks.Capacity > 0 )
                {
                    if( MessageBox.Show( Resources.PACKAGE_IS_COMPRESSED,
                        Resources.NOTICE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question
                    ) == DialogResult.OK )
                    {
                        Process.Start( "http://www.gildor.org/downloads" );
                        MessageBox.Show( Resources.COMPRESSED_HOWTO, 
                            Resources.NOTICE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information 
                        );
                    }
                    TabControl_General.Selected -= TabControl_General_Selected;
                    TabControl_General.TabPages.Remove( TabPage_Objects );
                    TabControl_General.TabPages.Remove( TabPage_Tables );
                    InitializeMetaInfo();
                    InitializeUI();
                    return;
                }

                TabControl_General.TabPages.Remove( TabPage_Chunks );
            }
            catch( FileLoadException )
            {
                _UnrealPackage = null;
                if( MessageBox.Show(
                        Resources.PACKAGE_UNKNOWN_SIGNATURE,
                        Resources.Warning, MessageBoxButtons.YesNo
                    ) == DialogResult.No
                )
                {
                    Tabs.Remove( this );
                    return;
                }
                UnrealConfig.SuppressSignature = true;
                goto reload;
            }
            catch( Exception e )
            {
                throw new UnrealException( e.Message, e );
            }

            string ntlPath = Path.Combine( Application.StartupPath, "Native Tables", Program.Options.NTLPath );
            if( File.Exists( ntlPath + NativesTablePackage.Extension ) )
            {
                // Load the native names.
                try
                {
                    _UnrealPackage.NTLPackage = new NativesTablePackage();
                    _UnrealPackage.NTLPackage.LoadPackage( ntlPath ); 
                }
                catch( Exception e )
                {
                    _UnrealPackage.NTLPackage = null;
                    throw new UnrealException
                    ( 
                        String.Format( "Couldn't load {0}! \r\nEvent:Loading Package", ntlPath ), e 
                    );
                }
            }

            InitializeMetaInfo();
            InitializePackage();
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
                    EngineValue.Text 				=  _UnrealPackage.EngineVersion.ToString( CultureInfo.InvariantCulture );
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
                    CookerValue.Text				= _UnrealPackage.CookerVersion.ToString( CultureInfo.InvariantCulture );
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

        private abstract class UnrealTableNode : TreeNode, IContainsTable 
        {
            public UObjectTableItem Table{ get; set; }
            protected bool _Initialized;

            public abstract void Initialize();
        }

        private class ExportNode : UnrealTableNode, IDecompilableObject
        {
            public IUnrealDecompilable Object
            {
                get{ return Table.Object; }
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
                    var flagNode = Nodes.Add( flagTitle  );
                    flagNode.ToolTipText = UnrealMethods.FlagsListToString
                    ( 
                        UnrealMethods.FlagsToList( typeof(ObjectFlagsLO), typeof(ObjectFlagsHO), exp.ObjectFlags ) 
                    );	
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
                TreeView.EndUpdate();
            }
        }

        private class ImportNode : UnrealTableNode, IDecompilableObject
        {
            public IUnrealDecompilable Object
            {
                get{ return Table.Object; }
            }

            public string Decompile()
            {
                string output = String.Empty;
                output += "\r\nOffset:" + Table.Offset;
                output += "\r\nSize:" + Table.Size;
                return output;
            }

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

        private ProgramForm _Form;

        private List<UClass> _ClassesList;

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

        [MTAThreadAttribute]
        protected void InitializeUI()
        {
            ProgressStatus.SetStatus( Resources.INITIALIZING_UI );

            // Disable misc' functionalities.
            //causesvalidation = false;
            SuspendLayout();

            exportDecompiledClassesToolStripMenuItem.Click 			+= _OnExportClassesClick;
            exportScriptClassesToolStripMenuItem.Click 				+= _OnExportScriptsClick;

            TreeView_Classes.AfterSelect 							+= _OnClassesNodeSelected;
            TreeView_Classes.BeforeExpand							+= _OnClassesNodeExpand;
            // Package Info

            if( _UnrealPackage.Objects != null )
            {
                CreateClassesList();
                // HACK:Add a MetaData object to the classes tree(hack because MetaData is not an actual class)
                var metobj = _UnrealPackage.FindObject( "MetaData", typeof( UMetaData ) );
                if( metobj != null )
                {
                    var node = new ObjectNode( metobj )
                    {
                        ImageKey = "UClass", 
                        SelectedImageKey = "UClass", 
                        Text = metobj.Name
                    };
                    node.Nodes.Add( "DUMMYNODE" );
                    if( metobj.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
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

            if( _UnrealPackage.Generations != null )
            {
                CreateGenerationsList();
            }

            if( _UnrealPackage.CompressedChunks != null )
            {
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

            ResumeLayout();
            CausesValidation = true;
        }

        private void EmptyIsPackage()
        {
            exportScriptClassesToolStripMenuItem.Enabled = false;
            exportDecompiledClassesToolStripMenuItem.Enabled = false;

            Num_ObjectIndex.Enabled = false;
            Num_NameIndex.Enabled = false;
        }

        private delegate void CreateTableDelegate( object nameTable );

        protected void CreateNameTable( object nameTable )
        {
            if( DataGridView_NameTable.InvokeRequired )
            {
                var del = new CreateTableDelegate( CreateNameTable );
                foreach( var t in _UnrealPackage.Names )
                {
                    Invoke( del, t );
                }
            }
            else
            {
                DataGridView_NameTable.Rows.Add( 
                    ((UNameTableItem)nameTable).Name, 
                    String.Format( "{0:x4}", 
                    ((UNameTableItem)nameTable).Flags ) 
                );
            }
        }

        private TreeNode[] _ExportNodes;
        private int _NIndex;
        protected void CreateExportTable( object exportTable )
        {
            if( TreeView_Exports.InvokeRequired )
            {
                var del = new CreateTableDelegate( CreateExportTable );
                foreach( var t in _UnrealPackage.Exports )
                {
                    TreeView_Exports.Invoke( del, t );
                }
            }
            else
            {
                if( _NIndex == 0 )
                {
                    _ExportNodes = new TreeNode[_UnrealPackage.Exports.Count];
                }

                var exp = exportTable as UExportTableItem;
                var node = new ExportNode {Table = exp, Text = exp.ObjectName};
                SetImageKeyForObject( exp, node );

                node.SelectedImageKey = node.ImageKey;
                _ExportNodes[_NIndex ++] = node;

                if( exp.Object != null && exp.Object.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                {
                    AddSerToolTipError( node, exp.Object );
                }
                else if( !_UnrealPackage.IsRegisteredClass( exp.ClassName != String.Empty ? exp.ClassName : "Class" ) )
                {
                    node.ForeColor = Color.DarkOrange;
                    node.ToolTipText = String.Format( Resources.CLASS_ISNT_SUPPORTED, exp.ClassName );
                }

                if( _NIndex == _UnrealPackage.Exports.Count )
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
                CreateTableDelegate del = CreateImportTable;
                for( int i = 0; i < _UnrealPackage.Imports.Count; ++ i )
                {
                    Invoke( del, _UnrealPackage.Imports[i] );
                }
            }
            else
            {
                if( _NIndex == 0 )
                {
                    _ExportNodes = new TreeNode[_UnrealPackage.Imports.Count];
                }

                var imp = (importTable as UImportTableItem);
                ImportNode node = new ImportNode
                {
                    Table = imp,
                    Text = imp.ObjectName
                };
                _ExportNodes[_NIndex ++] = node;

                SetImageKeyForObject( imp, node );


                if( _NIndex == _UnrealPackage.Imports.Count )
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

            findInClassesToolStripMenuItem.Enabled = true;
            _ClassesList.Sort( (cl, cl2) => String.Compare( cl.Name, cl2.Name, StringComparison.Ordinal ) );

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

                if( Object.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
                {
                    node.ForeColor = Color.Red;
                }
                TreeView_Classes.Nodes.Add( node );	
            }
            TreeView_Classes.EndUpdate();
        }

        protected void CreateDependenciesList()
        {
            if( _UnrealPackage.Objects == null || _UnrealPackage.Objects.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Deps );
                return;
            }

            foreach( var table in _UnrealPackage.Imports )
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
            }
        }

        protected void GetDependencyOn( UImportTableItem parent, TreeNode node )
        {
            if( node == null )
                return;

            foreach( var table in _UnrealPackage.Imports.Where( table => table != parent && table.OuterTable == parent ) )
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
                    node.ImageKey = tableObject.ClassName == "Package" ? "List" : tableObject.Object.GetType().Name;	
                }
                node.SelectedImageKey = node.ImageKey;
            }
        }

        protected void CreateGenerationsList()
        {
            if( _UnrealPackage.Generations == null || _UnrealPackage.Generations.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Generations );
                return;
            }

            foreach( var gen in _UnrealPackage.Generations )
            {
                DataGridView_GenerationsTable.Rows.Add( gen.NamesCount, gen.ExportsCount, gen.NetObjectsCount );
            }
        }

        protected void CreateContentList()
        {
            if( _UnrealPackage.Objects == null || _UnrealPackage.Objects.Count == 0 )
            {
                TabControl_Objects.Controls.Remove( TabPage_Content );
                return;
            }

            var groups = new List<ObjectNode>();
            foreach( var obj in _UnrealPackage.Objects.Where(
                    o => (int)o > 0 && o.Outer != null 
                    && (o.ResistsInGroup() || o.HasObjectFlag( ObjectFlagsLO.Automated ))
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
            Tabs.Remove( this );
            Tabs.Form.LoadFile( FileName );
        }

        private void OutputNodeObject( TreeNode treeNode )
        {
            try
            {
                if( treeNode is IUnrealDecompilable )
                {
                    SetContentText( treeNode, ((IUnrealDecompilable)treeNode).Decompile() );

                    // Assemble a title
                    string newTitle;
                    if( treeNode is IDecompilableObject )
                    {
                        UObject obj;
                        if( (obj = ((IDecompilableObject)treeNode).Object as UObject) != null )
                        {
                            newTitle = obj.GetOuterGroup();
                            if( obj.DeserializationState.HasFlag( UObject.ObjectState.Errorlized ) )
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
                return;

            viewToolsContextMenu.ItemClicked += itemClicked;
            viewToolsContextMenu.Show( tree, e.Location );	
        }

        private static void BuildItemNodes( TreeNode performingNode, ToolStripItemCollection itemCollection, ToolStripItemClickedEventHandler itemClickEvent = null )
        {
            itemCollection.Clear();

            var addItem = (Action<string, string>)((title, id) =>
            {
                var item = itemCollection.Add( title );	
                item.Name = id;
            });

            var decompilableNode = performingNode as IUnrealDecompilable;
            if( decompilableNode != null ) 
            { 
                addItem.Invoke( Resources.NodeItem_ViewObject, "OBJECT" );
                var decompilableObjectNode = decompilableNode as IDecompilableObject;
                if( decompilableObjectNode != null )
                {
                    if( decompilableObjectNode.Object is IUnrealViewable )
                    { 
                        if( File.Exists( Program.Options.UEModelAppPath	) )
                        {
                            addItem.Invoke( Resources.NodeItem_OpenInUEModelViewer, "OPEN_UEMODELVIEWER" );
        #if DEBUG
                            addItem.Invoke( Resources.NodeItem_ExportWithUEModelViewer, "EXPORT_UEMODELVIEWER" );
        #endif
                        }
        #if DEBUG
                        addItem.Invoke( Resources.NodeItem_ViewContent, "CONTENT" );
        #endif	
                    }

                    if( decompilableObjectNode.Object is UMetaData )
                    {
                        addItem.Invoke( Resources.NodeItem_ViewUsedTags, "USED_TAGS" );	
                    }

                    var unStruct = (decompilableObjectNode.Object as UStruct); 
                    if( unStruct != null )
                    {
                        if( unStruct.DataScriptSize > 0 )
                        {
                            if( decompilableObjectNode.Object is UClass )
                            {
                                addItem.Invoke( Resources.NodeItem_ViewReplication, "REPLICATION" );	
                            }
                            addItem.Invoke( Resources.NodeItem_ViewTokens, "TOKENS" );
                        }

                        var unClass = decompilableObjectNode.Object as UClass;
                        if( unClass != null && unClass.ScriptBuffer != null )
                        {
                            addItem.Invoke( Resources.NodeItem_ViewScript, "SCRIPT" );	
                        }

                        if( unStruct.Properties != null && unStruct.Properties.Any() )
                        {
                            addItem.Invoke( Resources.NodeItem_ViewDefaultProperties, "DEFAULTPROPERTIES" );	
                        }
                    }

                    var myObj = decompilableObjectNode.Object as UObject;

                    var bufferedObject = decompilableObjectNode.Object as IBuffered;
                    if( bufferedObject != null && bufferedObject.GetBuffer() != null )
                    {
                        var bufferedItem = new ToolStripMenuItem 
                        {
                            Text = Resources.NodeItem_ViewBuffer,
                            Name = "BUFFER"
                        };

                        bool shouldAddBufferItem = bufferedObject.GetBufferSize() > 0;

                        var tableNode = decompilableObjectNode.Object as IContainsTable;
                        if( tableNode != null && tableNode.Table != null )
                        { 
                            var tableBufferItem = bufferedItem.DropDownItems.Add( Resources.NodeItem_ViewTableBuffer );
                            tableBufferItem.Name = "TABLEBUFFER";
                            shouldAddBufferItem = true;
                        }

                        if( myObj != null && myObj.Default != null && myObj.Default != myObj )
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

                    if( myObj != null )
                    {
                        if( myObj.ThrownException != null )
                        {
                            itemCollection.Add( new ToolStripSeparator() );
                            addItem.Invoke( Resources.NodeItem_ViewException, "EXCEPTION" );		
                        }
                        itemCollection.Add( new ToolStripSeparator() );
                        addItem.Invoke( Resources.NodeItem_ManagedProperties, "MANAGED_PROPERTIES" );
                        #if DEBUG
                            addItem.Invoke( "Force Deserialize", "FORCE_DESERIALIZE" );
                        #endif	
                    }
                }
            }
        }

        private void _OnClassesItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Classes.SelectedNode as IDecompilableObject, e.ClickedItem.Name );	
        }

        private void _OnExportsItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Exports.SelectedNode as IDecompilableObject, e.ClickedItem.Name );
        }

        private void _OnContentItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            PerformNodeAction( TreeView_Content.SelectedNode as IDecompilableObject, e.ClickedItem.Name );
        }

        private void PerformNodeAction( IDecompilableObject node, string action )
        {
            if( node == null )
                return;

            try
            {
                switch( action )
                {
                    case "USED_TAGS":
                    {
                        var n = node as ObjectNode;
                        if( n != null )
                        {
                            var cnode = n.Object as UMetaData;
                            if( cnode != null )
                            {
                                Label_ObjectName.Text = ((TreeNode)node).Text;
                                SetContentText( node as TreeNode, cnode.GetUniqueMetas() );
                            }
                        }
                        break;
                    }
#if DEBUG
                    case "CONTENT":
                    {
                        var n = node as ObjectNode;
                        if( n != null )
                        {
                            var cnode = n.Object as IUnrealViewable;
                            if( cnode is UTexture )
                            {
                                var tex = ((UTexture)cnode);
                                tex.BeginDeserializing();
                                if( tex.MipMaps == null || tex.MipMaps.Count == 0 )
                                    break;

                                Image picture = new Bitmap( (int)tex.MipMaps[0].Width, (int)tex.MipMaps[0].Height );
                                var painter = Graphics.FromImage( picture );
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
                    case "OPEN_UEMODELVIEWER":
                    {
                        Process.Start
                        ( 
                            Program.Options.UEModelAppPath, 
                            "-path=" + _UnrealPackage.PackageDirectory
                            + " " + _UnrealPackage.PackageName
                            + " " + ((TreeNode)node).Text
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
                            + " " + ((TreeNode)node).Text;
                        var appInfo = new ProcessStartInfo
                        (
                            Program.Options.UEModelAppPath, 
                            appArguments
                        );
                        appInfo.UseShellExecute = false;
                        appInfo.RedirectStandardOutput = true;
                        appInfo.CreateNoWindow = false;
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
                        Label_ObjectName.Text = ((TreeNode)node).Text;
                        SetContentText( node as TreeNode, node.Decompile() );
                        break;

                    case "MANAGED_PROPERTIES":
                        var propDialog = new PropertiesDialog();
                        propDialog.ObjectLabel.Text = ((TreeNode)node).Text;
                        propDialog.ObjectPropertiesGrid.SelectedObject = node.Object;
                        propDialog.ShowDialog( this );
                        break;

#if DEBUG
                    case "FORCE_DESERIALIZE":
                        Label_ObjectName.Text = ((TreeNode)node).Text;

                        ((UObject)node.Object).BeginDeserializing();
                        ((UObject)node.Object).PostInitialize();
                        break;
#endif

                    case "REPLICATION":
                    {
                        var unClass = node.Object as UClass;
                        if( unClass != null )
                        {
                            Label_ObjectName.Text = unClass.Name;
                            SetContentText( node as TreeNode, unClass.FormatReplication() );
                        }
                        break;
                    }

                    case "SCRIPT":
                    {
                        var unClass = node.Object as UClass;
                        if( unClass != null && unClass.ScriptBuffer != null )
                        {
                            Label_ObjectName.Text = unClass.ScriptBuffer.Name;
                            SetContentText( node as TreeNode, unClass.ScriptBuffer.Decompile() );
                        }
                        break;
                    }

                    case "DEFAULTPROPERTIES":
                    {
                        var unStruct = node.Object as UStruct;
                        if( unStruct != null )
                        {
                            Label_ObjectName.Text = unStruct.Default.Name;
                            SetContentText( node as TreeNode, unStruct.FormatDefaultProperties() );
                        }
                        break;
                    }

                    case "TOKENS":
                    {
                        var unStruct = node.Object as UStruct;
                        if( unStruct != null && unStruct.DataScriptSize > 0 )
                        {
                            Label_ObjectName.Text = ((TreeNode)node).Text;

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

                    case "BUFFER":
                    {
                        var bufferObject = node.Object as IBuffered;
                        if( bufferObject.GetBufferSize() > 0 )
                        {
                            ViewBufferFor( bufferObject );
                        }
                        break;
                    }

                    case "TABLEBUFFER":
                    {
                        var tableObject = node as IContainsTable ?? node.Object as IContainsTable;
                        ViewBufferFor( tableObject.Table );
                        break;
                    }

                    case "DEFAULTBUFFER":
                    {
                        var unObject = node.Object as UObject;
                        if( unObject != null )
                        {
                            ViewBufferFor( unObject.Default );
                        }
                        break;
                    }

                    case "EXCEPTION":
                    {
                        var oNode = node as ObjectNode;
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

            if( e.KeyChar == '\r' )
            {
                EditorUtil.FindText( TextEditorPanel, SearchBox.Text );	  
                e.Handled = true;
            }
        }

        public override void TabFind()
        {
            new FindDialog( TextEditorPanel ).Show();
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
                SearchBox.Enabled = true;
                ExportButton.Enabled = true;
                WPFHost.Enabled = true;
                ViewTools.Enabled = true;
                findInDocumentToolStripMenuItem.Enabled = true;
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

            var hexDialog = new HexViewDialog( target, this );
            hexDialog.Show( _Form );
        }

        private readonly List<TreeNode> _FilteredNodes = new List<TreeNode>();

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

        private readonly Pen _BorderPen = new Pen( Color.FromArgb( 237, 237, 237 ) );
        private readonly Pen _LinePen = new Pen( Color.White );

        private void panel4_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.DrawRectangle( _BorderPen, 0, 0, panel4.Width-1, panel4.Height-1 );
        }

        private void panel1_Paint( object sender, PaintEventArgs e )
        {
            e.Graphics.DrawRectangle( _BorderPen, 0, 0, panel1.Width-1, panel1.Height-1 );
        }

        private void toolStripSeparator1_Paint( object sender, PaintEventArgs e )
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
            foreach( string ext in exportableObject.ExportableExtensions )
            {
                extensions += string.Format( "{0}(*" + ".{0})|*.{0}", ext );
                if( ext != exportableObject.ExportableExtensions.Last() )
                {
                    extensions += "|";
                }
            }
            var dialog = new SaveFileDialog{Filter = extensions, FileName = ((UObject)exportableObject).Name};
            if( dialog.ShowDialog() == DialogResult.OK )
            {
                var stream = new FileStream( dialog.FileName, FileMode.Create, FileAccess.Write );
                exportableObject.SerializeExport( exportableObject.ExportableExtensions.ElementAt( dialog.FilterIndex - 1 ), stream );
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

        private void ViewTools_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            if( _LastNodeContent == null )
                return;

            var decompilableObject = _LastNodeContent as IDecompilableObject;
            if( decompilableObject == null )
            {
                return;
            }

            PerformNodeAction( decompilableObject, e.ClickedItem.Name );
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

        public class DocumentResult
        {
            public Object Document;
            public List<FindResult> Results;
        };

        private int _FindCount;
        private TabPage _FindTab;
        private void FindInClassesToolStripMenuItem_Click( object sender, EventArgs e )
        {
            var findDialog = new FindDialog();
            if( findDialog.ShowDialog() != DialogResult.OK )
            {
                return;
            }

            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus( Resources.SEARCHING_CLASSES_STATUS );

            var documentResults = new List<DocumentResult>();

            var findText = findDialog.FindInput.Text;
            foreach( var content in _ClassesList )
            {
                var	findContent = content.Decompile();
                var findResults = FindText( findContent, findText );
                if( !findResults.Any() )
                {
                    continue;
                }

                var document = new DocumentResult
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
                var findResult = nodeEvent.Node.Tag as FindResult;
                if( findResult == null )
                {
                    return;
                }

                var documentResult = nodeEvent.Node.Parent.Tag as DocumentResult;

                var unClass = ((UClass)documentResult.Document);
                Label_ObjectName.Text = String.Format( "{0}: {1}, {2}", unClass.Name, findResult.TextLine, findResult.TextColumn ); 
                SetContentText( nodeEvent.Node, unClass.Decompile() );
                TextEditorPanel.textEditor.ScrollTo( findResult.TextLine, findResult.TextColumn );
                TextEditorPanel.textEditor.Select( findResult.TextIndex, findText.Length );
            };
        }

        public class FindResult
        {
            public int TextIndex;
            public int TextLine;
            public int TextColumn;

            public override string ToString()
            {
                return String.Format( "({0}, {1})", TextLine, TextColumn );
            }
        };

        public static List<FindResult> FindText( string text, string keyword )
        {
            keyword = keyword.ToLower();
            var results = new List<FindResult>();

            var currentLine = 1;
            var currentColumn = 1;
            for( int i = 0; i < text.Length; ++ i )
            {
                if( text[i] == '\n' )
                {
                    ++ currentLine;
                    currentColumn = 1;
                    continue;
                }

                var startIndex = i;
                for( int j = 0; j < keyword.Length; ++ j )
                {
                    if( Char.ToLower( text[startIndex] ) == keyword[j] )
                    {
                        ++ startIndex;
                        if( j == keyword.Length - 1 )
                        {
                            var result = new FindResult
                            {
                                TextIndex = startIndex - keyword.Length,
                                TextLine = currentLine,
                                TextColumn = currentColumn
                            };
                            results.Add( result );
                            break;
                        }
                        continue;	
                    }
                    break;
                }
                ++ currentColumn;
            }
            return results;
        }

        private void findInDocumentToolStripMenuItem_Click( object sender, EventArgs e )
        {
            SearchBox.Focus();
        }

        private void SearchBox_TextChanged( object sender, EventArgs e )
        {
            FindButton.Enabled = SearchBox.Text.Length > 0;
            findNextToolStripMenuItem.Enabled = FindButton.Enabled;
        }

        private void findNextToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if( TextEditorPanel == null )
            {
                return;
            }
            EditorUtil.FindText( TextEditorPanel, SearchBox.Text );
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
    }
}
