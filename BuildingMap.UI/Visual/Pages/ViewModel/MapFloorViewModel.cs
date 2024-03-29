﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.View.Core.Utils;
using BuildingMap.UI.Visual.Logics;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapFloorViewModel : ObservableObject
	{
		private bool _singleKeyDownFlag;

		private MapItemViewModel _drawingItem;

		private Floor _floor;

		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly MapItemsFactory _mapItemsFactory;

		public MapFloorViewModel(
			MapManager mapManager,
			SettingsManager settingsManager,
			MapItemsFactory mapItemsFactory)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_mapItemsFactory = mapItemsFactory;

			PasteElementCommand = new RelayCommand(InsertElementImpl, CanInsertElement);
		}

		public RelayCommand PasteElementCommand { get; }

		public ObservableCollection<MapItemViewModel> Items { get; } = new ObservableCollection<MapItemViewModel>();

		public MapItemViewModel SelectedItem { get; private set; }

		public Vector MousePosition { get; set; }

		public void SelectItem(MapItemViewModel item)
		{
			if (SelectedItem != null)
			{
				SelectedItem.IsSelected = false;
			}

			if (item != null)
			{
				item.IsSelected = true;
			}

			SelectedItem = item;
		}

		public void OnClickItem(object sender, MouseButtonEventArgs args)
		{
			if (!DataContextHelper.TryGetDataContext<MapItemViewModel>(sender, out var item)) return;
			if (DragManager.Moving) return;
			
			SelectItem(item);
		}

		public void OnStartDraw(object sender, StartDrawEventArgs args)
		{
			var viewModel = _mapItemsFactory.CreateNew();
			args.CreatedObject = viewModel;

			AddNewItem(viewModel);

			_drawingItem = viewModel;
		}

		public void OnStopDraw()
		{
			if (_drawingItem.Size.IsZeroSize())
			{
				RemoveItem(_drawingItem);
			}

			_drawingItem = null;
		}

		public void CopyElement()
		{
			OnSingleKeyDown(() => SelectedItem?.CopyToClipboard());
		}

		public void InsertElement()
		{	
			OnSingleKeyDown(InsertElementImpl);
		}

		public void OnKeyUp()
		{
			_singleKeyDownFlag = false;
		}

		public void DeleteElement()
		{
			if (SelectedItem == null) return;

			RemoveItem(SelectedItem);
			SelectedItem = null;
		}

		public void Update(Floor floor)
		{
			Items.Clear();

			_floor = floor;
			foreach (var mapItem in _floor.MapItems.Values)
			{
				var mapItemViewModel = _mapItemsFactory.Create(mapItem);
				Items.Add(mapItemViewModel);
			}
		}

		public void Unselect()
		{
			SelectItem(null);
		}

		private bool CanInsertElement()
		{
			return _mapItemsFactory.CanCreateCopyFromClipboard();
		}

		private void InsertElementImpl()
		{
			if (!_mapItemsFactory.TryCreateCopyFromClipboard(out var copy)) return;

			copy.Position = (MousePosition - copy.Size.ToVector() / 2).Floor().ToPoint();

			AddNewItem(copy);
		}

		private void OnSingleKeyDown(Action action)
		{
			if (_singleKeyDownFlag) return;

			action();

			_singleKeyDownFlag = true;
		}

		private void AddNewItem(MapItemViewModel viewModel)
		{
			Items.Add(viewModel);
			_floor.MapItems.Add(viewModel.MapItem.Id, viewModel.MapItem);
		}

		private void RemoveItem(MapItemViewModel viewModel)
		{
			Items.Remove(viewModel);
			_mapManager.Map.TryRemoveItem(viewModel.MapItem);
		}
	}
}