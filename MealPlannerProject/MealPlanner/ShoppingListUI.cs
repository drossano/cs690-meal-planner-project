namespace MealPlanner;

using Spectre.Console;

public class ShoppingListUI(DataManager dataManager)
{
  DataManager datamManager = dataManager;
  public void ShoppingList()
  {
    PrintShoppingList();
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
  }
}
