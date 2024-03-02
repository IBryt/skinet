using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure.Data;

public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        await SeedDataAsync(context, "../Infrastructure/Data/SeedData/brands.json", c => c.ProductBrands);
        await SeedDataAsync(context, "../Infrastructure/Data/SeedData/types.json", c => c.ProductTypes);
        await SeedDataAsync(context, "../Infrastructure/Data/SeedData/products.json", c => c.Products);
    }

    private static async Task SeedDataAsync<T>(StoreContext context, string jsonPath, Func<StoreContext, DbSet<T>> dbSetSelector) where T : class
    {
        if (!dbSetSelector(context).Any())
        {
            var jsonData = File.ReadAllText(jsonPath);
            var entities = JsonSerializer.Deserialize<List<T>>(jsonData);
            await dbSetSelector(context).AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }
    }
}
