using System;
using System.Collections.Generic;
using System.Linq;
class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public double OriginalQuantity { get; set; }
    public string Unit { get; set; }
    public double IngredientCalories { get; set; }
    public string FoodGroup { get; set; }





}

class Step
{
    public string Description { get; set; }
}

class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Step> Steps { get; set; }

    public void DisplayRecipe()
    {
        Console.WriteLine("\nRecipe Details:\n");
        Console.WriteLine($"Recipe Name: {Name}\n");

        Console.WriteLine("Ingredients:");
        foreach (Ingredient ingredient in Ingredients)
        {
            Console.WriteLine($"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit}");
            Console.WriteLine("Calories: " + ingredient.IngredientCalories);
            Console.WriteLine("Food Group: " + ingredient.FoodGroup);

        }


        Console.WriteLine("\nSteps:");
        foreach (Step step in Steps)
        {
            Console.WriteLine(step.Description);
        }
        
       double totalCalories = Ingredients.Sum(ingredient => ingredient.IngredientCalories);

        static void NotifyCaloriesExceeded(int totalCalories) //broken
        {
            Console.WriteLine("Warning: The total calories in the recipe are equal to or exceed 300!");
        }
    }
}

class RecipeScaler //fixed
{
    public static void ScaleRecipe(Recipe recipe, int scaleOption) 
    {
        double scale = 1.0;

        if (scaleOption == 1)
        {
            scale = 0.5;
        }
        else if (scaleOption == 2)
        {
            scale = 2.0;
        }
        else if (scaleOption == 3)
        {
            scale = 3.0;
        }

        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            ingredient.OriginalQuantity = ingredient.Quantity;
            ingredient.Quantity *= scale;
            ingredient.IngredientCalories *= scale; //scales the calories as well 
        }
    }

    public static void ResetScale(Recipe recipe)
    {
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            ingredient.Quantity = ingredient.OriginalQuantity;
        }
    }
}

class RecipeProgram
{
    delegate void CaloriesNotificationHandler(int totalCalories);
    static event CaloriesNotificationHandler CaloriesNotification;
    static List<Recipe> recipes = new List<Recipe>();

    static Recipe GetRecipeFromUser()
    {
        Console.Write("Enter the recipe name: ");
        string recipeName = Console.ReadLine();

        Console.Write("Enter the number of ingredients: ");
        int ingredientCount = int.Parse(Console.ReadLine());

        List<Ingredient> ingredients = new List<Ingredient>();

        // Ask user to enter values for ingredients 
        for (int i = 0; i < ingredientCount; i++)
        {
            Console.Write($"Enter ingredient #{i + 1} name: ");
            string ingredientName = Console.ReadLine();

            Console.Write($"Enter quantity for {ingredientName}: ");
            double quantity = double.Parse(Console.ReadLine());

            Console.Write($"Enter unit of measurement for {ingredientName}: ");
            string unit = Console.ReadLine();

            Console.Write($"Enter the number of calories for {ingredientName}: ");
            int ingCalories = int.Parse(Console.ReadLine());

            Console.WriteLine($"Enter the food group for {ingredientName}: ");
            Console.WriteLine("Food Groups:");
            Console.WriteLine("1. Fruits");
            Console.WriteLine("2. Vegetables");
            Console.WriteLine("3. Grains");
            Console.WriteLine("4. Protein");
            Console.WriteLine("5. Dairy");

            int foodGroupChoice = int.Parse(Console.ReadLine());
            string foodGroupSelection;

            if (foodGroupChoice == 1)
            {
                foodGroupSelection = "Fruits";

            }

            else if (foodGroupChoice == 2)
                {
                foodGroupSelection = "Vegetables";
            }

            else if (foodGroupChoice == 3)
            {
                foodGroupSelection = "Grains";
            }
           
            else if (foodGroupChoice == 4)
            {
                foodGroupSelection = "Protein";
            }
            else if (foodGroupChoice == 5)
            {
                foodGroupSelection = "Dairy";
            }
            else
            {
                foodGroupSelection = "Other";
            }

            Ingredient ingredient = new Ingredient
            {
                Name = ingredientName,
                Quantity = quantity,
                OriginalQuantity = quantity,
                Unit = unit,
                IngredientCalories = ingCalories,
                FoodGroup = foodGroupSelection
            };

            ingredients.Add(ingredient);
        }

        Console.Write("Enter the number of steps: ");
        int stepCount = int.Parse(Console.ReadLine());

        List<Step> steps = new List<Step>();

        //  Ask user for steps
        for (int i = 0; i < stepCount; i++)
        {
            Console.Write($"Enter step #{i + 1} description: ");
            string stepDescription = Console.ReadLine();

            Step step = new Step
            {
                Description = stepDescription
            };

            steps.Add(step);
        }

        Recipe recipe = new Recipe
        {
            Name = recipeName,
            Ingredients = ingredients,
            Steps = steps
        };

        return recipe;
    }

    static void ResetRecipe(ref Recipe recipe) // fixed
    {
        Console.WriteLine("Resetting the recipe...\n");

        recipe = GetRecipeFromUser();

        Console.WriteLine("\nRecipe has been reset and updated.");
    }

    static void Main()
    {
        Console.WriteLine("Welcome to the Recipe Program!");

        bool exitProgram = false;
        while (!exitProgram)
        {
            Console.WriteLine("\nPlease select one of the following options:");
            Console.WriteLine("1. Enter a new recipe");
            Console.WriteLine("2. Scale a recipe");
            Console.WriteLine("3. Display a recipe");
            Console.WriteLine("4. Reset ingredient quantities");
            Console.WriteLine("5. Erase recipe and enter a new one");
            Console.WriteLine("0. Exit the program");

            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Recipe newRecipe = GetRecipeFromUser();
                    recipes.Add(newRecipe);
                    break;
                case 2:
                    if (recipes.Count > 0)
                    {
                        Console.WriteLine("\nScaling Options:");
                        Console.WriteLine("1. Half");
                        Console.WriteLine("2. Double");
                        Console.WriteLine("3. Triple");
                        Console.Write("Enter the scaling option: ");
                        int scaleOption = int.Parse(Console.ReadLine());

                        if (scaleOption >= 1 && scaleOption <= 3)
                        {
                            Console.WriteLine("\nRecipes:");
                            for (int i = 0; i < recipes.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                            }

                            Console.Write("Enter the number of the recipe to scale: ");
                            int recipeIndex = int.Parse(Console.ReadLine()) - 1;

                            if (recipeIndex >= 0 && recipeIndex < recipes.Count)
                            {
                                Recipe selectedRecipe = recipes[recipeIndex];
                                RecipeScaler.ScaleRecipe(selectedRecipe, scaleOption);

                                Console.WriteLine("\nScaled Recipe:\n");
                                selectedRecipe.DisplayRecipe();

                                Console.WriteLine("\nDo you want to keep the scaled quantities? (Y/N)");
                                string keepScaleChoice = Console.ReadLine();

                                if (keepScaleChoice.ToUpper() == "N")
                                {
                                    RecipeScaler.ResetScale(selectedRecipe);

                                    Console.WriteLine("\nReset to Original Recipe:\n");
                                    selectedRecipe.DisplayRecipe();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid recipe number. Scaling canceled.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid scaling option. Recipe remains unchanged.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No recipes found. Please enter a recipe first.");
                    }
                    break;
                case 3:
                    if (recipes.Count > 0)
                    {
                        Console.WriteLine("\nRecipes:");
                        for (int i = 0; i < recipes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                        }

                        Console.Write("Enter the number of the recipe to display: ");
                        int recipeIndex = int.Parse(Console.ReadLine()) - 1;

                        if (recipeIndex >= 0 && recipeIndex < recipes.Count)
                        {
                            Recipe selectedRecipe = recipes[recipeIndex];
                            selectedRecipe.DisplayRecipe();
                        }
                        else
                        {
                            Console.WriteLine("Invalid recipe");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No recipes found. Please enter a recipe first.");
                    }
                    break;
                case 4:
                    if (recipes.Count > 0)
                    {
                        Console.WriteLine("\nRecipes:");
                        for (int i = 0; i < recipes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                        }

                        Console.Write("Enter the number of the recipe to reset ingredient quantities: ");
                        int recipeIndex = int.Parse(Console.ReadLine()) - 1;

                        if (recipeIndex >= 0 && recipeIndex < recipes.Count)
                        {
                            Recipe selectedRecipe = recipes[recipeIndex];
                            RecipeScaler.ResetScale(selectedRecipe);

                            Console.WriteLine("\nIngredient quantities have been reset to their original values.");
                            selectedRecipe.DisplayRecipe();
                        }
                        else
                        {
                            Console.WriteLine("Invalid recipe number. Reset canceled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No recipes found. Please enter a recipe first.");
                    }
                    break;
                case 5:
                    if (recipes.Count > 0)
                    {
                        Console.WriteLine("\nRecipes:");
                        for (int i = 0; i < recipes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                        }

                        Console.Write("Enter the number of the recipe to erase and enter a new one: ");
                        int recipeIndex = int.Parse(Console.ReadLine()) - 1;

                        if (recipeIndex >= 0 && recipeIndex < recipes.Count)
                        {
                            recipes.RemoveAt(recipeIndex);

                            Recipe newRecipee = GetRecipeFromUser();
                            recipes.Add(newRecipee);

                            Console.WriteLine("\nRecipe has been erased and replaced with a new one.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid recipe number. Erase canceled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No recipes found. Please enter a recipe first.");
                    }
                    break;
                case 0:
                    exitProgram = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }

        Console.WriteLine("\nThank you for using the Recipe Program. Goodbye!");
    }
}
