using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BuildingMap.UI.Components
{
    public class BuildingGrid : Grid, IDraggable
    {
        private static readonly DragContext DragContext = new DragContext();
        private const double ZoomMin = 0.3;
        private const double ZoomMax = 8;
        private const int GridReserve = 400;

        private bool _showGrid = false;
        private int _gridSize;
        private double _zoom;

        private Brush _gridBrush;
        private readonly Thickness _originMargin;

        public bool ShowGrid { get => _showGrid; set => SetShowGrid(value); }

        public int GridSize { get => _gridSize; set => SetGridSize(value); }

        public double GridZoom { get => _zoom; set => SetZoom(value); }

        public BuildingGrid()
        {
            SetGridSize(20, 1);

            _originMargin = Margin = new Thickness(Margin.Left - GridReserve, Margin.Top - GridReserve, Margin.Right - GridReserve, Margin.Bottom - GridReserve);
        }

        private void SetZoom(double zoom)
        {
            var oldSize = (int)Math.Max(1, _gridSize * _zoom);

            SetGridSize(_gridSize, zoom);

            var size = (int)Math.Max(1, _gridSize * _zoom);

            //var fieldSize = new Vector(ActualWidth, ActualHeight);
            //var parts = fieldSize / oldSize;
            //var newSize = parts * size;

            //var offset = (newSize - fieldSize) / 2;
            //offset.X = (int)offset.X;
            //offset.Y = (int)-offset.Y;

            //Shift(-offset);


            var fieldSize = new Vector(ActualWidth, ActualHeight);
            var centerPoint = fieldSize / 2 - new Vector(_shift.X, - _shift.Y);
            var point = centerPoint / oldSize;
            var newCentralPoint = point * size;

            var offset = newCentralPoint - centerPoint;
            offset.X = offset.X;
            offset.Y = offset.Y;

            Shift(-offset);

            var cp = fieldSize / 2 - new Vector(_shift.X, - _shift.Y);

            var p = cp / size;
            foreach (var child in Children.OfType<VertexView>())
            {
                child.Position = new Point(size * (int)p.X, ActualHeight -size * (int)p.Y);
            }
            Debug.WriteLine($"{p}           {cp}        {new Point(size * (int)p.X, size * (int)p.Y)}    {_offset}");
        }

        private void SetGridSize(int gridSize)
        {
            SetGridSize(gridSize, _zoom);
        }

        private void SetGridSize(int gridSize, double zoom)
        {
            _gridSize = gridSize;
            _zoom = Math.Max(ZoomMin, Math.Min(ZoomMax, zoom));

            var size = (int) Math.Max(1, _gridSize * _zoom);


            var bitmap = new System.Drawing.Bitmap(size * 2, size * 2);
            bitmap.MakeTransparent();

            var blackPen = new System.Drawing.SolidBrush(System.Drawing.Color.LightGray);

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
            brush.AlignmentY = AlignmentY.Bottom;
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.Viewport = new Rect(0, 0, bitmap.Width, bitmap.Height);

            _gridBrush = brush;
            SetShowGrid(_showGrid);
        }

        private void SetShowGrid(bool showGrid)
        {
            _showGrid = showGrid;

            if (showGrid)
            {
                Background = _gridBrush;
            }
            else
            {
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
        }

        public DragContext StartDrag()
        {
            return DragContext;
        }

        public void StopDrag()
        {

        }

        private Vector _offset;
        private Vector _shift;

        public void Drag(Point position, Vector offset)
        {
            Shift(offset);
        }

        //protected override void OnMouseWheel(MouseWheelEventArgs e)
        //{
        //    var offset = e.Delta / 1000d;
        //    GridZoom += offset;
        //}

        private void Shift(Vector offset)
        {
            var sectionSize = (int)(_gridSize * _zoom) * 2;

            _offset += offset;

            var oldShift = _shift;
            _shift = new Vector(_offset.X % sectionSize, _offset.Y % sectionSize);

            Move(oldShift, offset);
        }

        private void Move(Vector oldShift, Vector realOffset)
        {
            Margin = Move(_originMargin, _shift);

            foreach (var child in Children.OfType<VertexView>())
            {
                //child.Position += oldShift - _shift + realOffset;
            }
        }

        private Thickness Move(Thickness margin, Vector offset)
        {
            return new Thickness(margin.Left + offset.X, margin.Top + offset.Y, margin.Right - offset.X, margin.Bottom - offset.Y);
        }

        private Vector GetSize()
        {
            return new Vector(Margin.Right - Margin.Left, Margin.Bottom - Margin.Top);
        }
    }
}
