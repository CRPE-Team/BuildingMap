using System;
using System.Numerics;
using System.Windows;
using Vector = System.Windows.Vector;

namespace BuildingMap.UI.Visual
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

        public static Vector FloorInt(this Vector vector)
        {
            return new Vector((int)vector.X, (int)vector.Y);
        }

        public static Vector Floor(this Vector vector)
        {
            return new Vector(Math.Floor(vector.X), Math.Floor(vector.Y));
        }

        public static Vector Ceiling(this Vector vector)
        {
            return new Vector(Math.Ceiling(vector.X), Math.Ceiling(vector.Y));
        }

        public static Vector Round(this Vector vector)
        {
            return new Vector(Math.Round(vector.X), Math.Round(vector.Y));
        }

        public static Vector Abs(this Vector vector)
        {
            return new Vector(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        public static Vector Fix(this Vector vector, int fixAverage)
        {
            return (vector * fixAverage).FloorInt() / fixAverage;
        }

        public static Point Floor(this Point vector)
        {
            return new Point(Math.Floor(vector.X), Math.Floor(vector.Y));
        }

        public static Point Round(this Point vector)
        {
            return new Point(Math.Round(vector.X), Math.Round(vector.Y));
        }

        public static Point ToPoint(this Vector vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static Vector ToVector(this Point vector)
        {
            return new Vector(vector.X, vector.Y);
        }

        public static Vector ToVector(this Size vector)
        {
            return new Vector(vector.Width, vector.Height);
        }

        public static Size ToSize(this Vector vector)
        {
            return new Size(vector.X, vector.Y);
        }

        public static Size Round(this Size size)
        {
            return new Size(Math.Round(size.Width), Math.Round(size.Height));
        }

        public static bool IsZeroSize(this Size size)
        {
            return size.Width == 0 || size.Height == 0;
        }

		public static Vector2 ToNumerics(this Size size)
		{
			return new Vector2((float) size.Width, (float) size.Height);
		}

		public static Vector2 ToNumerics(this Point point)
		{
			return new Vector2((float) point.X, (float) point.Y);
		}

		public static Vector2 ToNumerics(this Vector vector)
		{
			return new Vector2((float) vector.X, (float) vector.Y);
		}

		public static Size ToSize(this Vector2 vector)
		{
			return new Size(vector.X, vector.Y);
		}

		public static Point ToPoint(this Vector2 vector)
		{
			return new Point(vector.X, vector.Y);
		}

		public static Vector ToVector(this Vector2 vector)
		{
			return new Vector(vector.X, vector.Y);
		}
	}
}
