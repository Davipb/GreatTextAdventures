using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public class Map
	{
		Dictionary<int[], Room> rooms;

		public int[] CurrentPosition { get; set; }
		public Room CurrentRoom { get { return rooms[CurrentPosition]; } }

		public Map()
		{
			CurrentPosition = new[] { 0, 0 };

			rooms = new Dictionary<int[], Room>();
			rooms.Add(CurrentPosition, GenerateRandomRoom());
			
		}

		public void Move(Directions target)
		{
			if (!CurrentRoom.Exits.HasFlag(target) || !CurrentRoom.CanExit) return;

			int[] pos = MovePosition(CurrentPosition, target);

			// If this position hasn't been explored yet, generate a random room
			if (!rooms.ContainsKey(pos))
			{
				rooms.Add(pos, GenerateRandomRoom());
			}

			// Move to the required position
			CurrentPosition = pos;
		}

		public void Update()
		{
			CurrentRoom.Update(true);
		}

		Room GenerateRandomRoom()
		{
			// TODO: Remove debug code

			return new Rooms.GenericRoom();
		}

		int[] MovePosition(int[] pos, Directions dir)
		{
			if (dir.HasFlag(Directions.North)) pos[1]++;
			if (dir.HasFlag(Directions.South)) pos[1]--;
			if (dir.HasFlag(Directions.West)) pos[0]--;
			if (dir.HasFlag(Directions.East)) pos[0]++;

			return pos;
		}
	}
}
