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
    Console.WriteLine("Not yet implemented!");
  }
  public void Recipes()
  {
    PrintRecipes();
    string module;
    do
    {
      List<string> choices = ["Add Recipe", "Exit"];
      if (dataManager.Recipes.Count != 0)
      { choices.Insert(1, "Remove Recipe"); }
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(choices));
      switch (module)
      {
        case "Add Recipe":
          AddRecipe();
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
  public void AddRecipe()
  {
    var recipeName = AnsiConsole.Prompt(
    new TextPrompt<string>("What's the name of the recipe that you would like to add? Type \"quit\" to return to the previous menu."));
    if (recipeName != "quit")
    {
      Recipe newRecipe = new(recipeName);
      dataManager.AddRecipe(newRecipe);
      Console.Clear();
      Console.WriteLine(newRecipe.Name + " added to recipe list");
    }
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
    Console.WriteLine("Not yet implemented!");
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
