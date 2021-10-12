using System;
using System.Linq;
using Nabu.Models;
using Newtonsoft.Json;

namespace Nabu.Services
{
	public class JsonConfigReader : IConfigReader<string>
	{
		public VocConfig ReadConfig(string json)
		{
			var vocConfig = JsonConvert.DeserializeObject<VocConfig>(json);
			if (vocConfig == null)
			{
				throw new InvalidOperationException("Could not load voc config!");
			}
			var voc = vocConfig.Vocabulary;
			voc.Src.Labels = voc.Src.Label.Split(',');
			var map = voc.Mapping.Col;
			var desc = map.ToDictionary(k => k.Id, v => v.Name);
			foreach (var mode in voc.Modes)
			{
				var longName = mode.Short
					.Replace("=?", " -> ")
					.Replace("=", ", ");
				foreach (var pair in desc)
					longName = longName.Replace(pair.Key, pair.Value);
				mode.Long = longName;
			}
			return vocConfig;
		}
	}
}