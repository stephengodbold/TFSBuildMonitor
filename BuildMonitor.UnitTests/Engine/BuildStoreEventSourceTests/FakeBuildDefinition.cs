using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    public class FakeBuildDefinition : IBuildDefinition
    {
        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public Uri Uri { get; set; }
        public string Name { get; set; }
        public string TeamProject { get; set; }
        public string FullPath { get; set; }
        public ISchedule AddSchedule()
        {
            throw new NotImplementedException();
        }

        public IBuildRequest CreateBuildRequest()
        {
            throw new NotImplementedException();
        }

        public IBuildDetail CreateManualBuild(string buildNumber)
        {
            throw new NotImplementedException();
        }

        public IBuildDetail CreateManualBuild(string buildNumber, string dropLocation)
        {
            throw new NotImplementedException();
        }

        public IBuildDetail CreateManualBuild(string buildNumber, string dropLocation, BuildStatus buildStatus, IBuildAgent agent, string requestedFor)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IBuildDefinitionSpec CreateSpec()
        {
            throw new NotImplementedException();
        }

        public IBuildDetail[] QueryBuilds()
        {
            throw new NotImplementedException();
        }

        public string ConfigurationFolderPath { get; set; }
        public string Description { get; set; }
        public string DefaultDropLocation { get; set; }
        public IBuildAgent DefaultBuildAgent { get; set; }
        public Uri DefaultBuildAgentUri { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<BuildStatus, IRetentionPolicy> RetentionPolicies { get; set; }
        public List<ISchedule> Schedules { get; set; }
        public IWorkspaceTemplate Workspace { get; set; }
        public Uri LastBuildUri { get; set; }
        public Uri LastGoodBuildUri { get; set; }
        public string LastGoodBuildLabel { get; set; }
        public ContinuousIntegrationType ContinuousIntegrationType { get; set; }
        public int ContinuousIntegrationQuietPeriod { get; set; }
        public IBuildServer BuildServer { get; set; }

        public IRetentionPolicy AddRetentionPolicy(BuildReason reason, BuildStatus status, int numberToKeep, DeleteOptions deleteOptions)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> AttachedProperties
        {
            get { throw new NotImplementedException(); }
        }

        public int BatchSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IBuildController BuildController
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Uri BuildControllerUri
        {
            get { throw new NotImplementedException(); }
        }

        public void CopyFrom(IBuildDefinition buildDefinition)
        {
            throw new NotImplementedException();
        }

        public IBuildDetail CreateManualBuild(string buildNumber, string dropLocation, BuildStatus buildStatus, IBuildController controller, string requestedFor)
        {
            throw new NotImplementedException();
        }

        public DateTime DateCreated
        {
            get { throw new NotImplementedException(); }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public IProcessTemplate Process
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ProcessParameters
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DefinitionQueueStatus QueueStatus { get; set; }

        public void Refresh(string[] propertyNameFilters, QueryOptions queryOptions)
        {
            throw new NotImplementedException();
        }

        public List<IRetentionPolicy> RetentionPolicyList
        {
            get { throw new NotImplementedException(); }
        }

        public DefinitionTriggerType TriggerType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}