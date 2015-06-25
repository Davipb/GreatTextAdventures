using System.Collections.Generic;
using System;
using GreatTextAdventures.Items;
using GreatTextAdventures.People;
using System.Linq;

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

		protected static void ListItemPossibilities(ILookable item)
		{
			List<GameAction> possible = new List<GameAction>();

			if (item as Person != null)
			{
				possible.Add(GameSystem.Actions.Find(x => x is AttackAction));
				possible.Add(GameSystem.Actions.Find(x => x is TalkAction));
			}

			if (item as Weapon != null)
			{
				possible.Add(GameSystem.Actions.Find(x => x is EquipAction));
			}

			if (item as IContainer != null)
			{
				possible.Add(GameSystem.Actions.Find(x => x is OpenAction));
			}

			if (item as IUsable != null)
			{
				possible.Add(GameSystem.Actions.Find(x => x is UseAction));
			}

			GameSystem.WriteLine(GameSystem.Enumerate<string>(possible.Select(x => x.Aliases.First()), "Try", null, "", "or"));
		}
	}
}
