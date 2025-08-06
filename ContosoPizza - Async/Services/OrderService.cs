using ContosoPizza.Models;
using Dapper;
using Microsoft.Data.SqlClient;


namespace ContosoPizza.Services
{
    public class OrderService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task AddAsync(Order order)
        {
            using var connection = GetConnection();
            order.Id = await connection.QuerySingleAsync<int>(
                "INSERT INTO OrderHistory (PizzaId) OUTPUT INSERTED.Id VALUES (@PizzaId)",
                order);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            using var connection = GetConnection();
            var orders = await connection.QueryAsync<Order>("SELECT * FROM OrderHistory ORDER BY OrderDate DESC");
            return [.. orders];
        }

        public async Task<List<Order>> GetByPizzaIdAsync(int pizzaId)
        {
            using var connection = GetConnection();
            var orders = await connection.QueryAsync<Order>(
                "SELECT * FROM OrderHistory WHERE PizzaId = @PizzaId ORDER BY OrderDate DESC",
                new { PizzaId = pizzaId });
            return [.. orders];
        }
    }
}
