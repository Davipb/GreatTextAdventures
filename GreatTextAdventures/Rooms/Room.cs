using System.Collections.Generic;
using System.Text;

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
				sb.Append("There are exits to ");
				sb.Append(Exits.ToString());
				sb.Append(". ");
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
