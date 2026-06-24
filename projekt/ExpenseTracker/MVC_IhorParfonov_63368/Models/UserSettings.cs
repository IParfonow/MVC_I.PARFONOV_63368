namespace MVC_IhorParfonov_63368.Models;

public class UserSettings
{
    public int Id { get; set; }
    public int MonthStartDay { get; set; } = 1;
    public bool CarryOverBalance { get; set; } = false;
    
    public string WalletName { get; set; } = "Twój Portfel"; 
}