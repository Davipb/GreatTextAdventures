using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Rooms
{
	/// <summary>
	/// A generic room with no special events
	/// </summary>
	public class GenericRoom : Room
	{
		public const int MaxDecorations = 3;
		public const int DecorationChance = 25;

		public override bool CanExit { get; set; }

		public GenericRoom()
		{
			Exits = Directions.None;
			Items = new List<Item>();
			CanExit = true;
		}

		public static Room Random(Directions obligatory, Directions blocked)
		{
			GenericRoom room = new GenericRoom();

			List<Directions> newExits = new List<Directions>();

			// Loop through each possible exit
			foreach (Directions d in Enum.GetValues(typeof(Directions)))
			{
				if (obligatory.HasFlag(d))
				{
					// Must have this exit, add it
					room.Exits |= d;
				}
				else if (blocked.HasFlag(d))
				{
					// Exit is blocked by another room, skip it
					continue;
				}
				else
				{
					// Exit is neither obligatory nor blocked, add it to the new exits list
					newExits.Add(d);
					
				}
			}

			// Shuffle the new exits list and take a random amount of new exits
			newExits = newExits.OrderBy(x => GameSystem.RNG.Next()).Take(GameSystem.RNG.Next(1, newExits.Count)).ToList();

			// Add the selected new exits
			newExits.ForEach(x => room.Exits |= x);

			// Add random decorations to the room
			if (GameSystem.RNG.Next(0, 101) < DecorationChance)
			{				
				room.Items.AddRange(GreatTextAdventures.Items.DecorationItem.Random(GameSystem.RNG.Next(MaxDecorations)));
			}

			return room;
		}
	}
}
