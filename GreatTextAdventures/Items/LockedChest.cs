using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items
{
	public class LockedChest : Chest
	{
		public bool Locked { get; private set; } = true;

		public override string DisplayName
		{
			get
			{
				if (Locked)
					return "Locked Chest";
				return base.DisplayName;
			}
		}

		public override string Description
		{
			get
			{
				if (Locked)
					return "It has a keyhole in the front.";
				return base.Description;
			}
		}

		public LockedChest(int level) : base(level) { }

		public override void Open()
		{
			if (Locked)
			{
				if (GameSystem.Player.Inventory.OfType<ChestKey>().Any())
				{
					GameSystem.WriteLine($"{GameSystem.Player.DisplayName} unlocked the chest");
					GameSystem.WriteLine("The key is now stuck in the keyhole and won't budge");

					GameSystem.Player.Inventory.RemoveAt(
						GameSystem.Player.Inventory.FindIndex(x => x is ChestKey));

					Locked = false;

					base.Open();
				}
				else
				{
					GameSystem.WriteLine("You don't have a key!");
				}
			}
			else
			{
				base.Open();
			}
		}
	}
}
