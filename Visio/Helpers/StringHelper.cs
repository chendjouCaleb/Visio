using System.Collections.Generic;

namespace Visio.Helpers
{
	public static class StringHelper
	{
		public static string WhiteSpaceChars = "\t\r\n\v\f ";
		public static List<char> WhiteSpaceCharList = new () { '\t', '\r', '\n', '\v', '\f', ' '};
	}
}