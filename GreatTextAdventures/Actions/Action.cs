using System.Collections.Generic;

namespace GreatTextAdventures.Actions
{
	public abstract class GameAction
	{
		/// <summary>
		/// All possible names this action will respond to
		/// </summary>
		public abstract IEnumerable<string> Aliases { get; }
		/// <summary>
		/// Performs the action
		/// </summary>
		/// <param name="action">GameAction's parameters</param>
		/// <returns>Should the game update?</returns>
		public abstract bool Do(string action);
		/// <summary>
		/// Shows syntax help for the action
		/// </summary>
		public abstract void Help();
	}
}
