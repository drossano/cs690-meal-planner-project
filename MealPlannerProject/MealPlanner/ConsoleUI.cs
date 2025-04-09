namespace MealPlanner;

using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Spectre.Console;
class ConsoleUI
{
  DataManager dataManager;

  public ConsoleUI()
  {
    dataManager = new DataManager();
  }
  public void Show()
  {
    dataManager = new DataManager();
    Console.Clear();
    Console.WriteLine("Welcome to the Meal Planner app.");
    string module;
    do
    {
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices([
                "Meal Planner", "Shopping List", "Recipes",
                "Ingredients", "Exit",
              ]));

      switch (module)
      {
        case "Meal Planner":
          MealPlanner();
          break;
        case "Shopping List":
          ShoppingList();
          break;
        case "Recipes":
          Recipes();
          break;
        case "Ingredients":
          Ingredients();
          break;
      }
    } while (module != "Exit");
  }

  public void MealPlanner()
  {
    Console.Clear();
    GenerateTable();
    string module;
    do
    {
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices([
                "Edit Meal Plan", "Clear Meal Plan", "Exit",
              ]));

      switch (module)
      {
        case "Edit Meal Plan":
          if (dataManager.Recipes.Count == 0)
          {
            Console.Clear();
            GenerateTable();
            Console.WriteLine("No dishes available. Please add recipes through the Recipes module");
          }
          else
          {
            EditMealPlan();
          }

          break;
        case "Clear Meal Plan":
          ClearMealPlan();
          break;
      }
    } while (module != "Exit");
  }

  public void EditMealPlan()
  {
    Day selectedDay = SelectDay();
    if (selectedDay.Name != "Exit")
    {
      string selectedMeal = SelectMeal(selectedDay);
      if (selectedMeal != "Exit")
      {
        EditMeal(selectedDay, selectedMeal);
      }
      else
      {
        Console.Clear();
        Console.WriteLine("No changes have been made.");
        GenerateTable();
      }
    }
  }

  public void ClearMealPlan()
  {
    string confirmation = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
        .Title("Are you sure you would like to clear your meal plan?")
        .AddChoices("No", "Yes")
          );
    if (confirmation == "Yes")
    {
      dataManager.ClearMealPlan();
      Console.Clear();
      Console.WriteLine("Meal plan cleared");
      GenerateTable();
    }
  }

  public Day SelectDay()
  {
    Day selectedDay = AnsiConsole.Prompt(
          new SelectionPrompt<Day>()
              .Title("Please select a day to edit.")
              .AddChoices(dataManager.Days)
              .AddChoices(new Day("Exit"))
                );
    return selectedDay;
  }

  public static string SelectMeal(Day day)
  {
    string selectedMeal = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
          .Title("Please select a meal to edit.")
          .AddChoices(day.meals.Keys)
          .AddChoices("Exit")
            );
    return selectedMeal;
  }

  public void EditMeal(Day selectedDay, string selectedMeal)
  {
    string module;
    do
    {
      List<string> choices = ["Add Dish", "Exit"];
      if (selectedDay.meals[selectedMeal].Count != 0)
      {
        choices.Insert(1, "Remove Dish");
      }
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(choices));
      switch (module)
      {
        case "Add Dish":
          AddDish(selectedDay, selectedMeal);
          break;
        case "Remove Dish":
          RemoveDish(selectedDay, selectedMeal);
          break;
      }

    } while (module != "Exit");
  }

  public void AddDish(Day selectedDay, string selectedMeal)
  {
    if (dataManager.Recipes.Count == 0)
    {
      Console.WriteLine("No dishes available. Please add recipes through the Recipes module");
    }
    Recipe dishToAdd = AnsiConsole.Prompt(
              new SelectionPrompt<Recipe>()
                  .Title("Please select a Dish to add.")
                  .AddChoices(dataManager.Recipes)
                  .AddChoices(new Recipe("Exit"))
                    );
    if (dishToAdd.Name != "Exit")
    {
      dataManager.AddDish(selectedDay, selectedMeal, dishToAdd);
      Console.Clear();
      Console.WriteLine(dishToAdd + " added to " + selectedMeal);
      GenerateTable();
    }
    else
    {
      Console.Clear();
      Console.WriteLine("No changes have been made");
      GenerateTable();
    }
  }

  public void RemoveDish(Day selectedDay, string selectedMeal)
  {
    Recipe deletedDish = AnsiConsole.Prompt(
    new SelectionPrompt<Recipe>()
        .Title("Please select a dish to remove.")
        .AddChoices(selectedDay.meals[selectedMeal])
        .AddChoices(new Recipe("Exit"))
          );
    if (deletedDish.Name != "Exit")
    {
      dataManager.RemoveDish(selectedDay, selectedMeal, deletedDish);
      Console.Clear();
      Console.WriteLine(deletedDish + " has been removed from " + selectedMeal);
      GenerateTable();
    }
  }

  public void ShoppingList()
  {
    PrintShoppingList();
  }

  public void PrintShoppingList()
  {
    List<Ingredient> shoppingList = [];
    foreach (var Day in dataManager.Days)
    {
      foreach (var meal in Day.meals)
      {
        foreach (var dish in meal.Value)
        {
          var recipe = dataManager.Recipes.Find(recipe => recipe.Name == dish.Name);
          foreach (var ingredient in recipe.Ingredients)
          {
            shoppingList.Add(ingredient);
          }
        }
      }
    }
    List<Ingredient> shoppingListNoDupes = shoppingList.DistinctBy(dish => dish.Name).ToList();
    List<Ingredient> ownedIngredients = dataManager.Ingredients;
    List<Ingredient> neededIngredients = shoppingListNoDupes.ExceptBy(ownedIngredients.Select(sl => sl.Name), i=> i.Name).ToList();
    Console.WriteLine("Shopping List");
    foreach (var item in neededIngredients)
    {
      Console.WriteLine("- " + item);
    }
  }
  public void Recipes()
  {
    Console.Clear();
    PrintRecipes();
    string module;
    do
    {
      List<string> choices = ["Add Recipe", "Exit"];
      if (dataManager.Recipes.Count != 0)
      { choices.Insert(1, "Edit Recipe" );
      choices.Insert(2, "Remove Recipe" ); }
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(choices));
      switch (module)
      {
        case "Add Recipe":
          AddRecipe();
          break;
        case "Edit Recipe":
          EditRecipe();
          break;
        case "Remove Recipe":
          RemoveRecipe();
          break;
      }
      PrintRecipes();
    } while (module != "Exit");
    if (module == "Exit")
    {
      Console.Clear();
    }
  }

  public void PrintRecipes()
  {

    Console.WriteLine("Recipes");
    foreach (var recipe in dataManager.Recipes)
    {
      Console.WriteLine("- " + recipe.Name);
    }
  }

    public void PrintRecipeIngredients(Recipe recipe)
  {

    Console.WriteLine(recipe.Name);
    foreach (var ingredient in recipe.Ingredients)
    {
      Console.WriteLine("- " + ingredient);
    }
  }
  public void AddRecipe()
  {
    var recipeName = AnsiConsole.Prompt(
    new TextPrompt<string>("What's the name of the recipe that you would like to add? Type \"quit\" to return to the previous menu."));
    if (recipeName != "quit")
    {
      Recipe newRecipe = new(recipeName);
      string addIngredient = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
        .Title("Would you like to add ingredients to this recipe?")
        .AddChoices("Yes", "No")
          );
      if (addIngredient == "Yes")
      {
        AddRecipeIngredient(newRecipe);
      }
      dataManager.AddRecipe(newRecipe);
      Console.Clear();
      Console.WriteLine(newRecipe.Name + " added to recipe list");
    }
  }

  public void EditRecipe()
  {
     Recipe recipeToEdit = AnsiConsole.Prompt(
    new SelectionPrompt<Recipe>()
      .Title("Please select a Recipe to edit.")
      .AddChoices(dataManager.Recipes)
      .AddChoices(new Recipe("Exit"))
        );
    if (recipeToEdit.Name != "Exit")
    {
      Console.Clear();
      PrintRecipeIngredients(recipeToEdit);
     List<string> choices = ["Add Ingredients", "Exit"];
      if (recipeToEdit.Ingredients.Count != 0)
      { choices.Insert(1, "Remove Ingredients" ); }
      string module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(choices));
      switch (module)
      {
        case "Add Ingredients":
          AddRecipeIngredient(recipeToEdit);
          dataManager.SyncRecipes();
          break;
        case "Remove Ingredients":
          RemoveRecipeIngredient(recipeToEdit);
          dataManager.SyncRecipes();
          break;
        case "Remove Recipe":
          RemoveRecipe();
          break;
      }
    }
  }

  public void AddRecipeIngredient(Recipe recipe)
  {
    string confirmation;
    do
    {


      var ingredientName = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the name of the ingredient that you would like to add. Enter \"quit\" if you don't want to add an ingredient."));
      if (ingredientName != "quit")
      {
        recipe.Ingredients.Add(new Ingredient(ingredientName));
      }
      Console.Clear();
      Console.WriteLine(ingredientName + " added to " + recipe.Name + ".");
      PrintRecipeIngredients(recipe);
      confirmation = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Would you like to add another ingredient?")
                .AddChoices("Yes", "No")

      );
    } while (confirmation != "No");
    Console.Clear();
  }

    public void RemoveRecipeIngredient(Recipe recipe)
  {
    string confirmation;
    do
    {
    Console.Clear();
    Ingredient deletedIngredient= AnsiConsole.Prompt(
    new SelectionPrompt<Ingredient>()
      .Title("Please select an ingredient to remove.")
      .AddChoices(recipe.Ingredients)
      .AddChoices(new Ingredient("Exit"))
        );
    if (deletedIngredient.Name != "Exit")
    {
      recipe.Ingredients.Remove(deletedIngredient);
      Console.Clear();
      Console.WriteLine(deletedIngredient + " removed from " + recipe.Name + ".");
      PrintRecipeIngredients(recipe);
    }

      if (recipe.Ingredients.Count != 0)
      {
              confirmation = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Would you like to remove another ingredient?")
                .AddChoices("Yes", "No")

      );
      }
      else
      {
        confirmation = "No";
      }

    } while (confirmation != "No" && recipe.Ingredients.Count != 0);
    Console.Clear();
  }

  public void RemoveRecipe()
  {
    Recipe deletedRecipe = AnsiConsole.Prompt(
    new SelectionPrompt<Recipe>()
      .Title("Please select a Recipe to remove.")
      .AddChoices(dataManager.Recipes)
      .AddChoices(new Recipe("Exit"))
        );
    if (deletedRecipe.Name != "Exit")
    {
      dataManager.RemoveRecipe(deletedRecipe);
      Console.Clear();
      Console.WriteLine(deletedRecipe + " removed");
    }
  }

  public void Ingredients()
  {
    Console.Clear();
    PrintIngredients();
    string module;
    do
    {
      List<string> choices = ["Add Ingredient", "Exit"];
      if (dataManager.Recipes.Count != 0)
      { choices.Insert(1, "Remove Ingredient" ); }
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(choices));
      switch (module)
      {
        case "Add Ingredient":
          AddIngredient();
          break;
        case "Remove Ingredient":
          RemoveIngredient();
          break;
      }
      PrintIngredients();
    } while (module != "Exit");
    if (module == "Exit")
    {
      Console.Clear();
    }
  }
public void AddIngredient()
  {
    var ingredientName = AnsiConsole.Prompt(
    new TextPrompt<string>("What's the name of the ingredient that you would like to add? Type \"quit\" to return to the previous menu."));
    if (ingredientName != "quit")
    {
      Ingredient newIngredient = new(ingredientName);
      
      dataManager.AddIngredient(newIngredient);
      Console.Clear();
      Console.WriteLine(newIngredient.Name + " added to ingredient list");
    }
  }
  public void RemoveIngredient()
  {
    Ingredient deletedIngredient = AnsiConsole.Prompt(
    new SelectionPrompt<Ingredient>()
      .Title("Please select an ingredient to remove.")
      .AddChoices(dataManager.Ingredients)
      .AddChoices(new Ingredient("Exit"))
        );
    if (deletedIngredient.Name != "Exit")
    {
      dataManager.RemoveIngredient(deletedIngredient);
      Console.Clear();
      Console.WriteLine(deletedIngredient + " removed");
    }
  }

public void PrintIngredients()
  {

    Console.WriteLine("Ingredients");
    foreach (var ingredient in dataManager.Ingredients)
    {
      Console.WriteLine("- " + ingredient.Name);
    }
  }
  public void GenerateTable()
  {
    var mealPlan = new Table();
    mealPlan.ShowRowSeparators();
    mealPlan.AddColumn("Meal");
    foreach (var day in dataManager.Days)
    {
      mealPlan.AddColumn(day.Name);
    }
    mealPlan.AddRow("Breakfast");
    mealPlan.AddRow("Lunch");
    mealPlan.AddRow("Dinner");
    int col = 1;
    foreach (var day in dataManager.Days)
    {
      int row = 0;
      foreach (var meal in day.meals)
      {
        string dishes = "";
        foreach (var dish in meal.Value)
        {
          dishes += dish.Name + "\n";
        }
        mealPlan.UpdateCell(row, col, dishes);
        row += 1;
      }
      col += 1;
    }
    AnsiConsole.Write(mealPlan);

  }
}
