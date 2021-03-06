﻿using GreatTextAdventures.People;
using System;
using System.Collections.Generic;
using System.Linq;

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

		public void Use(Person user)
		{
			if (user != GameSystem.Player)
				return;

			var recipes = GameSystem.Player.Inventory.OfType<CraftingRecipe>();

			if (!recipes.Any())
			{
				GameSystem.WriteLine("You don't have any recipes!");
				return;
			}

			var chosenRecipe = GameSystem.Choice(recipes.Select(x => Tuple.Create(x, x.DisplayName)).ToList());

			// Get the number of materials of each type in the Player's inventory
			var groupedMaterials =
				GameSystem.Player.Inventory
				.OfType<CraftingMaterial>()
				.GroupBy(x => x.MaterialName)
				.ToDictionary(k => k.Key, e => e.Count());

			// Check if the Player's inventory has the right amount of ingredients
			foreach (var ingredient in chosenRecipe.Ingredients)
			{
				if (!groupedMaterials.ContainsKey(ingredient.Key) || groupedMaterials[ingredient.Key] < ingredient.Value)
				{
					var name = (string)CraftingMaterial.AllMaterials[ingredient.Key]["DisplayName"];
					int have = groupedMaterials.ContainsKey(ingredient.Key) ? groupedMaterials[ingredient.Key] : 0;
					GameSystem.WriteLine(
						$"Not enough {name}! Needed: {ingredient.Value}, Have: {have}");
					return;
				}
			}

			// Remove ingredients from player's inventory
			foreach (var ingredient in chosenRecipe.Ingredients)
			{
				for (int i = 0; i < ingredient.Value; i++)
					GameSystem.Player.Inventory.RemoveAt(
						GameSystem.Player.Inventory.FindIndex(
							x => x is CraftingMaterial && ((CraftingMaterial)x).MaterialName == ingredient.Key));
			}

			GameSystem.WriteLine($"{GameSystem.Player.DisplayName} crafted {chosenRecipe.Result.DisplayName}!");
			GameSystem.Player.Inventory.Add(chosenRecipe.Result);

			GameSystem.WriteLine("The recipe was destroyed in the process.");
			GameSystem.Player.Inventory.Remove(chosenRecipe);
		}
	}
}
