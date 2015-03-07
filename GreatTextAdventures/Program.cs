using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	class Program
	{
		static void Main(string[] args)
		{
			GameSystem.Initialize();
			GameSystem.Loop();
		}
	}
}
