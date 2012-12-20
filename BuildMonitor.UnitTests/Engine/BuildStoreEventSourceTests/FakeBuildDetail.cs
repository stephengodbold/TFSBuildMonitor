using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    public class FakeBuildDetail : IBuildDetail
    {
        public event StatusChangedEventHandler StatusChanging;
        public event StatusChangedEventHandler StatusChanged;
        public event PollingCompletedEventHandler PollingCompleted;
        public void Connect(int pollingInterval, ISynchronizeInvoke synchronizingObject)
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public IBuildDeletionResult Delete()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void RefreshMinimalDetails()
        {
            throw new NotImplementedException();
        }

        public void RefreshAllDetails()
        {
            throw new NotImplementedException();
        }

        public void Refresh(string[] informationTypes, QueryOptions queryOptions)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Wait()
        {
            throw new NotImplementedException();
        }

        public string BuildNumber { get; set; }
        public BuildPhaseStatus CompilationStatus { get; set; }
        public string ConfigurationFolderPath { get; set; }
        public string DropLocation { get; set; }
        public string LabelName { get; set; }
        public bool KeepForever { get; set; }
        public string LogLocation { get; set; }
        public string Quality { get; set; }
        public BuildStatus Status { get; set; }
        public BuildPhaseStatus TestStatus { get; set; }
        public IBuildAgent BuildAgent { get; set; }
        public Uri BuildAgentUri { get; set; }
        public IBuildDefinition BuildDefinition { get; set; }
        public Uri BuildDefinitionUri { get; set; }
        public bool BuildFinished { get; set; }
        public IBuildServer BuildServer { get; set; }
        public string CommandLineArguments { get; set; }
        public IBuildInformation Information { get; set; }
        public Uri ConfigurationFolderUri { get; set; }
        public string LastChangedBy { get; set; }
        public DateTime LastChangedOn { get; set; }
        public BuildReason Reason { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedFor { get; set; }
        public string SourceGetVersion { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public Uri Uri { get; set; }

        public IBuildController BuildController
        {
            get { throw new NotImplementedException(); }
        }

        public Uri BuildControllerUri
        {
            get { throw new NotImplementedException(); }
        }

        public void Connect(int pollingInterval, int timeout, ISynchronizeInvoke synchronizingObject)
        {
            throw new NotImplementedException();
        }

        public IBuildDeletionResult Delete(DeleteOptions options)
        {
            throw new NotImplementedException();
        }

        public string DropLocationRoot
        {
            get { throw new NotImplementedException(); }
        }

        public void FinalizeStatus(BuildStatus status)
        {
            throw new NotImplementedException();
        }

        public void FinalizeStatus()
        {
            throw new NotImplementedException();
        }

        public bool IsDeleted
        {
            get { throw new NotImplementedException(); }
        }

        public string LastChangedByDisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public string ProcessParameters
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<int> RequestIds
        {
            get { throw new NotImplementedException(); }
        }

        public Guid RequestIntermediateLogs()
        {
            throw new NotImplementedException();
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IQueuedBuild> Requests
        {
            get { throw new NotImplementedException(); }
        }

        public string ShelvesetName
        {
            get { throw new NotImplementedException(); }
        }

        public string TeamProject
        {
            get { throw new NotImplementedException(); }
        }

        public bool Wait(TimeSpan pollingInterval, TimeSpan timeout, ISynchronizeInvoke synchronizingObject)
        {
            throw new NotImplementedException();
        }

        public bool Wait(TimeSpan pollingInterval, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}