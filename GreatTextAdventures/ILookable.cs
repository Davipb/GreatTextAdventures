using System.Collections.Generic;

namespace GreatTextAdventures
{
	public interface ILookable
	{
		string DisplayName { get; }
		IEnumerable<string> CodeNames { get; }
		string Description { get; }
		bool CanTake { get; }

		void Update();
	}
}
