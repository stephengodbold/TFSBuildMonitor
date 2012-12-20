using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using BuildMonitor.Engine;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildMonitor.Service
{
    public partial class TFSBuildMonitor : ServiceBase
    {
        BuildWatcher watcher;
        ILog logger;

        public TFSBuildMonitor()
        {
            InitializeComponent();

            this.logger = new EventLogger(this.EventLog, "TFS Build Monitor");
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
                base.ExitCode = 1;
                logger.Error(ex.Message);
            }
        }

        private void watcher_BuildCompletionEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string StatusMessage = "Build {0}, Status {1}";
            var data = buildWatcherEventArgs.Data;
            logger.Information(string.Format(StatusMessage, data.BuildNumber, data.Status.ToString()));            
        }

        private void watcher_BuildQualityChangeEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            const string QualityChangeMessage = "Build {0}, Quality {1}";
            var data = buildWatcherEventArgs.Data;
            logger.Information(string.Format(QualityChangeMessage, data.BuildNumber, data.Quality));
        }

        protected override void OnStop()
        {
            if (watcher != null)
            {
                watcher.BuildCompletionEvent -= watcher_BuildCompletionEvent;
                watcher.BuildQualityChangeEvent -= watcher_BuildQualityChangeEvent;
                watcher.Stop();
            }
        }
    }
}
