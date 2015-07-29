using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public class ChestKey : ILookable
	{
		public string DisplayName => "Chest Key";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "chest key";
				yield return "key";
			}
		}
		public string Description => "Can be used to open a Locked Chest.";
		public bool CanTake => true;

		public void Update() { }
	}
}
