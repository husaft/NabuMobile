using System.IO;

namespace Nabu.Services
{
	public interface IFileLoader
	{
		Stream LoadFile(string name);
	}
}