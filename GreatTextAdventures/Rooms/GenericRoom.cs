using GreatTextAdventures.Items;
using GreatTextAdventures.Items.Crafting;
using GreatTextAdventures.People;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Rooms
{
	/// <summary>
	/// A generic room with no special events
	/// </summary>
	public class GenericRoom : Room
	{
		const int MaxDecorations = 3;
		const int DecorationChance = 25;
		const int LootChance = 50;
		const int EnemyChance = 25;
		const int ManaFountainChance = 25;
		const int CraftingStationChance = 5;

		public override bool CanExit { get; set; }

		public GenericRoom()
		{
			Exits = Directions.None;
			Members = new List<ILookable>();
			CanExit = true;
		}

		public static Room Random(Directions obligatory, Directions blocked)
		{
			var room = new GenericRoom();

			var newExits = new List<Directions>();

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
			if (GameSystem.RNG.Next(0, 100) < DecorationChance)			
				room.Members.AddRange(DecorationItem.Random(GameSystem.RNG.Next(MaxDecorations)));

			// Add random loot to the room
			if (GameSystem.RNG.Next(0, 100) < LootChance)
				room.Members.Add(Chest.Random(GameSystem.Player.Level));

			// Add random enemies to the room
			if (GameSystem.RNG.Next(0, 100) < EnemyChance)
			{
				int level = GameSystem.RNG.Next(GameSystem.Player.Level - 2, GameSystem.Player.Level + 2);
				
				switch(GameSystem.RNG.Next(0, 2))
				{
					case 0:
						room.Members.Add(new ThugPerson(level));
						break;
					case 1:
						room.Members.Add(new WizardPerson(level));
						break;
				}
			}

			if (GameSystem.RNG.Next(0, 100) < ManaFountainChance)
				room.Members.Add(new ManaFountain());

			if (GameSystem.RNG.Next(0, 100) < CraftingStationChance)
				room.Members.Add(new CraftingStation());			

			return room;
		}
	}
}
