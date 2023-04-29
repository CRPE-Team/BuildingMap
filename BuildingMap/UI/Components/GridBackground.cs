using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BuildingMap.UI.Components
{
    public class GridBackground : Grid
    {
        private bool _showGrid = false;
        private int _gridSize;

        private Brush _gridBrush;

        public bool ShowGrid { get => _showGrid; set => SetShowGrid(value); }

        public int GridSize { get => _gridSize; set => SetGridSize(value); }

        public GridBackground()
        {
            SetGridSize(50);
        }

        private void SetGridSize(int gridSize)
        {
            _gridSize = gridSize;

            var scaleCoefficient = 5;
            var size = Math.Max(1, GridSize) * scaleCoefficient;


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
            SetShowGrid(_showGrid);
        }

        private void SetShowGrid(bool showGrid)
        {
            _showGrid = showGrid;

            if (ShowGrid)
            {
                Background = _gridBrush;
            }
            else
            {
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
        }
    }
}
