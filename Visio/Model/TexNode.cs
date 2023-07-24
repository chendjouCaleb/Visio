using System.Collections.Generic;

namespace Visio.Model
{
	public class TexNode
	{
		public TexNode(TexNodeType type)
		{
			Type = type;
		}

		public readonly TexNodeType Type;
		public readonly List<TexNode> ChildNodes = new();
		public TexNode ParentNode { get; set; }
	}
}