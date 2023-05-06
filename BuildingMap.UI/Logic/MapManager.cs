using System;
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
		public event EventHandler FloorAdded;

		public void LoadMap()
		{
			OnMapLoaded();
		}

		public void SetEmptyMap()
		{
			Map = new Map();
			AddNewFloor();

			OnMapLoaded();
		}

		private void AddNewFloor()
		{
			Map.Floors.Add(new Floor() { Number = Map.Floors.Count + 1 } );
			FloorAdded?.Invoke(this, new EventArgs());
		}

		private void OnMapLoaded()
		{
			MapLoaded?.Invoke(this, new EventArgs());
		}
	}
}
