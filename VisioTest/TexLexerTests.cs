using System;
using NUnit.Framework;
using Visio.Helpers;
using Visio.Parsers.Tex;

namespace VisioTest
{
#pragma warning disable NUnit2005
	public class TexLexerTests
	{
		[Test]
		public void EmptyTest()
		{
			var tokens = new TexLexer("").Tokenize();

			Assert.AreEqual(0, tokens.Count);

		}
		
		[Test]
		public void NullTextShouldThrowError()
		{
			Assert.Throws<ArgumentNullException>(() => new TexLexer(null!).Tokenize());
		}
		
		[Test]
		[TestCase(@"\text")]
		[TestCase(@"\text ")]
		[TestCase(@"\text[]")]
		[TestCase(@"\text{} ")]
		[TestCase("\\text{}\n")]
		[TestCase("\\text{}\t")]
		[TestCase("\\text{}\r")]
		public void Identifier(string codeText)
		{
			TexToken token = new TexLexer(codeText).Tokenize()[1];

			Assert.AreEqual("text", token.Text);
			Assert.AreEqual(TexTokenType.Identifier, token.Type);
			Assert.IsTrue(token.Index.Equals(1));
			Assert.IsTrue(token.EndIndex.Equals(4));
		}


		[Test]
		[TestCase(@"\( ", "(", TexTokenType.MathInlineOpen)]
		[TestCase(@"\) ", ")", TexTokenType.MathInlineClose)]
		[TestCase(@"\[ ", "[", TexTokenType.MathDisplayOpen)]
		[TestCase(@"\] ", "]", TexTokenType.MathDisplayClose)]
		public void TakeSymbolTag(string codeText, string value, TexTokenType type)
		{
			TexToken token = new TexLexer(codeText).Tokenize()[1];

			Assert.AreEqual(value, token.Text);
			Assert.AreEqual(type, token.Type);
			Assert.IsTrue(token.Index.Equals(1));
			Assert.IsTrue(token.EndIndex.Equals(1));
		}


		[TestCase("{e", "{", TexTokenType.OpenBlock)]
		[TestCase("}\\", "}", TexTokenType.CloseBlock)]
		[TestCase("$\\", "$", TexTokenType.Dollar)]
		public void SimpleSymbols(string codeText, string tokenValue, TexTokenType type)
		{
			TexToken token = new TexLexer(codeText).Tokenize()[0];
			Assert.AreEqual(tokenValue, token.Text);
			Assert.AreEqual(type, token.Type);
			Assert.IsTrue(token.Index.Equals(0));
			Assert.IsTrue(token.EndIndex.Equals(0));
		}
		

		[Test]
		public void SkipWhiteSpace()
		{
			var lexer = new TexLexer(StringHelper.WhiteSpaceChars);
			var tokens = lexer.Tokenize();

			Assert.AreEqual(0, tokens.Count);
		}
	}
}