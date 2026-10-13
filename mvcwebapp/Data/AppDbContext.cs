using Microsoft.EntityFrameworkCore;
using mvcwebapp.Models;

namespace mvcwebapp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
//    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//     {
        
//     }

public DbSet <User> Users{ get; set; }
public DbSet <Product> Products{ get; set; }


}
