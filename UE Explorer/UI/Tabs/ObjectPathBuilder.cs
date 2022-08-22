using UELib;
using UELib.Core;

namespace UEExplorer.UI.Tabs
{
    public class ObjectPathBuilder
    {
        public static string GetPath(object obj)
        {
            return obj.ToString();
        }

        public static string GetPath(UObject obj)
        {
            return obj.GetOuterGroup();
        }

        public static string GetPath(IBinaryData obj)
        {
            return obj.GetBufferId();
        }
    }
}