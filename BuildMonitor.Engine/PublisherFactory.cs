namespace BuildMonitor.Engine
{
    public static class PublisherFactory
    {
        public static void InstantiatePublishers(ILog log, BuildWatcher buildWatcher)
        {
            new DelcomBuildPublisher(log, buildWatcher);
        }
    }
}