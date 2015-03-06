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
			Console.WriteLine(action);
		}
	}
}
