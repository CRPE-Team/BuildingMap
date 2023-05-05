using BuildingMap.UI.Components.View.Core.Utils;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingMap.UI.Components.View.Core
{
    public class DragGrid : Grid
    {
        public DragManager DragManager { get; }

        public DragGrid()
        {
            DragManager = new DragManager(this);

            MouseDown += OnStartDrag;
        }

		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			if (DragManager.WasMoving)
			{
				e.Handled = true;
			}

			base.OnContextMenuOpening(e);
		}

		protected virtual void OnStartDrag(object sender, MouseButtonEventArgs e)
        {
            var directlyOver = DragManager.MouseDirectlyOver();
            if (directlyOver != null)
            {
                DragManager.StartDrag(directlyOver);
            }
        }
    }
}
