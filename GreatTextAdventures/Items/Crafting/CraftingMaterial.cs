using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GreatTextAdventures.Items.Crafting
{
	public class CraftingMaterial : ILookable
	{
		public static JObject AllMaterials { get; private set; }

		public string DisplayName { get; }
		public IEnumerable<string> CodeNames { get; }
		public string Description { get; }
		public bool CanTake => true;

		public string MaterialName { get; }

		static CraftingMaterial()
		{
			AllMaterials = JObject.Parse(File.ReadAllText(@"Items\Crafting\CraftingMaterials.json"));
		}

		protected CraftingMaterial(string name, string description, string material, IEnumerable<string> codenames)
		{
			DisplayName = name;
			Description = description;
			MaterialName = material;
			CodeNames = codenames;
		}

		public static CraftingMaterial Create(string material)
		{
			// Returns null when the key 'material' has not been found i.e. the material doesn't exist
			if (AllMaterials[material] == null)
				return null;

			return new CraftingMaterial(
				(string)AllMaterials[material]["DisplayName"],
				(string)AllMaterials[material]["Description"],
				material,
				AllMaterials[material]["CodeNames"].Select(x => (string)x)
				);
		}
	}
}
