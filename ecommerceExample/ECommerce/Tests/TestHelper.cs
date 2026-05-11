using ECommerce.AppCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class TestHelper
{
    public static ECommerceDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ECommerceDbContext(options);
    }
}
