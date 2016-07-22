using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyclicReferenceFinder.Model;
using CyclicReferenceFinder.Parser;
using CyclicReferenceFinder.ReferencesAnalyzer;

namespace ConsoleRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			IEnumerable<Project> projects = Parser.Instance.Parse(@"D:\Dev\WS_1\Branches\ReworkedServer\Solution\XMS_VS2010.sln");
			Console.WriteLine($"Parsed {projects.Count()} projects");
			Project sourceProject = projects.First(p => p.Name.Equals("Xms.DataManagement"));
			Project projectToReference = projects.First(p => p.Name.Equals("XmsClientCore"));
			IList <Chain> chains = CyclicFinder.Instance.FindCylicChains(sourceProject, projectToReference).Result;

			foreach (Chain chain in chains){
				Console.WriteLine(chain);
			}
			Console.ReadLine();
		}
	}
}
