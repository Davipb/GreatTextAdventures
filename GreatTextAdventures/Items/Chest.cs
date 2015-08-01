using GreatTextAdventures.Items.Crafting;
using GreatTextAdventures.Items.Weapons;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class Chest : ILookable, IContainer
	{
		private const int LockedChestChance = 5;
		private const int LockedChestExtraLevels = 2;
		private const int LockedChestKeyChance = 15;
		private const int CraftingChestChance = 5;
		private const int CraftingRecipeChance = 10;
		private const int CraftingMaterialChance = 50;

		public virtual string DisplayName
		{
			get
			{
				if (Content == null || !Content.Any())
					return "Chest (empty)";
				else
					return $"Chest ({Content.Count} item{(Content.Count > 1 ? "s" : "")})";
			}
		}
		public virtual string Description => "A large wooden chest begging to be opened. What are you waiting for?";
		public virtual IEnumerable<string> CodeNames
		{
			get
			{
				yield return "chest";
				yield return "box";
			}
		}
		public bool CanTake => false;

		public IList<ILookable> Content { get; } = new List<ILookable>();

		public Chest(int level)
		{
			PopulateContent(level);
		}

		public virtual void Open()
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

		public static Chest Random(int level)
		{
			if (GameSystem.RNG.Next(0, 100) < LockedChestChance)
				return new LockedChest(level + LockedChestExtraLevels);
			else if (GameSystem.RNG.Next(0, 100) < CraftingChestChance)
				return new CraftingChest(level);
			else
				return new Chest(level);
		}

		protected virtual void PopulateContent(int level)
		{
			switch (GameSystem.RNG.Next(0, 2))
			{
				case 0:
					Content.Add(RandomWeapon.Generate(level));
					break;
				case 1:
					Content.Add(SpellTome.Random(level));
					break;
			}

			if (GameSystem.RNG.Next(0, 100) < CraftingRecipeChance)
				Content.Add(CraftingRecipe.Generate(level));

			if (GameSystem.RNG.Next(0, 100) < LockedChestKeyChance)
				Content.Add(new ChestKey());

			if (GameSystem.RNG.Next(0, 100) <= CraftingMaterialChance)
			{
				int numb;
				switch (GameSystem.RNG.Next(0, 3))
				{
					case 0:
						numb = GameSystem.RNG.Next(1, 4);
						for (int i = 0; i < numb; i++)
							Content.Add(CraftingMaterial.Create("IronIngot"));
						break;
					case 1:
						Content.Add(CraftingMaterial.Create("Parchment"));
						break;
					case 2:
						numb = GameSystem.RNG.Next(1, 3);
						for (int i = 0; i < numb; i++)
							Content.Add(CraftingMaterial.Create("MagicRune"));
						break;
				}
			}
		}
	}
}
