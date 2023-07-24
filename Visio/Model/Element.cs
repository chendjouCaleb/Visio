using System.Collections.Generic;
using System.Text;

namespace Visio.Model
{
	public class Element: TexNode
	{
		private static long _idCount = 0;
		private static long GetId()
		{
			_idCount += 1;
			return _idCount;
		}

		public long Id { get; } = GetId();
		public int Index { get; set; }
		public string TagName { get; set; } = "";

		public Element Parent { get; set; }
		public Block Block { get; set; }
		
		public List<Element> Children { get; set; } = new ();
		public readonly List<Block> Blocks = new();
		public readonly Attributes Attributes = new ();


		public Element():base(TexNodeType.ElementNode) {}
		public Element(string tagName):base(TexNodeType.ElementNode)
		{
			TagName = tagName;
		}

		public override string ToString()
		{
			var builder = new StringBuilder($"\\{TagName}");

			builder.Append(Attributes);
			

			foreach (var block in Blocks)
			{
				builder.Append(block.ToString());
			}

			return builder.ToString();

		}
	}
}