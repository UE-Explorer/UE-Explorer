using System.Threading.Tasks;
using UEExplorer.UI.Tabs;
using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class BinaryDataFieldsPanel : ActionPanel, IActionPanel<object>
    {
        public BinaryDataFieldsPanel()
        {
            InitializeComponent();
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

        protected override void UpdateOutput(object target)
        {
            switch (target)
            {
                case null:
                    BinaryFieldBindingSource.DataSource = null;
                    break;

                case IBinaryData binaryDataObject:
                    BinaryFieldBindingSource.DataSource = binaryDataObject.BinaryMetaData?.Fields;
                    break;

                default:
                    BinaryFieldBindingSource.DataSource = null;
                    break;
            }
        }
    }
}