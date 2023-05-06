using System.Windows;

namespace BuildingMap.UI.Visual.Components.View.Core.Utils
{
	public class StartDrawEventArgs : RoutedEventArgs
	{
		public object CreatedObject { get; set; }

		public StartDrawEventArgs(RoutedEvent routedEvent) : base(routedEvent)
		{
		}
	}
}
