using System.Text;

namespace Visio.Model
{
	public abstract class EquationNode:TexNode
	{
		public EquationNode(TexNodeType type) : base(type)
		{
		}
		
		protected abstract (string, string) Delimiters { get; }

		public override string ToString()
		{
			var builder = new StringBuilder(Delimiters.Item1);

			foreach (var childNode in ChildNodes)
			{
				builder.Append(childNode);
			}

			builder.Append(Delimiters.Item2);
			return builder.ToString();
		}
	}
}