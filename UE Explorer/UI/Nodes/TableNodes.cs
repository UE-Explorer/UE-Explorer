using System;
using System.Windows.Forms;
using UELib;
using UELib.Flags;

namespace UEExplorer.UI.Nodes
{
    public abstract class UTableNode : TreeNode, IContainsTable, IDecompilableObject 
    {
        public UObjectTableItem Table{ get; set; }
        public IUnrealDecompilable Object{ get{ return Table.Object; } }

        protected bool _IsInitialized;

        public abstract void Expanded();
        public abstract void Selected();

        public abstract string Decompile();
    }

    public sealed class UExportNode : UTableNode
    {
        public new UExportTableItem Table{ private get{ return base.Table as UExportTableItem; } set{ base.Table = value; } }

        public UExportNode()
        {
            Nodes.Add( "DUMMYNODE" );
        }

        public override string Decompile()
        {
            if( Table == null || Object == null )
                return String.Empty;

            return Table.ToString( true ) + "\r\n" + Object.Decompile();
        }

        private void BuildChildren()
        {
            if( _IsInitialized )
                return;

            TreeView.BeginUpdate();
                Nodes.Clear();
                ulong objFlags = Table.ObjectFlags;
                if( objFlags != 0 )
                {
                    string flagTitle = "Flags(" + UnrealMethods.FlagToString( Table.ObjectFlags ) + ")";
                    var flagNode = Nodes.Add( flagTitle  );
                    flagNode.ToolTipText = UnrealMethods.FlagsListToString
                    ( 
                        UnrealMethods.FlagsToList( typeof(ObjectFlagsLO), typeof(ObjectFlagsHO), Table.ObjectFlags ) 
                    );	
                }

                if( Table.ExportFlags != 0 )
                {
                    Nodes.Add( "Export Flags:" + UnrealMethods.FlagToString( Table.ExportFlags ) );
                }

                Nodes.Add( "Object:" + Table );
                if( Table.ClassIndex != 0 )
                {
                    Nodes.Add( "Class:" + Table.ClassTable );
                }

                if( Table.SuperIndex != 0 )
                {
                    Nodes.Add( "Super:" + Table.SuperTable);
                }

                if( Table.OuterIndex != 0 )
                {
                    Nodes.Add( "Outer:" + Table.OuterTable );
                }

                if( Table.ArchetypeIndex != 0 )
                {
                    Nodes.Add( "Archetype:" + Table.ArchetypeTable );
                }

                if( Table.SerialSize > 0 )
                {
                    Nodes.Add( "Object Size:" + Table.SerialSize );
                    Nodes.Add( "Object Offset:" + Table.SerialOffset );
                }	
            TreeView.EndUpdate();
            _IsInitialized = true;
        }

        public override void Expanded()
        {
            BuildChildren();
        }

        public override void Selected()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class UImportNode : UTableNode
    {
        public new UImportTableItem Table{ private get{ return base.Table as UImportTableItem; } set{ base.Table = value; } }

        public UImportNode()
        {
            Nodes.Add( "DUMMYNODE" );
        }

        public override string Decompile()
        {
            if( Table == null )
                return String.Empty;

           return Table.ToString( true );
        }

        private void BuildChildren()
        {
            if( _IsInitialized )
                return;

            TreeView.BeginUpdate();
                Nodes.Clear();
                Nodes.Add( "Object:" + Table );
                Nodes.Add( "Class:" + Table.ClassName + "(" + Table.ClassIndex + ")" );
                Nodes.Add( "Package:" + Table.PackageName + "(" + Table.PackageIndex + ")" );
                if( Table.OuterIndex != 0 )
                {
                    Nodes.Add( "Outer:" + Table.OuterTable );
                }
            TreeView.EndUpdate();
            _IsInitialized = true;
        }

        public override void Expanded()
        {
            BuildChildren();
        }

        public override void Selected()
        {
            throw new NotImplementedException();
        }
    }
}
