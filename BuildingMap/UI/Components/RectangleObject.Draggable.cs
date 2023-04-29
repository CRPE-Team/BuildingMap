using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace BuildingMap.UI.Components
{
    public partial class RectangleObject : IDraggable
    {
        private static readonly DragContext DragContext = new DragContext();

        private ResizeDirection? _resizing;
        private Cursor _defaultCursor;

        public DragContext StartDrag()
        {
            if (IsMouseNearCorner(out var direction))
            {
                _resizing = direction;

                _defaultCursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = DirectionToCursor(direction) ?? _defaultCursor;
            }

            return DragContext;
        }

        public void StopDrag()
        {
            Position = Position.Floor();
            Size = Size.ToVector().Ceiling().ToSize();
            
            if (_resizing != null)
            {
                _resizing = null;
                Mouse.OverrideCursor = _defaultCursor;
            }
        }

        public void Drag(Point position, Vector offset)
        {
            if (_resizing != null)
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
            var shift = offset / (_grid.GridSize * _grid.Zoom);

            Vector directionVector;
            if (_resizing == ResizeDirection.West || _resizing == ResizeDirection.NorthWest || _resizing == ResizeDirection.SouthWest)
            {
                directionVector.X = -1;
            } 
            else if (_resizing == ResizeDirection.East || _resizing == ResizeDirection.NorthEast || _resizing == ResizeDirection.SouthEast)
            {
                directionVector.X = 1;
            }

            if (_resizing == ResizeDirection.North || _resizing == ResizeDirection.NorthWest || _resizing == ResizeDirection.NorthEast)
            {
                directionVector.Y = -1;
            }
            else if (_resizing == ResizeDirection.South || _resizing == ResizeDirection.SouthWest || _resizing == ResizeDirection.SouthEast)
            {
                directionVector.Y = 1;
            }

            directionVector = new Vector(directionVector.X * shift.X, directionVector.Y * shift.Y);

            Position -= directionVector;
            Size = (Size.ToVector() + directionVector).ToSize();

            Trace.TraceInformation($"{Position}  {Size}");
        }

        private void Move(Vector offset)
        {
            Position += offset / (_grid.GridSize * _grid.Zoom);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_resizing == null && !_drawing)
            {
                if (IsMouseNearCorner(out var direction))
                {
                    Cursor = DirectionToCursor(direction);
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
            var cornerNear = near * 3;
            var mousePosition = _grid.MousePosition;
            var positionVector = Position.ToVector();

            var firstDiff = (positionVector - mousePosition).Abs();
            firstDiff = new Vector(firstDiff.X / Size.Width, firstDiff.Y / Size.Height);

            var secondDiff = (positionVector + Size.ToVector() - mousePosition).Abs();
            secondDiff = new Vector(secondDiff.X / Size.Width, secondDiff.Y / Size.Height);

            //if (firstDiff.X > near
            //    && secondDiff.X > near
            //    && firstDiff.Y > near
            //    && secondDiff.Y > near)
            //{
            //    direction = ResizeDirection.None;
            //    return false;
            //}

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

        private Cursor DirectionToCursor(ResizeDirection direction)
        {
            return direction switch
            {
                ResizeDirection.West or ResizeDirection.East => Cursors.SizeWE,
                ResizeDirection.NorthWest or ResizeDirection.SouthEast => Cursors.SizeNWSE,
                ResizeDirection.North or ResizeDirection.South => Cursors.SizeNS,
                ResizeDirection.NorthEast or ResizeDirection.SouthWest => Cursors.SizeNESW,

                _ => null
            };
        }

        private enum ResizeDirection
        {
            None,

            West,
            East,
            South,
            North,
            
            NorthWest,
            NorthEast,
            SouthWest,
            SouthEast
        }
    }
}
