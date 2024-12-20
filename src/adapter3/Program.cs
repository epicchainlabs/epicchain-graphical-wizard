﻿using System;
using System.IO;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol;
using McMaster.Extensions.CommandLineUtils;


namespace EpicChainTraceVisualizer.EpicChain
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
        private string DefaultDebugView { get; } = string.Empty;

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

            var defaultDebugView = DefaultDebugView.Length > 0
                ? Enum.Parse<DebugView>(DefaultDebugView, true)
                : DebugView.Source;

            if (defaultDebugView == DebugView.Toggle)
                throw new ArgumentException($"invalid DefaultDebugView {defaultDebugView}", nameof(DefaultDebugView));

            var adapter = new DebugAdapter(
                Console.OpenStandardInput(),
                Console.OpenStandardOutput(),
                LogMessage,
                defaultDebugView);

            adapter.Run();
        }

        void LogMessage(LogCategory category, string message)
        {
            if (Log)
            {
                File.AppendAllText(logFile, $"\n{category} {message}");
            }
        }
    }
}
