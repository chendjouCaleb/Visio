namespace Visio.Model
{
	public class EquationDisplayNode:EquationNode
	{
		public EquationDisplayNode() : base(TexNodeType.EquationDisplayNode)
		{
		}

		protected override (string, string) Delimiters => ("\\[", "\\]");
	}
}