using System.Windows;

namespace BuildingMap.UI
{
    public interface IDraggable
    {
        DragContext StartDrag();

        void StopDrag();

        void Drag(Point position, Vector offset);
    }
}
