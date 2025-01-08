using Microsoft.EntityFrameworkCore;
using SmartCredit.BackEnd.Domain.CustomEntities;
using SmartCredit.BackEnd.Domain.Entities;

namespace SmartCredit.BackEnd.Persistence.Context;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<CreditCard>()
            .HasIndex(u => u.CardNumber)
            .IsUnique();

        modelBuilder.Entity<CreditCardStatement>().HasNoKey();
    }

    public DbSet<CreditCard> CreditCards { get; set;}
    public DbSet<Transaction> Transactions { get; set;}
    public DbSet<User> Users { get; set;}



    //No register entities

    public DbSet<CreditCardStatement> CreditCardStatement { get; set; }

}