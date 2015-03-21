using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GreatTextAdventures.Spells;
using GreatTextAdventures.People;

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
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Cast what?");
				return false;
			}

			// Command format is "cast spell at target", so we divide it at "at"
			string[] split = action.Split(new[] { "at" }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length != 2)
			{
				Console.WriteLine("Invalid target");
				return false;
			}

			GameSpell spell = GameSystem.Player.GetSpellWithName(split[0].Trim());
			if (spell == null) return false;

			ILookable found = GameSystem.GetMemberWithName(split[1].Trim());
			if (found == null) return false;

			Person target = found as Person;

			if (target == null)
			{
				Console.WriteLine("Can't target {0}", found.DisplayName);
				return false;
			}

			return spell.Cast(GameSystem.Player, target);
		}

		public override void Help()
		{
			Console.WriteLine("Cast:");
			Console.WriteLine("\tcast *spell* at *target*");
			Console.WriteLine("\t\tspell: Spell to cast");
			Console.WriteLine("\t\ttarget: Spell target");
		}
	}
}
