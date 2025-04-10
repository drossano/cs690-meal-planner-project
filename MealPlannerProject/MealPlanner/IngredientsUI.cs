namespace MealPlanner;

using Spectre.Console;

public class IngredientsUI(DataManager dataManager)
{
 DataManager datamManager = dataManager;
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
}
