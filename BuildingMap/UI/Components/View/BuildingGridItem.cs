using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core.Utils;

namespace BuildingMap.UI.Components.View
{
    public class BuildingGridItem : Grid, IBuildingGridItem, ICopyable
    {
        private Point _position;

        private BuildingGrid _grid;

        private readonly TransformGroup _transformGroup;
        private readonly TranslateTransform _translateTransform;

        protected BuildingGrid Grid => _grid;
        protected TransformGroup TransformGroup => _transformGroup;

        public BuildingGridItem()
        {
            RenderTransform = _transformGroup = new TransformGroup();

            _transformGroup.Children.Add(_translateTransform = new TranslateTransform());
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            _grid = Parent as BuildingGrid;

            base.OnVisualParentChanged(oldParent);
        }

        public virtual void Update()
        {
            UpdatePosition();
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

        private void UpdatePosition()
        {
            var position = (_position.ToVector() + (_grid.RenderOffset / 2).FloorInt() * 2) * _grid.Grid.GridSize;
            UpdatePosition((position / _grid.Grid.GridSize).Round() * _grid.Grid.GridSize);
        }

        protected virtual void UpdatePosition(Vector position)
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
