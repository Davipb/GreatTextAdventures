using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items.Crafting
{
	public class IronBar : ILookable
	{
		public string DisplayName => "Iron Bar";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "iron bar";
				yield return "iron";
				yield return "bar";
			}
		}
		public string Description => "A shining Iron Bar. Used for crafting.";
		public bool CanTake => true;
		public void Update() { }
	}
}
