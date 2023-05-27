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
		private readonly Dictionary<object, ObservableItemsTemplate> _teplateMap = new Dictionary<object, ObservableItemsTemplate>();
		private readonly Dictionary<ObservableItemsTemplate, List<object>> _itemsCache = new Dictionary<ObservableItemsTemplate, List<object>>();

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);

			if (visualAdded is ObservableItemsTemplate addedItemsTemplate)
			{
				_itemsCache.Add(addedItemsTemplate, new List<object>());

				addedItemsTemplate.ItemsSourceChanged += OnItemsSourceChanged;
				OnItemsSourceChanged(addedItemsTemplate, new DependencyPropertyChangedEventArgs(ObservableItemsTemplate.ItemsSourceProperty, null, addedItemsTemplate.ItemsSource));

				addedItemsTemplate.ItemTemplateChanged += OnItemTemplateChanged;
				OnItemsSourceChanged(addedItemsTemplate, new DependencyPropertyChangedEventArgs(ObservableItemsTemplate.ItemTemplateProperty, null, addedItemsTemplate.ItemTemplate));
			}

			if (visualRemoved is ObservableItemsTemplate removedItemsTemplate)
			{
				removedItemsTemplate.ItemsSourceChanged -= OnItemsSourceChanged;
				OnItemsSourceChanged(removedItemsTemplate, new DependencyPropertyChangedEventArgs(ObservableItemsTemplate.ItemsSourceProperty, removedItemsTemplate.ItemsSource, null));

				removedItemsTemplate.ItemTemplateChanged -= OnItemTemplateChanged;
				OnItemsSourceChanged(removedItemsTemplate, new DependencyPropertyChangedEventArgs(ObservableItemsTemplate.ItemTemplateProperty, removedItemsTemplate.ItemTemplate, null));

				_itemsCache.Remove(removedItemsTemplate);
			}
		}

		protected void AddItem(object item, ObservableItemsTemplate template)
		{
			Add(new[] { item }, template);
		}

		protected void RemoveItem(object item, ObservableItemsTemplate template)
		{
			Remove(new[] { item }, template);
		}

		private void OnItemsSourceChanged(ObservableItemsTemplate d, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != null) _teplateMap.Remove(e.OldValue);
			if (e.NewValue != null) _teplateMap[d.ItemsSource] = d;

			if (e.OldValue is INotifyCollectionChanged oldValue)
			{
				oldValue.CollectionChanged -= OnItemsSourceCollectionChanged;
			}

			if (e.OldValue is IList oldList) Remove(oldList, d);

			if (e.NewValue is INotifyCollectionChanged newValue)
			{
				newValue.CollectionChanged += OnItemsSourceCollectionChanged;
			}

			if (e.NewValue is IList newList) Add(newList, d);
		}

		private void OnItemTemplateChanged(ObservableItemsTemplate d, DependencyPropertyChangedEventArgs e)
		{
			var items = _itemsCache.ToArray();

			if (d.ItemsSource != null)
			{
				_teplateMap[d.ItemsSource] = d;
			}	

			Remove(items, d);
			Add(items, d);
		}

		private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Add(e.NewItems, _teplateMap.GetValueOrDefault(sender));
					break;

				case NotifyCollectionChangedAction.Remove:
					Remove(e.OldItems, _teplateMap.GetValueOrDefault(sender));
					break;

				case NotifyCollectionChangedAction.Replace:
				{
					var template = _teplateMap.GetValueOrDefault(sender);
					Remove(e.OldItems, template);
					Add(e.NewItems, template);
					break;
				}

				case NotifyCollectionChangedAction.Move:
					break;

				case NotifyCollectionChangedAction.Reset:
				{
					var template = _teplateMap.GetValueOrDefault(sender);
					Remove(_itemsCache.GetValueOrDefault(template)?.ToArray() ?? new object[0], template);
					break;
				}
			}
		}

		private void Add(IList items, ObservableItemsTemplate template)
		{
			foreach (var item in items)
			{
				if (template != null) _itemsCache[template].Add(item);

				var child = template.ItemTemplate?.LoadContent();
				if (child is FrameworkElement frameworkElement)
				{
					frameworkElement.DataContext = item;
					Children.Add(frameworkElement);
				}
			}
		}

		private void Remove(IList items, ObservableItemsTemplate template)
		{
			foreach (var item in items)
			{
				if (template != null) _itemsCache[template].Remove(item);

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
