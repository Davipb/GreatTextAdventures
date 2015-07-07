using System;
using System.Collections.Generic;
using System.Drawing;

using GreatTextAdventures.Rooms;

namespace GreatTextAdventures
{
	/// <summary>
	/// Represents a series of Rooms organized in x and y coordinates.
	/// </summary>
	public class Map
	{
		Dictionary<Tuple<int, int>, Room> rooms;

		public Tuple<int, int> CurrentPosition { get; set; }
		public Room CurrentRoom { get { return rooms[CurrentPosition]; } }

		/// <summary>
		/// Creates a new instance of Map, with the default values.
		/// </summary>
		public Map()
		{
			CurrentPosition = Tuple.Create(0, 0);

			rooms = new Dictionary<Tuple<int, int>, Room>();
			GenerateRandomRoom(CurrentPosition);

			// Describe new location
			GameSystem.WriteLine(CurrentRoom.Description);
		}

		/// <summary>
		/// Moves the current position to the specified direction, creating a new room if necessary.
		/// </summary>
		/// <param name="target">Direction to move in</param>
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

			CurrentPosition = pos;

			GameSystem.WriteLine("You moved {0}", Enum.GetName(typeof(Directions), target));
			GameSystem.WriteLine(CurrentRoom.Description);
		}

		/// <summary>
		/// Updates all the necessary components in this Map.
		/// </summary>
		public void Update()
		{
			CurrentRoom.Update();
		}

		/// <summary>
		/// Creates an image representing this Map.
		/// </summary>
		/// <param name="size">Radius, in rooms, of the map to show. Centered at (0,0), the starting room.</param>
		/// <returns>The generated image</returns>
		public Bitmap Draw(int size)
		{
			// x2 because the size is replicated to each side, x3 because each room is a 3-pixel square
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

		/// <summary>
		/// Creates a random room at the specified position
		/// </summary>
		/// <param name="pos">The position to create a room at</param>
		void GenerateRandomRoom(Tuple<int, int> pos)
		{
			if (rooms.ContainsKey(pos))
				throw new ArgumentException(string.Format("Room at ({0};{1}) already exists", pos.Item1, pos.Item2));

			Directions obligatory = Directions.None;
			Directions blocked = Directions.None;

			// Check every possible direction to see if a room already exists there
			foreach(Directions d in Enum.GetValues(typeof(Directions)))
			{
				Tuple<int, int> newpos = MovePosition(pos, d);

				if (rooms.ContainsKey(newpos))
				{
					if (rooms[newpos].Exits.HasFlag(OppositeDirection(d)))
					{
						// Adjacent room exists and has exit to this room, so we need an exit to that side
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

		/// <summary>
		/// Transforms the specified position into a new position using the specified direction
		/// </summary>
		/// <param name="pos">Position to be modified</param>
		/// <param name="dir">Direction to modify the position in</param>
		/// <returns>The modified position</returns>
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

		/// <summary>
		/// Returns the direction opposite to the specified one
		/// </summary>
		/// <param name="d">Direction to obtain the opposite of</param>
		/// <returns>Direction opposite of d</returns>
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
