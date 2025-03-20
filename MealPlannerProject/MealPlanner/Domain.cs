using System.Globalization;
using Microsoft.VisualBasic;

namespace MealPlanner;

public class Recipe(string name)
{
  public string Name { get; } = name;
  public override string ToString()
  {
    return this.Name;
  }
}

public class RecipeList(string name)
{
  public string Name { get; } = name;
  public List<Recipe> Recipes { get; } = [];
  public override string ToString()
  {
    return this.Name;
  }
}
public class Meal(string name)
{
  public string Name { get; } = name;

  public List<Recipe> Recipes { get; set; } = [];
  public override string ToString()
  {
    return this.Name;
  }
}

public class Day(string name)
{
  public string Name { get; } = name;

  public Dictionary<string, List<Recipe>> meals = new(){{"Breakfast", []},
                                                  {"Lunch", []},
                                                  {"Dinner",[]}};

  public override string ToString()
  {
    return this.Name;
  }
}