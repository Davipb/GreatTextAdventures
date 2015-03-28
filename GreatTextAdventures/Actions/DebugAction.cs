using System;
using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public class DebugAction : GameAction
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "debug"; }
		}

		public override bool Do(string action)
		{
			// Arguments are obligatory
			if (string.IsNullOrWhiteSpace(action))
			{
				Console.WriteLine("Invalid command");
				return false;
			}

			string[] split = action.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length == 0)
			{
				// Show help
				Help();
			}
			else if (split[0] == "map")
			{
				if (split.Length != 3)
				{
					Console.WriteLine("Invalid number of arguments");
					return false;
				}

				int size;
				
				if (int.TryParse(split[1], out size))
				{
					try
					{
						GameSystem.CurrentMap.Draw(size).Save(split[2]);
						Console.WriteLine("Map saved successfully");
					}
					catch (Exception e)
					{
						Console.WriteLine("Error saving map: {0}", e.Message);
					}
				}
				else
				{
					Console.WriteLine("Invalid size '{0}'", split[1]);
				}								
			}
			else if (split[0] == "error")
			{
				Console.WriteLine("Throwing exception");
				throw new Exception("Controlled Exception");
			}
			else if (split[0] == "weapon")
			{
				if (split.Length != 2)
				{
					Console.WriteLine("Invalid number of arguments");
					return false;
				}

				int level;

				if (int.TryParse(split[1], out level))
				{
					Items.Weapon spawn = Items.Weapon.Random(level);
					GameSystem.CurrentMap.CurrentRoom.Members.Add(spawn);

					Console.WriteLine("Spawned {0}", spawn.DisplayName);
					return false;
				}
				else
				{					
					Console.WriteLine("Invalid level '{0}'", split[1]);
					return false;
				}
			}
			else if (split[0] == "loot")
			{
				GameSystem.CurrentMap.CurrentRoom.Members.Add(Items.LootChestItem.Random());
				Console.WriteLine("Spawned chest");
				return false;
			}
			else if (split[0] == "enemy")
			{
				if (split.Length != 2)
				{
					Console.WriteLine("Invalid number of arguments");
					return false;
				}

				int level;

				if (int.TryParse(split[1], out level))
				{
					People.ThugPerson spawn = new People.ThugPerson(level);
					GameSystem.CurrentMap.CurrentRoom.Members.Add(spawn);

					Console.WriteLine("Spawned {0}", spawn.DisplayName);
					return false;
				}
				else
				{
					Console.WriteLine("Invalid level '{0}'", split[1]);
					return false;
				}
			}

			return false;
		}

		public override void Help()
		{
			Console.WriteLine("Debug:");
			Console.WriteLine("\tdebug map *size* *file*");
			Console.WriteLine("\t\tsize: Radius, centered in 0;0, of the map to show");
			Console.WriteLine("\t\tfile: Path of the file where the map will be saved");
			Console.WriteLine("\tdebug error");
			Console.WriteLine("\tdebug weapon *level*");
			Console.WriteLine("\t\tlevel: Level of the weapon to spawn");
			Console.WriteLine("\tdebug enemy *level*");
			Console.WriteLine("\t\tlevel: Level of the enemy to spawn");
			Console.WriteLine("\tdebug loot");
		}
	}
}
