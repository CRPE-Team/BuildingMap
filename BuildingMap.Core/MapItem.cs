using System;
using System.Numerics;

namespace BuildingMap.Core
{
	public class MapItem : ICloneable
    {
		public int Id { get; set; }

		public Vector2 Position { get; set; }

		public Vector2 Size { get; set; }

		public string Text { get; set; }

		public RotationAngle RotationAngle { get; set; }

		public int StyleId { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
