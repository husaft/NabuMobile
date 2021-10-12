using System.Collections.Generic;
using System.Threading.Tasks;
using Nabu.Helpers;
using Nabu.Models;
using Newtonsoft.Json;

namespace Nabu.Services
{
	public class JsonDataStore : IDataStore2<VocConfig>
	{
		private readonly EmbeddedLoader _loader;
		private readonly JsonConfigReader _reader;

		public JsonDataStore()
		{
			_loader = new EmbeddedLoader();
			_reader = new JsonConfigReader();
		}

		public async Task<IEnumerable<VocConfig>> GetItemsAsync(bool forceRefresh = false)
		{
			var vocJson = _loader.LoadTextFile("voc.json");
			var vocCfg = _reader.ReadConfig(vocJson);
			return await Task.FromResult(new[] { vocCfg });
		}

		internal void LoadWords(VocConfig vocCfg) => LoadWords(vocCfg.Vocabulary);

		internal void LoadWords(Vocabulary voc)
		{
			var wordsFile = voc.Src.Src;
			var wordsJson = _loader.LoadTextFile(wordsFile);
			var wordsList = JsonConvert.DeserializeObject<string[][]>(wordsJson);
			voc.Src.Words = wordsList;
		}
	}
}