using System.Windows.Input;

namespace BuildingMap.UI.Components.View.Core
{
    public static class ResizeDirectionExtensions
    {
        public static Cursor ToCursor(this ResizeDirection direction)
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
    }
}
