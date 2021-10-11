using System.IO;
using System.Text;
using Nabu.Services;

namespace Nabu.Helpers
{
	public static class LoaderHelper
	{
		public static string LoadTextFile(this IFileLoader loader, string name)
		{
			using (var input = loader.LoadFile(name))
			{
				using (var reader = new StreamReader(input, Encoding.UTF8))
				{
					var text = reader.ReadToEnd();
					return text;
				}
			}
		}
	}
}