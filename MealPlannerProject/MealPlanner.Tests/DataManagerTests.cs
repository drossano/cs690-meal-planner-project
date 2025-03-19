namespace MealPlanner.Tests;

using System.Linq.Expressions;
using MealPlanner;



public class DataManagerTests
{
    DataManager testDataManager;

    public DataManagerTests()
    {

        File.WriteAllText("recipeList.txt", "One" + Environment.NewLine + "Two" + Environment.NewLine + "Three" + Environment.NewLine + "Four" + Environment.NewLine + "Five");
        testDataManager = new();

    }
    [Fact]
    public void Test_DataManager_Add()
    {
        // Given
        Assert.Equal(6, testDataManager.Recipes.Count);
        // When
        testDataManager.AddRecipe(new Recipe("Six"));
        // Then
        Assert.Equal(7, testDataManager.Recipes.Count);
    }

    [Fact]
    public void Test_DataManager_Remove()
    {

        // Given
        Assert.Equal(6, testDataManager.Recipes.Count);
        // When
        testDataManager.RemoveRecipe(testDataManager.Recipes[4]);
        // Then
        Assert.Equal(5, testDataManager.Recipes.Count);
    }
}
