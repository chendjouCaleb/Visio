using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visio.Model
{
	public class Attributes
	{
		private readonly Dictionary<string, string> _items = new ();
		
		
		public void Add(string key, string value)
		{
			
			_items.Add(key, value);
		}
		
		public bool Remove(string key)
		{

			if (!ContainsKey(key))
			{
				return false;
			}
			return _items.Remove(key);
		}
		
		
		

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _items.GetEnumerator();
		
		public int Count => _items.Count;

		public bool ContainsKey(string key) => _items.ContainsKey(key);
		
		public string this[string key]
		{
			get => _items[key];
			set => Add(key, value);
		}

		public ICollection<string> Keys => _items.Keys.ToList();
		public ICollection<string> Values => _items.Values.ToList();

		public override string ToString()
		{
			if (Count == 0)
				return "";
			var pairs = new List<string>();
			foreach (var kvp in _items)
			{
				var builder = new StringBuilder(kvp.Key);
				if (!string.IsNullOrWhiteSpace(kvp.Value))
				{
					builder.Append($"='{kvp.Value}'");
				}

				pairs.Add(builder.ToString());
			}
			
			return $"[{String.Join(' ', pairs)}]";
		}
	}
}