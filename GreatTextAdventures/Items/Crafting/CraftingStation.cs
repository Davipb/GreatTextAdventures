using GreatTextAdventures.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items.Crafting
{
	public class CraftingStation : ILookable, IUsable
	{
		public string DisplayName => "Anvil";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "anvil";
			}
		}
		public string Description => "Looks like you can craft items here.";
		public bool CanTake => false;
		public void Update() { }

		public void Use(Person user)
		{
			if (user != GameSystem.Player)
				return;

			var recipes = GameSystem.Player.Inventory.Where(x => x is CraftingRecipe).Select(x => (CraftingRecipe)x);

			if (!recipes.Any())
			{
				GameSystem.WriteLine("You don't have any recipes!");
				return;
			}

			var chosenRecipe = GameSystem.Choice(recipes.Select(x => Tuple.Create(x, x.DisplayName)).ToList());
			// Get the number of items of each type in the Player's inventory
			var groupedInventory = GameSystem.Player.Inventory.GroupBy(x => x.GetType()).ToDictionary(k => k.Key, e => e.Count());

			// Check if the Player's inventory has the right amount of ingredients
			foreach (var ingredient in chosenRecipe.Ingredients)
			{
				Type ingredientType = ingredient.Key.GetType();

				if (!groupedInventory.ContainsKey(ingredientType) || groupedInventory[ingredientType] < ingredient.Value)
				{
					int have = groupedInventory.ContainsKey(ingredientType) ? groupedInventory[ingredientType] : 0;
					GameSystem.WriteLine(
						$"Not enough {ingredient.Key.DisplayName}! Needed: {ingredient.Value}, Have: {have}");
					return;
				}
			}

			// Remove ingredients from player's inventory
			foreach (var ingredient in chosenRecipe.Ingredients)
			{
				for (int i = 0; i < ingredient.Value; i++)
					GameSystem.Player.Inventory.RemoveAt(
						GameSystem.Player.Inventory.FindIndex(
							x => x.GetType() == ingredient.Key.GetType()));
			}

			GameSystem.WriteLine($"{GameSystem.Player.DisplayName} crafted {chosenRecipe.DisplayName}!");
			GameSystem.Player.Inventory.Add(chosenRecipe.Result);
        }
	}
}
