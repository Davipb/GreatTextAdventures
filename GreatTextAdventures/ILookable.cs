using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures
{
	public interface ILookable
	{
		string DisplayName { get; }
		IEnumerable<string> CodeNames { get; }
		string Description { get; }

		void Update();
	}
}
