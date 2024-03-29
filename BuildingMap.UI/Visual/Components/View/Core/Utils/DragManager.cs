﻿using System.Windows;
using System.Windows.Input;

namespace BuildingMap.UI.Visual.Components.View.Core.Utils
{
    public class DragManager
    {
        private DragContext _dragContext;
        private static bool _dragging;
        private static bool _moving;
		private static bool _wasMoving;
        private Point _mouseStartPosition;

		public static bool Dragging => _dragging;
        public static bool Moving => _moving;
		public static bool WasMoving => _wasMoving;


		public UIElement Source { get; }
        public static IDraggable SelectedElement { get; private set; }

        public DragManager(UIElement source)
        {
            Source = source;

            Mouse.AddMouseUpHandler(source, (_, _) => StopDrag());
        }

        public static IDraggable MouseDirectlyOver()
        {
            var directlyOver = Mouse.DirectlyOver as FrameworkElement;

            while (directlyOver != null)
            {
                if (directlyOver is IDraggable draggable && draggable.CanDrag) return draggable;
                var parent = directlyOver.Parent ?? directlyOver.TemplatedParent;

                directlyOver = parent as FrameworkElement;
            }

            return null;
        }

        public bool StartDrag(IDraggable element)
        {
            if (_dragging) return false;

            _dragging = true;
			_wasMoving = false;
            _mouseStartPosition = Mouse.GetPosition(Source);

            SelectedElement = element;
            _dragContext = element.StartDrag();

            if (_dragContext == null)
            {
                SelectedElement = null;
                _dragging = false;

                return false;
            }

            Mouse.AddPreviewMouseMoveHandler(Source, MouseMove);

            return true;
        }

        private void StopDrag()
        {
            if (!_dragging) return;

            Mouse.RemovePreviewMouseMoveHandler(Source, MouseMove);

            _dragging = false;
            _moving = false;
            _mouseStartPosition = default;

            SelectedElement.StopDrag();
            SelectedElement = null;
        }

        private int _moveId = 0;

        private void MouseMove(object source, MouseEventArgs e)
        {
            if (!_dragging) return;

            if (!SelectedElement.CanDrag)
            {
                StopDrag();
                return;
            }

            var current = e.GetPosition(Source);
            var offset = current - _mouseStartPosition;
            var distance = offset.Length;

            if (!_moving)
            {
                if (distance > _dragContext.DragStartDistance)
				{
					_moving = true;
					_wasMoving = true;
				}
				else
                {
                    return;
                }
            }

            if (distance > 1)
            {
                if (_moveId++ % (int)(distance * distance) == 0)
                {
                    _moveId = 0;
                }
                else
                {
                    return;
                }

                SelectedElement.Drag(current, offset);
                _mouseStartPosition = current;
            }
        }
    }
}
