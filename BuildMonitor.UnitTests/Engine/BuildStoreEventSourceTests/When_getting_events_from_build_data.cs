using System.Linq;
using BuildMonitor.Engine;
using BuildMonitor.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    [TestClass]
    public sealed class When_getting_events_from_build_data : A_BuildStoreEventSource_with_multiple_TeamProjects_BuildDefinitions_and_Builds
    {
        [TestMethod]
        public void Should_return_one_event_per_project_per_build_definition()
        {
            var eventSource = CreateBuildStoreEventSource();

            var events = eventSource.GetListOfBuildStoreEvents();

            Assert.AreEqual(ProjectInfoQuantity * BuildDefinitionQuantity, events.Count());
        }

        [TestMethod]
        public void Should_return_event_for_most_recent_build_per_build_definition()
        {
            var eventSource = CreateBuildStoreEventSource();

            var allDefinitions = ProjectInfos.SelectMany(p => MockBuildServer.QueryBuildDefinitions(p.Name));

            var mostRecentBuilds = allDefinitions.Select(d => MockBuildServer.QueryBuilds(d)
                                                                  .OrderBy(b => b.StartTime)
                                                                  .LastOrDefault());

            var events = eventSource.GetListOfBuildStoreEvents();
            var eventBuilds = events.Select(e => e.Data);

            CollectionAssert.AreEquivalent(mostRecentBuilds.ToList(), eventBuilds.ToList());
        }

        private BuildStoreEventSource CreateBuildStoreEventSource()
        {
            return CreateBuildStoreEventSource(null);
        }


    }
}
