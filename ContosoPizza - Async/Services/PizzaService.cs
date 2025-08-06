using ContosoPizza.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly string _connectionString;

    public PizzaService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private SqlConnection GetConnection() => new SqlConnection(_connectionString);

    public async Task<List<Pizza>> GetAllAsync()
    {
        using var connection = GetConnection();
        var pizzas = await connection.QueryAsync<Pizza>("SELECT * FROM Pizza");
        return [.. pizzas];
    }

    public async Task<Pizza?> GetAsync(int id)
    {
        using var connection = GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Pizza>(
            "SELECT * FROM Pizza WHERE Id = @Id",
            new { Id = id });
    }

    public async Task AddAsync(Pizza pizza)
    {
        using var connection = GetConnection();
        pizza.Id = await connection.QuerySingleAsync<int>(
            "INSERT INTO Pizza (Name, IsGlutenFree,Price) OUTPUT INSERTED.Id VALUES (@Name, @IsGlutenFree, @Price)",
            pizza);
    }

    public async Task UpdateAsync(Pizza pizza)
    {
        using var connection = GetConnection();
        await connection.ExecuteAsync(
            "UPDATE Pizza SET Name = @Name, IsGlutenFree = @IsGlutenFree WHERE Id = @Id",
            pizza);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = GetConnection();
        await connection.ExecuteAsync(
            "DELETE FROM Pizza WHERE Id = @Id",
            new { Id = id });
    }

    //  PAGINATION WITHOUT STORED PROCEDURE
    // public async Task<List<Pizza>> GetPaginatedAsync(int pageNumber, int pageSize)
    // {
    //     using var connection = GetConnection();
    //     var offset = (pageNumber - 1) * pageSize;
    //     var query = "SELECT * FROM Pizza ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
    //     var pizzas = await connection.QueryAsync<Pizza>(query, new { Offset = offset, PageSize = pageSize });
    //     return [.. pizzas];
    // }

    // PAGINATION WITH STORED PROCEDURE
    public async Task<List<Pizza>> GetPaginatedFromSPAsync(int pageNumber, int pageSize)
    {
        using var connection = GetConnection();
        var pizzas = await connection.QueryAsync<Pizza>(
            "GetPaginatedPizzas",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: CommandType.StoredProcedure);

        return [.. pizzas];
    }

    // PAGINATION WITH STORED PROCEDURE AND SORTING
    public async Task<List<Pizza>> GetPaginatedSortedAsync(int pageNumber, int pageSize)
    {
        using var connection = GetConnection();
        var pizzas = await connection.QueryAsync<Pizza>(
            "GetPaginatedSortedPizzas",
            new { PageNumber = pageNumber, PageSize = pageSize },
            commandType: CommandType.StoredProcedure);
        return [.. pizzas];
    }



}
