using System.Runtime.CompilerServices;
using System.Windows;

namespace BuildingMap.UI.Utils
{
	public static class EventManagerEx
	{
		private const string EventSuffix = "Event";
		private const string TunnelStrategySuffix = "Preview";

		public static RoutedEvent Register<TProperty, TOwner>([CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			var resolvedName = ResolveEventName(name);
			var strategy = GetRoutingStrategyFromName(ref resolvedName);

			return EventManager.RegisterRoutedEvent(
				resolvedName,
				strategy,
				typeof(TProperty),
				typeof(TOwner));
		}

		public static RoutedEvent Register<TProperty, TOwner>(RoutingStrategy routingStrategy, [CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return EventManager.RegisterRoutedEvent(
				ResolveEventName(name),
				routingStrategy,
				typeof(TProperty),
				typeof(TOwner));
		}

		private static RoutingStrategy GetRoutingStrategyFromName(ref string name)
		{
			if (name.EndsWith(TunnelStrategySuffix))
			{
				name = name.Substring(0, name.Length - TunnelStrategySuffix.Length);
				return RoutingStrategy.Tunnel;
			}

			return RoutingStrategy.Bubble;
		}

		private static string ResolveEventName(string name)
		{
			if (name.EndsWith(EventSuffix))
			{
				return name.Substring(0, name.Length - EventSuffix.Length);
			}

			return name;
		}
	}
}
