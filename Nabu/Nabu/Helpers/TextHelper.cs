using System;

namespace Nabu.Helpers
{
	public static class TextHelper
	{
		public static StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

		public static StringComparison InvIgnoreCase = StringComparison.InvariantCultureIgnoreCase;

		public static bool Contains(this string source, string find, StringComparison comp)
		{
			return source?.IndexOf(find, comp) >= 0;
		}
	}
}