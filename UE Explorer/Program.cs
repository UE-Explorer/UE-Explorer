﻿/***********************************************************************
 *	Author - Eliot Van Uytfanghe
 *	Copyright - (C) Eliot Van Uytfanghe 2009 - 2011
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using UELib;

namespace UEExplorer
{
	using UI;

    public static class Program
    {
	    private static readonly string LogFilePath = Path.Combine( Application.StartupPath, "Log.txt" );
	    private static FileStream _LogStream;

        [STAThread]
        static void Main()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			StartLogStream();
			var app = new SingleInstanceApplication();
			app.Run( Environment.GetCommandLineArgs() );
			EndLogStream();
		}

		private static void StartLogStream()
		{
			_LogStream = new FileStream( LogFilePath, FileMode.Create, FileAccess.Write  );
			Console.SetOut( new StreamWriter( _LogStream ) );		
		}

		private static void EndLogStream()
		{
			if( _LogStream == null ) 
				return;

			_LogStream.Flush();
			_LogStream.Close();
			_LogStream.Dispose();	
			_LogStream = null;
		}

		public class SingleInstanceApplication : WindowsFormsApplicationBase
		{
			internal SingleInstanceApplication()
			{
				IsSingleInstance = true;
			}

			protected override void OnCreateMainForm()
			{
				MainForm = new ProgramForm();
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
						((ProgramForm)MainForm).LoadFile( args[i] );
					}
				}
			}
		}

#region Options
		private static readonly string SettingsPath = Path.Combine( 
			Application.StartupPath, 
			"Config", 
			"UEExplorerConfig.xml" 
		);
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
			UELib.UnrealConfig.PreBeginBracket = ParseFormatOption( Program.Options.PreBeginBracket );
			UELib.UnrealConfig.PreEndBracket = ParseFormatOption( Program.Options.PreEndBracket );
			UpdateIndention();
		}

		internal static void UpdateIndention()
		{
			if( Program.Options.Indention == 4 )
			{
				UELib.UnrealConfig.Indention = "\t";
			}
			else
			{
				UELib.UnrealConfig.Indention = String.Empty;
				for( var i = 0; i < Program.Options.Indention; ++ i )
				{
					UELib.UnrealConfig.Indention += " ";
				}		
			}
		}

		internal static string ParseFormatOption( string input )
		{
			return input.Replace( "%NEWLINE%", "\r\n" ).Replace( "%TABS%", "{0}" );
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
#endregion

		internal const string WEBSITE_URL = 
#if DEBUG
			"http://localhost/eliot/";
#else
			"http://eliotvu.com/";
#endif

		internal const string Donate_URL = WEBSITE_URL + "donate.html";
		internal const string Program_URL = WEBSITE_URL + "portfolio/view/21/ue-explorer";
		internal const string Program_Parm_ID = "data[items][id]=21";
		internal const string Version_URL = WEBSITE_URL +  "apps/version/";
		internal const string Forum_URL = WEBSITE_URL + "forum/";

		internal static string Post( string url, string data )
		{
			var webReq = (HttpWebRequest)WebRequest.Create( url );
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			var buffer = Encoding.UTF8.GetBytes( data );
			webReq.ContentLength = buffer.Length;

			using( var postStream = webReq.GetRequestStream())
			{
				postStream.Write( buffer, 0, buffer.Length );
			}

			var response = webReq.GetResponse();
			string result;
			using( var responseReader = new StreamReader( response.GetResponseStream() ) )
			{
				
			    result = responseReader.ReadToEnd();
			}
			return result;
		}

#region Registry	
		private const string RegistryFileFolderName = "UEExplorer.AnyUnrealFile";

		public static bool AreFileTypesRegistered()
		{
			return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( RegistryFileFolderName ) != null;
		}

		public static void ToggleRegisterFileTypes( bool undo = false )
		{		
			var extkeys = new List<Microsoft.Win32.RegistryKey>();
			var extensions = UnrealExtensions.FormatUnrealExtensionsAsList();
			if( undo )
			{
				Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree( RegistryFileFolderName );
				foreach( string ext in extensions )
				{
					Microsoft.Win32.RegistryKey extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext, true );
					if( extkey != null )
					{
						if( (string)extkey.GetValue( "" ) == RegistryFileFolderName )
						{
							Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree( ext );
						}
						else
						{
							extkeys.Add( extkey );
						}
					}	
				}

				foreach( Microsoft.Win32.RegistryKey key in extkeys )
				{
					var reference = (string)key.GetValue( "" );
					if( reference != null )
					{
						Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( reference, true );
						if( k != null )
						{
							ToggleFileProperties( k, true );
						}
					}
				}
			}
			else
			{
				foreach( string ext in extensions )
				{
					Microsoft.Win32.RegistryKey extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext, true );
					if( extkey == null )
					{
						extkey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey( ext );
						extkey.SetValue( "", RegistryFileFolderName, Microsoft.Win32.RegistryValueKind.String );
						extkey.SetValue( "Content Type", "application", Microsoft.Win32.RegistryValueKind.String );
					}
					else if( (string)(extkey.GetValue( "" )) != RegistryFileFolderName )
					{		  
						extkeys.Add( extkey );
					}
				}

				Microsoft.Win32.RegistryKey unrealfilekey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey( RegistryFileFolderName );
				if( unrealfilekey != null )
				{
					unrealfilekey.SetValue( "", "Unreal File" );
					ToggleFileProperties( unrealfilekey );
				}

				foreach( Microsoft.Win32.RegistryKey key in extkeys )
				{
					string reference = (string)key.GetValue( "" );
					if( reference != null )
					{
						Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( reference, true );
						if( k != null )
						{
							ToggleFileProperties( k );
						}
					}
				}
			}
		}

		private static void ToggleFileProperties( Microsoft.Win32.RegistryKey key, bool undo = false )
		{
			if( undo )
			{
				var xkey = key.OpenSubKey( "DefaultIcon", true );
				// Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
				if( xkey != null )
				{
					string curkey = (string)xkey.GetValue( "" );
					if( curkey != null )
					{
						string oldasc = (string)xkey.GetValue( "OldAssociation" );
						xkey.SetValue( "OldAssociation", curkey, Microsoft.Win32.RegistryValueKind.String );
						xkey.SetValue( "", oldasc ?? "", Microsoft.Win32.RegistryValueKind.String );
					}
				}

				var shellkey = key.OpenSubKey( "shell", true );
				if( shellkey != null )
				{
					shellkey.DeleteSubKeyTree( "open in " + Application.ProductName );
				}
			}
			else
			{
				// Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
				if( key.OpenSubKey( "DefaultIcon" ) == null )
				{
					using( var defaulticonkey = key.CreateSubKey( "DefaultIcon" ) )
					{
						if( defaulticonkey != null )
						{
							string mykey = Path.Combine( Application.StartupPath, "unrealfile.ico" );
							string oldassociation = (string)defaulticonkey.GetValue( "" );
							if( oldassociation != mykey )
							{
								if( oldassociation != null )
								{
									defaulticonkey.SetValue( "OldAssociation", oldassociation, Microsoft.Win32.RegistryValueKind.String );
								}	
								defaulticonkey.SetValue( "", mykey, Microsoft.Win32.RegistryValueKind.String );
							}
						}
					}
				}

				var shellkey = key.CreateSubKey( "shell" );
				var editkey = shellkey.CreateSubKey( "open in " + Application.ProductName );
				editkey.SetValue( "", "&Open in " + Application.ProductName );
				var cmdkey = editkey.CreateSubKey( "command" );
				cmdkey.SetValue( "", "\"" + Application.ExecutablePath + "\" \"%1\"", Microsoft.Win32.RegistryValueKind.ExpandString );
			}
		}
#endregion
    }

	[System.Reflection.ObfuscationAttribute(Exclude = true)]
	public class XMLSettings
	{
		#region Unreal Packages Decompiler Related Members
		public string NTLPath = "NativesTableList_UT2004";

		public UELib.UnrealPackage.InitFlags InitFlags = UELib.UnrealPackage.InitFlags.All;
		public bool bForceVersion;
		public bool bForceLicenseeMode;
		public ushort Version;
		public ushort LicenseeMode;
		public string Platform = "PC";
		#endregion

		#region Unreal Cache Extractor Related Membbers
		public string InitialCachePath = String.Empty;
		#endregion

		#region DECOMPILER
		public bool bSuppressComments;
		public string PreBeginBracket = "%NEWLINE%%TABS%";
		public string PreEndBracket = "%NEWLINE%%TABS%";
		public int Indention = 4;
		#endregion

		#region THIRDPARY
		public string UEModelAppPath = String.Empty;
		#endregion
	}
}
