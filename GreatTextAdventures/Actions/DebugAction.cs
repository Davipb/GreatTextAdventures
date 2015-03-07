using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GreatTextAdventures.Actions
{
	public class DebugAction : Action
	{
		public override IEnumerable<string> Aliases
		{
			get { yield return "debug"; }
		}

		public override void Do(string action)
		{
			if (action.StartsWith("map"))
			{
				int size = int.Parse(action.Split(' ')[1]);
				action = action.Split(' ')[2];

				GameSystem.CurrentMap.Draw(size).Save(action);				
			}
		}
	}
}
