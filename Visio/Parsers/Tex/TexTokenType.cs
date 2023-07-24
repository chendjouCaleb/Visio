namespace Visio.Parsers.Tex
{
	public enum TexTokenType
	{
		TagBegin,
		Identifier,
		MathInlineOpen,
		MathInlineClose,
		MathDisplayOpen,
		MathDisplayClose,

		OpenBlock,
		CloseBlock,
		Text,
		Comma,
		Dot,
		Percent,
		Caret,
		Underscore,
		QuoteMarks,
		Apostrophe,

		LineEnd,
		Tab,
		Equal,

		Command,
		AttributeName,
		AttributeValue,
		
		SingleQuote,
		DoubleQuote,
		

		// []
		BracketOpen,
		BracketClose,

		// {}
		Brace,
		
		// $
		Dollar,

		HtmlEntity,
		HtmlCode,

		Comment,

		Space,
		Escape,
		TagName,
		
		MathInlineModelOpen,
		MathInlineModelClose,
		MathDisplayModeOpen,
		MathDisplayModeClose,
	}
}