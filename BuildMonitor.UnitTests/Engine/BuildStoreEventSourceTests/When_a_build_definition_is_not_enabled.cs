using System.Linq;
using BuildMonitor.Engine;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    [TestClass]
// ReSharper disable InconsistentNaming
    public sealed class When_a_build_definition_is_not_enabled : A_BuildStoreEventSource_with_multiple_TeamProjects_BuildDefinitions_and_Builds

    {
        private IBuildDefinition DisabledBuildDefinition;
        private IBuildDefinition PausedBuildDefinition;

        [TestMethod]
        public void Should_not_include_builds_from_disabled_definitions()
        {
            var eventSource = CreateBuildStoreEventSource();

            var events = eventSource.GetListOfBuildStoreEvents();

            var eventsForDisabledBuildDefinition = from e in events
                                                   where e.Data.BuildDefinition == DisabledBuildDefinition
                                                   select e;

            Assert.AreEqual(0, eventsForDisabledBuildDefinition.Count());
        }

        [TestMethod]
        public void Should_not_include_builds_from_paused_definitions()
        {
            var eventSource = CreateBuildStoreEventSource();

            var events = eventSource.GetListOfBuildStoreEvents();

            var eventsForDisabledBuildDefinition = from e in events
                                                   where e.Data.BuildDefinition == PausedBuildDefinition
                                                   select e;

            Assert.AreEqual(0, eventsForDisabledBuildDefinition.Count());
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
            var eventSource = CreateBuildStoreEventSource(null);
            var firstProject = ProjectInfos[0];

            DisabledBuildDefinition = MockBuildServer.QueryBuildDefinitions(firstProject.Name).First();
            DisabledBuildDefinition.QueueStatus = DefinitionQueueStatus.Disabled;

            PausedBuildDefinition = MockBuildServer.QueryBuildDefinitions(firstProject.Name).First();
            PausedBuildDefinition.QueueStatus = DefinitionQueueStatus.Paused;

            return eventSource;
        }
        
    }
}
// ReSharper restore InconsistentNaming