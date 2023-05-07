using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using BuildingMap.Core.Utils;

namespace BuildingMap.Core
{
	public class Map
	{
		private int _itemIdIncrementor;

		public string Name { get; set; }

		public int ItemIdIncrementor { get => _itemIdIncrementor; set => _itemIdIncrementor = value; }

		public List<Floor> Floors { get; set; } = new List<Floor>();

		public Dictionary<string, byte[]> ImagesData { get; set; } = new Dictionary<string, byte[]>();

		public Floor GetFloorByNumber(int number)
		{
			return Floors[number - 1];
		}

		public int GetNextItemId()
		{
			return Interlocked.Increment(ref _itemIdIncrementor);
		}

		public string AddNewImage(byte[] data)
		{
			var imageId = data.GetHashString();
			ImagesData.TryAdd(imageId, data);
			return imageId;
		}

		public void RemoveImage(string imageId) 
		{ 
			if (Floors.Any(floor => floor.MapItems.Values.Any(item => item.ImageId == imageId))) return;

			ImagesData.Remove(imageId);
		}
	}
}
