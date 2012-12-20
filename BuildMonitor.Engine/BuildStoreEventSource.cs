using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BuildMonitor.Service;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Server;

namespace BuildMonitor.Engine
{
    public class BuildStoreEventSource
    {
        private readonly IServiceProvider teamFoundationServiceProvider;
        private readonly Regex buildDefinitionNameExclusionRegex;
        private readonly IDictionary<string, IBuildDetail> cacheLookup = new Dictionary<string, IBuildDetail>();

        public BuildStoreEventSource(IServiceProvider teamFoundationServiceProvider,
                                     string buildDefinitionNameExclusionPattern)
        {
            this.teamFoundationServiceProvider = teamFoundationServiceProvider;

            if (!string.IsNullOrEmpty(buildDefinitionNameExclusionPattern))
            {
                buildDefinitionNameExclusionRegex = new Regex(buildDefinitionNameExclusionPattern,
                                                              RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        public IEnumerable<BuildStoreEventArgs> GetListOfBuildStoreEvents()
        {
            var teamProjectNames = GetListOfTeamProjectNames();
            var builds = GetBuildsForTeamProjects(teamProjectNames);

            return builds.Select(GetBuildStoreEventIfAny)
                .Where(buildStoreEvent => buildStoreEvent != null);
        }

        private IEnumerable<string> GetListOfTeamProjectNames()
        {
            var structureService = teamFoundationServiceProvider.GetService<ICommonStructureService>();
            if (structureService == null)
                return new string[] {};

            var projectInfos = structureService.ListProjects();
            var projectInfoNames = projectInfos.Select(p => p.Name);
            return projectInfoNames;
        }

        private IEnumerable<IBuildDetail> GetBuildsForTeamProjects(IEnumerable<string> teamProjectNames)
        {
            var buildServer = teamFoundationServiceProvider.GetService<IBuildServer>();
            foreach (var teamProjectName in teamProjectNames)
            {
                var definitions = buildServer.QueryBuildDefinitions(teamProjectName);
                foreach (var definition in definitions)
                {
                    if (ShouldDefinitionBeIncluded(definition))
                    {
                        var builds = buildServer.QueryBuilds(definition);
                        var mostRecentBuild = builds.OrderBy(b => b.StartTime).LastOrDefault();
                        if (mostRecentBuild != null)
                            yield return mostRecentBuild;
                    }
                }
            }
        }

        private bool ShouldDefinitionBeIncluded(IBuildDefinition definition)
        {
            var isExcludedByRegex = false;
            if (buildDefinitionNameExclusionRegex != null)
            {
                isExcludedByRegex = buildDefinitionNameExclusionRegex.IsMatch(definition.Name);
            }
            return (definition.QueueStatus == DefinitionQueueStatus.Enabled)
                   && !isExcludedByRegex;
        }

        private BuildStoreEventArgs GetBuildStoreEventIfAny(IBuildDetail build)
        {
            BuildStoreEventArgs buildStoreEvent;
            if (!cacheLookup.ContainsKey(build.Uri.AbsoluteUri))
            {
                cacheLookup.Add(build.Uri.AbsoluteUri, build);

                buildStoreEvent = new BuildStoreEventArgs
                                      {
                                          Type = BuildStoreEventType.Build,
                                          Data = build
                                      };
                return buildStoreEvent;
            }

            IBuildDetail originalBuild = cacheLookup[build.Uri.AbsoluteUri];
            cacheLookup[build.Uri.AbsoluteUri] = build;

            if (originalBuild.Quality != build.Quality || originalBuild.Status != build.Status)
            {
                buildStoreEvent = new BuildStoreEventArgs
                                      {
                                          Data = build,
                                          Type = originalBuild.Quality != build.Quality
                                                     ? BuildStoreEventType.QualityChanged
                                                     : BuildStoreEventType.Build
                                      };
                return buildStoreEvent;
            }

            return null;
        }
    }
}