using System.Linq;
using BuildMonitor.Engine;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    [TestClass]
    public sealed class When_excluding_build_definitions_by_regular_expression : A_BuildStoreEventSource_with_multiple_TeamProjects_BuildDefinitions_and_Builds
    {
        private const string BuildDefinitionNameExclusionPattern = "Trial$";
        private const string ProjectNameExclusionPattern = "";
        private IBuildDefinition DefinitionNamedBuildTrial;
        
        [TestMethod]
        public void Should_not_include_definitions_ending_with_Trial()
        {
            var eventSource = CreateBuildStoreEventSource();

            var events = eventSource.GetListOfBuildStoreEvents();

            var eventsForDefinitionNamedBuildTrial = from e in events
                                                     where e.Data.BuildDefinition == DefinitionNamedBuildTrial
                                                     select e;

            Assert.AreEqual(0, eventsForDefinitionNamedBuildTrial.Count());
        }

        [TestMethod]
        public void Should_still_return_other_build_events()
        {
            var eventSource = CreateBuildStoreEventSource();
            var events = eventSource.GetListOfBuildStoreEvents();
            Assert.AreNotEqual(0, events.Count());
        }

        private BuildStoreEventSource CreateBuildStoreEventSource()
        {
            var eventSource = CreateBuildStoreEventSource(ProjectNameExclusionPattern, BuildDefinitionNameExclusionPattern);

            var firstProject = ProjectInfos[0];
            DefinitionNamedBuildTrial = MockBuildServer.QueryBuildDefinitions(firstProject.Name).First();
            DefinitionNamedBuildTrial.Name = "BuildTrial";

            return eventSource;
        }
    }
}