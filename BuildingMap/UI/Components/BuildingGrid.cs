using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuildingMap.UI.Components
{
    public partial class BuildingGrid : Grid, IDraggable
    {
        private static readonly DragContext DragContext = new DragContext();
        private const int GridReserve = 600;

        private bool _showGrid = false;
        private int _gridSize;

        private readonly ScaleTransform _scaleTransform;
        private readonly TranslateTransform _translateTransform;

        private Brush _gridBrush;
        private readonly Thickness _originMargin;

        public bool ShowGrid { get => _showGrid; set => SetShowGrid(value); }

        public int GridSize { get => _gridSize; set => SetGridSize(value); }

        public BuildingGrid()
        {
            _originMargin = Margin;// = new Thickness(Margin.Left - GridReserve, Margin.Top - GridReserve, Margin.Right - GridReserve, Margin.Bottom - GridReserve);

            RenderTransformOrigin = new Point(0.5, 0.5);

            _scaleTransform = new ScaleTransform();
            _translateTransform = new TranslateTransform();

            var transformGroup = new TransformGroup();
            RenderTransform = transformGroup;

            transformGroup.Children.Add(_scaleTransform);
            transformGroup.Children.Add(_translateTransform);

            SetGridSize(20);

            Focusable = true;
        }

        private void SetGridSize(int gridSize)
        {
            _gridSize = gridSize;

            var scaleCoefficient = 5;
            var size = Math.Max(1, _gridSize) * scaleCoefficient;


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
            brush.ViewportUnits = BrushMappingMode.Absolute;
            brush.Viewport = new Rect(0, 0, bitmap.Width / scaleCoefficient, bitmap.Height / scaleCoefficient);

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                OnPreviewMouseWheel(new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 0));
            }
            else if (e.Key == Key.F)
            {
                Shift(new Vector());
            }
            else if (e.Key == Key.Right)
            {
                Shift(new Vector(1, 0));
            }
            else if (e.Key == Key.Left)
            {
                Shift(new Vector(-1, 0));
            }
            else if (e.Key == Key.Up)
            {
                Shift(new Vector(0, -1));
            }
            else if (e.Key == Key.Down)
            {
                Shift(new Vector(0, 1));
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var diff = 1 + e.Delta / 1700d;

            var oldScale = _scaleTransform.ScaleX;
            var newScale = Math.Max(0.4, Math.Min(10, oldScale * diff));

            _scaleTransform.ScaleX = _scaleTransform.ScaleY = newScale;

            UpdateShift();
        }

        private void Shift(Vector offset)
        {
            var gridSize = _gridSize * _scaleTransform.ScaleX;

            _offset += offset / gridSize;

            _shift = new Vector(_offset.X % 2, _offset.Y % 2);

            Move();
        }

        private void Move()
        {
            UpdateShift();

            foreach (var child in Children.OfType<VertexView>())
            {
                var pos = (child.RealPos.ToVector() + (_offset / 2).Floor() * 2) * _gridSize;

                child.Position = pos.ToPoint();
            }
        }

        private void UpdateShift()
        {
            var sectionSize = _gridSize * _scaleTransform.ScaleX;
            _translateTransform.X = _shift.X * sectionSize;
            _translateTransform.Y = _shift.Y * sectionSize;
        }
    }
}
