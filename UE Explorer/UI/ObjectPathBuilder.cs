using UELib;
using UELib.Core;

namespace UEExplorer.UI
{
    public static class ObjectPathBuilder
    {
        public static string GetPath(object obj)
        {
            return obj.ToString();
        }

        public static string GetPath(UObject obj)
        {
            return obj.GetPath();
        }

        public static string GetPath(IBinaryData obj)
        {
            return obj.GetBufferId();
        }
    }
}