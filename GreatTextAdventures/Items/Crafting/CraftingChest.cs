using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatTextAdventures.Items.Crafting
{
	public class CraftingChest : Chest
	{
		public override string DisplayName
		{
			get
			{
				if (Content == null || !Content.Any())
					return "Metal Chest (empty)";
				return $"Metal Chest ({Content.Count} item{(Content.Count > 1 ? "s" : "")})";
			}
		}

		public override IEnumerable<string> CodeNames
		{
			get
			{
				yield return "metal chest";
				yield return "chest";
			}
		}
		public override string Description => "A sturdy-looking metal chest.";

		public CraftingChest(int level) : base(level) { }

		protected override void PopulateContent(int level)
		{
			CraftingRecipe recipe = CraftingRecipe.Generate(level);

			foreach(var ingredient in recipe.Ingredients)
				for (int i = 0; i < ingredient.Value; i++)
					Content.Add(CraftingMaterial.Create(ingredient.Key));

			Content.Add(recipe);
		}
	}
}