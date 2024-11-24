using McMaster.Extensions.CommandLineUtils;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol;
using System;
using System.IO;

namespace EpicChainTraceVisualizer
{

    class Program
    {
        private static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private readonly string logFile;

        [Option]
        private bool Debug { get; }

        [Option]
        private bool Log { get; }

        [Option("-v|--debug-view")]
        private string DebugView { get; } = string.Empty;

        public Program()
        {
            var EpicChainTraceVisualizerLogPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "epicchain-graphical-wizard",
                "logs");

            if (!Directory.Exists(EpicChainTraceVisualizerLogPath))
            {
                Directory.CreateDirectory(EpicChainTraceVisualizerLogPath);
            }

            logFile = Path.Combine(EpicChainTraceVisualizerLogPath, $"{DateTime.Now:yyMMdd-hhmmss}.log");
        }

        private void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (Debug)
            {
                while (!System.Diagnostics.Debugger.IsAttached)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            var defaultDebugView = DebugView.Length > 0
                ? Enum.Parse<DebugSession.DebugView>(DebugView, true)
                : DebugSession.DebugView.Source;

            if (defaultDebugView == DebugSession.DebugView.Toggle)
                throw new ArgumentException(nameof(DebugView));

            var adapter = new DebugAdapter(
                Console.OpenStandardInput(),
                Console.OpenStandardOutput(),
                (cat, msg) => LogMessage(msg, cat),
                defaultDebugView);

            adapter.Run();
        }


        public void LogMessage(string message, LogCategory category = LogCategory.Trace)
        {
            if (Log)
            {
                File.AppendAllText(logFile, $"\n{category} {message}");
            }
        }
    }
}
