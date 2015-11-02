using GreatTextAdventures.Items;
using GreatTextAdventures.Items.Crafting;
using GreatTextAdventures.People;

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

		public override bool CanExit { get; protected set; } = true;

		public static Room Random(Directions obligatory, Directions blocked)
		{
			var room = new GenericRoom();

			room.Exits = GenerateExits(obligatory, blocked);

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

				switch (GameSystem.RNG.Next(0, 2))
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
