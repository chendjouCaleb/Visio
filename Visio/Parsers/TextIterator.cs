using System;

namespace Visio.Parsers
{
	public class TextIterator
	{
		private TokenIndex _index = new ();
		private string _text;

		public TokenIndex Index => new (_index);

		public bool Has => _index.Index <= _text.Length - 1;
		public char Current => _text[_index.Index];
        

		public TextIterator(string text)
		{
			_text = text ?? throw new ArgumentNullException(text);
		}

		public bool IsIn(string value)
		{
			return Has && !string.IsNullOrEmpty(value) && value.Contains(Current);
		}
        
		public bool IsNotIn(string value)
		{
			return string.IsNullOrEmpty(value) || !value.Contains(Current);
		}
        
		public bool IsLetter()
		{
			return Has && (Current >= 'a' && Current <= 'z' || (Current >= 'A' && Current <= 'Z'));
		}
        
		public bool IsDigit()
		{
			return Has && Current >= '0' && Current <= '9';
		}


		public bool Is(char c) => Has && Current == c;
		public bool IsNot(char c) => !Has || Current != c;
        
		public bool Next()
		{
			// if (!HasMore)
			// {
			//     return false;
			// }
            
			_index.Index += 1;
            
			if (Has && Current == '\n')
			{
				_index.Row += 1;
				_index.Column = 0;
			}
			else
			{
				_index.Column += 1;
			}

            
			return true;
		}
	}
}