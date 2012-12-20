using System;
using Microsoft.TeamFoundation.Client;

namespace BuildMonitor.Engine
{
    public class TfsServiceProvider : IServiceProvider
    {
        private readonly Uri projectCollectionUri;
        private readonly ILog log;

        public TfsServiceProvider(string projectCollectionUri, ILog log)
        {
            this.projectCollectionUri = new Uri(projectCollectionUri);
            this.log = log;
        }

        public object GetService(Type serviceType)
        {
            var collection = new TfsTeamProjectCollection(projectCollectionUri);
            object service = null;

            try
            {
                collection.EnsureAuthenticated();
                service = collection.GetService(serviceType);
            }
            catch (Exception ex)
            {
                log.Error("Error communication with TFS server: " + projectCollectionUri.ToString());
                log.Error(ex.ToString());
            }

            log.Information("Connection to TFS established.");
            return service;
        }
    }
}