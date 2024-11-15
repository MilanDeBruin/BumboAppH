namespace Bumbo.App.Web.Models.ViewModels.Forecast;

public class GenerateForecastDayViewModel
{
    public DateOnly Date { get; set; }
    public int AmountOfCustomers { get; set; }
    public int AmountOfCollies { get; set; }
    public double? Temperature { get; set; }
}