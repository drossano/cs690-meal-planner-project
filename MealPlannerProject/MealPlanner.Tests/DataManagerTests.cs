namespace MealPlanner.Tests;

using System.Linq.Expressions;
using MealPlanner;



public class DataManagerTests
{
    DataManager testDataManager;

    public DataManagerTests()
    {

        File.WriteAllText("recipeList.txt", "One:IngredientOne,IngredientThree" + Environment.NewLine + "Two:" + Environment.NewLine + "Three:" + Environment.NewLine + "Four:" + Environment.NewLine + "Five:");
        File.WriteAllText("mealList.txt", "Sunday:Dinner:One" + Environment.NewLine + "Sunday:Dinner:Two" + Environment.NewLine);
        File.WriteAllText("ingredientList.txt", "IngredientOne" + Environment.NewLine + "IngredientTwo" + Environment.NewLine + "Three" + Environment.NewLine + "Four" + Environment.NewLine + "Five");   
        testDataManager = new();

    }
    [Fact]
    public void Test_DataManager_AddRecipe()
    {
        // Given
        Assert.Equal(5, testDataManager.Recipes.Count);
        // When
        testDataManager.AddRecipe(new Recipe("Six"));
        // Then
        Assert.Equal(6, testDataManager.Recipes.Count);
    }

    [Fact]
    public void Test_DataManager_RemoveRecipe()
    {

        // Given
        Assert.Equal(5, testDataManager.Recipes.Count);
        // When
        testDataManager.RemoveRecipe(testDataManager.Recipes[4]);
        // Then
        Assert.Equal(4, testDataManager.Recipes.Count);
    }

    [Fact]
    public void Test_DataManager_AddRecipeIngredient()
    {
        // Given
        Assert.Equal(2, testDataManager.Recipes[0].Ingredients.Count);
        // When
        testDataManager.AddRecipeIngredient(testDataManager.Recipes[0], new Ingredient("IngredientTwo")); 
        // Then
        Assert.Equal(3, testDataManager.Recipes[0].Ingredients.Count);
    }

    [Fact]
    public void Test_DataManager_RemoveRecipeIngredient()
    {
        // Given
        Assert.Equal(2, testDataManager.Recipes[0].Ingredients.Count);
        // When
        testDataManager.RemoveRecipeIngredient(testDataManager.Recipes[0], testDataManager.Recipes[0].Ingredients[0]); 
        // Then
        Assert.Equal(1, testDataManager.Recipes[0].Ingredients.Count);
    }
    [Fact]
    public void Test_DataManager_AddDish()
    {
        // Given
        Day testDay = testDataManager.Days[0];
        string testMealName = "Dinner";

        Assert.Equal(2, testDay.meals[testMealName].Count);
        // When
        testDataManager.AddDish(testDay, testMealName, new Recipe("Three"));
        // Then
        Assert.Equal(3, testDay.meals[testMealName].Count);
    }

    [Fact]
    public void Test_DataManager_RemoveDish()
    {
        // Given
        Day testDay = testDataManager.Days[0];
        string testMealName = "Dinner";

        Assert.Equal(2, testDay.meals[testMealName].Count);
        // When
        testDataManager.RemoveDish(testDay, testMealName, testDay.meals[testMealName][0]);
        // Then
        Assert.Single(testDay.meals[testMealName]);
    }

    [Fact]
    public void Test_DataManager_ClearMealPlan()
    {
        // Given
        Day testDay = testDataManager.Days[0];
        string testMealName = "Dinner";

        Assert.NotEmpty(testDay.meals[testMealName]);
        // When
        testDataManager.ClearMealPlan();
        // Then
        Assert.Empty(testDay.meals[testMealName]);
    }

    [Fact]
    public void Test_DataManager_AddIngredient()
    {
        // Given
        Assert.Equal(5, testDataManager.Ingredients.Count);
        // When
        testDataManager.AddIngredient(new Ingredient("Six"));
        // Then
        Assert.Equal(6, testDataManager.Ingredients.Count);
    }

    [Fact]
    public void Test_DataManager_RemoveIngredient()
    {
        // Given
        Assert.Equal(5, testDataManager.Ingredients.Count);
        
        // When
        testDataManager.RemoveIngredient(testDataManager.Ingredients[0]);
        // Then
        Assert.Equal(4, testDataManager.Ingredients.Count);
    }
    
    [Fact]
    public void Test_DataManager_GenerateShoppingList()
    {
        // Given

        // When
        List<Ingredient> list = testDataManager.GenerateShoppingList();
        // Then
        Assert.Equal(1, list.Count);
    }

}
