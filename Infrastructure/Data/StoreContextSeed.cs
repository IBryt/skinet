using Core.Entities;
using System.Text.Json;

namespace Infrastructure.Data;

public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        await SeedDataAsync<ProductBrand>(context, "../Infrastructure/Data/SeedData/brands.json");
        await SeedDataAsync<ProductType>(context, "../Infrastructure/Data/SeedData/types.json");
        await SeedDataAsync<Product>(context, "../Infrastructure/Data/SeedData/products.json");
    }

    private static async Task SeedDataAsync<T>(StoreContext context, string jsonPath) where T : BaseEntity
    {
        if (context.Set<T>().Any())
        {
            return;
        }
        var jsonData = File.ReadAllText(jsonPath);
        var entities = JsonSerializer.Deserialize<List<T>>(jsonData);
        context.Set<T>().AddRange(entities);
        await context.SaveChangesAsync();

    }
}
