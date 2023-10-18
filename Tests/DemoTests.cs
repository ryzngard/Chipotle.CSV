namespace Tests;

#nullable enable

public class DemoTests
{
    // TODO: List pattern matching
    [Fact]
    public void TestListPatternMatching()
    {
        int[] arr1 = [1, 2, 3, 4];

        Assert.True(arr1 is [1, .., 4]);              // .. represents 0 or more elements
        Assert.True(arr1 is [.., 1, 2, 3, 4]);        // Still true because .. matches 0 elements
        Assert.True(arr1 is [_, 2, _, 4]);            // _ is a discard, matches any element
    }

    [Fact]
    public void TestPropertyPatterns()
    {
        // please don't judge my order
        var food = new Burrito(true, true, "Chipotle", "Denver", "CO", ["Cheese", "Chicken", "Lettuce", "Guac", "mild"]); // <!-- collection expression
        var order = new Order(food, 25.24, null); // <!-- inflation hurts

        // Before
        Assert.True(order is { Burrito: { IsHealthy: true, IsTasty: true }, Cost: <= 30.00, Drink: null });

        if (order is { Burrito: { IsHealthy: true }, Cost: <= 30.00, Drink: null })
        {

        }

        // After
        Assert.True(order is { Burrito.IsHealthy: true, Burrito.IsTasty: true, Cost: <= 30.00, Drink: null });
    }

    record class Order(Burrito Burrito, double Cost, string? Drink);
    record class Burrito(bool IsTasty, bool IsHealthy, string Restaurant, string City, string State, string[] Toppings);
}
