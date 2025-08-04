using ContosoPizza.Models;
using Microsoft.Data.SqlClient;

using Dapper;


namespace ContosoPizza.Services;


public class PizzaService
{
    private readonly string _connectionString;

    public PizzaService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }


    public List<Pizza> GetAll()
    {
        using var connection = GetConnection();
        return [.. connection.Query<Pizza>("SELECT * FROM Pizza")];
    }
    public Pizza? Get(int id)
    {
        using var connection = GetConnection();
        return connection.QueryFirstOrDefault<Pizza>("SELECT * FROM Pizza WHERE Id = @Id", new { Id = id });
    }
    public void Add(Pizza pizza)
    {
        using var connection = GetConnection();
        pizza.Id = connection.QuerySingle<int>("INSERT INTO Pizza (Name, IsGlutenFree) OUTPUT INSERTED.Id VALUES (@Name, @IsGlutenFree)", pizza);
    }
    public void Update(Pizza pizza)
    {
        using var connection = GetConnection();
        connection.Execute("UPDATE Pizza SET Name = @Name, IsGlutenFree = @IsGlutenFree WHERE Id = @Id", pizza);
    }
    public void Delete(int id)
    {
        using var connection = GetConnection();
        connection.Execute("DELETE FROM Pizza WHERE Id = @Id", new { Id = id });
    }



}
