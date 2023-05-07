using System.Collections.Generic;

namespace BuildingMap.Core
{
	public class Floor
	{
		public int Number { get; set; }

		public Dictionary<int, MapItem> MapItems { get; set; } = new Dictionary<int, MapItem>();

		public ImageInfo ImageInfo { get; set; }
	}
}
