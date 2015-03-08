using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Person : ILookable
	{
		public string DisplayName { get; set; }
		public IEnumerable<string> CodeNames { get; set; }
		public string Description { get; set; }

		public int Health { get; set; }
		public List<Item> Inventory { get; set; }
		public ulong Currency { get; set; }

		public void Update() { /* ¯\_(ツ)_/¯ */ }

		public virtual void Talk()
		{
			Console.WriteLine("{0} doesn't answer", DisplayName);
		}
	}
}
