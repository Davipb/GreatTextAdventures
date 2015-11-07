using GreatTextAdventures.People;
using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public class GraveyardSign : ILookable, IUsable
	{
		bool Raveyard = false;

		public bool CanTake => false;

		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "sign";
				yield return "graveyard sign";
			}
		}

		public string Description
		{
			get
			{
				if (Raveyard)
					return "The now glowing sign says '&C12R&CEE&C13a&CEE&C11v&CEE&C10e&CEE&C09y&CEE&C15a&CEE&C14r&CEE&C10d&CEE'";
				else
					return "A big sign warns you that you are in a Graveyard. Spooky.";
			}
		}

		public string DisplayName => "Graveyard Sign";

		public void Use(Person user)
		{
			if (user != GameSystem.Player)
				return;

			if (Raveyard)
			{
				GameSystem.WriteLine("You can't improve this sign any more");
				return;
			}

			GameSystem.WriteLine("You knock the 'G' off the sign. It starts to glow.");
			Raveyard = true;
		}
	}
}
