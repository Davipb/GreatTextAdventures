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
			ILookable found = GameSystem.GetMemberWithName(action);

			if (found == null) return;

			IContainer container = found as IContainer;

			if (container == null)
			{
				Console.WriteLine("You can't open '{0}'", found.DisplayName);
				return;
			}

			container.Open();
		}

		public override void Help()
		{
			Console.WriteLine("Open:");
			Console.WriteLine("\topen *target*");
			Console.WriteLine("\t\ttarget: What to open");
		}
	}
}
