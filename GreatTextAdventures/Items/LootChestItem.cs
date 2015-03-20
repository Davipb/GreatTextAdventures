using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class LootChestItem : Item, IContainer
	{
		public IList<Item> Items { get; set; }
		bool opened = false;

		public LootChestItem()
		{
			DisplayName = "chest";
			CodeNames = new[] { "chest", "box" };
			Description = "A wooden chest begging to be opened. What are you waiting for?";
			Items = new List<Item>();
		}

		public void Open()
		{
			if (opened)
			{
				Console.WriteLine("You've already emptied it");
				return;
			}

			opened = true;

			if (Items == null || !Items.Any())
			{
				Console.WriteLine("It's empty.");				
				return;
			}

			Console.Write("Inside the chest you find: ");

			foreach (var item in Items)
			{
				Console.Write(item.DisplayName + ", ");
				GameSystem.CurrentMap.CurrentRoom.Members.Add(item);
			}

			Console.WriteLine();
		}

		public static LootChestItem Random()
		{
			LootChestItem result = new LootChestItem();
			result.Items.Add(Weapon.Random());

			return result;
		}
	}
}
