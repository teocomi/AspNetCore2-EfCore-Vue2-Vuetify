using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Sample.Model;

namespace AspNetCore.Sample.Model
{
  public class DataContext : DbContext
  {

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Room> Rooms { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

  }
}
