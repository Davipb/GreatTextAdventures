using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items
{
	public class LootChestItem : Item, IContainer
	{
		bool opened = false;
		IEnumerable<Item> items;

		public LootChestItem()
		{
			DisplayName = "chest";
			CodeNames = new[] { "chest", "box" };
			Description = "A wooden chest begging to be opened. What are you waiting for?";	
		}

		public void Open()
		{
			if (opened)
			{
				Console.WriteLine("You've already emptied it");
				return;
			}

			opened = true;

			if (items == null || !items.Any())
			{
				Console.WriteLine("It's empty.");				
				return;
			}

			Console.Write("Inside the chest you find: ");

			foreach (var item in items)
			{
				Console.Write(item.DisplayName + ", ");
				GameSystem.CurrentMap.CurrentRoom.Members.Add(item);
			}

			Console.WriteLine();
		}

		public static LootChestItem Random()
		{
			return new LootChestItem();
		}
	}
}
