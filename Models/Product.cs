public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    // Foreign key and navigation property
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}