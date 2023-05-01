using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
    public class BackgroundGrid : Grid
	{
		public static readonly DependencyProperty ShowProperty = DependencyPropertyEx.Register<bool, BackgroundGrid>(OnShowChanged);
		public static readonly DependencyProperty GridSizeProperty = DependencyPropertyEx.Register<int, BackgroundGrid>(OnGridSizeChanged, GridSizeCoerce, 5);
		public static readonly DependencyProperty BackgroundColorProperty = DependencyPropertyEx.Register<Color, BackgroundGrid>(OnBackgroundColorChanged, Color.FromArgb(0, 0, 0, 0));

		private Brush _gridBrush;

        public bool Show { get => (bool) GetValue(ShowProperty); set => SetValue(ShowProperty, value); }

        public int GridSize { get => (int) GetValue(GridSizeProperty); set => SetValue(GridSizeProperty, value); }

		public Color BackgroundColor { get => (Color) GetValue(BackgroundColorProperty); set => SetValue(BackgroundColorProperty, value); }

		public BackgroundGrid()
        {
			UpdateGridSize(GridSize);
        }

		private static void OnShowChanged(BackgroundGrid d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateShowGrid((bool) e.NewValue);
		}

		private static object GridSizeCoerce(BackgroundGrid d, int gridSize)
		{
			return Math.Max(1, gridSize);
		}

		private static void OnGridSizeChanged(BackgroundGrid d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateGridSize((int) e.NewValue);
		}

		private static void OnBackgroundColorChanged(BackgroundGrid d, DependencyPropertyChangedEventArgs e)
		{
			if (!d.Show)
			{
				d.Background = new SolidColorBrush((Color) e.NewValue);
			}
		}

		private void UpdateGridSize(int gridSize)
        {
            var scaleCoefficient = 5;
            var size = Math.Max(1, gridSize) * scaleCoefficient;

            var bitmap = new System.Drawing.Bitmap(size * 2, size * 2);
            bitmap.MakeTransparent();

            var blackPen = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(60, System.Drawing.Color.Black));

            using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(blackPen, 0, 0, size, size);
                graphics.FillRectangle(blackPen, size, size, bitmap.Width, bitmap.Height);
            }

            var source = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

            var brush = new ImageBrush(source);
            brush.TileMode = TileMode.Tile;
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.Viewport = new Rect(0, 0, bitmap.Width / scaleCoefficient, bitmap.Height / scaleCoefficient);

            _gridBrush = brush;
            UpdateShowGrid(Show);
        }

        private void UpdateShowGrid(bool showGrid)
        {
            if (showGrid)
            {
                Background = _gridBrush;
            }
            else
            {
                Background = new SolidColorBrush(BackgroundColor);
            }
        }
    }
}
