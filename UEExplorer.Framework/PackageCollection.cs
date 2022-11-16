using System.Collections.ObjectModel;
using UELib.Annotations;

namespace UEExplorer.Framework
{
    public class PackageCollection : Collection<PackageReference>
    {
        [CanBeNull]
        public PackageReference Get(string filePath)
        {
            foreach (var item in Items)
            {
                if (item.FilePath == filePath)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
