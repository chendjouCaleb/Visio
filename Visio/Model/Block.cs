using System.Collections.Generic;
using System.Text;

namespace Visio.Model
{
	public class Block: TexNode
	{

		public Element Parent { get; set; }
		public int Index { get; set; }

		public readonly List<Element> Children = new();

		public Block(Element parent, int index) : base(TexNodeType.BlockNode)
		{
			Parent = parent;
			Index = index;
		}


		public override string ToString()
		{
			var builder = new StringBuilder("{");

			foreach (var node in ChildNodes)
			{
				builder.Append(node);
			}

			builder.Append('}');
			return builder.ToString();
		}
	}
}