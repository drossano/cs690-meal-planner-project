namespace MealPlanner.Tests;

using System.Linq.Expressions;
using MealPlanner;



public class DataManagerTests
{
    DataManager testDataManager;

    public DataManagerTests()
    {

        File.WriteAllText("recipeList.txt", "One" + Environment.NewLine + "Two" + Environment.NewLine + "Three" + Environment.NewLine + "Four" + Environment.NewLine + "Five");
        File.WriteAllText("mealList.txt", "Sunday:Dinner:One" + Environment.NewLine + "Sunday:Dinner:Two" + Environment.NewLine);

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
}
