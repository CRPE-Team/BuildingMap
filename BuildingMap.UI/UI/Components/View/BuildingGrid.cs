using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core;
using BuildingMap.UI.Components.View.Core.Utils;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
	public partial class BuildingGrid : ObservableGrid, IDraggable
    {
		public static readonly DependencyProperty AllowEditProperty = DependencyPropertyEx.Register<bool, BuildingGrid>();
		public static readonly DependencyProperty OffsetProperty = DependencyPropertyEx.Register<Vector, BuildingGrid>(OnOffsetChanged, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
		public static readonly DependencyProperty ZoomProperty = DependencyPropertyEx.Register<double, BuildingGrid>(OnZoomChanged, ZoomCoerce, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 1);
		public static readonly DependencyProperty MousePositionProperty = DependencyPropertyEx.Register<Vector, BuildingGrid>(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
		public static readonly RoutedEvent StartDrawEvent = EventManagerEx.Register<EventHandler<StartDrawEventArgs>, BuildingGrid>();
		public static readonly RoutedEvent StopDrawEvent = EventManagerEx.Register<RoutedEventHandler, BuildingGrid>();

		private static readonly DragContext DragContext = new DragContext();
        private const int GridReserve = 200;

        private Vector _offsetCenterFix;
        private Vector _shift;

        private readonly ScaleTransform _scaleTransform;
        private readonly TranslateTransform _translateTransform;

        private BackgroundGrid _gridBackground;
        private BackgroundImage _imageBackground;
		private IDrawable _drawable;

        public bool CanDrag => Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed;

		[Bindable(true)]
		public bool AllowEdit { get => (bool) GetValue(AllowEditProperty); set => SetValue(AllowEditProperty, value); }

		[Bindable(true)]
		public double Zoom { get => (double) GetValue(ZoomProperty); set => SetValue(ZoomProperty, value); }

		public Vector RenderOffset { get => Offset + _offsetCenterFix; set => Offset = value - _offsetCenterFix; }

		[Bindable(true)]
		public Vector Offset { get => (Vector) GetValue(OffsetProperty); set => SetValue(OffsetProperty, value); }

		[Bindable(true)]
		public Vector MousePosition { get => (Vector) GetValue(MousePositionProperty); set => SetValue(MousePositionProperty, value); }

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

		public event EventHandler<StartDrawEventArgs> StartDraw { add { AddHandler(StartDrawEvent, value); } remove { RemoveHandler(StartDrawEvent, value); } }
		public event RoutedEventHandler StopDraw { add { AddHandler(StopDrawEvent, value); } remove { RemoveHandler(StopDrawEvent, value); } }

		public BuildingGrid()
        {
			Margin = new Thickness(-GridReserve);

            RenderTransformOrigin = new Point(0.5, 0.5);

            var transformGroup = new TransformGroup();
            RenderTransform = transformGroup;

            transformGroup.Children.Add(_scaleTransform = new ScaleTransform());
            transformGroup.Children.Add(_translateTransform = new TranslateTransform());
		}

        public DragContext StartDrag()
        {
            if (AllowEdit)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					var args = new StartDrawEventArgs(StartDrawEvent);
					RaiseEvent(args);

					_drawable = Children.OfType<FrameworkElement>().FirstOrDefault(obj => obj.DataContext == args.CreatedObject) as IDrawable;
					_drawable?.StartDraw();
                }
            }

            return DragContext;
        }

        public void StopDrag()
		{
			if (_drawable != null)
			{
				RaiseEvent(new RoutedEventArgs(StopDrawEvent));
				_drawable.EndDraw();
				_drawable = null;
			}	
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

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			MousePosition = Mouse.GetPosition(this).ToVector() / GridSize + _shift - RenderOffset;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			_offsetCenterFix = new Vector(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height) / GridSize / 2;
			Shift();

			base.OnRenderSizeChanged(sizeInfo);
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
