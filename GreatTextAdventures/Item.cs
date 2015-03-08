﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public abstract class Item : ILookable
	{
		public string DisplayName { get; set; }
		public IEnumerable<string> CodeNames { get; set; }
		public string Description { get; set; }

		public bool CanTake { get; set; }
		public bool CanEquip { get; set; } 
		public ulong Price { get; set; }

		public void Update() { /* ¯\_(ツ)_/¯ */ }
		public void Use()
		{
			Console.WriteLine("You can't use {0}.", DisplayName);
		}
	}
}
