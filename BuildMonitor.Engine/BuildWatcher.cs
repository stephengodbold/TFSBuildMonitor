// Copyright (c) 2007 Readify Pty. Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Threading;
using BuildMonitor.Engine.Properties;

namespace BuildMonitor.Engine
{
    public class BuildWatcher
    {
        private readonly BuildStoreEventSource eventSource;
        private readonly ILog logger;
        private Timer pollTimer;

        public BuildWatcher(ILog logger)
        {
            var teamFoundationServiceProvider = new TfsServiceProvider(Settings.Default.TeamFoundationCollectionUrl,
                                                                       logger);
            eventSource = new BuildStoreEventSource(teamFoundationServiceProvider,
                                                    Settings.Default.BuildDefinitionNameExclusionPattern);
            this.logger = logger;

            Initalize();
        }

        private void Initalize()
        {
            var events = eventSource.GetListOfBuildStoreEvents();
            if (BuildWatcherInitializing != null)
                BuildWatcherInitializing(this, events);
        }

        private void Worker(object state)
        {
            try
            {
                var buildStoreEvents = eventSource.GetListOfBuildStoreEvents();
                foreach (var buildEvent in buildStoreEvents)
                {
                    ProcessBuildEvent(buildEvent);
                }
            }
            catch (ThreadAbortException)
            {
                logger.Warning("Thread abort in watcher");
            }
        }

        private void ProcessBuildEvent(BuildStoreEventArgs buildEvent)
        {
            switch (buildEvent.Type)
            {
                case BuildStoreEventType.Build:
                    if (BuildCompletionEvent != null)
                        BuildCompletionEvent(this, buildEvent);
                    break;
                case BuildStoreEventType.QualityChanged:
                    if (BuildQualityChangeEvent != null)
                        BuildQualityChangeEvent(this, buildEvent);
                    break;
                default:
                    throw new Exception("Event was not recognised.");
            }
        }

        public void Start()
        {
            pollTimer = new Timer(Worker, null, 0, Settings.Default.PollPeriodInSeconds * 1000);
        }

        public void Stop()
        {
            pollTimer = null;
        }

        public event BuildWatcherInitializingHandler BuildWatcherInitializing;
        public event BuildWatcherEventHandler BuildCompletionEvent;
        public event BuildWatcherEventHandler BuildQualityChangeEvent;
        public event BuildWatcherStoppingHandler BuildWatcherStopping;
    }
}