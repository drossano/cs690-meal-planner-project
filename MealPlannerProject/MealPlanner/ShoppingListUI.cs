namespace MealPlanner;

using Spectre.Console;

public class ShoppingListUI(DataManager dataManager)
{

  public void ShoppingList()
  {
    PrintShoppingList();
    Console.WriteLine("Press Enter to return to the main menu");
    Console.ReadLine();
                Console.Clear();
  }

  public void PrintShoppingList()
  {if (dataManager.CheckMealPlannerEmpty() == true)
  {
    Console.Clear();
    Console.WriteLine("Add meals to your meal planner to populate your shopping list");
  }
  else if (dataManager.GenerateShoppingList().Count == 0 )
  {
    Console.Clear();
    Console.WriteLine("You currently have all the ingredients that you need on hand!");
  }else
  {
        Console.Clear();
    List<Ingredient> shoppingList = dataManager.GenerateShoppingList();
    var shoppingListPanel = new Panel(String.Join(Environment.NewLine, shoppingList))
    .Header("Shopping List")
    .HeaderAlignment(Justify.Center)
    .Padding(8,0,8,0);

    AnsiConsole.Write(shoppingListPanel);
  }

  }
}
