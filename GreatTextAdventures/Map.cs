using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public class Map
	{
		Dictionary<string, Room> rooms;

		public int[] CurrentPosition { get; set; }
		public Room CurrentRoom { get { return rooms[PosToString(CurrentPosition)]; } }

		public Map()
		{
			CurrentPosition = new[] { 0, 0 };

			rooms = new Dictionary<string, Room>();
			rooms.Add(PosToString(CurrentPosition), GenerateRandomRoom(Directions.None));

			// Describe new location
			Console.WriteLine("You are in a {0}", CurrentRoom.Describe());
		}

		public void Move(Directions target)
		{
			if (!CurrentRoom.Exits.HasFlag(target) || !CurrentRoom.CanExit)
			{
				Console.WriteLine("You can't move {0}", Enum.GetName(typeof(Directions), target));
				return;
			}

			int[] pos = MovePosition(CurrentPosition, target);

			// If this position hasn't been explored yet, generate a random room
			if (!rooms.ContainsKey(PosToString(pos)))
			{
				rooms.Add(PosToString(pos), GenerateRandomRoom(OppositeDirection(target)));
			}

			// Move to the required position
			CurrentPosition = pos;

			// Notification of movement
			Console.WriteLine("You moved {0}", Enum.GetName(typeof(Directions), target));

			// Describe new location
			Console.WriteLine("You are in a {0}", CurrentRoom.Describe());
		}

		public void Update()
		{
			CurrentRoom.Update(true);
		}

		Room GenerateRandomRoom(Directions obligatory)
		{
			// TODO: Remove debug code

			return new Rooms.GenericRoom(obligatory);
		}

		int[] MovePosition(int[] pos, Directions dir)
		{
			if (dir.HasFlag(Directions.North)) pos[1]++;
			if (dir.HasFlag(Directions.South)) pos[1]--;
			if (dir.HasFlag(Directions.West)) pos[0]--;
			if (dir.HasFlag(Directions.East)) pos[0]++;

			return pos;
		}

		string PosToString(int[] pos)
		{
			return string.Format("X{0:0000}Y{1:0000}", pos[0], pos[1]);
		}

		Directions OppositeDirection(Directions d)
		{
			switch(d)
			{
				case Directions.North:
					return Directions.South;
				case Directions.South:
					return Directions.North;
				case Directions.East:
					return Directions.West;
				case Directions.West:
					return Directions.East;
				default:
					return Directions.None;
			}
		}
	}
}
