using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Room
	{
		public List<Item> Items { get; set; }
		public Directions Exits { get; set; }
		public abstract bool CanExit { get; }

		public virtual string Describe()
		{
			StringBuilder sb = new StringBuilder("a room. ");

			if (Exits != Directions.None)
			{
				sb.Append("There are exits to ");
				sb.Append(Exits.ToString());
			}			

			return sb.ToString();
		}

		public virtual void Update(bool active) 
		{
			Items.ForEach(x => x.Update(active));
		}

		public Room(Directions obligatory)
		{
			Exits = obligatory;
		}
	}	
}
