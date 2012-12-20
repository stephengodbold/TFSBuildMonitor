using System;
using System.Linq;
using System.ServiceProcess;
using System.ComponentModel;
using BuildMonitor.Engine;

namespace BuildMonitor.Service
{
    static class Program
    {
        static readonly ConsoleText defaultLogger = new ConsoleText();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Contains("/i"))
            {
                try
                {
                    defaultLogger.PropertyChanged += LogToConsole;
                    var watcher = new BuildWatcher(defaultLogger);
                    PublisherFactory.InstantiatePublishers(defaultLogger, watcher);

                    watcher.BuildCompletionEvent += watcher_BuildCompletionEvent;
                    watcher.BuildQualityChangeEvent += watcher_BuildQualityChangeEvent;
                    watcher.Start();
                    defaultLogger.Information("Build Monitor started");
                }
                catch (Exception ex)
                {
                    defaultLogger.Error(ex.Message);
                }

                Console.ReadKey();
                return;
            }

            ServiceBase.Run(new ServiceBase[] { new TFSBuildMonitor() });
        }

        private static void watcher_BuildCompletionEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string StatusMessage = "Build {0}, Status {1}";
            var data = buildWatcherEventArgs.Data;
            defaultLogger.Information(string.Format(StatusMessage, data.BuildNumber, data.Status.ToString()));
        }

        private static void watcher_BuildQualityChangeEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string QualityChangeMessage = "Build {0}, Quality {1}";
            var data = buildWatcherEventArgs.Data;
            defaultLogger.Information(string.Format(QualityChangeMessage, data.BuildNumber, data.Quality));
        }

        public static void LogToConsole(object sender, PropertyChangedEventArgs e)
        {
            var consoleLogger = sender as ConsoleText;

            if ((consoleLogger != null) && (e.PropertyName.Equals("LastMessage")))
            {
                Console.WriteLine(consoleLogger.LastMessage);
            }
        }
    }
}
