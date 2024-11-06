public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Navigation property for related Categories
    public ICollection<Category> Categories { get; set; }
}