using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BuildingMap.UI.Components
{
    public partial class RectangleObject : Grid, IBuildingGridItem
    {
        private BuildingGrid _grid;

        private readonly Rectangle _rectangle;
        private readonly TranslateTransform _translateTransform;

        private Point _position;
        private Size _size;

        private static readonly Brush UnselectedColor = new SolidColorBrush(Color.FromRgb(0x00, 0xD3, 0xEA));

        public RectangleObject()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            Children.Add(_rectangle = new Rectangle());
            _rectangle.Fill = UnselectedColor;

            RenderTransform = _translateTransform = new TranslateTransform();

            _rectangle.RadiusX = 10;
            _rectangle.RadiusY = 10;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            _grid = Parent as BuildingGrid;

            base.OnVisualParentChanged(oldParent);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        public void Update()
        {
            UpdatePosition();
            UpdateSize();
        }

        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdatePosition();
            }
        }

        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                UpdateSize();
            }
        }

        private void UpdatePosition()
        {
            var position = (_position.ToVector() + (_grid.Offset / 2).FloorInt() * 2) * _grid.GridSize;
            position = (position / _grid.GridSize).Floor() * _grid.GridSize;

            _translateTransform.X = position.X;
            _translateTransform.Y = position.Y;
        }

        private void UpdateSize()
        {
            var size = _size.ToVector().Ceiling() * _grid.GridSize;

            _rectangle.Width = Width = size.X;
            _rectangle.Height = Height = size.Y;
        }
    }
}
