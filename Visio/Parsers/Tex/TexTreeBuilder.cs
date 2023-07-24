using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Visio.Exceptions;
using Visio.Helpers;
using Visio.Model;

namespace Visio.Parsers.Tex
{
	public class TexTreeBuilder
	{
		private readonly List<TexError> _errors = new();
		private readonly Iterator<TexToken> _it;
		private readonly List<TexNode> _nodes = new();

		public ReadOnlyCollection<TexError> Errors => _errors.AsReadOnly();
		public ReadOnlyCollection<TexNode> Nodes => _nodes.AsReadOnly();

		public TexTreeBuilder(List<TexToken> tokens)
		{
			_it = new Iterator<TexToken>(tokens);
		}

		public IList<TexNode> Parse()
		{
			while (_it.Has)
			{
				var node = TakeNode();
				if (node != null)
				{
					_nodes.Add(node);
				}
				
			}

			return Nodes;
		}

		public TexNode? TakeNode()
		{
			AssertHelper.True(_it.Has);
			var current = _it.Current;
			if (_it.Current.Type == TexTokenType.Text)
			{
				return TakeText();
			}
			else if (_it.Current.Type == TexTokenType.TagBegin)
			{
				return HandleTag();
			}

			throw new UnexpectedTokenException($"Unexpected token: {current.Text} at {current.Index}.");
		}

		public TexNode TakeText()
		{
			AssertHelper.True(_it.Current.Type == TexTokenType.Text);
			Text node = new(_it.Current.Text);
			_it.Next();
			return node;
		}

		public Element TakeElement()
		{
			AssertHelper.True(_it.Current.Type == TexTokenType.Identifier);
			var element = new Element(_it.Current.Text);
			_it.Next();

			if (_it.Current.Type == TexTokenType.BracketOpen)
			{
				TakeAttributes(element.Attributes);
				
			}

			while (_it.Has && _it.Current.Type == TexTokenType.OpenBlock)
			{
				element.Blocks.Add(TakeBlock(element));
			}

			return element;
		}

		private void TakeAttributes(Attributes attrs)
		{
			Expect(TexTokenType.BracketOpen);
			_it.Next();
			
			while (_it.Has && _it.Current.Type != TexTokenType.BracketClose)
			{
				Expect(TexTokenType.AttributeName);
				var name = _it.Current.Text;
				_it.Next();
				string value = "";
				if (_it.Current.Type == TexTokenType.Equal)
				{
					_it.Next();

					if (_it.Current.Type == TexTokenType.SingleQuote)
					{
						_it.Next();
						Expect(TexTokenType.AttributeValue);
						value = _it.Current.Text;
						_it.Next();
						Expect(TexTokenType.SingleQuote);
						_it.Next();
					}else if (_it.Current.Type == TexTokenType.DoubleQuote)
					{
						_it.Next();
						Expect(TexTokenType.AttributeValue);
						value = _it.Current.Text;
						_it.Next();
						Expect(TexTokenType.DoubleQuote);
						_it.Next();
					}
				}
				attrs.Add(name, value);
			}
			Expect(TexTokenType.BracketClose);
			_it.Next();
		}

		public EquationInlineNode TakeInlineEquation()
		{
			Expect(TexTokenType.MathInlineOpen);
			_it.Next();
			Console.WriteLine("Eq begin");
			var node = new EquationInlineNode();

			while (_it.Has && _it.Current.Type != TexTokenType.TagBegin)
			{
				var childNode = TakeNode();
				if (childNode != null)
				{
					node.ChildNodes.Add(childNode);
				}
			}
			
			Expect(TexTokenType.TagBegin);
			Console.WriteLine("Tag end");
			_it.Next();
			Expect(TexTokenType.MathInlineClose);
			_it.Next();
			return node;
		}

		public Block TakeBlock(Element element)
		{
			AssertHelper.True(_it.Current.Type == TexTokenType.OpenBlock);
			var openToken = _it.Current;
			_it.Next();

			var block = new Block(element, element.Blocks.Count);
			while (_it.Has && _it.Current.Type != TexTokenType.CloseBlock)
			{
				var node = TakeNode();
				if (node != null)
				{
					block.ChildNodes.Add(node);
				}
			}

			if (_it.Has && _it.Current.Type == TexTokenType.CloseBlock)
			{
				_it.Next();
			}
			else
			{
				_errors.Add(new TexError(TexErrorType.NoCloseBlock, openToken));
			}

			return block;
		}

		public TexNode? HandleTag()
		{
			AssertHelper.True(_it.Current.Type == TexTokenType.TagBegin);
			var current = _it.Current;
			_it.Next();

			if (_it.Has && _it.Current.Type == TexTokenType.Identifier)
			{
				var element = TakeElement();
				return element;
			}else if (_it.Has && _it.Current.Type == TexTokenType.MathInlineOpen)
			{
				var node = TakeInlineEquation();
				return node;
			}

			throw new UnexpectedTokenException("Expect tag begin at: " + current.Index);
		}

		private void Expect(TexTokenType type)
		{
			if (!_it.Has)
			{
				throw new UnexpectedTokenException($"Expected: {type.ToString()}, but is finish.");
			}

			if (_it.Has && _it.Current.Type != type)
			{
				var current = _it.Current;
				throw new UnexpectedTokenException($"Expected: {type.ToString()}, but was: {current.Type.ToString()}:{current.Text}, " +
				                                   $"at: Line:{current.Row}, Col: {current.Column}");
			}
		}
	}
}