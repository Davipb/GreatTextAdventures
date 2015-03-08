using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Actions
{
	public class LookAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "look"; }
		}

		public override void Do(string action)
		{
			if (action.StartsWith("at"))
			{
				action = action.Substring(2).Trim();
			}

			if (string.IsNullOrWhiteSpace(action) || action == "around" || action == "room")
			{
				// Just look at the room, in general
				Console.WriteLine("You are in a {0}", GameSystem.CurrentMap.CurrentRoom.Describe());
			}
			else
			{
				Item[] found = GameSystem.CurrentMap.CurrentRoom.Items.Where(x => x.CodeNames.Contains(action)).ToArray();

				if (found.Length == 0)
				{
					Console.WriteLine("There is no '{0}'.", action);
				}
				else if (found.Length > 1)
				{
					Console.WriteLine("There are multiple '{0}'. Please specify:", action);

					for (int i = 0; i < found.Length; i++)
					{
						Console.WriteLine("{0}. {1}", i + 1, found[i].DisplayName);
					}

					while(true)
					{
						int pressed;

						if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out pressed))
						{
							if (pressed > 0 && pressed <= found.Length)
							{								
								Console.WriteLine(found[pressed - 1].Description);
								break;
							}
						}
					}
				}
				else
				{
					Console.WriteLine(found[0].Description);
				}
			}
			
		}
	}
}
