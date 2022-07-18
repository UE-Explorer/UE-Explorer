namespace UEExplorer.UI.Tabs
{
    public struct ActionState
    {
        public object SelectedNode;
        public object Target;
        public ContentNodeAction Action;

        public double Y, X;
            
        public int SelectStart;
        public int SelectLength;
    }
}