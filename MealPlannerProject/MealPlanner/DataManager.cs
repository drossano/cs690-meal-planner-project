using System.Windows.Markup;
using System.IO;

namespace MealPlanner;

public class DataManager
{

  public List<Recipe> Recipes { get; }


  public DataManager()
  {
    Recipes = [];
    var recipesFileContent = File.ReadAllLines("recipeList.txt");
    foreach (var recipeName in recipesFileContent)
    {
      Recipes.Add(new Recipe(recipeName));
    }
  }



  public void SyncRecipes()
  {
    File.Delete("recipeList.txt");
    foreach (var recipe in Recipes)
    {
      File.AppendAllText("recipeList.txt", recipe.Name + Environment.NewLine);
    }
  }

  public void AddRecipe(Recipe recipe)
  {
    Recipes.Add(recipe);
    SyncRecipes();
  }

  public void RemoveRecipe(Recipe recipe)
  {
    Recipes.Remove(recipe);
    SyncRecipes();
  }
}
