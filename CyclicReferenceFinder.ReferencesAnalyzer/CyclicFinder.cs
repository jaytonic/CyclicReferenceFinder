using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyclicReferenceFinder.Model;

namespace CyclicReferenceFinder.ReferencesAnalyzer
{
    public class CyclicFinder
    {
		public static CyclicFinder Instance { get; } = new CyclicFinder();

		/// <summary>
		/// This search the projects references that prevent referencing "referencedProject" from "sourceProject"
		/// </summary>
		/// <param name="sourceProject">The source project.</param>
		/// <param name="referencedProject">The referenced project.</param>
		/// <returns></returns>
		public async Task<IList<Chain>> FindCylicChains( Project sourceProject,Project referencedProject)
		{
			return await FindChains(sourceProject, referencedProject);
		}

		/// <summary>
		/// Finds the chains.
		/// </summary>
		/// <param name="projectToFind">The project to find.</param>
		/// <param name="currentProject">The current project.</param>
		/// <returns></returns>
		private async Task<List<Chain>> FindChains(Project projectToFind, Project currentProject)
	    {
			List<Chain> projectsChains = new List<Chain>();
			//We are trying to find a reference from referencedProject --> sourceProject which would prevent to add the opposite reference
			foreach (Project reference in currentProject.References	)
			{
				if (reference.Equals(projectToFind))
				{
					Chain chain = new Chain();
					chain.InsertProject(reference);
					projectsChains.Add(chain);
				}
				else
				{
					projectsChains.AddRange( await FindChains(projectToFind, reference));
				}
			}

		    foreach (Chain projectsChain in projectsChains)
		    {
			    projectsChain.InsertProject(currentProject);
		    }

			return projectsChains;
		}
    }
}
