using System.Collections.Generic;

namespace GreatTextAdventures.Items
{
	public interface IContainer
	{
		void Open();
		IList<Item> Items { get; set; }
	}
}
