namespace BuildingMap.UI.Visual
{
	public static class ColorExtensions
	{
		public static System.Windows.Media.Color ToWindows(this System.Drawing.Color color) 
		{ 
			return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static System.Drawing.Color ToDrawing(this System.Windows.Media.Color color)
		{
			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
