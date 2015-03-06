using System;

namespace GreatTextAdventures
{
	[Flags]
	public enum Directions
	{
		None = 0,
		North = 1,
		South = 2,
		West = 4,
		East = 8
	}
}