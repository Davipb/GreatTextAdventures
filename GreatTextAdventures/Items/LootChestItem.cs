using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class LootChestItem : ILookable, IContainer
	{
		public string DisplayName 
		{ 
			get 
			{
				if (Content == null || !Content.Any())
					return "Chest (empty)";
				else
					return "Chest";
			} 
		}
		public string Description { get { return "A large wooden chest begging to be opened. What are you waiting for?"; } }
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "chest";
				yield return "box";
			}
		}

		public IList<ILookable> Content { get; set; }

		public LootChestItem()
		{
			Content = new List<ILookable>();
		}

		public void Open()
		{
			if (Content == null || !Content.Any())
			{
				Console.WriteLine("It's empty.");				
				return;
			}

			Console.Write("Inside the chest you find: ");

			foreach (var item in Content)
			{
				Console.Write(item.DisplayName + ", ");
				GameSystem.CurrentMap.CurrentRoom.Members.Add(item);				
			}

			Content.Clear();

			Console.WriteLine();
		}

		public static LootChestItem Random()
		{
			LootChestItem result = new LootChestItem();
			result.Content.Add(Weapon.Random());

			return result;
		}

		public void Update() { /* ¯\_(ツ)_/¯ */ }
	}
}
