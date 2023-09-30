using UELib;

namespace UEExplorer.Framework
{
    public static class TargetResolver
    {
        public static object Resolve(object target)
        {
            return target is UObjectTableItem item ? item.Object : target;
        }

        public static object Resolve(UObjectTableItem item)
        {
            return item.Object;
        }
    }
}
