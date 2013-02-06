using System;
using System.Collections.Generic;
using System.Linq;
using BuildMonitor.Engine;
using Microsoft.TeamFoundation.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    // ReSharper disable InconsistentNaming
    [TestClass]
    public class When_excluding_projects_by_regex : A_BuildStoreEventSource_with_multiple_TeamProjects_BuildDefinitions_and_Builds
    {
        private const string BuildDefinitionNameExclusionPattern = "";
        private const string ProjectNameExclusionPattern = "^((?!Mettle).)*$";

        [TestMethod]
        public void no_builds_should_be_excluded_for_null_regex()
        {
            BuildDefinitionQuantity = 2;
            ProjectInfos = GetProjects().ToArray();
            AddBuildDefinitionsToProjects(ProjectInfos);

            var buildStore = CreateBuildStoreEventSource(null, null);
            var events = buildStore.GetListOfBuildStoreEvents();

            Assert.AreEqual(ProjectInfos.Count() * BuildDefinitionQuantity, events.Count());
        }

        [TestMethod]
        public void build_from_excluded_project_should_not_be_included()
        {
            const string excludedProject = "NightHawk";

            BuildDefinitionQuantity = 2;
            ProjectInfos = GetProjects().ToArray();
            AddBuildDefinitionsToProjects(ProjectInfos);
            var buildStore = CreateBuildStoreEventSource(excludedProject, null);
            
            var events = buildStore.GetListOfBuildStoreEvents().ToArray();

            Assert.AreEqual(0, events.Count(e => e.Data.TeamProject.Equals(excludedProject, StringComparison.InvariantCultureIgnoreCase)));
        }

        [TestMethod]
        public void builds_from_included_projects_should_be_returned()
        {
            const string excludedProject = "NightHawk";

            BuildDefinitionQuantity = 2;
            ProjectInfos = GetProjects().ToArray();
            AddBuildDefinitionsToProjects(ProjectInfos);
            var buildStore = CreateBuildStoreEventSource(excludedProject, null);
            var expectedBuilds = (ProjectInfos.Count() - 1)*BuildDefinitionQuantity;
            
            var events = buildStore.GetListOfBuildStoreEvents().ToArray();

            Assert.AreEqual(expectedBuilds, events.Count());
        }

        private BuildStoreEventSource CreateBuildStoreEventSource()
        {
            ProjectInfos = GetProjects().ToArray();

            AddBuildDefinitionsToProjects(ProjectInfos);

            return CreateBuildStoreEventSource(ProjectNameExclusionPattern, BuildDefinitionNameExclusionPattern); ;
        }

        private void AddBuildDefinitionsToProjects(IEnumerable<ProjectInfo> infos)
        {
            foreach (var projectInfo in infos)
            {
                var info = projectInfo;
                MockBuildServer.Stub(m => m.QueryBuildDefinitions(info.Name))
                    // ReSharper disable CoVariantArrayConversion
                               .Return(CreateFakeBuildDefinitions(projectInfo.Name,
                                                    BuildDefinitionQuantity,
                                                    BuildDetailQuantity,
                                                    MockBuildServer));
                // ReSharper restore CoVariantArrayConversion
            }
        }

        private IEnumerable<ProjectInfo> GetProjects()
        {
            return new[]
                {
                    new ProjectInfo
                        {
                            Name = "Q"
                        },
                    new ProjectInfo
                        {
                            Name = "NightHawk"
                        },
                    new ProjectInfo
                        {
                            Name = "Copernicus"
                        },
                    new ProjectInfo
                        {
                            Name = "Mettle"
                        }
                };
        }
    }
    // ReSharper restore InconsistentNaming

}