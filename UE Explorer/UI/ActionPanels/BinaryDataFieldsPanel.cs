using System;
using System.Windows.Forms;
using UEExplorer.UI.Tabs;
using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class BinaryDataFieldsPanel : Panel, IActionPanel<object>
    {
        public ContentNodeAction Action { get; } = ContentNodeAction.Binary;

        private object _Object;

        public object Object
        {
            get => _Object;
            set
            {
                _Object = value;
                UpdateOutput(value);
            }
        }
        
        public BinaryDataFieldsPanel()
        {
            InitializeComponent();
        }
        
        private void UpdateOutput(object target)
        {
            switch (target)
            {
                case null:
                    BinaryFieldBindingSource.DataSource = null;
                    return;

                case IBinaryData binaryDataObject:
                    BinaryFieldBindingSource.DataSource = binaryDataObject.BinaryMetaData?.Fields;
                    break;

                default:
                    throw new NotSupportedException($"{_Object} is not a supported type");
            }
        }

        public void RestoreState(ref ActionState state)
        {
            // ??
        }

        public void StoreState(ref ActionState state)
        {
            state.X = binaryDataGridView.HorizontalScrollingOffset;
            state.Y = binaryDataGridView.VerticalScrollingOffset;
        }
    }
}