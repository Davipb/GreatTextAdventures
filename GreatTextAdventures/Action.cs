using System.Collections.Generic;

namespace GreatTextAdventures
{
	public abstract class Action
	{
		public abstract IEnumerable<string> Aliases { get; }
		public abstract void Do(string action);
		public abstract void Help();
	}
}
