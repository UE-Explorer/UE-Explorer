using System;
using System.Windows.Forms;
using UELib;
using UELib.Flags;

namespace UEExplorer.UI.Nodes
{
    [Serializable]
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
        public new UExportTableItem Table{ private get{ return (UExportTableItem)base.Table; } set{ base.Table = value; } }

        public UExportNode()
        {
            Nodes.Add("DUMMYNODE", "Loading...");
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

            Nodes.Clear();
            ulong objFlags = Table.ObjectFlags;
            if( objFlags != 0 )
            {
                string flagTitle = $"ObjectFlags:{Table.ObjectFlags:X8}";
                var flagNode = Nodes.Add( flagTitle  );
                flagNode.ToolTipText = new UnrealFlags<ObjectFlag>(Table.ObjectFlags, Table.Owner.Branch.EnumFlagsMap[typeof(ObjectFlag)]).ToString();
            }

            if( Table.ExportFlags != 0 )
            {
                Nodes.Add( "Export Flags:" + UnrealMethods.FlagToString( Table.ExportFlags ) );
            }

            Nodes.Add( "Object:" + Table );
            if( Table.ClassIndex != 0 )
            {
                Nodes.Add( "Class:" + Table.Class );
            }

            if( Table.SuperIndex != 0 )
            {
                Nodes.Add( "Super:" + Table.Super);
            }

            if( Table.OuterIndex != 0 )
            {
                Nodes.Add( "Outer:" + Table.Outer );
            }

            if( Table.ArchetypeIndex != 0 )
            {
                Nodes.Add( "Archetype:" + Table.Archetype );
            }

            if (Table.ComponentMap != null && Table.ComponentMap.Count > 0)
            {
                var componentsNode = Nodes.Add("Components");
                foreach (var keyValuePair in Table.ComponentMap)
                {
                    var obj = Table.Owner.IndexToObject(keyValuePair.Value);
                    if (obj != null)
                    {
                        componentsNode.Nodes.Add(obj.ToString());
                    }
                }
            }
                
            if (Table.PackageFlags != 0)
            {
                Nodes.Add("PackageFlags:" + Table.PackageFlags);
            }
                
            if ((Guid)Table.PackageGuid != Guid.Empty)
            {
                Nodes.Add("PackageGuid:" + Table.PackageGuid);
            }

            if ( Table.SerialSize > 0 )
            {
                Nodes.Add( "Object Size:" + Table.SerialSize );
                Nodes.Add( "Object Offset:" + Table.SerialOffset );
            }

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
        public new UImportTableItem Table{ private get{ return (UImportTableItem)base.Table; } set{ base.Table = value; } }

        public UImportNode()
        {
            Nodes.Add("DUMMYNODE", "Loading...");
        }

        public override string Decompile()
        {
            if (Table == null)
                return string.Empty;

            return Table.ToString();
        }

        private void BuildChildren()
        {
            if( _IsInitialized )
                return;

            Nodes.Clear();
            Nodes.Add( "Object:" + Table );
            Nodes.Add("Class:" + Table.ClassName);
            Nodes.Add("Package:" + Table.ClassPackageName);
            if (Table.OuterIndex)
            {
                Nodes.Add("Outer:" + Table.Outer);
            }
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
