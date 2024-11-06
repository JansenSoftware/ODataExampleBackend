using Microsoft.AspNetCore.Mvc;
using MyNamespace.Controllers;

[Route("odata/[controller]")]
public class CategoriesController : GenericODataController<Category>
{
    public CategoriesController(AppDbContext context) : base(context) { }
}
