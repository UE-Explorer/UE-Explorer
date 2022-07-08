using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using UELib;
using UELib.Annotations;
using UELib.Core;

namespace UEExplorer.UI.Nodes
{
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ObjectNode : TreeNode, IWithDecompilableObject<UObject>
    {
        private static ObjectImageKeySelector _objectImageKeySelector = new ObjectImageKeySelector();

        public const string DummyNodeKey = "DUMMYNODE";
        
        [CanBeNull] public UObject Object { get; private set; }

        public ObjectNode(UObject objectRef)
        {
            Object = objectRef;

            Text = (int)objectRef < 0
                ? $"{objectRef.GetOuterGroup()}({(int)objectRef})"
                : $"{objectRef.Name}({(int)objectRef})";

            string imageKey = objectRef.Accept(_objectImageKeySelector);
            ImageKey = imageKey;
            SelectedImageKey = imageKey;

            
            if ((int)objectRef > 0)
            {
                Nodes.Add(DummyNodeKey, "Expandable");
            }
        }

        protected ObjectNode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            info.AddValue(Text, Object);
        }

        public virtual string Decompile()
        {
            UDecompilingState.ResetTabs();
            return Object?.Decompile();
        }
    }
}