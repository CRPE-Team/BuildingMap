using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BuildingMap.UI.Components
{
    public partial class BuildingGrid : Grid, IDraggable
    {
        private static readonly DragContext DragContext = new DragContext();
        private const int GridReserve = 2000;

        private bool _allowEdit = false;

        private Vector _offset;
        private Vector _offsetCenterFix;
        private Vector _shift;

        private readonly ScaleTransform _scaleTransform;
        private readonly TranslateTransform _translateTransform;

        private GridBackground _gridBackground;
        private ImageBackground _imageBackground;

        private IDrawable _drawable;

        public bool CanDrag => true;

        public bool AllowEdit { get => _allowEdit; set => SetAllowEdit(value); }

        public double Zoom { get => _scaleTransform.ScaleX; set => _scaleTransform.ScaleX = _scaleTransform.ScaleY = value; }

        public Vector RenderOffset => _offset;

        public Vector Offset { get => _offset - _offsetCenterFix; set => _offset = value + _offsetCenterFix; }

        public Vector MousePosition => Mouse.GetPosition(this).ToVector() / Grid.GridSize + _shift - RenderOffset;

        public double RenderGridSize => Grid.GridSize * Zoom;


        public GridBackground Grid 
        { 
            get => _gridBackground; 
            set
            {
                if (_gridBackground != null) Children.Remove(_gridBackground);
                Children.Add(_gridBackground = value);
                SetZIndex(_gridBackground, -5);
            }
        }

        public ImageBackground BackgroundImage
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
            Margin = new Thickness(Margin.Left - GridReserve, Margin.Top - GridReserve, Margin.Right - GridReserve, Margin.Bottom - GridReserve);

            RenderTransformOrigin = new Point(0.5, 0.5);

            var transformGroup = new TransformGroup();
            RenderTransform = transformGroup;

            transformGroup.Children.Add(_scaleTransform = new ScaleTransform());
            transformGroup.Children.Add(_translateTransform = new TranslateTransform());

            Grid = new GridBackground();

            //debug
            Application.Current.MainWindow.KeyDown += OnKeyDown;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _offset -= _offsetCenterFix;
            _offset += _offsetCenterFix = new Vector(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height) / Grid.GridSize / 2;
            Shift();

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void SetAllowEdit(bool allowEdit)
        {
            _allowEdit = allowEdit;

            Grid.ShowGrid = _allowEdit;
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
                _imageBackground.Move(offset);
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
                Grid.ShowGrid = !Grid.ShowGrid;
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
            else if (e.Key == Key.V && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                var copy = ICopyable.GetCopy() as UIElement;
                if (copy != null) Children.Add(copy);
                if (copy is BuildingGridItem buildingGridItem)
                {
                    buildingGridItem.Position = MousePosition.ToPoint();
                    buildingGridItem.Update();
                }
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var scaleChange = 1 + e.Delta / 1700d;

            if (AllowEdit && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                _imageBackground.Scale *= scaleChange;
                _imageBackground.Update();
                return;
            }

            UpdateScale(scaleChange);
        }

        private void UpdateScale(double scaleChange)
        {
            var mouseFromCenterPos = Mouse.GetPosition(this).ToVector() / Grid.GridSize + _shift - _offsetCenterFix;

            Zoom = Math.Max(0.4, Math.Min(10, Zoom * scaleChange));

            UpdateShift();

            var newMouseFromCenterPos = Mouse.GetPosition(this).ToVector() / Grid.GridSize + _shift - _offsetCenterFix;
            _offset -= mouseFromCenterPos - newMouseFromCenterPos;

            Shift();
        }

        private void Shift(Vector offset = default)
        {
            _offset += offset / RenderGridSize;

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

            _shift = new Vector(_offset.X % 2, _offset.Y % 2);

            _translateTransform.X = _shift.X * gridSize;
            _translateTransform.Y = _shift.Y * gridSize;
        }
    }
}
