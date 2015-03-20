using System.Collections.Generic;

namespace GreatTextAdventures
{
	public abstract class Action
	{
		/// <summary>
		/// All possible names this action will respond to
		/// </summary>
		public abstract IEnumerable<string> Aliases { get; }
		/// <summary>
		/// Performs the action
		/// </summary>
		/// <param name="action">Action's parameters</param>
		/// <returns>Should the game update?</returns>
		public abstract bool Do(string action);
		/// <summary>
		/// Shows syntax help for the action
		/// </summary>
		public abstract void Help();
	}
}
