using System;
using System.Windows;
using System.Windows.Input;
using BuildingMap.UI.Components.View.Core;
using BuildingMap.UI.Components.View.Core.Utils;

namespace BuildingMap.UI.Components.View
{
    public partial class RectangleObject : IDraggable
    {
        private static readonly DragContext DragContext = new DragContext();

        private ResizeDirection? _reseDirection;
        private Vector _resizeFirst;
        private Vector _resizeSecond;

        private Cursor _defaultCursor;

        public bool CanDrag => Grid.AllowEdit && Mouse.LeftButton == MouseButtonState.Pressed;

        public DragContext StartDrag()
        {
            if (IsMouseNearCorner(out var direction))
            {
                _reseDirection = direction;

                _defaultCursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = direction.ToCursor() ?? _defaultCursor;

                _resizeFirst = Position.ToVector();
                _resizeSecond = _resizeFirst + Size.ToVector();
            }

            return DragContext;
        }

        public void StopDrag()
        {
            Position = Position.Round();

            if (_reseDirection != null)
            {
                Size = Size.Round();

                _reseDirection = null;
                Mouse.OverrideCursor = _defaultCursor;

                //if (Size.IsZeroSize())
                //{
                //    Grid.Children.Remove(this);
                //}
            }
        }

        public void Drag(Point position, Vector offset)
        {
            if (_reseDirection != null)
            {
                Resize(offset);
            }
            else
            {
                Move(offset);
            }
        }

        private void Resize(Vector offset)
        {
            var shift = offset / Grid.RenderGridSize;

            if (_reseDirection == ResizeDirection.West || _reseDirection == ResizeDirection.NorthWest || _reseDirection == ResizeDirection.SouthWest)
            {
                _resizeFirst.X += shift.X;
            } 
            else if (_reseDirection == ResizeDirection.East || _reseDirection == ResizeDirection.NorthEast || _reseDirection == ResizeDirection.SouthEast)
            {
                _resizeSecond.X += shift.X;
            }

            if (_reseDirection == ResizeDirection.North || _reseDirection == ResizeDirection.NorthWest || _reseDirection == ResizeDirection.NorthEast)
            {
                _resizeFirst.Y += shift.Y;
            }
            else if (_reseDirection == ResizeDirection.South || _reseDirection == ResizeDirection.SouthWest || _reseDirection == ResizeDirection.SouthEast)
            {
                _resizeSecond.Y += shift.Y;
            }

            UpdateShape(_resizeFirst, _resizeSecond);
        }

        private void UpdateShape(Vector first, Vector second)
        {
            Point pos;
            Size size;

            if (first.X < second.X)
            {
                pos.X = first.X;
                size.Width = second.X - pos.X;
            }
            else
            {
                pos.X = second.X;
                size.Width = Math.Abs(first.X - pos.X);
            }

            if (first.Y < second.Y)
            {
                pos.Y = first.Y;
                size.Height = second.Y - pos.Y;
            }
            else
            {
                pos.Y = second.Y;
                size.Height = Math.Abs(first.Y - pos.Y);
            }

            Position = pos;
            Size = size;
        }

        private void Move(Vector offset)
        {
            Position += offset / Grid.RenderGridSize;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_reseDirection == null && !_drawing)
            {
                if (Grid.AllowEdit && IsMouseNearCorner(out var direction))
                {
                    Cursor = direction.ToCursor();
                }
                else if (Cursor == Cursors.SizeWE
                    || Cursor == Cursors.SizeNWSE
                    || Cursor == Cursors.SizeNS
                    || Cursor == Cursors.SizeNESW)
                {
                    Cursor = null;
                }
            }
        }

        private bool IsMouseNearCorner(out ResizeDirection direction)
        {
            var near = 0.05;
            var cornerNear = near * 2;
            var minNearFakeSize = 10;

            var mousePosition = Grid.MousePosition;
            var positionVector = Position.ToVector();

            var firstDiff = (positionVector - mousePosition).Abs();
            firstDiff = new Vector(firstDiff.X / Math.Max(Size.Width, minNearFakeSize), firstDiff.Y / Math.Max(Size.Height, minNearFakeSize));

            var secondDiff = (positionVector + Size.ToVector() - mousePosition).Abs();
            secondDiff = new Vector(secondDiff.X / Math.Max(Size.Width, minNearFakeSize), secondDiff.Y / Math.Max(Size.Height, minNearFakeSize));

            if (firstDiff.X > near
                && secondDiff.X > near
                && firstDiff.Y > near
                && secondDiff.Y > near)
            {
                direction = ResizeDirection.None;
                return false;
            }

            if (firstDiff.X < cornerNear && firstDiff.Y < cornerNear)
            {
                direction = ResizeDirection.NorthWest;
                return true;
            }
            else if (secondDiff.X < cornerNear && secondDiff.Y < cornerNear)
            {
                direction = ResizeDirection.SouthEast;
                return true;
            }
            else if (firstDiff.X < cornerNear && secondDiff.Y < cornerNear)
            {
                direction = ResizeDirection.SouthWest;
                return true;
            }
            else if (secondDiff.X < cornerNear && firstDiff.Y < cornerNear)
            {
                direction = ResizeDirection.NorthEast;
                return true;
            }
            else if (firstDiff.X < near)
            {
                direction = ResizeDirection.West;
                return true;
            }
            else if (secondDiff.X < near)
            {
                direction = ResizeDirection.East;
                return true;
            }
            else if (firstDiff.Y < near)
            {
                direction = ResizeDirection.North;
                return true;
            }
            else if (secondDiff.Y < near)
            {
                direction = ResizeDirection.South;
                return true;
            }

            direction = ResizeDirection.None;
            return false;
        }
    }
}
