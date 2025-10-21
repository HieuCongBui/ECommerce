using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Shared.DataAccess.Contracts
{
    public interface IDbContextBuilder
    {
        void ConfigureContext(DbContextOptionsBuilder optionsBuilder);
        void ConfigureContext(DbContextOptionsBuilder optionsBuilder, string schema);
    }
}
