using Microsoft.EntityFrameworkCore;
using MVC_IhorParfonov_63368.Models;

namespace MVC_IhorParfonov_63368.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<IncomeCategory> IncomeCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
    }
}