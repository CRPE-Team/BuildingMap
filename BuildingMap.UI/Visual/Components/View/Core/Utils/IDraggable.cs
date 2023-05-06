using System.Windows;

namespace BuildingMap.UI.Visual.Components.View.Core.Utils
{
    public interface IDraggable
    {
        bool CanDrag { get; }

        DragContext StartDrag();

        void StopDrag();

        void Drag(Point position, Vector offset);
    }
}
