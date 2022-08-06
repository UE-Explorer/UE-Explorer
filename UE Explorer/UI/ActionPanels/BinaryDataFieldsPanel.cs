using System;
using System.Windows.Forms;
using UEExplorer.UI.Tabs;
using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class BinaryDataFieldsPanel : ActionPanel, IActionPanel<object>
    {
        public ContentNodeAction Action { get; } = ContentNodeAction.Binary;

        public BinaryDataFieldsPanel()
        {
            InitializeComponent();
        }

        protected override void UpdateOutput(object target)
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
                    throw new NotSupportedException($"{Object} is not a supported type");
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