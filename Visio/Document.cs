using System.Collections.Generic;
using Visio.Model;

namespace Visio
{
	public class Document
	{
		public List<TexNode> Nodes { get; }

		public Document(List<TexNode> nodes)
		{
			Nodes = nodes;
		}
	}
}