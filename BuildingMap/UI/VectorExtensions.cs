using System;
using System.Windows;

namespace BuildingMap.UI
{
    public static class VectorExtensions
    {
        public static double GetAngleTo(this Vector input)
        {
            var normalized = input;
            normalized.Normalize();

            return Math.Atan2(normalized.Y, normalized.X);
        }

        public static Vector ToVector(this double angle)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public static Vector ToVector(this double angle, double multiplier)
        {
            return angle.ToVector() * multiplier;
        }

        public static Vector SwitchTop(this Vector vector, UIElement element)
        {
            vector.Y = element.RenderSize.Height - vector.Y;
            return vector;
        }

        public static Point SwitchTop(this Point vector, UIElement element)
        {
            vector.Y = element.RenderSize.Height - vector.Y;
            return vector;
        }
    }
}
