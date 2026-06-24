using Microsoft.AspNetCore.Mvc;
using MVC_IhorParfonov_63368.Data;
using MVC_IhorParfonov_63368.Models;

namespace MVC_IhorParfonov_63368.Controllers;

public class ExpensesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExpensesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Expenses
    public IActionResult Index()
    {
        var expenses = _context.Expenses.ToList();
        return View(expenses);
    }
    // GET: Expenses/Create
    public IActionResult Create()
    {
        return View();
    }

// POST: Expenses/Create
    [HttpPost]
    public IActionResult Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(expense);
    }
}