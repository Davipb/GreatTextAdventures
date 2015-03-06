using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Rooms
{
	public class GenericRoom : Room
	{
		public bool CanExit { get { return true; } }

		public string Describe()
		{
			return "a dark room";
		}
	}
}
