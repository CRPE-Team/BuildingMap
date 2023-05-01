using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core.Utils;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
    public class BuildingGridItem : Grid, IBuildingGridItem, ICopyable
	{
		public static readonly DependencyProperty PositionProperty = DependencyPropertyEx.Register<Point, BuildingGridItem>(OnPositionChanged, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);

		private Point _position;

        private BuildingGrid _grid;

        private readonly TransformGroup _transformGroup;
        private readonly TranslateTransform _translateTransform;

        protected BuildingGrid Grid => _grid;
        protected TransformGroup TransformGroup => _transformGroup;

		public Point Position { get => (Point) GetValue(PositionProperty); set => SetValue(PositionProperty, value); }

		public BuildingGridItem()
        {
            RenderTransform = _transformGroup = new TransformGroup();

            _transformGroup.Children.Add(_translateTransform = new TranslateTransform());
		}

		private static void OnPositionChanged(BuildingGridItem d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdatePosition((Point) e.NewValue);
		}

		protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            _grid = Parent as BuildingGrid;

            base.OnVisualParentChanged(oldParent);
        }

        public virtual void Update()
        {
            UpdatePosition(Position);
        }

        private void UpdatePosition(Point position)
        {
            var offset = (position.ToVector() + (_grid.RenderOffset / 2).FloorInt() * 2) * _grid.GridSize;
            UpdateOffset((offset / _grid.GridSize).Round() * _grid.GridSize);
        }

        protected virtual void UpdateOffset(Vector position)
        {
            _translateTransform.X = position.X;
            _translateTransform.Y = position.Y;
        }

        public virtual ICopyable CreateCopy()
        {
            var copy = (BuildingGridItem)Activator.CreateInstance(GetType());
            copy._position = _position;

            return copy;
        }
    }
}
