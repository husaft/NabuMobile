using System;

namespace Nabu.Helpers
{
	public static class TextHelper
	{
		public static StringComparison InvIgnoreCase = StringComparison.InvariantCultureIgnoreCase;

		public static bool Contains(this string source, string find, StringComparison comp)
		{
			return source?.IndexOf(find, comp) >= 0;
		}

		public static string ToASCII(this string text)
		{
			var copy = text.ToCharArray();
			for (var i = 0; i < copy.Length; i++)
			{
				var letter = copy[i];
				switch (letter)
				{
					case 'ā':
						letter = 'a';
						break;
					case 'č':
						letter = 'c';
						break;
					case 'ǧ':
						letter = 'g';
						break;
					case 'ḫ':
						letter = 'h';
						break;
					case 'š':
						letter = 's';
						break;
					case 'ž':
						letter = 'z';
						break;
					default:
						continue;
				}
				copy[i] = letter;
			}
			return new string(copy);
		}
	}
}