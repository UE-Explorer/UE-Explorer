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

		public static string SettingsPath = Path.Combine( Application.StartupPath, "Config", "UEExplorerConfig.xml" );
		public static XMLSettings Options;

		public static void LoadConfig()
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
		}

		public static void SaveConfig()
		{
			if( Options == null )
				Options = new XMLSettings();

			using( var w = new XmlTextWriter( SettingsPath, Encoding.ASCII ) )
			{
				var xser = new XmlSerializer( typeof(XMLSettings) );
				xser.Serialize( w, Options );
			}
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
		#endregion
	}
}
