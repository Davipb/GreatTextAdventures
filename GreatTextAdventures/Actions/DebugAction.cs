using GreatTextAdventures.People;
using GreatTextAdventures.StatusEffects;
using GreatTextAdventures.Items.Crafting;
using System;
using System.Collections.Generic;
using System.Linq;
using GreatTextAdventures.Items.Weapons;

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
				GameSystem.WriteLine("Invalid command");
				return false;
			}

			string[] split = action.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length == 0)
			{
				// Show help
				Help();
			}
			#region Map
			else if (split[0] == "map")
			{
				if (split.Length != 3)
				{
					GameSystem.WriteLine("Invalid number of arguments");
					return false;
				}

				int size;
				
				if (int.TryParse(split[1], out size))
				{
					try
					{
						GameSystem.CurrentMap.Draw(size).Save(split[2]);
						GameSystem.WriteLine("Map saved successfully");
					}
					catch (Exception e)
					{
						GameSystem.WriteLine("Error saving map: {0}", e.Message);
					}
				}
				else
				{
					GameSystem.WriteLine("Invalid size '{0}'", split[1]);
				}
			}
			#endregion
			#region Error
			else if (split[0] == "error")
			{
				GameSystem.WriteLine("Throwing exception");
				throw new Exception("Controlled Exception");
			}
			#endregion
			#region Weapon
			else if (split[0] == "weapon")
			{
				if (split.Length != 2)
				{
					GameSystem.WriteLine("Invalid number of arguments");
					return false;
				}

				int level;

				if (int.TryParse(split[1], out level))
				{
					Weapon spawn = RandomWeapon.Random(level);
					GameSystem.CurrentMap.CurrentRoom.Members.Add(spawn);

					GameSystem.WriteLine("Spawned {0}", spawn.DisplayName);
					return false;
				}
				else
				{					
					GameSystem.WriteLine("Invalid level '{0}'", split[1]);
					return false;
				}
			}
			#endregion
			#region Loot
			else if (split[0] == "loot")
			{
				GameSystem.CurrentMap.CurrentRoom.Members.Add(Items.LootChestItem.Random());
				GameSystem.WriteLine("Spawned chest");
				return false;
			}
			#endregion
			#region Enemy
			else if (split[0] == "enemy")
			{
				if (split.Length != 3)
				{
					GameSystem.WriteLine("Invalid number of arguments");
					return false;
				}

				int level;

				if (int.TryParse(split[2], out level))
				{
					switch(split[1])
					{
						case "thug":
							GameSystem.CurrentMap.CurrentRoom.Members.Add(new ThugPerson(level));
							GameSystem.WriteLine("Spawned Thug level {0}", level);
							break;
						case "wizard":
							GameSystem.CurrentMap.CurrentRoom.Members.Add(new WizardPerson(level));
							GameSystem.WriteLine("Spawned Wizard level {0}", level);
							break;
						default:
							GameSystem.WriteLine("Unknown enemy type '{0}'", split[1]);
							break;
					}

					return false;
				}
				else
				{
					GameSystem.WriteLine("Invalid level '{0}'", split[1]);
					return false;
				}
			}
			#endregion
			#region Heal
			else if (split[0] == "heal")
			{
				GameSystem.Player.Health = GameSystem.Player.MaxHealth;
				GameSystem.Player.Mana = GameSystem.Player.MaxMana;

				GameSystem.WriteLine("Restored Player's health and mana");
				return false;
			}
			#endregion
			#region Effect
			else if (split[0] == "effect")
			{
				if (split.Length != 4)
				{
					GameSystem.WriteLine("Invalid number of arguments");
					return false;
				}

				// First argument is target
				Person target = GameSystem.GetLookableWithName(split[1]) as Person;

				if (target == null)
				{
					GameSystem.WriteLine("Invalid target '{0}'", split[1]);
					return false;
				}

				// Second argument is duration
				int duration;
				
				if (!int.TryParse(split[2], out duration))
				{
					GameSystem.WriteLine("Invalid duration '{0}'", split[2]);
					return false;
				}

				// Third argument is effect name
				StatusEffect effect = null;

				switch(split[3].ToLowerInvariant())
				{
					case "poison":
						effect = new PoisonEffect(target, duration);
						break;
					default:
						GameSystem.WriteLine("Invalid effect name '{0}'", split[3]);
						return false;
				}

				target.CurrentStatus.Add(effect);

				GameSystem.WriteLine("Added {0} to {1} for {2} turns", effect.DisplayName, target.DisplayName, duration);
			}
			#endregion
			#region Skill Points
			else if (split[0] == "skill")
			{
				if (split.Length < 2)
				{
					GameSystem.WriteLine("Invalid number of arguments");
					return false;
				}

				int total;

				if (!int.TryParse(split[1], out total))
				{
					GameSystem.WriteLine("Invalid number '{0}'", split[1]);
					return false;
				}

				GameSystem.Player.PendingSkillPoints += total;
				GameSystem.WriteLine("Added {0} skill points", total);
				return false;
			}
			#endregion
			#region Crafting
			else if (split[0] == "crafting")
			{
				var dict = new Dictionary<string, int>();
				dict.Add("IronIngot", 5);
				GameSystem.CurrentMap.CurrentRoom.Members.Add(new CraftingRecipe(dict, RandomWeapon.Random(10)));

				for (int i = 0; i < 5; i++)
					GameSystem.CurrentMap.CurrentRoom.Members.Add(CraftingMaterial.Create("IronIngot"));

				GameSystem.CurrentMap.CurrentRoom.Members.Add(new CraftingStation());

				GameSystem.WriteLine("Added crafting supplies");
			}
			#endregion

			return false;
		}

		public override void Help()
		{
			GameSystem.WriteLine("Debug:");
			GameSystem.WriteLine("\tdebug map *size* *file*");
			GameSystem.WriteLine("\t\tsize: Radius, centered in 0;0, of the map to show");
			GameSystem.WriteLine("\t\tfile: Path of the file where the map will be saved");
			GameSystem.WriteLine("\tdebug error");
			GameSystem.WriteLine("\tdebug weapon *level*");
			GameSystem.WriteLine("\t\tlevel: Level of the weapon to spawn");
			GameSystem.WriteLine("\tdebug loot");
			GameSystem.WriteLine("\tdebug enemy *type* *level*");
			GameSystem.WriteLine("\t\ttype: thug, wizard");
			GameSystem.WriteLine("\t\tlevel: Level of the enemy to spawn");
			GameSystem.WriteLine("\tdebug heal");
			GameSystem.WriteLine("\tdebug effect *target* *duration* *name*");
			GameSystem.WriteLine("\t\ttarget: Target of the effect");
			GameSystem.WriteLine("\t\tduration: Duration, in turns, of the effect");
			GameSystem.WriteLine("\t\teffect: Name of the effect to add");
			GameSystem.WriteLine("\tdebug skill *number*");
			GameSystem.WriteLine("\t\tnumber: Number of skill points to add");
		}
	}
}
