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
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Pizza>>> GetAll()
    {
        var pizzalist = await _pizzaService.GetAllAsync();
        return Ok(pizzalist);
    }

    /// <summary>
    /// Retrieves a specific pizza by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> Get(int id)
    {
        var pizza = await _pizzaService.GetAsync(id);
        if (pizza == null)
            return NotFound();

        return Ok(pizza);
    }

    /// <summary>
    /// Creates a new pizza.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Pizza pizza)
    {
        await _pizzaService.AddAsync(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    /// <summary>
    /// Updates an existing pizza.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Pizza pizza)
    {
        var existingPizza = await _pizzaService.GetAsync(id);
        if (existingPizza is null)
            return NotFound();

        pizza.Id = id;
        await _pizzaService.UpdateAsync(pizza);
        return Ok(pizza);
    }

    /// <summary>
    /// Deletes a pizza by ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var pizza = await _pizzaService.GetAsync(id);
        if (pizza == null)
            return NotFound();

        await _pizzaService.DeleteAsync(id);
        return NoContent();
    }
    // PAGINATION WITHOUT STORED PROCEDURE
    // [HttpGet("paginated")]
    // public async Task<ActionResult<List<Pizza>>> GetPaginated(int pageNumber = 1, int pageSize = 10)
    // {
    //     var pizzas = await _pizzaService.GetPaginatedAsync(pageNumber, pageSize);
    //     return Ok(pizzas);
    // }
    // PAGINATION WITH STORED PROCEDURE
    [HttpGet("paginated")]
    public async Task<ActionResult<List<Pizza>>> GetPaginated(int pageNumber, int pageSize)
    {
        var pizzas = await _pizzaService.GetPaginatedFromSPAsync(pageNumber, pageSize);
        return Ok(pizzas);
    }
    [HttpGet("paginatedsorted")]
    public async Task<ActionResult<List<Pizza>>> GetPaginatedSorted(int pageNumber, int pageSize)
    {
        var pizzas = await _pizzaService.GetPaginatedSortedAsync(pageNumber, pageSize);
        return Ok(pizzas);
    }

}
