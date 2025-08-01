using ContosoPizza.Models;

namespace ContosoPizza.Services;

/// <summary>
/// Provides in-memory data operations for pizzas.
/// </summary>
public static class PizzaService
{
    /// <summary>
    /// Static list that holds all pizza entries.
    /// </summary>
    static List<Pizza> Pizzas { get; }

    /// <summary>
    /// Used to assign unique IDs to new pizzas.
    /// </summary>
    static int nextId = 3;

    // Static constructor to initialize some sample pizzas.
    static PizzaService()
    {
        Pizzas = new List<Pizza>
        {
            new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
            new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true }
        };
    }

    /// <summary>
    /// Retrieves all pizzas.
    /// </summary>
    /// <returns>A list of all pizzas.</returns>
    public static List<Pizza> GetAll() => Pizzas;

    /// <summary>
    /// Retrieves a pizza by its ID.
    /// </summary>
    /// <param name="id">The ID of the pizza to retrieve.</param>
    /// <returns>The pizza if found; otherwise, null.</returns>
    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    /// <summary>
    /// Adds a new pizza to the list.
    /// </summary>
    /// <param name="pizza">The pizza to add.</param>
    public static void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    /// <summary>
    /// Deletes a pizza by its ID.
    /// </summary>
    /// <param name="id">The ID of the pizza to delete.</param>
    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null)
            return;

        Pizzas.Remove(pizza);
    }

    /// <summary>
    /// Updates an existing pizza.
    /// </summary>
    /// <param name="pizza">The pizza with updated values.</param>
    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1)
            return;

        Pizzas[index] = pizza;
    }
}
