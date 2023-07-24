using System;

namespace Visio.Parsers.Tex
{
	public class TexToken
	{
		public TexToken(string text, TexTokenType type, TokenIndex index)
		{
			Text = text;
			Type = type;
			Index = index.Index;
			Row = index.Row;
			Column = index.Column;
		}
        
		public TexToken(string text, TexTokenType type, TokenIndex index, TokenIndex endIndex):this(text, type, index)
		{
			EndIndex = endIndex;
		}
        

		public string Text { get; }
		public TexTokenType Type { get; }
		public string Role => Type.ToString();
		public int Index { get;  }
		public int Row { get; set; }
		public int Column { get; set; }

		public TokenIndex EndIndex { get; set; }

		public TokenIndex GetEndIndex()
		{
			int index = Index + Text.Length;
            
			return new TokenIndex{Index = index, Row = Row, Column = Column + Text.Length};
		}
		
		

		public override string ToString()
		{
			return $"[Index = {Index}; Text = {Text}; Type={Type} ]";
		}
	}
}