using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclicReferenceFinder.Model
{
	public class Chain
	{
		public IList<Project> Projects { get; }= new List<Project>();

		public void InsertProject(Project project)
		{
			Projects.Insert(0,project);
		}

		public override string ToString()
		{
			return String.Join(" -> ", Projects);
		}
	}
}
