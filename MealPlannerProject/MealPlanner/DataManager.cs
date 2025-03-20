using System.Windows.Markup;
using System.IO;
using System.Configuration.Assemblies;

namespace MealPlanner;

public class DataManager
{

  public List<Recipe> Recipes { get; }
  public List<Day> Days { get; }

  public DataManager()
  {
    Recipes = [];
    Days = [new Day("Sunday"), new Day("Monday"), new Day("Tuesday"), new Day("Wednesday"), new Day("Thursday"), new Day("Friday"), new Day("Saturday")];
    var recipesFileContent = File.ReadAllLines("recipeList.txt");
    foreach (var recipeName in recipesFileContent)
    {
      Recipes.Add(new Recipe(recipeName));
    }
  }

  public void SyncMeals()
  {
    string mealList = "mealList.txt";
    File.Delete(mealList);
    foreach (var day in Days)
    {
      foreach (var meal in day.meals)
      {
        foreach (var dish in meal.Value)
        {
          File.AppendAllText(mealList, day.Name + ":" + meal.Key + ":" + dish + Environment.NewLine);
        }

      }

    }
  }

  public void AddDish(Day day, string meal, Recipe dish)
  {
    List<Recipe> dishes = day.meals[meal];
    dishes.Add(dish);
    SyncMeals();
  }

  public void RemoveDish(Day day, string meal, Recipe dish)
  {
    List<Recipe> dishes = day.meals[meal];
    dishes.Remove(dish);
    SyncMeals();
  }
  public void SyncRecipes()
  {
    string recipeList = "recipelist.txt";
    File.Delete(recipeList);
    foreach (var recipe in Recipes)
    {
      File.AppendAllText(recipeList, recipe.Name + Environment.NewLine);
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
