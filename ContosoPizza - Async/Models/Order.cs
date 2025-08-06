namespace ContosoPizza.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
