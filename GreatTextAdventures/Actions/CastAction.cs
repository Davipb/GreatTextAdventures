using GreatTextAdventures.People;
using GreatTextAdventures.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class CastAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "cast"; }
		}

		public override bool Do(string action)
		{
			if (!GameSystem.Player.KnownSpells.Any())
			{
				GameSystem.WriteLine("You don't know any spells");
				return false;
			}

			if (string.IsNullOrWhiteSpace(action))
			{				
				foreach(GameSpell knownSpell in GameSystem.Player.KnownSpells)
				{
					GameSystem.WriteLine(knownSpell.DisplayName);
					GameSystem.WriteLine("Cost: {0} mana", knownSpell.Cost);
					GameSystem.WriteLine(knownSpell.Description);
					GameSystem.WriteLine();	
				}
				return false;
			}

			// Command format is "cast spell at target", so we divide it at "at"
			string[] split = action.Split(new[] { "at" }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length != 2)
			{
				GameSystem.WriteLine("Invalid target");
				return false;
			}

			GameSpell spell = GameSystem.Player.GetSpellWithName(split[0].Trim());
			if (spell == null) return false;

			ILookable found = GameSystem.GetMemberWithName(split[1].Trim());
			if (found == null) return false;

			Person target = found as Person;

			if (target == null)
			{
				GameSystem.WriteLine("Can't target {0}", found.DisplayName);
				return false;
			}

			return spell.Cast(GameSystem.Player, target);
		}

		public override void Help()
		{
			GameSystem.WriteLine("Cast:");
			GameSystem.WriteLine("\tcast *spell* at *target*");
			GameSystem.WriteLine("\t\tspell: Spell to cast");
			GameSystem.WriteLine("\t\ttarget: Spell target");
		}
	}
}
