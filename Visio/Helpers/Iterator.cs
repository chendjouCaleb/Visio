using System.Collections.Generic;

namespace Visio.Helpers
{
	public class Iterator<T>
	{
		private readonly IList<T> _items;

		public Iterator(IList<T> items)
		{
			_items = items;
		}

		public int Index { get; private set; }
		public int Count => _items.Count;
		public bool Has => Index < _items.Count;

		public T Current => _items[Index];

		public void Next()
		{
			Index++;
		}
	}
}