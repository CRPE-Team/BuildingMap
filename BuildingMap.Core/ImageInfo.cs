using System.Numerics;

namespace BuildingMap.Core
{
	public class ImageInfo
	{
		public byte[] Data { get; set; }

		public double Scale { get; set; }

		public Vector2 Position { get; set; }

		public bool Show { get; set; }
	}
}
