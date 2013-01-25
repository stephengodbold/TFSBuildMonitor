using System;
using BuildMonitor.Engine;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Server;
using Rhino.Mocks;

namespace BuildMonitor.UnitTests.Engine.BuildStoreEventSourceTests
{
    public abstract class A_BuildStoreEventSource_with_multiple_TeamProjects_BuildDefinitions_and_Builds
    {
        protected ProjectInfo[] ProjectInfos;
        protected IBuildServer MockBuildServer;
        protected const int ProjectInfoQuantity = 2;
        protected const int BuildDefinitionQuantity = 2;
        protected const int BuildDetailQuantity = 2;

        protected BuildStoreEventSource CreateBuildStoreEventSource(string buildDefinitionNameExclusionPattern)
        {
            var mockServiceProvider = MockRepository.GenerateStub<IServiceProvider>();
            var mockStuctureService = MockRepository.GenerateStub<ICommonStructureService>();
            MockBuildServer = MockRepository.GenerateStub<IBuildServer>();

            mockServiceProvider.Stub(m => m.GetService(typeof(ICommonStructureService)))
                .Return(mockStuctureService);
            mockServiceProvider.Stub(m => m.GetService(typeof(IBuildServer)))
                .Return(MockBuildServer);

            ProjectInfos = CreateProjectInfos(ProjectInfoQuantity, BuildDefinitionQuantity, BuildDetailQuantity, MockBuildServer);

            mockStuctureService.Stub(m => m.ListProjects())
                .Return(ProjectInfos);

            return new BuildStoreEventSource(mockServiceProvider, buildDefinitionNameExclusionPattern);
        }

        private static ProjectInfo[] CreateProjectInfos(int quantity, int buildDefinitionQuantity, int buildDetailQuantity, IBuildServer mockBuildServer)
        {
            var projectInfos = new ProjectInfo[quantity];
            for (var i = 0; i < quantity; i++)
            {
                var projectInfo = new ProjectInfo {Name = string.Format("TeamProject{0}", i)};
                projectInfos[i] = projectInfo;
                mockBuildServer.Stub(m => m.QueryBuildDefinitions(projectInfo.Name))
                    .Return(CreateFakeBuildDefinitions(projectInfo.Name, buildDefinitionQuantity, buildDetailQuantity, mockBuildServer));
            }
            return projectInfos;
        }

        private static FakeBuildDefinition[] CreateFakeBuildDefinitions(string teamProjectName, int quantity, int buildDetailQuantity, IBuildServer mockBuildServer)
        {
            var definitions = new FakeBuildDefinition[quantity];
            for (var i = 0; i < quantity; i++)
            {
                var definition = new FakeBuildDefinition
                                     {
                                         Name = string.Format("{0}_{1}", teamProjectName, i),
                                         TeamProject = teamProjectName,
                                         Enabled = true
                                     };
                definitions[i] = definition;
                mockBuildServer.Stub(m => m.QueryBuilds(definition))
                    .Return(CreateFakeBuildDetails(buildDetailQuantity, definition));
            }
            return definitions;
        }

        private static FakeBuildDetail[] CreateFakeBuildDetails(int quantity, IBuildDefinition definition)
        {
            var details = new FakeBuildDetail[quantity];
            for (var i = 0; i < quantity; i++)
            {
                var detail = new FakeBuildDetail
                                 {
                                     Uri = new Uri(string.Format("http://fake/{0}", Guid.NewGuid())),
                                     StartTime = DateTime.Now.AddHours(-i),
                                     BuildDefinition = definition
                                 };
                details[i] = detail;
            }
            return details;
        }

    }
}