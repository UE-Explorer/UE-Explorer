using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class PropertiesManagerPanel : ActionPanel
    {
        public PropertiesManagerPanel()
        {
            InitializeComponent();
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
