using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public interface IContainer
	{
		void Open();
		IList<ILookable> Content { get; set; }
	}
}
