using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items.Crafting
{
	public class CraftingRecipe : ILookable
	{
		public string DisplayName => $"Crafting Recipe ({Result.DisplayName})";
		public IEnumerable<string> CodeNames
		{
			get
			{
				yield return "crafting recipe";
				yield return "recipe";
				foreach (var codename in Result.CodeNames)
					yield return codename;
			}
		}
		public string Description
		{
			get
			{
				var sb = new StringBuilder();
				sb.AppendLine("Crafting Recipe");
				sb.AppendLine("Ingredients:");

				foreach (var ingredient in Ingredients)
					sb.AppendLine($"{ingredient.Value}x {CraftingMaterial.AllMaterials[ingredient.Key]["DisplayName"]}");

				sb.AppendLine();
				sb.AppendLine("Result:");
				sb.AppendLine(Result.DisplayName);

				return sb.ToString();
			}
		}
		public bool CanTake => true;
		public void Update() { }

		public IDictionary<string, int> Ingredients { get; }
		public ILookable Result { get; }

		public CraftingRecipe(IDictionary<string, int> ingredients, ILookable result)
		{
			Ingredients = ingredients;
			Result = result;
		}
	}
}
