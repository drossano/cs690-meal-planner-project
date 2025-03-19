using System.Windows.Markup;
using System.IO;

namespace MealPlanner;

public class DataManager
{

  public List<Recipe> Recipes { get; }
  public List<Day> Days { get; }

  public DataManager()
  {
    Recipes = [];
    Days = [new Day("Sunday"), new Day("Monday"), new Day("Tuesday"), new Day("Wednesday"), new Day("Thursday"), new Day("Friday"), new Day("Saturday"), new Day("Exit")];
    var recipesFileContent = File.ReadAllLines("recipeList.txt");
    foreach (var recipeName in recipesFileContent)
    {
      Recipes.Add(new Recipe(recipeName));
    }
    if (!Recipes.Exists(x => x.Name == "Exit "))
    {
      Recipes.Add(new Recipe("Exit"));
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
