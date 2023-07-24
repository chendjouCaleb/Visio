using System;
using System.Runtime.Serialization;

namespace Visio.Exceptions
{
	public class UnexpectedTokenException:ApplicationException
	{
		public UnexpectedTokenException()
		{
		}

		protected UnexpectedTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public UnexpectedTokenException(string? message) : base(message)
		{
		}

		public UnexpectedTokenException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		public static void AssertExpected(char expected, char current)
		{
			if (expected != current)
			{
				string message = $"Unexpected token: expected: '{expected}', current: '{current}'.";
				throw new UnexpectedTokenException(message);
			}
			
		}
	}
}