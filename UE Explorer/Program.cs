using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.ApplicationServices;
using UEExplorer.UI.Dialogs;
using UELib;
using UELib.Types;
using Eliot.Utilities;

namespace UEExplorer
{
    using UI;

    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args) Console.WriteLine("Argument: " + arg);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args.Length >= 2 && ((IList)args).Contains("-console"))
                {
                    var console = new UI.Main.ProgramConsole();
                    Application.Run(console);
                    Application.Exit();
                }
                else if (((IList)args).Contains("-newwindow"))
                {
                    var window = new ProgramForm();
                    Application.Run(window);
                }
                else
                {
                    //Thread.CurrentThread.CurrentCulture = CultureInfo.InstalledUICulture;
                    var app = new SingleInstanceApplication();
                    app.Run(Environment.GetCommandLineArgs());
                }
            }
            catch (Exception exception)
            {
                ExceptionDialog.Show("Internal crash!", exception);
            }
            finally
            {
                LogManager.EndLogStream();
            }
        }

        public static IEnumerable<string> ParseArguments(IEnumerable<string> args)
        {
            IList<string> options = new List<string>();
            foreach (string arg in args)
            {
                if (arg.StartsWith("-")) options.Add(arg.Substring(1));
            }

            return options;
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

            protected override bool OnStartup(StartupEventArgs eventArgs)
            {
                return true;
            }

            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                eventArgs.BringToForeground = true;
                var args = eventArgs.CommandLine;
                for (var i = 1; i < args.Count; ++i)
                {
                    if (File.Exists(args[i])) ((ProgramForm)MainForm).LoadFile(args[i]);
                }
            }
        }

        public static class LogManager
        {
            private const string LogFileName = "Log{0}.txt";
            private static readonly string LogFilePath = Path.Combine(Application.StartupPath, LogFileName);
            private static FileStream _LogStream;

            public static void StartLogStream()
            {
                var failCount = 0;
            retry:
                string logPath = string.Format(LogFilePath,
                    failCount > 0 ? failCount.ToString(CultureInfo.InvariantCulture) : string.Empty);
                try
                {
                    _LogStream = new FileStream(logPath, FileMode.Create, FileAccess.Write);
                }
                catch (IOException)
                {
                    ++failCount;
                    goto retry;
                }

                Debug.Assert(_LogStream != null, "Couldn't open file" + Path.GetFileName(logPath));
                Console.SetOut(new StreamWriter(_LogStream));
            }

            public static void EndLogStream()
            {
                if (_LogStream == null)
                    return;

                _LogStream.Flush();
                _LogStream.Dispose();
                _LogStream = null;
            }
        }

        #region Options

        public static readonly string ConfigDir = Path.Combine(Application.StartupPath, "Config");

        private static readonly string _SettingsPath = Path.Combine(
            ConfigDir,
            "UEExplorerConfig.xml"
        );
        public static readonly string DockingConfigPath = Path.Combine(Application.StartupPath, "Docking.xml");

        public static XMLSettings Options;

        public static void LoadConfig()
        {
            if (File.Exists(_SettingsPath))
            {
                using (var r = new XmlTextReader(_SettingsPath))
                {
                    var xser = new XmlSerializer(typeof(XMLSettings));
                    Options = (XMLSettings)xser.Deserialize(r);
                }
            }
            else
                SaveConfig();

            UnrealConfig.SuppressComments = Options.bSuppressComments;
            UnrealConfig.PreBeginBracket = ParseFormatOption(Options.PreBeginBracket);
            UnrealConfig.PreEndBracket = ParseFormatOption(Options.PreEndBracket);
            if (Options.VariableTypes == null || Options.VariableTypes.Count == 0)
            {
                Options.VariableTypes = new List<string>
                {
                    "Engine.Actor.Skins:ObjectProperty",
                    "Engine.Actor.Components:ObjectProperty",
                    "Engine.SkeletalMeshComponent.AnimSets:ObjectProperty",
                    "Engine.SequenceOp.InputLinks:StructProperty",
                    "Engine.SequenceOp.OutputLinks:StructProperty",
                    "Engine.SequenceOp.VariableLinks:StructProperty",
                    "Engine.SequenceAction.Targets:ObjectProperty",
                    "XInterface.GUIComponent.Controls:ObjectProperty",
                    "Engine.Material.Expressions:ObjectProperty",
                    "Engine.ParticleSystem.Emitters:ObjectProperty"
                };
            }

            CopyVariableTypes();
            UnrealConfig.Indention = ParseIndention(Options.Indention);
        }

        internal static Tuple<string, string, PropertyType> ParseVariable(string data)
        {
        retry:
            int groupIndex = data.IndexOf(':');
            if (groupIndex == -1)
            {
                data += ":ObjectProperty";
                goto retry;
            }

            string varGroup = data.Left(groupIndex);
            string varName = varGroup.Mid(varGroup.LastIndexOf('.') + 1);
            PropertyType varType;
            try
            {
                varType = (PropertyType)Enum.Parse(typeof(PropertyType), data.Substring(groupIndex + 1));
            }
            catch (Exception)
            {
                varType = PropertyType.ObjectProperty;
            }

            return Tuple.Create(varName, varGroup, varType);
        }

        internal static void CopyVariableTypes()
        {
            UnrealConfig.VariableTypes = new Dictionary<string, Tuple<string, PropertyType>>();
            foreach (string varType in Options.VariableTypes)
            {
                var varData = ParseVariable(varType);
                UnrealConfig.VariableTypes.Add(varData.Item1, Tuple.Create(varData.Item2, varData.Item3));
            }
        }

        internal static string ParseIndention(int indentionCount)
        {
            return string.Empty.PadLeft(indentionCount, ' ');
        }

        internal static string ParseFormatOption(string input)
        {
            return input.Replace("%NEWLINE%", "\r\n").Replace("%TABS%", "{0}");
        }

        public static void SaveConfig()
        {
            if (Options == null)
                Options = new XMLSettings();

            using (var w = new XmlTextWriter(_SettingsPath, Encoding.ASCII))
            {
                var xser = new XmlSerializer(typeof(XMLSettings));
                xser.Serialize(w, Options);
            }
        }

        #endregion

        internal const string WEBSITE_URL =
#if DEBUG_WITH_LOCALHOST
            "https://localhost/Eliot/";
#else
            "https://eliotvu.com/";
#endif

        internal const string Donate_URL = WEBSITE_URL + "donate.html";

        internal const string Contact_URL = WEBSITE_URL + "contact.html";

        //internal const string Program_URL = WEBSITE_URL + "portfolio/view/21/ue-explorer";
        //internal const string Program_Parm_ID = "data[items][id]=21";
        //internal const string Version_URL = WEBSITE_URL +  "apps/version/";
        internal const string Forum_URL = WEBSITE_URL + "forum/";
        internal const string APPS_URL = WEBSITE_URL + "apps/ue_explorer/";
        internal const string UPDATE_URL = WEBSITE_URL + "updates/ue-explorer.xml" + UPDATE_QUERY;
        internal const string UPDATE_QUERY = "?auto=1&installed_version={0}";

        // TODO: Deprecate

        #region Registry

        private const string RegistryFileFolderName = "UEExplorer.AnyUnrealFile";

        public static bool AreFileTypesRegistered()
        {
            return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RegistryFileFolderName) != null;
        }

        public static void ToggleRegisterFileTypes(bool undo = false)
        {
            var extkeys = new List<Microsoft.Win32.RegistryKey>();
            var extensions = UnrealExtensions.FormatUnrealExtensionsAsList();
            if (undo)
            {
                Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(RegistryFileFolderName);
                foreach (string ext in extensions)
                {
                    var extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext, true);
                    if (extkey != null)
                    {
                        if ((string)extkey.GetValue(string.Empty) == RegistryFileFolderName)
                            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(ext);
                        else
                            extkeys.Add(extkey);
                    }
                }

                foreach (var key in extkeys)
                {
                    var reference = (string)key.GetValue(string.Empty);
                    if (reference != null)
                    {
                        var k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(reference, true);
                        if (k != null) ToggleFileProperties(k, true);
                    }
                }
            }
            else
            {
                foreach (string ext in extensions)
                {
                    var extkey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext, true);
                    if (extkey == null)
                    {
                        extkey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(ext);
                        extkey.SetValue(string.Empty, RegistryFileFolderName, Microsoft.Win32.RegistryValueKind.String);
                        extkey.SetValue("Content Type", "application", Microsoft.Win32.RegistryValueKind.String);
                    }
                    else if ((string)extkey.GetValue(string.Empty) != RegistryFileFolderName) extkeys.Add(extkey);
                }

                var unrealfilekey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(RegistryFileFolderName);
                if (unrealfilekey != null)
                {
                    unrealfilekey.SetValue(string.Empty, "Unreal File");
                    ToggleFileProperties(unrealfilekey);
                }

                foreach (var key in extkeys)
                {
                    var reference = (string)key.GetValue(string.Empty);
                    if (reference != null)
                    {
                        var k = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(reference, true);
                        if (k != null) ToggleFileProperties(k);
                    }
                }
            }
        }

        private static void ToggleFileProperties(Microsoft.Win32.RegistryKey key, bool undo = false)
        {
            if (undo)
            {
                var xkey = key.OpenSubKey("DefaultIcon", true);
                // Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
                if (xkey != null)
                {
                    var curkey = (string)xkey.GetValue(string.Empty);
                    if (curkey != null)
                    {
                        var oldasc = (string)xkey.GetValue("OldAssociation");
                        xkey.SetValue("OldAssociation", curkey, Microsoft.Win32.RegistryValueKind.String);
                        xkey.SetValue(string.Empty, oldasc ?? string.Empty, Microsoft.Win32.RegistryValueKind.String);
                    }
                }

                var shellkey = key.OpenSubKey("shell", true);
                if (shellkey != null) shellkey.DeleteSubKeyTree("open in " + Application.ProductName);
            }
            else
            {
                // Should only add a icon reference if none was already set, so that .UT2 map files keep their original icon.
                if (key.OpenSubKey("DefaultIcon") == null)
                {
                    using (var defaulticonkey = key.CreateSubKey("DefaultIcon"))
                    {
                        if (defaulticonkey != null)
                        {
                            string mykey = Path.Combine(Application.StartupPath, "unrealfile.ico");
                            var oldassociation = (string)defaulticonkey.GetValue(string.Empty);
                            if (oldassociation != mykey)
                            {
                                if (oldassociation != null)
                                    defaulticonkey.SetValue("OldAssociation", oldassociation,
                                        Microsoft.Win32.RegistryValueKind.String);
                                defaulticonkey.SetValue(string.Empty, mykey, Microsoft.Win32.RegistryValueKind.String);
                            }
                        }
                    }
                }

                var shellkey = key.CreateSubKey("shell");
                var editkey = shellkey.CreateSubKey("open in " + Application.ProductName);
                editkey.SetValue(string.Empty, "&Open in " + Application.ProductName);
                var cmdkey = editkey.CreateSubKey("command");
                cmdkey.SetValue(string.Empty, "\"" + Application.ExecutablePath + "\" \"%1\"",
                    Microsoft.Win32.RegistryValueKind.ExpandString);
            }
        }

        #endregion
    }
}