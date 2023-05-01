using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core.Utils;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
	public partial class BuildingGrid : Grid, IDraggable
    {
		public static readonly DependencyProperty AllowEditProperty = DependencyPropertyEx.Register<bool, BuildingGrid>();
		public static readonly DependencyProperty OffsetProperty = DependencyPropertyEx.Register<Vector, BuildingGrid>(OnOffsetChanged, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
		public static readonly DependencyProperty ZoomProperty = DependencyPropertyEx.Register<double, BuildingGrid>(OnZoomChanged, ZoomCoerce, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 1);

		private static readonly DragContext DragContext = new DragContext();
        private const int GridReserve = 200;

        private Vector _offsetCenterFix;
        private Vector _shift;

        private readonly ScaleTransform _scaleTransform;
        private readonly TranslateTransform _translateTransform;

        private BackgroundGrid _gridBackground;
        private BackgroundImage _imageBackground;

        private IDrawable _drawable;

        public bool CanDrag => true;

		[Bindable(true)]
		public bool AllowEdit { get => (bool) GetValue(AllowEditProperty); set => SetValue(AllowEditProperty, value); }

		[Bindable(true)]
		public double Zoom { get => (double) GetValue(ZoomProperty); set => SetValue(ZoomProperty, value); }

		public Vector RenderOffset { get => Offset + _offsetCenterFix; set => Offset = value - _offsetCenterFix; }

		[Bindable(true)]
		public Vector Offset  { get => (Vector) GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }

		public Vector MousePosition => Mouse.GetPosition(this).ToVector() / GridSize + _shift - RenderOffset;

        public double RenderGridSize => GridSize * Zoom;

		public int GridSize => Grid?.GridSize ?? 1;

		public BackgroundGrid Grid
        {
            get => _gridBackground;
            set
            {
                if (_gridBackground != null) Children.Remove(_gridBackground);
                Children.Add(_gridBackground = value);
                SetZIndex(_gridBackground, -5);
            }
        }

        public BackgroundImage BackgroundImage
        {
            get => _imageBackground;
            set
            {
                if (_imageBackground != null) Children.Remove(_imageBackground);
                Children.Add(_imageBackground = value);
                SetZIndex(_imageBackground, -10);
            }
        }

		public BuildingGrid()
        {
			Margin = new Thickness(-GridReserve);

            RenderTransformOrigin = new Point(0.5, 0.5);

            var transformGroup = new TransformGroup();
            RenderTransform = transformGroup;

            transformGroup.Children.Add(_scaleTransform = new ScaleTransform());
            transformGroup.Children.Add(_translateTransform = new TranslateTransform());

			//debug
			if (Application.Current?.MainWindow != null) Application.Current.MainWindow.KeyDown += OnKeyDown;
		}

		private static void OnOffsetChanged(BuildingGrid d, DependencyPropertyChangedEventArgs e)
		{
			d.Shift();
		}

		private static object ZoomCoerce(BuildingGrid d, double scale)
		{
			return Math.Max(0.2, Math.Min(10, scale));
		}

		private static void OnZoomChanged(BuildingGrid d, DependencyPropertyChangedEventArgs e)
		{
			var mouseFromCenterPos = Mouse.GetPosition(d).ToVector() / d.GridSize + d._shift - d._offsetCenterFix;

			var scale = d._scaleTransform.ScaleX = d._scaleTransform.ScaleY = (double) e.NewValue;

			d.Margin = new Thickness(-GridReserve / scale * 4);
			d._offsetCenterFix = new Vector(d.ActualWidth, d.ActualHeight) / d.GridSize / 2;

			d.UpdateShift();

			var newMouseFromCenterPos = Mouse.GetPosition(d).ToVector() / d.GridSize + d._shift - d._offsetCenterFix;
			d.RenderOffset -= mouseFromCenterPos - newMouseFromCenterPos;

			d.Shift();
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
			_offsetCenterFix = new Vector(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height) / GridSize / 2;
            Shift();

            base.OnRenderSizeChanged(sizeInfo);
        }

        public DragContext StartDrag()
        {
            if (AllowEdit)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    _drawable = new RectangleObject();
                    Children.Add(_drawable as UIElement);

                    _drawable.StartDraw();
                }
            }

            return DragContext;
        }

        public void StopDrag()
        {
            if (!_drawable?.EndDraw() ?? false)
            {
                Children.Remove(_drawable as UIElement);
            }

            _drawable = null;
        }

        public void Drag(Point position, Vector offset)
        {
            if (AllowEdit
                && Mouse.RightButton == MouseButtonState.Pressed
                && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                _imageBackground?.Move(offset);
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed || !AllowEdit)
            {
                Shift(offset);
            }
            else if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _drawable?.Draw();
            }
        }

        //debug
        protected void OnKeyDown(object source, KeyEventArgs e)
        {
            if (e.Key == Key.Q)
            {
                OnPreviewMouseWheel(new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 0));
            }
            else if (e.Key == Key.F)
            {
                Shift(new Vector());
            }
            else if (e.Key == Key.G)
            {
                Grid.Show = !Grid.Show;
            }
            else if (e.Key == Key.E)
            {
                AllowEdit = !AllowEdit;
            }
            else if (e.Key == Key.Right)
            {
                Shift(new Vector(5, 0));
            }
            else if (e.Key == Key.Left)
            {
                Shift(new Vector(-5, 0));
            }
            else if (e.Key == Key.Up)
            {
                Shift(new Vector(0, -5));
            }
            else if (e.Key == Key.Down)
            {
                Shift(new Vector(0, 5));
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var scaleChange = 1 + e.Delta / 1700d;

			if (_imageBackground != null && AllowEdit && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                _imageBackground.Scale *= scaleChange;
                return;
            }

			Zoom *= scaleChange;
		}

        private void Shift(Vector offset = default)
        {
			RenderOffset += offset / RenderGridSize;

            UpdateShift();
            UpdateChildren();
        }

        private void UpdateChildren()
        {
            foreach (var child in Children.OfType<IBuildingGridItem>())
            {
                child.Update();
            }
        }

        private void UpdateShift()
        {
            var gridSize = RenderGridSize;
			var renderOffset = RenderOffset;

			_shift = new Vector(renderOffset.X % 2, renderOffset.Y % 2);

            _translateTransform.X = _shift.X * gridSize;
            _translateTransform.Y = _shift.Y * gridSize;
        }
    }
}
