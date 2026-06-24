using Microsoft.AspNetCore.Mvc;
using MVC_IhorParfonov_63368.Data;
using MVC_IhorParfonov_63368.Models;

public class IncomesController : Controller
{
    private readonly ApplicationDbContext _context;
    public IncomesController(ApplicationDbContext context) => _context = context;

    [HttpPost]
    [HttpPost]
    public IActionResult Create(Income income)
    {
        _context.Incomes.Add(income);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }
}