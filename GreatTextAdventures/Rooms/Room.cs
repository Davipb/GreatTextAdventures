using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace GreatTextAdventures.Rooms
{
	/// <summary>
	/// Base class for Rooms, contained in a Map
	/// </summary>
	public abstract class Room : IUpdatable
	{
		public List<ILookable> Members { get; set; }
		public Directions Exits { get; set; }
		public abstract bool CanExit { get; set; }

		public virtual string Description
		{
			get
			{
				var sb = new StringBuilder("You are in a room. ");

				// Check how many exits there are				
				List<Directions> dirs = (from Directions dir in Enum.GetValues(typeof(Directions))
										 where dir != Directions.None && Exits.HasFlag(dir)
										 select dir).ToList();

				sb.Append(GameSystem.Enumerate(Members.Select(x => x.DisplayName), "You can see:", null, "There's nothing in it", "and"));
				sb.Append(". ");
				sb.Append(GameSystem.Enumerate(dirs, "There are exits to the", "There's an exit to the", "There are no exits", "and"));
				sb.Append(". ");				

				return sb.ToString();
			}
		}

		public virtual void Update() 
		{
			// ToList() used to prevent LINQ's lazy evaluation, since the base collection may be modified
			var members = Members.OfType<IUpdatable>().ToList();
			foreach (var item in members) item.Update();
		}
	}	
}
