using GreatTextAdventures.People;
using GreatTextAdventures.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class SpellTome : ILookable, IUsable
	{
		public string DisplayName => $"Spell Tome ({spell.DisplayName})";
		public string Description => spell.Description;
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "spell tome";
				yield return "tome";
				yield return "spell";
				foreach (string codename in spell.CodeNames) yield return codename;
			}
		}
		public bool CanTake => true;

		GameSpell spell;

		public SpellTome(GameSpell containedSpell)
		{
			spell = containedSpell;
		}

		public void Use(Person user)
		{
			if (user.KnownSpells.Contains(spell))
			{
				GameSystem.WriteLine($"{user.DisplayName} already knows {spell.DisplayName}");
				return;
			}

			user.KnownSpells.Add(spell);
			GameSystem.WriteLine($"{user.DisplayName} learned {spell.DisplayName}!");
		}

		public static SpellTome Random()
		{
			// Choose a random spell with a random level (up to the player's level)
			GameSpell chosen = GameSpell.AllSpells(GameSystem.RNG.Next(1, GameSystem.Player.Level + 1))
				                         .OrderBy(x => GameSystem.RNG.Next()).First();

			return new SpellTome(chosen);
		}

		public void Update() { }

		
	}
}
