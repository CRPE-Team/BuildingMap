using BuildingMap.Core;

namespace BuildingMap.UI.Logic
{
	public class StyleUpdateEventArgs
	{
		public ItemStyle Style { get; set; }

		public string PropertyName { get; set; }

		public StyleUpdateEventArgs(ItemStyle itemStyle, string propertyName)
		{
			Style = itemStyle;
			PropertyName = propertyName;
		}
	}
}
