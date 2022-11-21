using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UEExplorer.Framework
{
    [Serializable]
    public class PackageCollection : SortedSet<PackageReference>
    {
        public PackageCollection()
        {
        }

        // We need this as public in order to expose it to the Xml serializer etc.
        public PackageCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
