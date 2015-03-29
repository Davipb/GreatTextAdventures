using GreatTextAdventures.People;
using GreatTextAdventures.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class SpellTome : ILookable, IUsable
	{
		public string DisplayName { get { return string.Format("Spell Tome ({0})", spell.DisplayName); } }
		public string Description { get { return spell.Description; } }
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

		GameSpell spell;

		public SpellTome(GameSpell containedSpell)
		{
			this.spell = containedSpell;
		}

		public void Use(Person user)
		{
			if (user.KnownSpells.Contains(spell))
			{
				Console.WriteLine("{0} already knows {1}", user.DisplayName, spell.DisplayName);
				return;
			}

			user.KnownSpells.Add(spell);
			Console.WriteLine("{0} learned {1}!", user.DisplayName, spell.DisplayName);
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
