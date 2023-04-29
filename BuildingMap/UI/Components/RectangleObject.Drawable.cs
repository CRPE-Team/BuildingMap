using System;
using System.Windows;
using System.Windows.Media;

namespace BuildingMap.UI.Components
{
    public partial class RectangleObject : IDrawable
    {
        private Vector _startDrawPosition;
        private bool _drawing;

        public void StartDraw()
        {
            _drawing = true;
            _startDrawPosition = _grid.MousePosition;

            Position = _startDrawPosition.Floor().ToPoint();
        }

        public void Draw()
        {
            var secondPosition = _grid.MousePosition;
            Point pos;
            Size size;

            if (_startDrawPosition.X < secondPosition.X)
            {
                pos.X = Math.Floor(_startDrawPosition.X);
                size.Width = Math.Ceiling(secondPosition.X - pos.X);
            }
            else
            {
                pos.X = Math.Floor(secondPosition.X);
                size.Width = Math.Abs(Math.Ceiling(_startDrawPosition.X - pos.X));
            }

            if (_startDrawPosition.Y < secondPosition.Y)
            {
                pos.Y = Math.Floor(_startDrawPosition.Y);
                size.Height = Math.Ceiling(secondPosition.Y - pos.Y);
            }
            else
            {
                pos.Y = Math.Floor(secondPosition.Y);
                size.Height = Math.Abs(Math.Ceiling(_startDrawPosition.Y - pos.Y));
            }

            Position = pos;
            Size = size;
        }

        public bool EndDraw()
        {
            _drawing = false;
            return Size.Width > 0 && Size.Height > 0;
        }
    }
}
