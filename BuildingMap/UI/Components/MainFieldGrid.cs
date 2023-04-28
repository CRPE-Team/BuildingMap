using System.Windows.Input;

namespace BuildingMap.UI.Components
{
    public class MainFieldGrid : DragGrid
    {
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var offset = e.Delta / 10d;

            Height = Width = Width - offset;
        }
    }
}
