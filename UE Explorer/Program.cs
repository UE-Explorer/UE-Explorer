/***********************************************************************
 *	Author - Eliot Van Uytfanghe
 *	Copyright - (C) Eliot Van Uytfanghe 2009 - 2011
 **********************************************************************/
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Web;

namespace UEExplorer
{
	using UI;

    public static class Program
    {
        [STAThread]
        static void Main()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			var app = new SingleInstanceApplication();
			app.Run( Environment.GetCommandLineArgs() );
		}

		public class SingleInstanceApplication : WindowsFormsApplicationBase
		{
			internal SingleInstanceApplication()
			{
				IsSingleInstance = true;
			}

			protected override void OnCreateMainForm()
			{
				MainForm = new UEExplorer_Form();
			}

			protected override bool OnStartup( StartupEventArgs eventArgs )
			{
				return true;
			}

			protected override void OnStartupNextInstance( StartupNextInstanceEventArgs eventArgs )
			{
				eventArgs.BringToForeground = true;
			   	var args = eventArgs.CommandLine;
				for( var i = 1; i < args.Count; ++ i )
				{
					if( File.Exists( args[i] ) )
					{
						((UEExplorer_Form)MainForm).LoadFile( args[i] );
					}
				}
			}
		}

		internal static string SettingsPath = Path.Combine( Application.StartupPath, "Config", "UEExplorerConfig.xml" );
		internal static XMLSettings Options;

		internal static void LoadConfig()
		{
			if( File.Exists( SettingsPath ) )
			{
				using( var r = new XmlTextReader( SettingsPath ) )
				{
					var xser = new XmlSerializer( typeof(XMLSettings) );
					Options = (XMLSettings)xser.Deserialize( r );
				}
			}
			else 
			{
				SaveConfig();
			}

			UELib.UnrealConfig.SuppressComments = Options.bSuppressComments;
			UELib.UnrealConfig.PreBeginBracket = ParseFormatOption( Program.Options.PreBeginBracket );
			UELib.UnrealConfig.PreEndBracket = ParseFormatOption( Program.Options.PreEndBracket );
		}

		internal static string ParseFormatOption( string input )
		{
			return input.Replace( "%NEWLINE%", "\r\n" ).Replace( "%TABS%", "{0}" );
		}

		internal static void SaveConfig()
		{
			if( Options == null )
				Options = new XMLSettings();

			using( var w = new XmlTextWriter( SettingsPath, Encoding.ASCII ) )
			{
				var xser = new XmlSerializer( typeof(XMLSettings) );
				xser.Serialize( w, Options );
			}
		}

#if DEBUG
		internal const string WEBSITE_URL = "http://localhost/eliot/";
#else
		internal const string WEBSITE_URL = "http://eliot.pwc-networks.com/";
#endif

		internal static string Post( string url, string data )
		{
			var buffer = Encoding.UTF8.GetBytes( data );
			var webReq = (HttpWebRequest)WebRequest.Create( url );
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			webReq.ContentLength = buffer.Length;

			using( var postStream = webReq.GetRequestStream())
			{
				postStream.Write( buffer, 0, buffer.Length );
			}

			var response = webReq.GetResponse();
			string result = String.Empty;
			using( var responseReader = new StreamReader( response.GetResponseStream() ) )
			{
				
			    result = responseReader.ReadToEnd();
			}
			return result;
		}
    }

	public class XMLSettings
	{
		#region Unreal Packages Decompiler Related Members
		public string NTLPath = "NativesTableList_UT2004";

		public UELib.UnrealPackage.InitFlags InitFlags = UELib.UnrealPackage.InitFlags.All;
		public bool bForceVersion;
		public bool bForceLicenseeMode;
		public ushort Version;
		public ushort LicenseeMode;
		#endregion

		#region Unreal Cache Extractor Related Membbers
		public string InitialCachePath = String.Empty;
		#endregion

		#region DECOMPILER
		public bool bSuppressComments;
		public string PreBeginBracket = "%NEWLINE%%TABS%";
		public string PreEndBracket = "%NEWLINE%%TABS%";
		#endregion

		#region THIRDPARY
		public string UEModelAppPath = String.Empty;
		#endregion
	}
}
