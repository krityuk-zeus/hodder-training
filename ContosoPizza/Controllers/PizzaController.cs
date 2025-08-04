using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pizzaService;
    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    /// <summary>
    /// Retrieves the list of all pizzas.  
    /// This endpoint is used to get all available pizzas.
    /// </summary>
    /// <returns>A list of pizzas.</returns>
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() =>
        _pizzaService.GetAll();

    /// <summary>
    /// Retrieves a specific pizza by its ID.
    /// </summary>
    /// <param name="id">The ID of the pizza to retrieve.</param>
    /// <returns>The requested pizza, or 404 if not found.</returns>
    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = _pizzaService.Get(id);
        if (pizza == null)
            return NotFound();

        return pizza;
    }

    /// <summary>
    /// Creates a new pizza.
    /// </summary>
    /// <param name="pizza">The pizza to create.</param>
    /// <returns>201 Created with the location of the new pizza.</returns>
    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    /// <summary>
    /// Updates an existing pizza.
    /// </summary>
    /// <param name="id">The ID of the pizza to update.</param>
    /// <param name="pizza">The updated pizza object.</param>
    /// <returns>204 No Content if successful, or error if not.</returns>
    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        var existingPizza = _pizzaService.Get(id);
        if (existingPizza is null)
            return NotFound();
         pizza.Id = id; // Set id here as nothing was passed in 'id' attribute so it is currently null, and null or 0 can go into table if update method got called with this obj
        _pizzaService.Update(pizza);

        return Ok(pizza);
    }

    /// <summary>
    /// Deletes a pizza by ID.
    /// </summary>
    /// <param name="id">The ID of the pizza to delete.</param>
    /// <returns>204 No Content if deleted, or 404 if not found.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _pizzaService.Get(id);

        if (pizza == null)
            return NotFound();

        _pizzaService.Delete(id);

        return NoContent();
    }
}
