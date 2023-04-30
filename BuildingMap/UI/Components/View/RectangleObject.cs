using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using BuildingMap.UI.Components.View.Core.Utils;

namespace BuildingMap.UI.Components.View
{
    public partial class RectangleObject : BuildingGridItem
    {
        private static readonly Brush UnselectedBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xD3, 0xEA));

        private const int AuraSize = 4;

        private readonly Rectangle _rectangle;

        private Size _size;

        public RectangleObject()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            Children.Add(_rectangle = new Rectangle());
            _rectangle.Fill = UnselectedBrush;
            _rectangle.Margin = new Thickness(AuraSize);

            //debug
            _rectangle.RadiusX = 10;
            _rectangle.RadiusY = 10;
        }

        public override void Update()
        {
            base.Update();
            UpdateSize();
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

        public override ICopyable CreateCopy()
        {
            var copy = (RectangleObject)base.CreateCopy();
            copy._size = _size;
            return copy;
        }

        protected override void UpdatePosition(Vector position)
        {
            base.UpdatePosition(position - new Vector(AuraSize, AuraSize));
        }

        private void UpdateSize()
        {
            var size = (_size.ToVector() + new Vector(0.001, 0.001)).Round() * Grid.Grid.GridSize;

            Width = (_rectangle.Width = size.X) + AuraSize * 2;
            Height = (_rectangle.Height = size.Y) + AuraSize * 2;
        }
    }
}
