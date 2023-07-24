using System;

namespace Visio.Parsers
{
	public class TokenIndex
	{
		/// <summary>
		/// Line index of the token.
		/// </summary>
		public int Row { get; set; }
        
		/// <summary>
		/// Index of token relative to the current line.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Global index of the token.
		/// </summary>
		public int Index { get; set; }

		public TokenIndex()
		{

		}

		public TokenIndex(int index, int row, int column)
		{
			Row = row;
			Column = column;
			Index = index;
		}
        
		public TokenIndex((int, int, int) tokenIndex)
		{
			Index = tokenIndex.Item1;
			Row = tokenIndex.Item2;
			Column = tokenIndex.Item3;
		}
        
		public TokenIndex(int index)
		{
			Row = 0;
			Column = index;
			Index = index;
		}
		
		public TokenIndex(TokenIndex index)
		{
			Row = index.Row;
			Column = index.Column;
			Index = index.Index;
		}
		
		public bool Equals(TokenIndex other)
		{
			return Row == other.Row && Column == other.Column && Index == other.Index;
		}

		public bool Equals((int, int, int) tokenIndex)
		{
			return Index == tokenIndex.Item1 && Row == tokenIndex.Item2 && Column == tokenIndex.Item3;
		}

		public bool Equals(int index)
		{
			return Row == 0 && Column == index && Index == index;
		}

        

		public override int GetHashCode()
		{
			return HashCode.Combine(Row, Column, Index);
		}

		public override string ToString()
		{
			return $"[Row = {Row}; Column = {Column}; Index = {Index}]";
		}
	}
}