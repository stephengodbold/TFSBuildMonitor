using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildMonitor.Engine
{
    public enum BuildStoreEventType
    {
        Build,
        QualityChanged
    }

    public class BuildStoreEventArgs : EventArgs
    {
        public BuildStoreEventType Type { get; set; }
        public IBuildDetail Data { get; set; }
    }

    public delegate void BuildWatcherEventHandler(object sender, BuildStoreEventArgs buildWatcherEventArgs);

    public delegate void BuildWatcherInitializingHandler(
        object sender, IEnumerable<BuildStoreEventArgs> buildWatcherEventArgs);

    public delegate void BuildWatcherStoppingHandler(object sender, EventArgs eventArgs);
}