namespace MealPlanner;

using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Spectre.Console;
public class ConsoleUI
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
          MealPlannerUI mealPlannerUI = new(dataManager);
          mealPlannerUI.MealPlanner();
          break;
        case "Shopping List":
          ShoppingListUI shoppingListUI = new(dataManager);
          shoppingListUI.ShoppingList();
          break;
        case "Recipes":
          RecipesUI recipesUI = new(dataManager);
          recipesUI.Recipes();
          break;
        case "Ingredients":
          IngredientsUI ingredientsUI = new(dataManager);
          ingredientsUI.Ingredients();
          break;
      }
    } while (module != "Exit");
  }
}