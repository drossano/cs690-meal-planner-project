using System.Windows.Markup;
using System.IO;
using System.Configuration.Assemblies;
using System.Diagnostics.Metrics;

namespace MealPlanner;

public class DataManager
{
  public List<Ingredient> Ingredients { get; }
  public List<Recipe> Recipes { get; }
  public List<Day> Days { get; }

  public DataManager()
  {
    Recipes = [];
    Ingredients = [];
    Days = [new Day("Sunday"), new Day("Monday"), new Day("Tuesday"), new Day("Wednesday"), new Day("Thursday"), new Day("Friday"), new Day("Saturday")];
    if (!File.Exists("recipeList.txt"))
    {
      File.Create("recipeList.txt").Close();
    }
    var recipesFileContent = File.ReadAllLines("recipeList.txt");
    foreach (var recipeLine in recipesFileContent)
    {
      string[] recipeAndIngredients = recipeLine.Split(":", StringSplitOptions.RemoveEmptyEntries);
      var recipeName = recipeAndIngredients[0];
      string recipeIngredients ="";
      if (recipeAndIngredients.Length > 1){
        recipeIngredients = recipeAndIngredients[1];
      }
      Recipes.Add(new Recipe(recipeName));
      string[] ingredientStrings = recipeIngredients.Split("," ,StringSplitOptions.RemoveEmptyEntries);
      List<Ingredient> recipeIngredientsList= [];
      foreach (string ingredient in ingredientStrings)
      {
       Ingredient newIngredient = new Ingredient(ingredient);
       recipeIngredientsList.Add(newIngredient);
      }
      foreach (Recipe recipe in Recipes)
      {
        if (recipe.Name == recipeName)
        {
          recipe.Ingredients = recipeIngredientsList;
        }
      }
    }
    if (!File.Exists("mealList.txt"))
    { File.Create("mealList.txt").Close(); }

    else
    {
      var mealFileContent = File.ReadAllLines("mealList.txt");
      foreach (var line in mealFileContent)
      {
        var splitted = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var dayName = splitted[0];
        var mealName = splitted[1];
        var dishName = splitted[2];

        foreach (Day day in Days)
        {
          if (day.Name == dayName)
          {
            day.meals[mealName].Add(new Recipe(dishName));
          }
        }

      }
    }
    if (!File.Exists("ingredientList.txt"))
    {
      File.Create("ingredientList.txt").Close();
    }
    var ingredientsFileContent = File.ReadAllLines("ingredientList.txt");
     foreach (var ingredientName in ingredientsFileContent)
    {
      Ingredients.Add(new Ingredient(ingredientName));
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
    string recipeList = "recipeList.txt";
    File.Delete(recipeList);
    foreach (var recipe in Recipes)
    {
      File.AppendAllText(recipeList, recipe.Name + ":");
      foreach (var ingredient in recipe.Ingredients)
      {
        File.AppendAllText(recipeList, ingredient + ",");
      }
      File.AppendAllText(recipeList, Environment.NewLine);
    }
  }

  public void AddRecipe(Recipe recipe)
  {
    Recipes.Add(recipe);
    SyncRecipes();
  }

  public bool RemoveRecipe(Recipe recipe)
  {
    if (CheckIfRecipeInMealPlan(recipe))
    {
      return false;
    }
    else
    {
      Recipes.Remove(recipe);
      SyncRecipes();
      return true;
    }

  }

  public bool CheckIfRecipeInMealPlan(Recipe recipe)
  {
    List<string> mealPlanDishes = [];
    foreach (Day day in Days)
    { 
      foreach (var meal in day.meals)
      {
        foreach (Recipe dish in meal.Value)
        {
          mealPlanDishes.Add(dish.Name);
        }

      } 
    }
    foreach (var item in mealPlanDishes)
    {
      Console.WriteLine(item);
    }
    Console.WriteLine(mealPlanDishes.Contains(recipe.Name));
    return mealPlanDishes.Contains(recipe.Name);
  }
  
  public void AddRecipeIngredient(Recipe recipe, Ingredient ingredient)
  {
    recipe.Ingredients.Add(ingredient);
    SyncRecipes();
  }

    public void RemoveRecipeIngredient(Recipe recipe, Ingredient ingredient)
  {
    recipe.Ingredients.Remove(ingredient);
    SyncRecipes();
  }
  
public void SyncIngredients()
  {
    string ingredientList = "ingredientList.txt";
    File.Delete(ingredientList);
    foreach (Ingredient ingredient in Ingredients) 
    {
      File.AppendAllText(ingredientList, ingredient.Name + Environment.NewLine);
    }
  }

  public void AddIngredient(Ingredient ingredient)
  {
    Ingredients.Add(ingredient);
    SyncIngredients();
  }

  public void RemoveIngredient(Ingredient ingredient)
  {
    Ingredients.Remove(ingredient);
    SyncIngredients();
  }

  public void ClearMealPlan()
  {
    foreach (var day in Days)
    {
      foreach (var meal in day.meals)
      {
        meal.Value.Clear();

      }

    }
    SyncMeals();
  }

  public List<Ingredient> GenerateShoppingList()
  {
    List<Ingredient> shoppingList = [];
    foreach (var Day in Days)
    {
      foreach (var meal in Day.meals)
      {
        foreach (var dish in meal.Value)
        {
          var recipe = Recipes.Find(recipe => recipe.Name == dish.Name);
          if (recipe.Ingredients.Count() != 0){
          foreach (var ingredient in recipe.Ingredients)
          {
            shoppingList.Add(ingredient);
          }}
        }
      }
    }
    List<Ingredient> shoppingListNoDupes = shoppingList.DistinctBy(dish => dish.Name).ToList();
    List<Ingredient> ownedIngredients = Ingredients;
    List<Ingredient> neededIngredients = shoppingListNoDupes.ExceptBy(ownedIngredients.Select(sl => sl.Name.ToLower()), i=> i.Name.ToLower()).ToList();
    return neededIngredients;
  }

    public bool CheckMealPlannerEmpty()
  {
    List<string> mealPlannerMeals = [];
    foreach (var Day in Days)
    {
      foreach (var meal in Day.meals)
      {
        foreach (var dish in meal.Value)
        {
          mealPlannerMeals.Add(dish.Name);
        }
      }
    }
    return mealPlannerMeals.Count == 0;
  }
}
