using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Action
	{
		public abstract IEnumerable<string> Aliases { get; }
		public abstract void Do(string action);
	}
}
