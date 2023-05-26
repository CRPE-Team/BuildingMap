using System;
using System.Drawing;

namespace BuildingMap.Core
{
	public class ItemStyle
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Color Color { get; set; }

		public Color SelectedColor { get; set; }

		public double Radius { get; set; }

		public Color ForegroundColor { get; set; }

		public int FontSize { get; set; }

		public string ImageId { get; set; }

		public double ImageScale { get; set; }

		public StyleFlags Flags { get; set; }
	}
}
