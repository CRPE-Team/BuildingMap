﻿namespace BuildingMap.UI.Utils
{
	public class ClipboardManager
    {
		private object _cache;

		public void SetData(object data)
		{
			_cache = data;
		}

		public bool TryGetData(out object data) 
		{
			data = _cache;
			return data != null;
		}

		public bool TryGetData<T>(out T data)
		{ 
			if (!TryGetData(out object cachedData) || cachedData is not T tData)
			{
				data = default;
				return false;
			}

			data = tData;
			return true;
		}
    }
}
