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
  {
    Console.Clear();
    List<Ingredient> shoppingList = dataManager.GenerateShoppingList();
    var shoppingListPanel = new Panel(String.Join(Environment.NewLine, shoppingList))
    .Header("Shopping List")
    .HeaderAlignment(Justify.Center)
    .Padding(8,0,8,0);

    AnsiConsole.Write(shoppingListPanel);
    Console.WriteLine("Shopping List");
    foreach (var item in shoppingList)
    {
      Console.WriteLine("- " + item);
    }
  }
}
