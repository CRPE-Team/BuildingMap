using System.Collections.Generic;
using System.Threading;

namespace BuildingMap.Core
{
	public class Map
	{
		private int _itemIdIncrementor;

		public string Name { get; set; }

		public int ItemIdIncrementor { get => _itemIdIncrementor; set => _itemIdIncrementor = value; }

		public List<Floor> Floors { get; set; } = new List<Floor>();

		public int GetNextItemId()
		{
			return Interlocked.Increment(ref _itemIdIncrementor);
		}
	}
}
