using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Components.View.Core
{
	[ContentProperty("ItemTemplate")]
	[DefaultProperty("ItemTemplate")]
	public class ObservableItemsTemplate : FrameworkElement
	{
		public static readonly DependencyProperty ItemsSourceProperty = DependencyPropertyEx.Register<IEnumerable, ObservableItemsTemplate>(OnItemsSourceChanged, new ObservableCollection<object>());
		public static readonly DependencyProperty ItemTemplateProperty = DependencyPropertyEx.Register<DataTemplate, ObservableItemsTemplate>(OnItemTemplateChanged);

		[Bindable(true)]
		public IEnumerable ItemsSource { get => (IEnumerable) GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

		[Bindable(true)]
		public DataTemplate ItemTemplate { get => (DataTemplate) GetValue(ItemTemplateProperty); set => SetValue(ItemTemplateProperty, value); }

		public event PropertyChangedCallback<ObservableItemsTemplate> ItemsSourceChanged;
		public event PropertyChangedCallback<ObservableItemsTemplate> ItemTemplateChanged;

		private static void OnItemsSourceChanged(ObservableItemsTemplate d, DependencyPropertyChangedEventArgs e)
		{
			d.ItemsSourceChanged?.Invoke(d, e);
		}

		private static void OnItemTemplateChanged(ObservableItemsTemplate d, DependencyPropertyChangedEventArgs e)
		{
			d.ItemTemplateChanged?.Invoke(d, e);
		}
	}
}
