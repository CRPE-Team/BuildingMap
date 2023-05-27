using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using BuildingMap.Core.Utils;

namespace BuildingMap.Core
{
	public class Map
	{
		private int _itemIdIncrementor;
		private int _styleIdIncrementor;

		public string Name { get; set; }

		public bool ReadOnly { get; set; }

		public int GridSize { get; set; }

		public Vector2 Offest { get; set; }

		public double Zoom { get; set; }

		public Color BackgroundColor { get; set; }

		public int ItemIdIncrementor { get => _itemIdIncrementor; set => _itemIdIncrementor = value; }

		public List<Floor> Floors { get; set; } = new List<Floor>();

		public Dictionary<string, byte[]> ImagesData { get; set; } = new Dictionary<string, byte[]>();

		public Dictionary<int, ItemStyle> Styles { get; set; } = new Dictionary<int, ItemStyle>();

		public Floor GetFloorByNumber(int number)
		{
			return Floors[number - 1];
		}

		public int GetNextItemId()
		{
			return Interlocked.Increment(ref _itemIdIncrementor);
		}

		public int GetStyleId()
		{
			return Interlocked.Increment(ref _styleIdIncrementor);
		}

		public string AddNewImage(byte[] data)
		{
			var imageId = data.GetHashString();
			ImagesData.TryAdd(imageId, data);
			return imageId;
		}

		public bool TryRemoveItem(MapItem item)
		{
			foreach (var floor in Floors)
			{
				if (floor.MapItems.Remove(item.Id))
				{
					TryRemoveStyle(item.StyleId, false);

					return true;
				}
			}

			return false;
		}

		public bool TryRemoveStyle(ItemStyle style)
		{
			return TryRemoveStyle(style.Id, true);
		}

		public bool TryRemoveImage(string imageId)
		{ 
			if (Floors.Any(floor => Styles.Values.Any(style => style.ImageId == imageId))) return false;

			ImagesData.Remove(imageId);
			return true;
		}

		private bool TryRemoveStyle(int styleId, bool all)
		{
			if (!Styles.TryGetValue(styleId, out var style)) return false;

			if (style.Flags.HasFlag(StyleFlags.System)) return false;
			if (!all && style.Flags.HasFlag(StyleFlags.Static)) return false;

			Styles.Remove(styleId);
			TryRemoveImage(style.ImageId);
			return true;
		}
	}
}
