using Nabu.Models;

namespace Nabu.Services
{
	public interface IConfigReader<in T>
	{
		VocConfig ReadConfig(T input);
	}
}