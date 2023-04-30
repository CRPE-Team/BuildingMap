using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core.Utils;

namespace BuildingMap.UI.Components.View
{
    public partial class RectangleObject : ISelectable
    {
        private static readonly Brush SelectedBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xDF, 0xE0));

        public bool CanSelect { get; set; } = true;

        public void Select()
        {
            if (!ISelectable.Select(this)) return;

            _rectangle.Stroke = UnselectedBrush;
            _rectangle.Fill = SelectedBrush;
            _rectangle.StrokeThickness = 3;

            Application.Current.MainWindow.KeyDown += OnKeyDown;
            Application.Current.MainWindow.MouseUp += OnMouseUp;
        }

        public void Unselect()
        {
            ISelectable.ResetSelected();

            _rectangle.Stroke = null;
            _rectangle.StrokeThickness = 0;
            _rectangle.Fill = UnselectedBrush;

            Application.Current.MainWindow.KeyDown -= OnKeyDown;
            Application.Current.MainWindow.MouseUp -= OnMouseUp;
        }

        private void OnKeyDown(object source, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Unselect();
            }
            else if (e.Key == Key.Delete && Grid.AllowEdit)
            {
                Grid.Children.Remove(this);
            }
            else if (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                ICopyable.CopyToCache(this);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (ISelectable.SelectedObject != this && !DragManager.Moving) Select();
        }

        private void OnMouseUp(object source, MouseButtonEventArgs e)
        {
            if (!IsMouseOver && ISelectable.SelectedObject == this)
            {
                Unselect();
            }
        }
    }
}
