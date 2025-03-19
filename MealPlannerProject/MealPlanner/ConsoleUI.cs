namespace MealPlanner;
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
        case "Recipes":
          Recipes();
          break;
      }
    } while (module != "Exit");
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
          List<Recipe> recipeList = [];
          var recipeListFileContent = File.ReadAllLines("recipeList.txt");
          foreach (var recipe in recipeListFileContent)
          {
            recipeList.Add(new Recipe(recipe));
          }
          Recipe deletedRecipe = AnsiConsole.Prompt(
      new SelectionPrompt<Recipe>()
          .Title("Please select a Recipe to remove.")
          .AddChoices(dataManager.Recipes));
          dataManager.RemoveRecipe(deletedRecipe);
          Console.WriteLine(deletedRecipe + " removed");

          break;
      }
    } while (module != "Exit");
  }
}
