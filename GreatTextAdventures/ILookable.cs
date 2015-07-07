using System.Collections.Generic;

namespace GreatTextAdventures
{
	/// <summary>
	/// Represents an object that can be seen by the Player.
	/// </summary>
	public interface ILookable
	{
		/// <summary>
		/// Name that will be displayed when the object is talked about.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Names that the player can use to identify the object through commands.
		/// </summary>
		IEnumerable<string> CodeNames { get; }

		/// <summary>
		/// Description of the object
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Represents if the Player can put this object in their inventory
		/// </summary>
		bool CanTake { get; }

		/// <summary>
		/// Called every turn, updates the required components 
		/// </summary>
		void Update();
	}
}
