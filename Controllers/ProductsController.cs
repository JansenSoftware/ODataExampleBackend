using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using MyNamespace.Controllers;

[Route("odata/[controller]")]
public class ProductsController : GenericODataController<Product>
{
    public ProductsController(AppDbContext context) : base(context) { }


    // Override the Post method
    [EnableQuery]
    public override IActionResult Post([FromBody] Product product)
    {
        // Additional logic before calling the base Post
        if (product.Price < 0)
        {
            return BadRequest("Product price cannot be negative.");
        }

        // Call the base Post method to handle the actual database insert
        var result = base.Post(product);

        // Additional logic after the base Post call, if needed
        // Example: Logging, notification, etc.
        
        return result;
    }
}