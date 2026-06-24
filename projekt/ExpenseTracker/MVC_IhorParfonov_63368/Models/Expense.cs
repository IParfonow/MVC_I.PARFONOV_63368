using System.ComponentModel.DataAnnotations;

namespace MVC_IhorParfonov_63368.Models;

public class Expense
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Obowiazkowe pole")]
    public string Category { get; set; } = string.Empty;
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Kwota ma byc wieksza od 0")]
    public decimal Amount { get; set; }
    
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}