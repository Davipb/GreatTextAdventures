using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GreatTextAdventures.Items
{
	public class DecorationItem : ILookable
	{
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public IEnumerable<string> CodeNames { get; set; }
		public bool CanTake { get { return false; } }

		public static IEnumerable<DecorationItem> Random(int amount)
		{
			// Read all possible decorations
			JArray items = JArray.Parse(File.ReadAllText(@"Items\Decorations.json"));

			// Randomly select 'amount' of them
			IEnumerable<JObject> selected = items.OrderBy(x => GameSystem.RNG.Next()).Take(amount).Select(x => (JObject)x);

			// Initialize and return every selected decoration
			foreach(var decor in selected)
			{
				var result = new DecorationItem();

				result.DisplayName = (string)decor["DisplayName"].OrderBy(x => GameSystem.RNG.Next()).First();
				result.Description = (string)decor["Description"].OrderBy(x => GameSystem.RNG.Next()).First();
				result.CodeNames = decor["CodeNames"].Select(x => (string)x);

				yield return result;
			}			
		}
	}
}
