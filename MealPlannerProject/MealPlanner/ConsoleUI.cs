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
          Day selectedDay = EditMeal();
          if (selectedDay.Name == "Exit")
          {
            break;
          }
          Meal selectedMeal = AnsiConsole.Prompt(
          new SelectionPrompt<Meal>()
              .Title("Please select a meal to edit.")
              .AddChoices(selectedDay.Meals)
                );
          if (selectedMeal.Name != "Exit")
          {
            if (!selectedMeal.Recipes.Any())
            {
              Console.WriteLine("This meal has no dishes, add some.");
            }
            else
            {
              foreach (var dish in selectedMeal.Recipes)
              {
                Console.WriteLine(dish);
              }
            }
          }
          break;
        case "Clear Meal Plan":
          Recipes();
          break;
      }
    } while (module != "Exit");
  }

  public Day EditMeal()
  {
    Day selectedDay = AnsiConsole.Prompt(
          new SelectionPrompt<Day>()
              .Title("Please select a day to edit.")
              .AddChoices(dataManager.Days)
              .AddChoices(new Day("Exit"))
                );
    return selectedDay;
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
              .AddChoices(dataManager.Recipes )
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
