using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("ProductsDB"));

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Product>("Products");
modelBuilder.EntitySet<Category>("Categories");
modelBuilder.EntitySet<Supplier>("Suppliers");

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", modelBuilder.GetEdmModel()).Select().Filter().Count().OrderBy().Expand());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Ensure controllers are mapped for OData
});

// Seed data into the in-memory database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!dbContext.Suppliers.Any())
    {
        dbContext.Suppliers.AddRange(
            new Supplier { Name = "Global Supplies" },
            new Supplier { Name = "Local Supplies" }
        );
        dbContext.SaveChanges();
    }

    // Seed categories if not already present
    if (!dbContext.Categories.Any())
    {
        var globalSupplies = dbContext.Suppliers.First(c => c.Name == "Global Supplies");
        var localSupplies = dbContext.Suppliers.First(c => c.Name == "Local Supplies");

        dbContext.Categories.AddRange(
            new Category { Name = "Electronics", Supplier = globalSupplies },
            new Category { Name = "Accessories", Supplier = localSupplies }
        );
        dbContext.SaveChanges();
    }

    // Seed products if not already present and assign categories
    if (!dbContext.Products.Any())
    {
        var electronicsCategory = dbContext.Categories.First(c => c.Name == "Electronics");
        var accessoriesCategory = dbContext.Categories.First(c => c.Name == "Accessories");

        dbContext.Products.AddRange(
            new Product { Name = "Laptop", Price = 1000, Category = electronicsCategory },
            new Product { Name = "Mouse", Price = 20, Category = accessoriesCategory },
            new Product { Name = "Keyboard", Price = 50, Category = accessoriesCategory },
            new Product { Name = "Phone", Price = 500, Category = accessoriesCategory }
        );
        dbContext.SaveChanges();
    }
}

app.Run();
