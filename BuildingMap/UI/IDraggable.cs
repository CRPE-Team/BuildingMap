using System.Windows;

namespace BuildingMap.UI
{
    public interface IDraggable
    {
        bool CanDrag { get; }

        DragContext StartDrag();

        void StopDrag();

        void Drag(Point position, Vector offset);
    }
}
