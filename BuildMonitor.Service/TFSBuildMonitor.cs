using System;
using System.ServiceProcess;
using BuildMonitor.Engine;

namespace BuildMonitor.Service
{
    public partial class TFSBuildMonitor : ServiceBase
    {
        BuildWatcher watcher;
        readonly ILog logger;

        public TFSBuildMonitor()
        {
            InitializeComponent();

            logger = new EventLogger(EventLog, "TFS Build Monitor");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                
                watcher = new BuildWatcher(logger);
                PublisherFactory.InstantiatePublishers(logger, watcher);

                watcher.BuildCompletionEvent += watcher_BuildCompletionEvent;
                watcher.BuildQualityChangeEvent += watcher_BuildQualityChangeEvent;              
                watcher.Start();
                logger.Information("Build Monitor started");
            }
            catch (Exception ex)
            {
                base.EventLog.WriteEntry(ex.Message);
                ExitCode = 1;
                logger.Error(ex.Message);
            }
        }

        private void watcher_BuildCompletionEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string statusMessage = "Build {0}, Status {1}";
            var data = buildWatcherEventArgs.Data;
            logger.Information(string.Format(statusMessage, data.BuildNumber, data.Status));            
        }

        private void watcher_BuildQualityChangeEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string qualityChangeMessage = "Build {0}, Quality {1}";
            var data = buildWatcherEventArgs.Data;
            logger.Information(string.Format(qualityChangeMessage, data.BuildNumber, data.Quality));
        }

        protected override void OnStop()
        {
            if (watcher == null) return;
            watcher.BuildCompletionEvent -= watcher_BuildCompletionEvent;
            watcher.BuildQualityChangeEvent -= watcher_BuildQualityChangeEvent;
            watcher.Stop();
        }
    }
}
