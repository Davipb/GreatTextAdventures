using System;
using System.Collections.Generic;
using System.Drawing;

using GreatTextAdventures.Rooms;

namespace GreatTextAdventures
{
	public class Map
	{
		Dictionary<Tuple<int, int>, Room> rooms;

		public Tuple<int, int> CurrentPosition { get; set; }
		public Room CurrentRoom { get { return rooms[CurrentPosition]; } }

		public Map()
		{
			CurrentPosition = Tuple.Create(0, 0);

			rooms = new Dictionary<Tuple<int, int>, Room>();
			GenerateRandomRoom(CurrentPosition);

			// Describe new location
			GameSystem.WriteLine(CurrentRoom.Describe());
		}

		public void Move(Directions target)
		{
			if (!CurrentRoom.Exits.HasFlag(target) || !CurrentRoom.CanExit)
			{
				GameSystem.WriteLine("You can't move {0}", Enum.GetName(typeof(Directions), target));
				return;
			}

			Tuple<int, int> pos = MovePosition(CurrentPosition, target);

			// If this position hasn't been explored yet, generate a random room
			if (!rooms.ContainsKey(pos))
			{
				GenerateRandomRoom(pos);
			}

			// Move to the required position
			CurrentPosition = pos;

			// Notification of movement
			GameSystem.WriteLine("You moved {0}", Enum.GetName(typeof(Directions), target));

			// Describe new location
			GameSystem.WriteLine(CurrentRoom.Describe());
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
					var pos = Tuple.Create(mapX, mapY);

					int imageX = mapX + size;
					int imageY = mapY + size;

					if (rooms.ContainsKey(pos))
					{
						// Corners
						result.SetPixel(imageX * 3, imageY * 3, Color.Gray);
						result.SetPixel((imageX * 3) + 2, imageY * 3, Color.Gray);
						result.SetPixel(imageX * 3, (imageY * 3) + 2, Color.Gray);
						result.SetPixel((imageX * 3) + 2, (imageY * 3) + 2, Color.Gray);

						// Exits
						result.SetPixel((imageX * 3) + 1, (imageY * 3), rooms[pos].Exits.HasFlag(Directions.North)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3) + 1, (imageY * 3) + 2, rooms[pos].Exits.HasFlag(Directions.South)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3), (imageY * 3) + 1, rooms[pos].Exits.HasFlag(Directions.West)? Color.Blue : Color.Gray);
						result.SetPixel((imageX * 3) + 2, (imageY * 3) + 1, rooms[pos].Exits.HasFlag(Directions.East)? Color.Blue : Color.Gray);

						// Center
						result.SetPixel((imageX * 3) + 1, (imageY * 3) + 1, mapX == CurrentPosition.Item1 && mapY == CurrentPosition.Item2? Color.Green : Color.Blue);
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

		void GenerateRandomRoom(Tuple<int, int> pos)
		{
			Directions obligatory = Directions.None;
			Directions blocked = Directions.None;

			// Check every possible direction to see if a room already exists there
			foreach(Directions d in Enum.GetValues(typeof(Directions)))
			{
				Tuple<int, int> newpos = MovePosition(pos, d);

				if (rooms.ContainsKey(pos))
				{
					if (rooms[pos].Exits.HasFlag(OppositeDirection(d)))
					{
						// Adjacent room exists has exit to this room, so we need an exit to that side
						obligatory |= d;
					}
					else
					{
						// Adjacent room exists but doesn't have exit to this room, so we can't have an exit to that side
						blocked |= d;
					}
				}
			}

			rooms.Add(pos, Rooms.GenericRoom.Random(obligatory, blocked));
			
		}

		Tuple<int, int> MovePosition(Tuple<int, int> pos, Directions dir)
		{
			int x = pos.Item1;
			int y = pos.Item2;

			if (dir.HasFlag(Directions.North)) y--;
			if (dir.HasFlag(Directions.South)) y++;
			if (dir.HasFlag(Directions.West)) x--;
			if (dir.HasFlag(Directions.East)) x++;

			return Tuple.Create(x, y);
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
