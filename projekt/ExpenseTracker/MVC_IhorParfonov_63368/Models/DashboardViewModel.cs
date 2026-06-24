namespace MVC_IhorParfonov_63368.Models;

public class DashboardViewModel
{
    public decimal TotalBalance { get; set; }
    public decimal TotalExpensesMonth { get; set; }
    public decimal TotalIncomes { get; set; }
    public decimal PreviousBalance { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UserSettings Settings { get; set; } = new UserSettings();
    
    // РОЗДІЛЕНО НА ДВІ КОЛЕКЦІЇ
    public IEnumerable<ChartDataPoint> ExpenseChartData { get; set; } = new List<ChartDataPoint>();
    public IEnumerable<ChartDataPoint> IncomeChartData { get; set; } = new List<ChartDataPoint>();
}