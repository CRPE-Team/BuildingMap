using System.Runtime.CompilerServices;
using System.Windows;

namespace BuildingMap.UI.Utils
{
	public delegate void PropertyChangedCallback<T>(T d, DependencyPropertyChangedEventArgs e) where T : DependencyObject;

	public static class DependencyPropertyEx
	{
		private const string PropertySufix = "Property";

		public static DependencyProperty Register<TProperty, TOwner>(TProperty defaultValue = default, [CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty),
				typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue));
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback, 
			TProperty defaultValue = default, 
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name), 
				typeof(TProperty), typeof(TOwner), 
				new FrameworkPropertyMetadata(defaultValue) 
				{ 
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args))
				});
		}

		private static string ResolvePropertyName(string name)
		{
			if (name.EndsWith(PropertySufix))
			{
				return name.Substring(0, name.Length - PropertySufix.Length);
			}

			return name;
		}
	}
}
