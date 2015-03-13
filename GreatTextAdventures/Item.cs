using System;
using System.Collections.Generic;

namespace GreatTextAdventures
{
	public abstract class Item : ILookable
	{
		public string DisplayName { get; set; }
		public IEnumerable<string> CodeNames { get; set; }
		public string Description { get; set; }

		public bool CanTake { get; set; }

		public Item()
		{
			DisplayName = "ERROR";
			CodeNames = new[] { "error" };
			Description = "SOMETHING BROKE";

			CanTake = false;
		}

		public void Update() { /* ¯\_(ツ)_/¯ */ }
		public void Use()
		{
			Console.WriteLine("You can't use {0}.", DisplayName);
		}
	}
}
