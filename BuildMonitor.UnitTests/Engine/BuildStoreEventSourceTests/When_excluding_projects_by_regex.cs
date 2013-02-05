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
            ProjectInfos = GetProjects().ToArray();
            var buildStore = CreateBuildStoreEventSource(null,null);
        }

        private BuildStoreEventSource CreateBuildStoreEventSource()
        {
            ProjectInfos = GetProjects().ToArray();
            return CreateBuildStoreEventSource(ProjectNameExclusionPattern, BuildDefinitionNameExclusionPattern); ;
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