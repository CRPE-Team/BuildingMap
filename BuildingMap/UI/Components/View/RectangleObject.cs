using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BuildingMap.UI.Components.View.Core.Utils;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
    public partial class RectangleObject : BuildingGridItem
	{
		public static readonly DependencyProperty SizeProperty = DependencyPropertyEx.Register<Size, RectangleObject>(OnSizeChanged);

		private static readonly Brush UnselectedBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xD3, 0xEA));

        private const int AuraSize = 4;

        private readonly Rectangle _rectangle;

		[Bindable(true)]
		public Size Size { get => (Size) GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

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
            UpdateSize(Size);
        }

        protected override void UpdateOffset(Vector position)
        {
            base.UpdateOffset(position - new Vector(AuraSize, AuraSize));
        }

		private static void OnSizeChanged(RectangleObject d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateSize((Size) e.NewValue);
		}

		private void UpdateSize(Size size)
        {
            var sizeVector = (size.ToVector() + new Vector(0.001, 0.001)).Round() * Grid.GridSize;

            Width = (_rectangle.Width = sizeVector.X) + AuraSize * 2;
            Height = (_rectangle.Height = sizeVector.Y) + AuraSize * 2;
        }
    }
}
