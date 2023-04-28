﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BuildingMap.UI.Components
{
	public class VertexView : Grid, IDraggable
	{
		private static readonly int Size = 25;
		private static readonly Brush UnselectedColor = new SolidColorBrush(Color.FromRgb(0x00, 0xD3, 0xEA));
		private static readonly Brush SelectedColor = Brushes.White;
		private static readonly Brush StrokeColor = new SolidColorBrush(Color.FromRgb(0x00, 0xBB, 0xCC));

        private static readonly DragContext DragContext = new DragContext();

        private static int _idCounter = 0;

		private Ellipse _ellipse;
		private Label _numberLabel;

		private int _number;
		private bool _selected;
		private Point _position;

        private readonly TranslateTransform _translateTransform;

        public VertexView()
		{
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;

			Height = Width = Size;

			Children.Add(_ellipse = new Ellipse());
			Children.Add(_numberLabel = new Label());

			_ellipse.Fill = UnselectedColor;
			_ellipse.Stroke = StrokeColor;
			_ellipse.StrokeThickness = 2;

			_numberLabel.HorizontalAlignment = HorizontalAlignment.Center;
			_numberLabel.VerticalAlignment = VerticalAlignment.Center;
			_numberLabel.Margin = new Thickness(0, -1.5, 0, 0);
			_numberLabel.FontSize = 13;
			_numberLabel.FontWeight = FontWeights.Bold;

			RenderTransform = _translateTransform = new TranslateTransform();
        }

		public int Id { get; } = _idCounter++;

		protected override void OnMouseDown(MouseButtonEventArgs e) => ProcessRoutedEvent(e);
		protected override void OnMouseUp(MouseButtonEventArgs e) => ProcessRoutedEvent(e);

		private void ProcessRoutedEvent(RoutedEventArgs e)
		{
			if (e.Source is VertexView && e.OriginalSource is not VertexView)
			{
				e.Handled = true;
			}
		}

		public int Number 
		{ 
			get => _number;
			set
			{
				_number = value;
				_numberLabel.Content = value;
			}
		}

		public bool Selected
		{
			get => _selected;
			set
			{
				_selected = value;
				_ellipse.Fill = value ? SelectedColor : UnselectedColor;
			}
		}

		public Point Position 
		{ 
			get => _position; 
			set
			{
				_position = value;
                _translateTransform.X = _position.X - Size / 2;
				_translateTransform.Y = _position.Y - Size / 2;
			}
		}

		public Point RealPos { get; set; } //Test

		public bool ShowNumber
		{
			get => _numberLabel.IsVisible;
			set => _numberLabel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
		}

		protected override void OnRender(DrawingContext dc)
		{
			if (ContextMenu != null)
			{
				_ellipse.ContextMenu = ContextMenu;
				_numberLabel.ContextMenu = ContextMenu;
				ContextMenu = null;
			}

			base.OnRender(dc);
		}

        public DragContext StartDrag()
        {
            return DragContext;
        }

        public void StopDrag()
        {
            
        }

        public void Drag(Point position, Vector offset)
        {
            Position += offset;
        }
    }
}
