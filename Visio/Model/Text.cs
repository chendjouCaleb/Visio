namespace Visio.Model
{
	public class Text : TexNode
	{
		public string Content { get; set; }
		
		public Text(string content) : base(TexNodeType.TextNode)
		{
			Content = content;
		}

		public override string ToString()
		{
			return $"\"{Content}\"";
		}
	}
}