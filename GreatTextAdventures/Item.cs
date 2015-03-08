using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Item
	{
		public abstract string DisplayName { get; set; }
		public abstract IEnumerable<string> CodeNames { get; set; }
		public abstract string Description { get; set; }

		public void Update(bool active) { /* ¯\_(ツ)_/¯ */ }
	}
}
