
using Microsoft.EntityFrameworkCore;

namespace LigaDeFutbol.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
    }
}
