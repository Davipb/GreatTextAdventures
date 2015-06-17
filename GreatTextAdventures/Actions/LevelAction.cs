using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Actions
{
	public class LevelAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get 
			{
				yield return "level";
			}
		}

		public override bool Do(string action)
		{
			if (GameSystem.Player.PendingSkillPoints <= 0)
			{
				Console.WriteLine("You don't have any skill points.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine
				(
					GameSystem.Enumerate<string>
					(
						new[] { "Strength (STR)", "Intelligence (INT)", "Physical Defense (PDF)", "Magical Defense (MDF)" },
						"Possible stats:",
						null,
						null,
						"or"
					)
				);
				return false;
			}

			bool success = true;

			switch(action)
			{
				case "STR":
				case "strength":					
					GameSystem.Player.Strength++;
					Console.WriteLine("{0}'s Strength increased by 1 (Total: {1})", GameSystem.Player.DisplayName, GameSystem.Player.Strength);					
					break;
				case "INT":
				case "intelligence":
					GameSystem.Player.Intelligence++;
					Console.WriteLine("{0}'s Intelligence increased by 1 (Total: {1})", GameSystem.Player.DisplayName, GameSystem.Player.Intelligence);					
					break;
				case "PDF":
				case "physical defense":
					GameSystem.Player.PhysicalDefense++;
					Console.WriteLine("{0}'s Physical Defense increased by 1 (Total: {1})", GameSystem.Player.DisplayName, GameSystem.Player.PhysicalDefense);					
					break;
				case "MDF":
				case "magical defense":
					GameSystem.Player.MagicalDefense++;
					Console.WriteLine("{0}'s Magical Defense increased by 1 (Total: {1})", GameSystem.Player.DisplayName, GameSystem.Player.MagicalDefense);
					break;
				case "defense":
					Console.WriteLine("Did you mean Physical Defense (PDF) or Magical Defense (MDF)?");
					success = false;
					break;
				default:
					Console.WriteLine("Unknown stat '{0}'. Type just 'level' for a list of stats.", action);
					success = false;
					break;
			}

			if (success)
			{
				GameSystem.Player.PendingSkillPoints--;
				Console.WriteLine("Remaining Skill Points: {0}", GameSystem.Player.PendingSkillPoints);
			}

			return false;
		}

		public override void Help()
		{
			Console.WriteLine("Level:");
			Console.WriteLine("\tlevel");
			Console.WriteLine("\t\tLists all stats");
			Console.WriteLine("\tlevel *stat*");
			Console.WriteLine("\t\tstat: Stat to level up.");
		}
	}
}
