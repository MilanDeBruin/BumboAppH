namespace Bumbo.App.Web.Models.ViewModels.Forecast;

public class DayForecastViewModel
{
    public DateOnly Date { get; set; }
    public int FreshHours { get; set; }
    public int CheckoutHours { get; set; }
    public int ShelfStackerHours { get; set; }
}