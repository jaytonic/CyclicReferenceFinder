using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclicReferenceFinder.Model
{
	public class Project:IEquatable<Project>
	{
		public String Name { get; }
		public String ProjectPath { get; }
		public Guid ProjectId { get; }

		public IList<Project> References { get; }

		public Project(String name, string projectPath, Guid projectId)
		{
			Name = name;
			ProjectPath = projectPath;
			ProjectId = projectId;
			References = new List<Project>();
		}

		public bool Equals(Project other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return ProjectId.Equals(other.ProjectId);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			Project other = obj as Project;
			return other != null && Equals(other);
		}

		public override int GetHashCode()
		{
			return ProjectId.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}
		
	}
}
