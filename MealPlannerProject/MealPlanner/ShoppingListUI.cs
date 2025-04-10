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
    Console.WriteLine("Shopping List");
    foreach (var item in shoppingList)
    {
      Console.WriteLine("- " + item);
    }
  }
}
