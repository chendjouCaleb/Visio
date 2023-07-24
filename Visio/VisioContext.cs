using System.Linq;
using SkiaSharp;
using Visio.Parsers.Tex;

namespace Visio
{
	public class VisioContext
	{
		private SKCanvas _skCanvas;

		public Document Document { get; }

		public VisioContext(string text, SKCanvas skCanvas)
		{
			_skCanvas = skCanvas;
			var lexer = new TexLexer(text);
			var treeBuilder = new TexTreeBuilder(lexer.Tokenize());

			var nodes = treeBuilder.Parse();
			Document = new Document(nodes.ToList());
		}


		public void Draw()
		{
		}
		
		
	}
}