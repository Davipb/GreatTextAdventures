using GreatTextAdventures.Items.Crafting;
using GreatTextAdventures.Items.Weapons;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class LootChestItem : ILookable, IContainer
	{

		private const int CraftingRecipeChance = 10;
		private const int CraftingMaterialChance = 50;

		public string DisplayName
		{
			get
			{
				if (Content == null || !Content.Any())
					return "Chest (empty)";
				else
					return $"Chest ({Content.Count} item{(Content.Count > 1 ? "s" : "")})";
			}
		}
		public string Description => "A large wooden chest begging to be opened. What are you waiting for?";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "chest";
				yield return "box";
			}
		}
		public bool CanTake => false;

		public IList<ILookable> Content { get; } = new List<ILookable>();

		public void Open()
		{
			GameSystem.WriteLine(
				GameSystem.Enumerate(
					Content.Select(x => x.DisplayName), 
					"Inside the chest you find:", 
					null, 
					"It's empty.", 
					"and")
				);

			GameSystem.CurrentMap.CurrentRoom.Members.AddRange(Content);
			Content.Clear();

			GameSystem.WriteLine();
		}

		public static LootChestItem Random()
		{
			LootChestItem result = new LootChestItem();

			switch (GameSystem.RNG.Next(0, 2))
			{
				case 0:
					result.Content.Add(RandomWeapon.Generate(GameSystem.Player.Level));
					break;
				case 1:
					result.Content.Add(SpellTome.Random());
					break;
			}

			if (GameSystem.RNG.Next(1, 101) < CraftingRecipeChance)
				result.Content.Add(CraftingRecipe.Generate());

			if (GameSystem.RNG.Next(1, 101) <= CraftingMaterialChance)
			{
				int numb;
				switch (GameSystem.RNG.Next(0, 3))
				{
					case 0:
						numb = GameSystem.RNG.Next(1, 4);
						for (int i = 0; i < numb; i++)
							result.Content.Add(CraftingMaterial.Create("IronIngot"));
						break;
					case 1:
						result.Content.Add(CraftingMaterial.Create("Parchment"));
						break;
					case 2:
						numb = GameSystem.RNG.Next(1, 3);
						for (int i = 0; i < numb; i++)
							result.Content.Add(CraftingMaterial.Create("MagicRune"));
						break;
				}
			}

			return result;
		}

		public void Update() { /* ¯\_(ツ)_/¯ */ }
	}
}
