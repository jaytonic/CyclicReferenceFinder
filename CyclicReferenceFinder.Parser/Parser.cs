using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using CyclicReferenceFinder.Model;

namespace CyclicReferenceFinder.Parser
{
	public class Parser
	{

		public static Parser Instance { get; } = new Parser();

		private Parser() { }


		public IEnumerable<Project> Parse(string slnPath)
		{
			List<Project> projects = LoadProjectsFromSln(slnPath);

			LoadReferences(projects);
			return projects;
		}

		private void LoadReferences(List<Project> projects)
		{
			XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
			foreach (Project project in projects)
			{
				XDocument xDocument = XDocument.Load(project.ProjectPath);
				IEnumerable<XElement> projectReferences = xDocument.Descendants(ns + "ProjectReference");
				foreach (XElement projectNode in projectReferences.Descendants(ns + "Project"))
				{
					string idString = projectNode.Value;
					Guid id;
					if (Guid.TryParse(idString, out id))
					{
						Project referencedProject = projects.FirstOrDefault(p => p.ProjectId == id);
						if (referencedProject != null)
						{
							project.References.Add(referencedProject);
						}
					}
				}
			}
		}

		private List<Project> LoadProjectsFromSln(string slnPath)
		{
			string slnDirectory = Path.GetDirectoryName(slnPath);
			if (slnDirectory == null)
			{
				throw new ArgumentException("Unable to parse SLN", nameof(slnPath));
			}
			List<Project> projects = new List<Project>();
			Regex regex = new Regex(@"Project\(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}""\) = ""([^""]+)"", ""([^""]+)"", ""{([^}]+)}""");
			using (StreamReader streamReader = new StreamReader(slnPath))
			{
				string line;
				while ((line = streamReader.ReadLine()) != null)
				{
					Match match = regex.Match(line);
					if (match.Success)
					{
						string name = match.Groups[1].Value;
						string path = match.Groups[2].Value;
						string stringId = match.Groups[3].Value;
						path = Path.Combine(slnDirectory, path);
						Guid id;
						if (Guid.TryParse(stringId, out id))
						{
							projects.Add(new Project(name, path, id));
						}
						else
						{
							Console.WriteLine($"Unable to parse {stringId} as a valid Guid");
						}
					}
				}
			}
			return projects;
		}
	}
}
