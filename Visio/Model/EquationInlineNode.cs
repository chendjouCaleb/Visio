namespace Visio.Model
{
	public class EquationInlineNode:EquationNode
	{
		public EquationInlineNode() : base(TexNodeType.EquationInlineNode)
		{
		}
		
		protected override (string, string) Delimiters => ("\\(", "\\)");
	}
}