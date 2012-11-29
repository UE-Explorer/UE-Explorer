/***********************************************************************
 * Copyright 2009-2012 Eliot Van Uytfanghe. All rights reserved. 
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.ApplicationServices;
using UEExplorer.UI.Dialogs;
using UELib;

namespace UEExplorer
{
	using UI;

    public static class Program
    {
        [STAThread]
        static void Main( string[] args )
        {
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault( false );

				if( args.Length >= 2 )
				{
					var console = new UI.Main.ProgramConsole();
					Application.Run( console );
					Application.Exit();
					return;
				}

				//Thread.CurrentThread.CurrentCulture = CultureInfo.InstalledUICulture;

				var app = new SingleInstanceApplication();
				app.Run( Environment.GetCommandLineArgs() );
			}
			catch( Exception exception )
			{
				ExceptionDialog.Show( "Internal crash!", exception );
			}
		}

		public class SingleInstanceApplication : WindowsFormsApplicationBase
		{
			public SingleInstanceApplication()
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

		public static class LogManager
		{
			private const string				LogFileName = "Log.txt";
			private static readonly string		LogFilePath = Path.Combine( Application.StartupPath, LogFileName );
			private static FileStream			_LogStream;	

			public static void StartLogStream()
			{
				_LogStream = new FileStream( LogFilePath, FileMode.Create, FileAccess.Write  );
				Console.SetOut( new StreamWriter( _LogStream ) );		
			}

			public static void EndLogStream()
			{
				if( _LogStream == null ) 
					return;

				_LogStream.Flush();
				_LogStream.Close();
				_LogStream.Dispose();	
				_LogStream = null;
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

			UnrealConfig.SuppressComments = Options.bSuppressComments;
			UnrealConfig.PreBeginBracket = ParseFormatOption( Options.PreBeginBracket );
			UnrealConfig.PreEndBracket = ParseFormatOption( Options.PreEndBracket );
			//if( Options.VariableTypes == null || Options.VariableTypes.Count == 0 )
			//{
			//    Options.VariableTypes = Options.DefaultVariableTypes;
			//}
			//UnrealConfig.VariableTypes = Options.VariableTypes;
			UnrealConfig.Indention = ParseIndention( Options.Indention );
		}

		internal static string ParseIndention( int indentionCount )
		{
			//if( indentionCount == 4 )
			//{
			//    return "\t";
			//}

			//string indention = String.Empty;
			//for( var i = 0; i < indentionCount; ++ i )
			//{
			//    indention += " ";
			//}	
			//return indention;

			return String.Empty.PadLeft( indentionCount, ' ' );
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
			"http://localhost/Eliot/";
#else
			"http://eliotvu.com/";
#endif

		internal const string Donate_URL = WEBSITE_URL + "donate.html";
		internal const string Contact_URL = WEBSITE_URL + "contact.html";
		internal const string Program_URL = WEBSITE_URL + "portfolio/view/21/ue-explorer";
		internal const string Program_Parm_ID = "data[items][id]=21";
		internal const string Version_URL = WEBSITE_URL +  "apps/version/";
		internal const string Forum_URL = WEBSITE_URL + "forum/";
		internal const string APPS_URL = WEBSITE_URL + "apps/ue_explorer/";

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
						if( (string)extkey.GetValue( String.Empty ) == RegistryFileFolderName )
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
					var reference = (string)key.GetValue( String.Empty );
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
						extkey.SetValue( String.Empty, RegistryFileFolderName, Microsoft.Win32.RegistryValueKind.String );
						extkey.SetValue( "Content Type", "application", Microsoft.Win32.RegistryValueKind.String );
					}
					else if( (string)(extkey.GetValue( String.Empty )) != RegistryFileFolderName )
					{		  
						extkeys.Add( extkey );
					}
				}

				Microsoft.Win32.RegistryKey unrealfilekey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey( RegistryFileFolderName );
				if( unrealfilekey != null )
				{
					unrealfilekey.SetValue( String.Empty, "Unreal File" );
					ToggleFileProperties( unrealfilekey );
				}

				foreach( Microsoft.Win32.RegistryKey key in extkeys )
				{
					string reference = (string)key.GetValue( String.Empty );
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
					string curkey = (string)xkey.GetValue( String.Empty );
					if( curkey != null )
					{
						string oldasc = (string)xkey.GetValue( "OldAssociation" );
						xkey.SetValue( "OldAssociation", curkey, Microsoft.Win32.RegistryValueKind.String );
						xkey.SetValue( String.Empty, oldasc ?? String.Empty, Microsoft.Win32.RegistryValueKind.String );
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
							string oldassociation = (string)defaulticonkey.GetValue( String.Empty );
							if( oldassociation != mykey )
							{
								if( oldassociation != null )
								{
									defaulticonkey.SetValue( "OldAssociation", oldassociation, Microsoft.Win32.RegistryValueKind.String );
								}	
								defaulticonkey.SetValue( String.Empty, mykey, Microsoft.Win32.RegistryValueKind.String );
							}
						}
					}
				}

				var shellkey = key.CreateSubKey( "shell" );
				var editkey = shellkey.CreateSubKey( "open in " + Application.ProductName );
				editkey.SetValue( String.Empty, "&Open in " + Application.ProductName );
				var cmdkey = editkey.CreateSubKey( "command" );
				cmdkey.SetValue( String.Empty, "\"" + Application.ExecutablePath + "\" \"%1\"", Microsoft.Win32.RegistryValueKind.ExpandString );
			}
		}
#endregion
    }

	[System.Reflection.ObfuscationAttribute(Exclude = true)]
	public class XMLSettings
	{																									  
		#region Unreal Packages Decompiler Related Members
		public string NTLPath = "NativesTableList_UT2004";

		public UnrealPackage.InitFlags InitFlags = UnrealPackage.InitFlags.All;
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

		//public List<UnrealConfig.VariableType> VariableTypes;
		//[XmlIgnore]
		//public readonly List<UnrealConfig.VariableType> DefaultVariableTypes = new List<UnrealConfig.VariableType>
		//{
		//    new UnrealConfig.VariableType{VFullName = "Engine.Actor.Skins", VType = "ObjectProperty"},	
		//    new UnrealConfig.VariableType{VFullName = "Engine.Actor.Components", VType = "ObjectProperty"},
		//    new UnrealConfig.VariableType{VFullName = "Engine.SkeletalMeshComponent.AnimSets", VType = "ObjectProperty"},
		//    new UnrealConfig.VariableType{VFullName = "Engine.SequenceOp.InputLinks", VType = "StructProperty"},
		//    new UnrealConfig.VariableType{VFullName = "Engine.SequenceOp.OutputLinks", VType = "StructProperty"},
		//    new UnrealConfig.VariableType{VFullName = "Engine.SequenceOp.VariableLinks", VType = "StructProperty"},
		//    new UnrealConfig.VariableType{VFullName = "Engine.SequenceAction.Targets", VType = "ObjectProperty"},
		//    new UnrealConfig.VariableType{VFullName = "XInterface.GUIComponent.Controls", VType = "ObjectProperty"},
		//};
		#endregion

		#region THIRDPARY
		public string UEModelAppPath = String.Empty;
		#endregion
	}
}
