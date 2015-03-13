using GreatTextAdventures.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatTextAdventures.Actions
{
	public class OpenAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "open"; }
		}

		public override void Do(string action)
		{
			List<ILookable> targets = GameSystem.CurrentMap.CurrentRoom.Members.Where(x => x.CodeNames.Contains(action)).ToList();

			ILookable selected;

			if (targets.Count == 0)
			{
				Console.WriteLine("There is no '{0}'", action);
				return;
			}
			else if (targets.Count > 1)
			{
				Console.WriteLine("There are multiple '{0}'. Please specify:", action);
				selected = GameSystem.Choice<ILookable>(targets, targets.Select(x => x.DisplayName).ToList());
			}
			else
			{
				selected = targets[0];
			}

			IContainer container = selected as IContainer;

			if (container == null)
			{
				Console.WriteLine("You can't open '{0}'", selected.DisplayName);
				return;
			}

			container.Open();
		}
	}
}
