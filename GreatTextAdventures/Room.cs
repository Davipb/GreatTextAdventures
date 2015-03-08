using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Room
	{
		public List<ILookable> Members { get; set; }
		public Directions Exits { get; set; }
		public abstract bool CanExit { get; set; }

		public virtual string Describe()
		{
			StringBuilder sb = new StringBuilder("a room. ");

			if (Exits != Directions.None)
			{
				sb.Append("There are exits to ");
				sb.Append(Exits.ToString());
				sb.Append(". ");
			}

			if (Members.Count > 0)
			{
				sb.Append("You can see: ");

				foreach(Item i in Members)
				{
					sb.Append(i.DisplayName);
					sb.Append(", ");
				}

				sb.Remove(sb.Length - 2, 2);
				sb.Append(". ");
			}

			return sb.ToString();
		}

		public virtual void Update() 
		{
			Members.ForEach(x => x.Update());
		}
	}	
}
