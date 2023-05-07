using System;
using System.Drawing;
using System.Numerics;

namespace BuildingMap.Core
{
	public class MapItem : ICloneable
    {
		public int Id { get; set; }

		public Vector2 Position { get; set; }

		public Vector2 Size { get; set; }

		public Color Color { get; set; }

		public Color SelectedColor { get; set; }

		public double Radius { get; set; }

		public string Text { get; set; }

		public int FontSize { get; set; }

		public Color ForegroundColor { get; set; }

		public RotationAngle RotationAngle { get; set; }

		public string ImageId { get; set; }

		public double ImageScale { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
