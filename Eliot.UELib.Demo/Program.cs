using System;
using UELib;
using System.Windows.Forms;
using System.IO;

namespace Eliot.UELib.Demo
{
    class Program
    {
        static void Main()
        {
            var source = Path.Combine( Application.StartupPath, "UT3Test.u" );
            var dest = Path.Combine( Application.StartupPath, "UT3TestSerializeDemo.u" );
            File.Copy( source, dest, true );

            using( var package = UnrealLoader.LoadFullPackage( dest, FileAccess.ReadWrite ) )
            { 
                foreach( var name in package.Names )
                {
                    if( name.Name == "Dot" )
                    {
                        name.Name = "Aot";
                    }
                }
               
                var stream = new UPackageStream( dest, FileMode.Open, FileAccess.ReadWrite );
                stream.PostInit( package );

                package.Serialize( stream );
                package.Stream.Flush();
            }

            // Load again and see if the rewriting was effective.
            using( var package = UnrealLoader.LoadFullPackage( dest, FileAccess.ReadWrite ) )
            { 
                if( package.Names.Exists( (name) => name.Name == "Aot" ) )
                {
                    Console.WriteLine( "Successfully edited Dot to Aot!" );
                }
            }
            Console.ReadKey();
        }
    }
}
