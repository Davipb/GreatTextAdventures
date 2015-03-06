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

		public override string Describe()
		{
			return "a dark room";
		}

		public override void Update(bool active)
		{
			// ¯\_(ツ)_/¯
		}

		public GenericRoom() : base()
		{
			Exits = Directions.North | Directions.East | Directions.South | Directions.West;
		}
	}
}
