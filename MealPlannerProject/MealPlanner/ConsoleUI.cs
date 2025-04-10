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
          Recipes();
          break;
        case "Ingredients":
          Ingredients();
          break;
      }
    } while (module != "Exit");
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