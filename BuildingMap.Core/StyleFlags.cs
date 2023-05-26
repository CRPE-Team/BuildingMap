using System;

namespace BuildingMap.Core
{
	[Flags]
	public enum StyleFlags
	{
		None = 0,
		System = 1 << 0,
		Static = 1 << 1,
		Hidden = 1 << 2
	}
}
