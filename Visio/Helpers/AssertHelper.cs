using System;

namespace Visio.Helpers
{
	public static class AssertHelper
	{
		public static void True(bool value)
		{
			if (!value)
			{
				throw new ArgumentException("Should be true.");
			}
		}
	}
}