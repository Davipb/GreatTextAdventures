using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace GreatTextAdventures.Rooms
{
	/// <summary>
	/// Base class for Rooms, contained in a Map
	/// </summary>
	public abstract class Room
	{
		public List<ILookable> Members { get; set; }
		public Directions Exits { get; set; }
		public abstract bool CanExit { get; set; }

		public virtual string Description
		{
			get
			{
				StringBuilder sb = new StringBuilder("You are in a room. ");

				// Check how many exits there are				
				List<Directions> dirs = (from Directions dir in Enum.GetValues(typeof(Directions))
										 where dir != Directions.None && Exits.HasFlag(dir)
										 select dir).ToList();

				sb.Append(GameSystem.Enumerate<Directions>(dirs, "There are exits to the", "There's an exit to the", "There are no exits", "and"));
				sb.Append(". ");
				sb.Append(GameSystem.Enumerate<string>(Members.Select(x => x.DisplayName), "You can see:", null, "There's nothing in it", "and"));

				return sb.ToString();
			}
		}

		public virtual void Update() 
		{
			// Clone the list so the original can be manipulated without generating errors
			List<ILookable> clone = new List<ILookable>(Members);

			clone.ForEach(x => x.Update());
		}
	}	
}
