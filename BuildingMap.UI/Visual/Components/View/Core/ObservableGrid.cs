using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Components.View.Core
{
	public class ObservableGrid : Grid
	{
		public static readonly DependencyProperty ItemsSourceProperty = DependencyPropertyEx.Register<IEnumerable, ObservableGrid>(OnItemsSourceChanged, new ObservableCollection<object>());
		public static readonly DependencyProperty ItemTemplateProperty = DependencyPropertyEx.Register<DataTemplate, ObservableGrid>(OnItemTemplateChanged);

		private List<object> _itemsCache = new List<object>();

		[Bindable(true)]
		public IEnumerable ItemsSource { get => (IEnumerable) GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

		[Bindable(true)]
		public DataTemplate ItemTemplate { get => (DataTemplate) GetValue(ItemTemplateProperty); set => SetValue(ItemTemplateProperty, value); }

		protected void AddItem(object item)
		{
			Add(new[] { item });
		}

		protected void RemoveItem(object item)
		{
			Remove(new[] { item });
		}

		private static void OnItemsSourceChanged(ObservableGrid d, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue is INotifyCollectionChanged oldValue)
			{
				oldValue.CollectionChanged -= d.OnItemsSourceCollectionChanged;
			}

			if (e.OldValue is IList oldList) d.Remove(oldList);

			if (e.NewValue is INotifyCollectionChanged newValue)
			{
				newValue.CollectionChanged += d.OnItemsSourceCollectionChanged;
			}

			if (e.OldValue is IList newList) d.Add(newList);
		}

		private static void OnItemTemplateChanged(ObservableGrid d, DependencyPropertyChangedEventArgs e)
		{
			var items = d._itemsCache.ToArray();

			d.Remove(items);
			d.Add(items);
		}

		private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Add(e.NewItems);
					break;

				case NotifyCollectionChangedAction.Remove:
					Remove(e.OldItems);
					break;

				case NotifyCollectionChangedAction.Replace:
					Remove(e.OldItems);
					Add(e.NewItems);
					break;

				case NotifyCollectionChangedAction.Move:

					break;

				case NotifyCollectionChangedAction.Reset:
					Remove(_itemsCache.ToArray());
					break;
			}
		}

		private void Add(IList items)
		{
			foreach (var item in items)
			{
				_itemsCache.Add(item);

				var child = ItemTemplate?.LoadContent();
				if (child is FrameworkElement frameworkElement)
				{
					frameworkElement.DataContext = item;
					Children.Add(frameworkElement);
				}
			}
		}

		private void Remove(IList items)
		{
			foreach (var item in items)
			{
				_itemsCache.Remove(item);

				foreach (var child in Children.OfType<FrameworkElement>().ToArray())
				{
					if (child.DataContext == item)
					{
						Children.Remove(child);
					}
				}
			}
		}
	}
}
