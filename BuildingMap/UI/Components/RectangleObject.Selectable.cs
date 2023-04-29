using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BuildingMap.UI.Components
{
    public partial class RectangleObject : ISelectable
    {
        private static readonly Brush SelectedBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xDF, 0xE0));

        public void Select()
        {
            ISelectable.Select(this);

            _rectangle.Stroke = UnselectedBrush;
            _rectangle.Fill = SelectedBrush;
            _rectangle.StrokeThickness = 3;

            Application.Current.MainWindow.KeyDown += OnKeyDown;
        }

        public void Unselect()
        {
            ISelectable.ResetSelected();

            _rectangle.Stroke = null;
            _rectangle.StrokeThickness = 0;
            _rectangle.Fill = UnselectedBrush;

            Application.Current.MainWindow.KeyDown -= OnKeyDown;
        }

        protected void OnKeyDown(object source, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Unselect();
            }
            else if (e.Key == Key.Delete && Grid.AllowEdit)
            {
                Grid.Children.Remove(this);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Select();
        }
    }
}
