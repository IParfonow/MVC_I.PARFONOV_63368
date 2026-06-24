namespace MVC_IhorParfonov_63368.Controllers;

using Microsoft.AspNetCore.Mvc;
using MVC_IhorParfonov_63368.Data;
using MVC_IhorParfonov_63368.Models;
using System;
using System.Linq;
using System.Collections.Generic;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(DateTime? startDate, DateTime? endDate)
    {
        var settings = _context.UserSettings.FirstOrDefault();
        if (settings == null)
        {
            settings = new UserSettings { MonthStartDay = 1, CarryOverBalance = false };
            _context.UserSettings.Add(settings);
            _context.SaveChanges();
        }

        DateTime today = DateTime.Today;
        DateTime defaultStart = today.Day >= settings.MonthStartDay
            ? new DateTime(today.Year, today.Month, settings.MonthStartDay)
            : new DateTime(today.Year, today.Month, settings.MonthStartDay).AddMonths(-1);
        DateTime defaultEnd = defaultStart.AddMonths(1).AddDays(-1);

        DateTime start = startDate ?? defaultStart;
        DateTime end = endDate ?? defaultEnd;

        var periodExpenses = _context.Expenses.Where(e => e.Date >= start && e.Date <= end).ToList();
        var periodIncomes = _context.Incomes.Where(i => i.Date >= start && i.Date <= end).ToList();

        decimal previousBalance = 0;
        if (settings.CarryOverBalance)
        {
            var pastExpenses = _context.Expenses.Where(e => e.Date < start).Sum(e => e.Amount);
            var pastIncomes = _context.Incomes.Where(i => i.Date < start).Sum(i => i.Amount);
            previousBalance = pastIncomes - pastExpenses;
        }

        ViewBag.ExpenseCategories = _context.ExpenseCategories.ToList();
        ViewBag.IncomeCategories = _context.IncomeCategories.ToList();

        var viewModel = new DashboardViewModel
        {
            StartDate = start,
            EndDate = end,
            Settings = settings,
            PreviousBalance = previousBalance,
            TotalIncomes = periodIncomes.Sum(i => i.Amount) + previousBalance,
            TotalExpensesMonth = periodExpenses.Sum(e => e.Amount),
            TotalBalance = periodIncomes.Sum(i => i.Amount) + previousBalance - periodExpenses.Sum(e => e.Amount),
            ExpenseChartData = periodExpenses.GroupBy(e => e.Category)
                .Select(g => new ChartDataPoint { Category = g.Key ?? "Inne", Total = (double)g.Sum(e => e.Amount) })
                .ToList(),
            IncomeChartData = periodIncomes.GroupBy(i => i.Source)
                .Select(g => new ChartDataPoint { Category = g.Key ?? "Inne", Total = (double)g.Sum(i => i.Amount) })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult UpdateSettings(int MonthStartDay, bool CarryOverBalance = false,
        string WalletName = "Twój Portfel")
    {
        var settings = _context.UserSettings.FirstOrDefault();
        if (settings != null)
        {
            settings.MonthStartDay = MonthStartDay;
            settings.CarryOverBalance = CarryOverBalance;

            if (!string.IsNullOrWhiteSpace(WalletName))
            {
                settings.WalletName = WalletName;
            }

            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteTransaction(int id, string type)
    {
        if (type.StartsWith("Wydat"))
        {
            var exp = _context.Expenses.Find(id);
            if (exp != null)
            {
                _context.Expenses.Remove(exp);
                _context.SaveChanges();
            }
        }
        else if (type.StartsWith("Przych"))
        {
            var inc = _context.Incomes.Find(id);
            if (inc != null)
            {
                _context.Incomes.Remove(inc);
                _context.SaveChanges();
            }
        }

        return Ok();
    }

    [HttpPost]
    public IActionResult EditTransaction(int id, string type, decimal amount, DateTime date, string comment)
    {
        if (type.StartsWith("Wydat"))
        {
            var exp = _context.Expenses.Find(id);
            if (exp != null)
            {
                exp.Amount = amount;
                exp.Date = date;
                exp.Comment = comment;
                _context.SaveChanges();
            }
        }
        else if (type.StartsWith("Przych"))
        {
            var inc = _context.Incomes.Find(id);
            if (inc != null)
            {
                inc.Amount = amount;
                inc.Date = date;
                inc.Comment = comment;
                _context.SaveChanges();
            }
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult GetTransactions(string filter, DateTime start, DateTime end)
    {
        var result = new List<object>();

        var expQuery = _context.Expenses.Where(e => e.Date >= start && e.Date <= end);
        var incQuery = _context.Incomes.Where(i => i.Date >= start && i.Date <= end);

        var settings = _context.UserSettings.FirstOrDefault();

        if (settings != null && settings.CarryOverBalance)
        {
            var pastExp = _context.Expenses.Where(e => e.Date < start).Sum(e => e.Amount);
            var pastInc = _context.Incomes.Where(i => i.Date < start).Sum(i => i.Amount);
            decimal previousBalance = pastInc - pastExp;

            if (previousBalance > 0 && (filter == "all" || filter == "income"))
            {
                result.Add(new
                {
                    Id = 0, Date = start.ToShortDateString(), RawDate = start.ToString("yyyy-MM-dd"),
                    Category = "Reszta z poprzedniego miesiąca", Amount = previousBalance, Type = "Przychód",
                    Comment = "Przeniesione automatycznie"
                });
            }
            else if (previousBalance < 0 && (filter == "all" || filter == "expense"))
            {
                result.Add(new
                {
                    Id = 0, Date = start.ToShortDateString(), RawDate = start.ToString("yyyy-MM-dd"),
                    Category = "Zadłużenie z poprzedniego miesiąca", Amount = Math.Abs(previousBalance),
                    Type = "Wydatek", Comment = "Przeniesione automatycznie"
                });
            }
        }

        if (filter == "all")
        {
            result.AddRange(expQuery.Select(e => new
            {
                Id = e.Id, Date = e.Date.ToShortDateString(), RawDate = e.Date.ToString("yyyy-MM-dd"),
                Category = e.Category, Amount = e.Amount, Type = "Wydatek", Comment = e.Comment
            }));
            result.AddRange(incQuery.Select(i => new
            {
                Id = i.Id, Date = i.Date.ToShortDateString(), RawDate = i.Date.ToString("yyyy-MM-dd"),
                Category = i.Source, Amount = i.Amount, Type = "Przychód", Comment = i.Comment
            }));
        }
        else if (filter == "expense")
        {
            result.AddRange(expQuery.Select(e => new
            {
                Id = e.Id, Date = e.Date.ToShortDateString(), RawDate = e.Date.ToString("yyyy-MM-dd"),
                Category = e.Category, Amount = e.Amount, Type = "Wydatek", Comment = e.Comment
            }));
        }
        else if (filter == "income")
        {
            result.AddRange(incQuery.Select(i => new
            {
                Id = i.Id, Date = i.Date.ToShortDateString(), RawDate = i.Date.ToString("yyyy-MM-dd"),
                Category = i.Source, Amount = i.Amount, Type = "Przychód", Comment = i.Comment
            }));
        }
        else
        {
            // ТЕПЕР ШУКАЄ ЯК У ВИДАТКАХ, ТАК І В ДОХОДАХ
            var expMatch = expQuery.Where(e => e.Category == filter).ToList();
            if (expMatch.Any())
            {
                result.AddRange(expMatch.Select(e => new
                {
                    Id = e.Id, Date = e.Date.ToShortDateString(), RawDate = e.Date.ToString("yyyy-MM-dd"),
                    Category = e.Category, Amount = e.Amount, Type = "Wydatek", Comment = e.Comment
                }));
            }

            var incMatch = incQuery.Where(i => i.Source == filter).ToList();
            if (incMatch.Any())
            {
                result.AddRange(incMatch.Select(i => new
                {
                    Id = i.Id, Date = i.Date.ToShortDateString(), RawDate = i.Date.ToString("yyyy-MM-dd"),
                    Category = i.Source, Amount = i.Amount, Type = "Przychód", Comment = i.Comment
                }));
            }
        }

        return Json(result.OrderByDescending(x => DateTime.Parse(((dynamic)x).Date)));
    }
}