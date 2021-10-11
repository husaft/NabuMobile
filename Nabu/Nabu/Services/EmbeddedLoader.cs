using System.IO;
using System.Reflection;

namespace Nabu.Services
{
	public class EmbeddedLoader : IFileLoader
	{
		private const string Prefix = nameof(Nabu) + ".Resources.";

		private readonly Assembly _ass;

		public EmbeddedLoader()
		{
			_ass = typeof(EmbeddedLoader).Assembly;
		}

		public Stream LoadFile(string name)
		{
			var res = _ass.GetManifestResourceStream(Prefix + name);
			return res;
		}
	}
}