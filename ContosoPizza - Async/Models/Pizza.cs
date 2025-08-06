namespace ContosoPizza.Models;

/// <summary>
/// Represents a pizza item.
/// </summary>
public class Pizza
{
    /// <summary>
    /// The unique identifier for the pizza.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the pizza (e.g., Margherita).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether the pizza is gluten-free.
    /// </summary>
    public bool IsGlutenFree { get; set; }

    public decimal Price { get; set; } 

}

