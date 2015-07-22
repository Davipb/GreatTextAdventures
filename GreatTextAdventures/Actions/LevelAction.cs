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
				GameSystem.WriteLine("You don't have any skill points.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(action))
			{
				GameSystem.WriteLine
				(
					GameSystem.Enumerate
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
				case "str":
				case "strength":					
					GameSystem.Player.Strength++;
					GameSystem.WriteLine($"{GameSystem.Player.DisplayName}'s Strength increased by 1 (Total: {GameSystem.Player.Strength})");					
					break;
				case "int":
				case "intelligence":
					GameSystem.Player.Intelligence++;
					GameSystem.WriteLine($"{GameSystem.Player.DisplayName}'s Intelligence increased by 1 (Total: {GameSystem.Player.Intelligence})");					
					break;
				case "pdf":
				case "physical defense":
					GameSystem.Player.PhysicalDefense++;
					GameSystem.WriteLine($"{GameSystem.Player.DisplayName}'s Physical Defense increased by 1 (Total: {GameSystem.Player.PhysicalDefense})");					
					break;
				case "mdf":
				case "magical defense":
					GameSystem.Player.MagicalDefense++;
					GameSystem.WriteLine($"{GameSystem.Player.DisplayName}'s Magical Defense increased by 1 (Total: {GameSystem.Player.MagicalDefense})");
					break;
				default:
					GameSystem.WriteLine($"Unknown stat '{action}'. Type just 'level' for a list of stats.");
					success = false;
					break;
			}

			if (success)
			{
				GameSystem.Player.PendingSkillPoints--;
				GameSystem.WriteLine($"Remaining Skill Points: {GameSystem.Player.PendingSkillPoints}");
			}

			return false;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Level:");
			GameSystem.WriteLine("\tlevel");
			GameSystem.WriteLine("\t\tLists all stats");
			GameSystem.WriteLine("\tlevel *stat*");
			GameSystem.WriteLine("\t\tstat: Stat to level up.");
		}
	}
}
