using MVC_IhorParfonov_63368.Models;

namespace MVC_IhorParfonov_63368.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.ExpenseCategories.Any())
        {
            var expenseCategories = new List<ExpenseCategory>
            {
                new ExpenseCategory { Name = "Jedzenie" },
                new ExpenseCategory { Name = "Mieszkanie" },
                new ExpenseCategory { Name = "Transport" },
                new ExpenseCategory { Name = "Rozrywka" },
                new ExpenseCategory { Name = "Zdrowie" },
                new ExpenseCategory { Name = "Edukacja" },
                new ExpenseCategory { Name = "Ubrania" },
                new ExpenseCategory { Name = "Rachunki" },
                new ExpenseCategory { Name = "Hobby" },
                new ExpenseCategory { Name = "Inne" }
            };
            context.ExpenseCategories.AddRange(expenseCategories);
        }
        
        if (!context.IncomeCategories.Any())
        {
            var incomeCategories = new List<IncomeCategory>
            {
                new IncomeCategory { Name = "Wynagrodzenie" },
                new IncomeCategory { Name = "Premia" },
                new IncomeCategory { Name = "Inne" }
            };
            context.IncomeCategories.AddRange(incomeCategories);
        }
    
        context.SaveChanges();
    }
}