using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.People
{
	public class MerchantPerson : Person
	{
		const double SellInflation = 1.3f;
		const double BuyInflation = 0.8f;

		public MerchantPerson()
		{
			DisplayName = "Merchant";
			CodeNames = new[] { "merchant", "seller" };
			Description = "The Merchant is wearing a dark coat and waving you in. \"Come in closer, friend, take a look at my wares\"";
		}

		public void AddDebug()
		{
			for (ulong i = 0; i < 50; i++)
			{
				Item random = Items.DecorationItem.Random(1).First();
				random.Price = i * 10;

				Inventory.Add(random);
			}
		}

		public override void Talk()
		{
			if (GameSystem.Choice<bool>(new[] {true, false}, new[] {"Buy", "Sell"}))
			{
				// Buying
				Item chosen = GameSystem.Choice<Item>
					(Inventory, 
					Inventory.Select(x => 
					    string.Format(
						  "{1}$\t- {0}", 
						  x.DisplayName, 
						  (ulong)((double)x.Price * SellInflation)))
					  .ToList()
					);

				Console.Clear();

				if (GameSystem.Player.Currency >= (ulong)((double)chosen.Price * SellInflation))
				{
					GameSystem.Player.Currency -= (ulong)((double)chosen.Price * SellInflation);
					GameSystem.Player.Inventory.Add(chosen);
					Inventory.Remove(chosen);

					Console.WriteLine("Thanks for the money!");
				}
				else
				{
					Console.WriteLine("Come back when you have some money, buddy");
				}
			}
			else
			{
				// Selling
				Item chosen = GameSystem.Choice<Item>
					(GameSystem.Player.Inventory,
					GameSystem.Player.Inventory.Select(x =>
						string.Format(
						  "{0}\t-\t{1}",
						  x.DisplayName,
						  (ulong)((double)x.Price * BuyInflation)))
					  .ToList()
					);

				Console.Clear();

				GameSystem.Player.Currency += (ulong)((double)chosen.Price * BuyInflation);
				GameSystem.Player.Inventory.Remove(chosen);
				Inventory.Add(chosen);

				Console.WriteLine("Good doing business with ya!");
			}
		}
	}
}
