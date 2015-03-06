using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Rooms
{
	public class GenericRoom : Room
	{
		public override bool CanExit { get { return true; } }		

		public override void Update(bool active)
		{
			// ¯\_(ツ)_/¯
		}

		public GenericRoom(Directions obligatory = Directions.None) : base(obligatory)
		{
			foreach(Directions d in Enum.GetValues(typeof(Directions)))
			{
				if (System.RNG.Next(1, 101) % 2 == 0)
				{
					Exits |= d;
				}
			}

			Items = new List<Item>();
		}
	}
}
