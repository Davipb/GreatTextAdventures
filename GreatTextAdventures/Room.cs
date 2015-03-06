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

		public abstract string Describe();
		public virtual void Update(bool active) 
		{
			Items.ForEach(x => x.Update(active));
		}
	}	
}
