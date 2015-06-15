using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace GreatTextAdventures.Rooms
{
	public abstract class Room
	{
		public List<ILookable> Members { get; set; }
		public Directions Exits { get; set; }
		public abstract bool CanExit { get; set; }

		public virtual string Describe()
		{
			StringBuilder sb = new StringBuilder("You are in a room. ");

			if (Exits != Directions.None)
			{						
				// Check how many exits there are				
				List<Directions> dirs = (from Directions dir in Enum.GetValues(typeof(Directions))
										 where dir != Directions.None && Exits.HasFlag(dir) 
										 select dir).ToList();

				if (dirs.Count > 1)
				{
					sb.Append("There are exits to the ");

					// Put directions in format "a, b, c, ..., d, and e"
					sb.AppendFormat("{0} ", dirs[0].ToString());

					for (int i = 1; i < dirs.Count - 1; i++)
						sb.AppendFormat(" {0},", dirs[i].ToString());

					sb.AppendFormat("and {0}. ", dirs[dirs.Count - 1].ToString());
				}
				else
				{
					sb.AppendFormat("There's an exit to the {0}. ", dirs[0]);
				}
			}
			else
			{
				sb.Append("There are no exits. ");
			}

			if (Members.Count > 0)
			{
				sb.Append("You can see: ");

				foreach(ILookable i in Members)
				{
					sb.Append(i.DisplayName);
					sb.Append(", ");
				}

				sb.Remove(sb.Length - 2, 2);
			}

			return sb.ToString();
		}

		public virtual void Update() 
		{
			// Clone the list so the original can be manipulated without generating errors
			List<ILookable> clone = new List<ILookable>(Members);

			clone.ForEach(x => x.Update());
		}
	}	
}
