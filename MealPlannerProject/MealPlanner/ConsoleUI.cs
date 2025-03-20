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
        case "Recipes":
          Recipes();
          break;
      }
    } while (module != "Exit");
  }


  public void MealPlanner()
  {
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
          Day selectedDay = SelectDay();
          if (selectedDay.Name == "Exit")
          {
            break;
          }

          string selectedMeal = SelectMeal(selectedDay);
          List<Recipe> mealRecipes = selectedDay.meals[selectedMeal];
          if (selectedMeal != "Exit")
          {
            if (mealRecipes.Count() == 0)
            {
              Console.WriteLine("This meal has no dishes, please add one.");
            }
          }
          break;
        case "Clear Meal Plan":
          Recipes();
          break;
      }
    } while (module != "Exit");
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

  public string SelectMeal(Day day)
  {
    string selectedMeal = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
          .Title("Please select a meal to edit.")
          .AddChoices(day.meals.Keys)
          .AddChoices("Exit")
            );
    return selectedMeal;
  }


  public void Recipes()
  {

    string module;
    do
    {
      module = AnsiConsole.Prompt(
          new SelectionPrompt<string>()
              .Title("Select an option")
              .AddChoices(new[] {
                "Add Recipe","Remove Recipe", "Exit",
              }));

      switch (module)
      {
        case "Add Recipe":
          var recipeName = AnsiConsole.Prompt(
          new TextPrompt<string>("What's the name of the recipe that you would like to add? Type \"quit\" to return to the previous menu."));
          if (recipeName != "quit")
          {
            Recipe newRecipe = new(recipeName);
            dataManager.AddRecipe(newRecipe);
            Console.WriteLine("Recipe added!");
          }
          break;
        case "Remove Recipe":
          Recipe deletedRecipe = AnsiConsole.Prompt(
          new SelectionPrompt<Recipe>()
              .Title("Please select a Recipe to remove.")
              .AddChoices(dataManager.Recipes)
              .AddChoices(new Recipe("Exit"))
                );
          if (deletedRecipe.Name == "Exit")
          {
            break;
          }

          dataManager.RemoveRecipe(deletedRecipe);
          Console.WriteLine(deletedRecipe + " removed");

          break;
      }
    } while (module != "Exit");
  }
}
