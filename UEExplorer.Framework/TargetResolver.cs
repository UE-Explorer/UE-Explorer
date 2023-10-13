using UELib;

namespace UEExplorer.Framework
{
    public static class TargetResolver
    {
        public static object Resolve(object target)
        {
            switch (target)
            {
                case UObjectTableItem item: return Resolve(item);
                case PackageReference packageReference: return Resolve(packageReference);
                default: return target;
            }
        }

        public static object Resolve(UObjectTableItem item)
        {
            return item.Object;
        }

        public static object Resolve(PackageReference packageReference)
        {
            return packageReference.Linker;
        }
    }
}
