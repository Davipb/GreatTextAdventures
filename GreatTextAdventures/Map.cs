using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

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
			GenerateRandomRoom(CurrentPosition);

			People.MerchantPerson merc = new People.MerchantPerson();
			merc.AddDebug();

			CurrentRoom.Members.Add(merc);

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
				GenerateRandomRoom(pos);
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
			CurrentRoom.Update();
		}

		public Bitmap Draw(int size)
		{
			Bitmap result = new Bitmap(size * 2 * 3, size * 2 * 3);

			for (int mapY = -size + 1; mapY < size; mapY++)
			{
				for (int mapX = -size + 1; mapX < size; mapX++)
				{
					int[] pos = new[] {mapX, mapY};
					string key = PosToString(pos);

					int imageX = mapX + size;
					int imageY = mapY + size;

					if (rooms.ContainsKey(key))
					{
						// Corners
						result.SetPixel(imageX * 3, imageY * 3, Color.Gray);
						result.SetPixel((imageX * 3) + 2, imageY * 3, Color.Gray);
						result.SetPixel(imageX * 3, (imageY * 3) + 2, Color.Gray);
						result.SetPixel((imageX * 3) + 2, (imageY * 3) + 2, Color.Gray);

						// Exits
						result.SetPixel((imageX * 3) + 1, (imageY * 3), rooms[key].Exits.HasFlag(Directions.North)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3) + 1, (imageY * 3) + 2, rooms[key].Exits.HasFlag(Directions.South)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3), (imageY * 3) + 1, rooms[key].Exits.HasFlag(Directions.West)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3) + 2, (imageY * 3) + 1, rooms[key].Exits.HasFlag(Directions.East)? Color.Blue : Color.Gray);

						// Center
						result.SetPixel((imageX * 3) + 1, (imageY * 3) + 1, mapX == CurrentPosition[0] && mapY == CurrentPosition[1]? Color.Green : Color.Blue);
					}
					else
					{
						for (int filly = 0; filly < 3; filly++)
						{
							for (int fillx = 0; fillx < 3; fillx++)
							{
								result.SetPixel((imageX * 3) + fillx, (imageY * 3) + filly, Color.Black);
							}
						}
					}
					
				}
			}

			return result;
		}

		void GenerateRandomRoom(int[] pos)
		{
			Directions obligatory = Directions.None;
			Directions blocked = Directions.None;

			// Check each possible exit
			foreach(Directions d in Enum.GetValues(typeof(Directions)))
			{
				int[] newpos = MovePosition(pos, d);
				string key = PosToString(newpos);

				if (rooms.ContainsKey(key))
				{
					if (rooms[key].Exits.HasFlag(OppositeDirection(d)))
					{
						// Generated room must have exit to that side
						obligatory |= d;
					}
					else
					{
						// Generated room must be blocked to that side
						blocked |= d;
					}
				}
			}

			rooms.Add(PosToString(pos), Rooms.GenericRoom.Random(obligatory, blocked));
			
		}

		int[] MovePosition(int[] pos, Directions dir)
		{
			int[] result = new[] { pos[0], pos[1] };

			if (dir.HasFlag(Directions.North)) result[1]--;
			if (dir.HasFlag(Directions.South)) result[1]++;
			if (dir.HasFlag(Directions.West)) result[0]--;
			if (dir.HasFlag(Directions.East)) result[0]++;

			return result;
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
