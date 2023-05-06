using System;
using System.Windows;
using BuildingMap.UI.Visual.Components.View.Core.Utils;

namespace BuildingMap.UI.Visual.Components.View
{
    public partial class RectangleObject : IDrawable
    {
        private Vector _startDrawPosition;
        private bool _drawing;

        public void StartDraw()
        {
            _drawing = true;
            _startDrawPosition = Grid.MousePosition;
			Size = new Size();
			Cursor = null;

            Position = _startDrawPosition.Floor().ToPoint();
        }

        public void Draw()
        {
            DrawShape(_startDrawPosition, Grid.MousePosition);
        }

        private void DrawShape(Vector first, Vector second)
        {
            Point pos;
            Size size;

            if (first.X < second.X)
            {
                pos.X = Math.Floor(first.X);
                size.Width = Math.Ceiling(second.X - pos.X);
            }
            else
            {
                pos.X = Math.Floor(second.X);
                size.Width = Math.Abs(Math.Ceiling(first.X - pos.X));
            }

            if (first.Y < second.Y)
            {
                pos.Y = Math.Floor(first.Y);
                size.Height = Math.Ceiling(second.Y - pos.Y);
            }
            else
            {
                pos.Y = Math.Floor(second.Y);
                size.Height = Math.Abs(Math.Ceiling(first.Y - pos.Y));
            }

            Position = pos;
            Size = size;
        }

        public bool EndDraw()
        {
            _drawing = false;
            return !Size.IsZeroSize();
        }
    }
}
