using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nabu.Services
{
	public interface IDataStore2<T>
	{
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
	}
}