using System;
using System.Drawing;
using BuildingMap.Core;

namespace BuildingMap.UI.Logic
{
	public class MapManager
	{
		public MapManager()
		{
			SetEmptyMap();
		}

		public Map Map { get; private set; }

		public event EventHandler MapLoaded;
		public event EventHandler MapMoved;
		public event EventHandler FloorsChanged;
		public event EventHandler<StyleUpdateEventArgs> StyleChanged;

		public void LoadMap()
		{
			OnMapLoaded();
		}

		public void SetEmptyMap()
		{
			var map = new Map() 
			{
				GridSize = 5,
				Zoom = 1
			};

			AddSystemStyles(map);
			AddNewFloor(map);

			//test
			AddNewFloor(map);
			AddNewFloor(map);
			AddNewFloor(map);
			//test

			Map = map;

			OnMapLoaded();
		}

		public void UpdateStyle(object sender, ItemStyle style, string propertyName)
		{
			StyleChanged?.Invoke(sender, new StyleUpdateEventArgs(style, propertyName));
		}

		public void MapUpdated(object sender)
		{
			MapMoved?.Invoke(sender, new EventArgs());
		}

		private void AddSystemStyles(Map map)
		{
			map.Styles.TryAdd(-1, new ItemStyle()
			{
				Id = -1,
				Name = "Object 1",
				Radius = 10,
				Color = Color.FromArgb(0x00, 0xD3, 0xEA),
				SelectedColor = Color.FromArgb(0x00, 0xF3, 0xEA),
				ForegroundColor = Color.FromArgb(0x00, 0x00, 0x00),
				FontSize = 12,
				Flags = StyleFlags.System,
				ImageScale = 0.8
			});
			map.Styles.TryAdd(-2, new ItemStyle()
			{
				Id = -2,
				Name = "Object 2",
				Radius = 10,
				Color = Color.FromArgb(0x9D, 0x5E, 0xD9),
				SelectedColor = Color.FromArgb(0x00),
				ForegroundColor = Color.FromArgb(0x00, 0x00, 0x00),
				FontSize = 12,
				Flags = StyleFlags.System,
				ImageScale = 0.8
			});
		}

		private void AddNewFloor(Map map)
		{
			map.Floors.Add(new Floor() { Number = map.Floors.Count + 1 } );
			FloorsChanged?.Invoke(this, new EventArgs());
		}

		private void OnMapLoaded()
		{
			MapLoaded?.Invoke(this, new EventArgs());
		}
	}
}
