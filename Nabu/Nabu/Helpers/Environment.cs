using System.Collections.Generic;
using System.Linq;
using Nabu.Models;
using Nabu.Services;

namespace Nabu.Helpers
{
	internal static class Environment
	{
		private static Dictionary<string, object> _listeners;
		private static Unit _unit;

		static Environment()
		{
			_listeners = new Dictionary<string, object>();
		}

		public static void UpdateUnit(object sender, Unit unit)
		{
			var key = GetKey(sender);
			_listeners[key] = sender;
			_unit = unit;
			foreach (var model in _listeners.Values.OfType<IUpdateable>().ToArray())
				model.Update(_unit);
		}

		public static Unit GetIfSet(object sender)
		{
			var key = GetKey(sender);
			_listeners[key] = sender;
			return _unit;
		}

		private static string GetKey(object sender)
			=> sender.GetType().Name;
	}
}