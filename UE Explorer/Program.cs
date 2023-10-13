using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using AutoUpdaterDotNET;
using Eliot.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic.ApplicationServices;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.Services;
using UEExplorer.Framework.Tasks;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Properties;
using UEExplorer.Tools;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Main;
using UEExplorer.UI.Services;
using UELib;
using UELib.Types;
using UnhandledExceptionEventArgs = Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs;

namespace UEExplorer
{
    public static class Program
    {
        private const string WebsiteUrl =
#if DEBUG_WITH_LOCALHOST
            "https://localhost/Eliot/";
#else
            "https://eliotvu.com/";
#endif

        internal const string DonateUrl = WebsiteUrl + "donate.html";

        internal const string ForumUrl = WebsiteUrl + "forum/";
        internal const string StartUrl = WebsiteUrl + "apps/ue_explorer/";
        private const string UpdateUrl = WebsiteUrl + "updates/ue-explorer.xml" + UpdateQuery;
        private const string UpdateQuery = "?auto=1&installed_version={0}";
        internal const string SubmitReportUrl = WebsiteUrl + "report/send/";

        private static IEnumerable<Type> InternalPluginModules =>
            new List<Type> { typeof(InternalToolsPlugin) };

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                LogManager.StartLogStream();

                foreach (string arg in args)
                {
                    Console.WriteLine($"Argument: {arg}");
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var builder = Host.CreateDefaultBuilder();

                // TODO: Deprecate -console
                if (args.Length >= 2 && args.Contains("-console"))
                {
                    var console = new ProgramConsole();
                    Application.Run(console);
                    Application.Exit();
                }
                else
                {
                    builder.ConfigureServices(services =>
                    {
                        // Framework services
                        services
                            .AddSingleton<CommandService>()
                            .AddSingleton<ContextCommandBuilder>()
                            .AddSingleton<ContextService>()
                            .AddSingleton<PackageManager>()
                            .AddSingleton<PluginService>()
                            .AddSingleton<TasksManager>()
                            .AddSingleton<IDockingService, DockingService>();

                        // Plugin services
                        var pluginModuleTypes = InternalPluginModules
                            .Concat(PluginManager.LoadModules(Path.Combine(Application.StartupPath, "plugins")));
                        foreach (var pluginModuleType in pluginModuleTypes)
                        {
                            var serviceAttributes = pluginModuleType
                                .GetCustomAttributes<PluginProvideServiceAttribute>();
                            foreach (var serviceAttribute in serviceAttributes)
                            {
                                Contract.Assert(
                                    serviceAttribute.ServiceType.IsAssignableFrom(serviceAttribute.ImplementationType));
                                services.AddSingleton(serviceAttribute.ServiceType,
                                    serviceAttribute.ImplementationType);
                            }

                            services.AddSingleton(typeof(IPluginModule), pluginModuleType);
                        }

                        // App services
                        services.AddTransient<ProgramForm>();
                    });

                    var serviceProvider = ServiceHost.Build(builder);

                    if (args.Contains("-newwindow"))
                    {
                        var window = serviceProvider.GetRequiredService<ProgramForm>();
                        Application.Run(window);
                    }
                    else
                    {
                        var app = new SingleInstanceApplication(serviceProvider);
                        app.Run(Environment.GetCommandLineArgs());
                    }
                }
            }
            catch (Exception exc)
            {
                ExceptionDialog.Show("Internal crash!", exc);
                Console.Error.WriteLine(exc.ToString());
            }
            finally
            {
                LogManager.EndLogStream();
            }
        }

        public static void PushRecentOpenedFile(string filePath)
        {
            var filePaths = UserHistory.Default.RecentFiles;
            // If we have one already, we'd like to move it to the most recent.
            filePaths.Remove(filePath);
            filePaths.Add(filePath);
            if (filePaths.Count > 15)
            {
                filePaths.RemoveAt(0);
            }

            UserHistory.Default.Save();
        }

        internal static void CheckForUpdates()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine(Resources.CHECKING_FOR_UPDATES_LOG, version);
            AutoUpdater.Start(string.Format(UpdateUrl, version));
        }

        private class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private readonly IServiceProvider _ServiceProvider;

            public SingleInstanceApplication() => IsSingleInstance = true;

            public SingleInstanceApplication(IServiceProvider serviceProvider)
            {
                _ServiceProvider = serviceProvider;
            }

            protected override void OnCreateMainForm() =>
                MainForm = _ServiceProvider.GetRequiredService<ProgramForm>();

            protected override bool OnStartup(StartupEventArgs eventArgs) => true;

            protected override void OnRun() => base.OnRun();

            protected override void OnShutdown() => base.OnShutdown();

            protected override bool OnUnhandledException(UnhandledExceptionEventArgs e)
            {
                ExceptionDialog.Show("Internal crash!", e.Exception);
                return base.OnUnhandledException(e);
            }

            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                eventArgs.BringToForeground = true;
                var args = eventArgs.CommandLine;
                for (int i = 1; i < args.Count; ++i)
                {
                    if (File.Exists(args[i]))
                    {
                        ((ProgramForm)MainForm).LoadFromFile(args[i]);
                    }
                }
            }
        }

        public static class LogManager
        {
            private const string LogFileName = "Log{0}.txt";

            private static readonly string s_logFilePath =
                Path.Combine(s_appDataDir, LogFileName);

            private static FileStream s_logStream;

            public static void StartLogStream()
            {
                int failCount = 0;
            retry:
                string logPath = string.Format(s_logFilePath,
                    failCount > 0 ? failCount.ToString(CultureInfo.InvariantCulture) : string.Empty);
                try
                {
                    s_logStream = new FileStream(logPath, FileMode.Create, FileAccess.Write);
                }
                catch (IOException)
                {
                    ++failCount;
                    goto retry;
                }

                Debug.Assert(s_logStream != null, "Couldn't open file" + Path.GetFileName(logPath));
                Console.SetOut(new StreamWriter(s_logStream));
            }

            public static void EndLogStream()
            {
                if (s_logStream == null)
                {
                    return;
                }

                s_logStream.Flush();
                s_logStream.Dispose();
                s_logStream = null;
            }
        }

        #region Options

        private static readonly string s_appDataDir = Application.UserAppDataPath;
        private static readonly string s_settingsPath = Path.Combine(s_appDataDir, "UEExplorerConfig.xml");
        public static readonly string DockingConfigPath = Path.Combine(s_appDataDir, "Docking.xml");

        public static XMLSettings Options;

        public static void LoadConfig()
        {
            if (File.Exists(s_settingsPath))
            {
                using (var r = new XmlTextReader(s_settingsPath))
                {
                    try
                    {
                        var xser = new XmlSerializer(typeof(XMLSettings));
                        Options = (XMLSettings)xser.Deserialize(r);
                    }
                    catch (Exception exc)
                    {
                        Console.Error.WriteLine("Failed to deserialize the configuration file {0}", exc.Message);
                        Options = new XMLSettings();
                    }
                }
            }
            else
            {
                SaveConfig();
            }

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

        internal static string ParseIndention(int indentionCount) => string.Empty.PadLeft(indentionCount, ' ');

        internal static string ParseFormatOption(string input) =>
            input.Replace("%NEWLINE%", "\r\n").Replace("%TABS%", "{0}");

        public static void SaveConfig()
        {
            if (Options == null)
            {
                Options = new XMLSettings();
            }

            using (var w = new XmlTextWriter(s_settingsPath, Encoding.ASCII))
            {
                var xser = new XmlSerializer(typeof(XMLSettings));
                xser.Serialize(w, Options);
            }
        }

        #endregion
    }
}
