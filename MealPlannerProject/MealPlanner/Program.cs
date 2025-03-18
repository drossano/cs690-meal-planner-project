using Spectre.Console;

namespace MealPlanner;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Meal Planner app.");
        string module;
        do
        {
            module = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option")
                    .AddChoices(new[] {
                "Meal Planner", "Shopping List", "Recipes",
                "Ingredients", "Exit",
                    }));

            switch (module)
            {
                case "Recipes":
                    Recipes();
                    break;
            }
        } while (module != "Exit");
    }

    static void Recipes()
    {
        string module;
        do
        {
            module = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option")
                    .AddChoices(new[] {
                "Add Recipe", "Exit",
                    }));

            switch (module)
            {
                case "Add Recipe":
                    var recipeName = AnsiConsole.Prompt(
                    new TextPrompt<string>("What's the name of the recipe that you would like to add? Type \"quit\" to return to the previous menu."));
                    if (recipeName != "quit")
                    {
                        Recipe newRecipe = new(recipeName);
                        FileManager recipeList = new("recipeList.txt");
                        recipeList.AppendLine(newRecipe.Name);
                        Console.WriteLine("Recipe added!");
                    }
                    break;
            }
        } while (module != "Exit");
    }
}
