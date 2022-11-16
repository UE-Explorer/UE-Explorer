using System;

namespace UEExplorer.Framework
{
    public class PackageEventArgs : EventArgs
    {
        public readonly PackageReference Package;

        public PackageEventArgs(PackageReference package) => Package = package;
    }

    public delegate void PackageEventHandler(object sender, PackageEventArgs e);
}
