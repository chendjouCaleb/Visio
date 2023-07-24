using System;
using System.Collections.Generic;
using System.Text;
using Visio.Exceptions;
using Visio.Helpers;

namespace Visio.Parsers.Tex
{
	public class TexLexer
	{
		private readonly List<TexToken> _tokens = new();
        private readonly TextIterator _it;
        private readonly List<char> _stopChars = new (){'\\', '{', '}', '$'};
        
        private bool _parsed;

        public List<TexToken> Tokens => new (_tokens);
        public TokenIndex Index => _it.Index;

        public TexLexer(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _it = new TextIterator(text);
        }

        public List<TexToken> Tokenize()
        {
            if (_parsed)
            {
                throw new InvalidOperationException("Lexer already parsed.");
            }

            _parsed = true;


            while (_it.Has)
            {
                if (_it.IsIn(StringHelper.WhiteSpaceChars))
                {
                    SkipSpace();
                } else if(!_stopChars.Contains(_it.Current))
                {
                    TakeText();
                }
                else if (_it.Current == '\\')
                {
                    TakeTag();
                }else if (_it.Current == '{')
                {
                    TakeChar('{', TexTokenType.OpenBlock);
                }else if (_it.Current == '}')
                {
                    TakeChar('}', TexTokenType.CloseBlock);
                }else if (_it.Current == '$')
                {
                    TakeChar('$', TexTokenType.Dollar);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected character: {_it.Current} at {_it.Index}.");
                }
            }

            return Tokens;
        }

        public void TakeTag()
        {
            TakeChar('\\', TexTokenType.TagBegin);
         
            if (_it.Has && _it.IsLetter())
            {
                TakeIdentifier();
                SkipSpace();
                if (_it.Has && _it.Current == '[')
                {
                    TakeAttributes();
                }
            }else if (_it.Has && _it.Current == '(')
            {
                TakeChar('(', TexTokenType.MathInlineOpen);
            }else if (_it.Has && _it.Current == ')')
            {
                TakeChar(')', TexTokenType.MathInlineClose);
            }else if (_it.Has && _it.Current == '[')
            {
                TakeChar('[', TexTokenType.MathDisplayOpen);
            }else if (_it.Has && _it.Current == ']')
            {
                TakeChar(']', TexTokenType.MathDisplayClose);
            }
        }

        public TexToken TakeInlineMathOpen(TokenIndex startIndex)
        {
            UnexpectedTokenException.AssertExpected('(', _it.Current);
            TokenIndex endIndex = _it.Index;
            
            TexToken token = new(@"\(", TexTokenType.MathInlineModelOpen, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }
        
        
        public TexToken TakeInlineMathClose(TokenIndex startIndex)
        {
            UnexpectedTokenException.AssertExpected(')', _it.Current);
            TokenIndex endIndex = _it.Index;
            
            TexToken token = new(@"\)", TexTokenType.MathInlineModelClose, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }
        
        public TexToken TakeDisplayMathOpen(TokenIndex startIndex)
        {
            UnexpectedTokenException.AssertExpected('[', _it.Current);
            TokenIndex endIndex = _it.Index;
            
            TexToken token = new(@"\[", TexTokenType.MathDisplayModeOpen, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }
        
        
        public TexToken TakeDisplayMathClose(TokenIndex startIndex)
        {
            UnexpectedTokenException.AssertExpected(']', _it.Current);
            TokenIndex endIndex = _it.Index;
            
            TexToken token = new(@"\]", TexTokenType.MathDisplayModeClose, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }

        private TexToken TakeText()
        {
            StringBuilder builder = new ();
            var startIndex = _it.Index;
            var endIndex = _it.Index;

            while (_it.Has && !_stopChars.Contains(_it.Current))
            {
                builder.Append(_it.Current);
                _it.Next();
                endIndex = _it.Index;
            }
            
            TexToken token = new(builder.ToString(), TexTokenType.Text, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }
        
        public TexToken TakeIdentifier()
        {
            StringBuilder builder = new();
            TokenIndex startIndex = _it.Index;
            TokenIndex endIndex = _it.Index;
            while (_it.Has && MatchTagChar(_it.Current))
            {
                builder.Append(_it.Current);
                endIndex = _it.Index;
                _it.Next();
            }
            
            TexToken token = new(builder.ToString(), TexTokenType.Identifier, startIndex, endIndex);
            _tokens.Add(token);

            return token;
        }

       

        public TexToken TakeChar(char character, TexTokenType tokenType)
        {
            if (!_it.Is(character))
            {
                throw new UnexpectedTokenException($"Expected char: '{character}'; current char: '{_it.Current}'");
            }

            TexToken token = new ($"{_it.Current}", tokenType, _it.Index, _it.Index);
            _tokens.Add(token);
            _it.Next();

            return token;
        }

        public TexToken TakeInlineComment(TokenIndex index)
        {
            StringBuilder builder = new("//");
            // Skip the current '/'
            _it.Next();
            while (_it.IsNot('\n'))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('\n'))
            {
                builder.Append('\n');
                _it.Next();
            }
            
            TexToken token = new TexToken(builder.ToString(), TexTokenType.Comment, index);
            _tokens.Add(token);

            return token;
        }
        
        
        public TexToken TakeMultilineComment(TokenIndex index)
        {
            StringBuilder builder = new("/*");
            // Skip the current '*'
            _it.Next();
            while (_it.Has)
            {
                if (_it.Is('*'))
                {
                    builder.Append('*');
                    _it.Next();
                    if (_it.Is('/'))
                    {
                        builder.Append("/");
                        _it.Next();
                        break;
                    }
                }
                else
                {
                    builder.Append(_it.Current);
                    _it.Next();
                }
            }

            TexToken token = new (builder.ToString(), TexTokenType.Comment, index);
            _tokens.Add(token);

            return token;
        }

        private void TakeAttributes()
        {
            TakeChar('[', TexTokenType.BracketOpen);
            SkipSpace();
            
            while (_it.IsNot(']'))
            {
                
                SkipSpace();
                var name = TakeAttributeName(_it.Index);
                Console.WriteLine("Take attr id: " + name.Text);
                SkipSpace();
                
                if (_it.Has && _it.Current == '=')
                {
                    TakeChar('=', TexTokenType.Equal);

                    if (_it.Has)
                    {
                        if (_it.Current == '\'')
                        {
                            TakeAttributeSingleQuotedValue();
                        }else if (_it.Current == '"')
                        {
                            TakeAttributeQuotedValue();
                        }
                    }
                }
                
            }

            if (_it.Is(']'))
            {
                TakeChar(']', TexTokenType.BracketClose);
            }
        }

        private TexToken? TakeAttributeName(TokenIndex index)
        {
            StringBuilder builder = new ();
            while (_it.Has && _it.IsNotIn("\r\n\t\0 =/]"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (builder.Length > 0)
            {
                TexToken token = new(builder.ToString(), TexTokenType.AttributeName, index);
                _tokens.Add(token);

                return token;
            }

            return null;
        }
        

        private TexToken? TakeAttributeValue()
        {
            SkipSpace();
            if (!_it.Has || _it.Is(']'))
            {
                return null;
            }

            TokenIndex index = _it.Index;
            string text = string.Empty;

            if (_it.Is('"'))
            {
                text = GetAttributeQuotedValue();
            }
            else
            {
                text = GetAttributeUnQuotedValue();
            }

            TexToken token = new (text, TexTokenType.AttributeValue, index);
            _tokens.Add(token);

            return token;
        }
        
        private TexToken TakeAttributeQuotedValue()
        {
            TakeChar('"', TexTokenType.DoubleQuote);
            StringBuilder builder = new ();
            var startIndex = _it.Index;
            var endIndex = _it.Index;
            
            while (_it.Has && _it.IsNot('"'))
            {
                builder.Append(_it.Current);
                endIndex = _it.Index;
                _it.Next();
            }

            var token = new TexToken(builder.ToString(), TexTokenType.AttributeValue, startIndex, endIndex);
            _tokens.Add(token);
            if (_it.Is('"'))
            {
                TakeChar('"', TexTokenType.DoubleQuote);
            }


            return token;
        }
        
        
        private TexToken TakeAttributeSingleQuotedValue()
        {
            TakeChar('\'', TexTokenType.SingleQuote);
            StringBuilder builder = new ();
            var startIndex = _it.Index;
            var endIndex = _it.Index;
            
            while (_it.Has && _it.IsNot('\''))
            {
                builder.Append(_it.Current);
                endIndex = _it.Index;
                _it.Next();
            }

            var token = new TexToken(builder.ToString(), TexTokenType.AttributeValue, startIndex, endIndex);
            _tokens.Add(token);

            if (_it.Is('\''))
            {
                TakeChar('\'', TexTokenType.SingleQuote);
            }


            return token;
        }

        private string GetAttributeQuotedValue()
        {
            TakeChar('"', TexTokenType.DoubleQuote);
            StringBuilder builder = new ();
            while (_it.Has && _it.IsNot('"'))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('"'))
            {
                TakeChar('"', TexTokenType.DoubleQuote);
            }
            

            return builder.ToString();
        }
        
        private string GetAttributeUnQuotedValue()
        {
            StringBuilder builder = new();
            while (_it.Has && _it.IsNotIn("\t\n\r /]"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            return builder.ToString();
        }

        private void SkipInlineSpace()
        {
            while (_it.IsIn("\t "))
            {
                _it.Next();
            }
        }
        
        private void SkipSpace()
        {
            while (_it.IsIn(StringHelper.WhiteSpaceChars))
            {
                _it.Next();
            }
        }
        

        private bool MatchTagChar(char c)
        {
            return IsDigit(c) || IsLetter(c);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter(char c)
        {
            return c >= 'a' && c <= 'z' || (c >= 'A' && c <= 'Z');
        }
        
        private void Expect(char c)
        {
            if (!_it.Has)
            {
                throw new UnexpectedTokenException($"Expected: {c}, but is finish.");
            }

            if (_it.Has && _it.Current != c)
            {
                var current = _it.Current;
                throw new UnexpectedTokenException($"Expected: {c}, but was: {current}.");
            }
        }
	}
}