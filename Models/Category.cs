public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Navigation property for related Products
    public ICollection<Product> Products { get; set; }
    
    // Foreign key for Supplier
    public int SupplierId { get; set; }
    
    // Navigation property for Supplier
    public Supplier Supplier { get; set; }
}
