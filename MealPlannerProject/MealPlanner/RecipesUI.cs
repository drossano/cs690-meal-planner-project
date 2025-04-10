namespace MealPlanner;

using Spectre.Console;

public class RecipesUI(DataManager dataManager)
{

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
    var recipesPanel = new Panel(String.Join(Environment.NewLine, dataManager.Recipes))
    .Header("Recipes")
    .HeaderAlignment(Justify.Center)
    .Padding(8,0,8,0);
    AnsiConsole.Write(recipesPanel);
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
          break;
        case "Remove Ingredients":
          RemoveRecipeIngredient(recipeToEdit);
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
        dataManager.AddRecipeIngredient(recipe, new Ingredient(ingredientName));
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
      dataManager.RemoveRecipeIngredient(recipe, deletedIngredient);
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
}
