namespace MVC_IhorParfonov_63368.Models;

public class Income
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty; // Źródło (Джерело)
    public decimal Amount { get; set; }
    
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}