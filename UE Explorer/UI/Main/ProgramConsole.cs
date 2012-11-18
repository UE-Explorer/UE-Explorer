using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UELib;

namespace UEExplorer.UI.Main
{
	public partial class ProgramConsole : Form
	{
		private ConsoleWriter _ConsoleWriter;

		public ProgramConsole()
		{
			InitializeComponent();

			_ConsoleWriter = new ConsoleWriter( ConsoleOutput );
			Console.SetOut( _ConsoleWriter );
		}

		private void Console_FormClosed( object sender, FormClosedEventArgs e )
		{
			if( _ConsoleWriter != null )
			{ 
				_ConsoleWriter.Dispose();
				_ConsoleWriter = null;
			}
			Application.Exit();
		}

		private void ProgramConsole_Shown( object sender, EventArgs e )
		{
			var args = Environment.GetCommandLineArgs();
			var filePath = args[1];
			if( !File.Exists( filePath ) )
			{
				Console.WriteLine( filePath + " doesn't exist!" );
				return;		
			}

			var options = args[2].Split( '=' );
			var option = options[0];
			switch( option )
			{
				case "export":
				{ 
					bool shouldExportScripts = false;

					var exportType = options[1];
					switch( exportType )
					{
						case "classes":
							break;

						case "scripts":
							shouldExportScripts = true;
							break;

						default:
							Console.WriteLine( "Unrecognized export type " + exportType );
							return;
					}

					try
					{
						Console.WriteLine( "Exporting package " + filePath );
						using( var package = UnrealLoader.LoadFullPackage( filePath ) )
						{ 
							var exportPath = package.ExportPackageClasses( shouldExportScripts );
							Console.WriteLine( "Package successfully exported to " + exportPath );
							//Close();
						}
					}
					catch
					{
						Console.WriteLine( "An exception occurred while exporting " + filePath );
					}
					break;
				}

				default:
					Console.WriteLine( "Unrecognized commandline option " + option );
					break;
			}
		}
	}

	public class ConsoleWriter : TextWriter
	{
		private readonly RichTextBox _Output;
		private readonly Encoding _Encoding = Encoding.ASCII;
		public override Encoding Encoding
		{
			get{ return _Encoding; }
		}

		public ConsoleWriter( RichTextBox output )
		{
			_Output = output;		
		}

		public override void Write( string value )
		{
			base.Write( value );
			_Output.Text += value;
		} 

		public override void WriteLine( string value )
		{
			base.WriteLine( value );
			_Output.Text += "\r\n" + value;
		}
	}
}
