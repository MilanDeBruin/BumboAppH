using BumboApp.Models.Models;

namespace Bumbo.Domain.Models.Forecast;

public class GenerateForecastDayModel
{
    public GenerateForecastDayModel(DateOnly date, int amountOfCustomers, int amountOfCollies, double? temperature)
    {
        Date = date;
        AmountOfCustomers = amountOfCustomers;
        AmountOfCollies = amountOfCollies;
        Temperature = temperature;
    }
    
    
    public DateOnly Date { get; private set; }
    public int AmountOfCustomers { get; private set; }
    public int AmountOfCollies { get; private set; }
    private double? Temperature { get; set; }

    public void CalculateAmountOfCustomers(HolidayModel? nextHoliday)
    {
        if (Temperature != null)
        {
            CalculateAmountOfCustomersOnWeather();
        }

        if (nextHoliday != null)
        {
            CalculateAmountOfCustomersOnHoliday(nextHoliday);
        }
    }

    private void CalculateAmountOfCustomersOnWeather()
    {
        switch (Temperature)
        {
            case var expression when Temperature < 0: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.8);
                break;
            case var expression when Temperature < 10: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.9);
                break;
            case var expression when Temperature < 15: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.95);
                break;
            case var expression when Temperature < 20: break;
            case var expression when Temperature < 25: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 1.1);
                break;
            case var expression when Temperature < 30: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 1.15);
                break;
            default: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.9);
                break;
        }
    }

    private void CalculateAmountOfCustomersOnHoliday(HolidayModel nextHoliday)
    {
        int daysUntilHoliday = (nextHoliday.Date - Date.ToDateTime(new TimeOnly())).Days;

        switch (daysUntilHoliday)
        {
            case 2: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 1.15);
                break;
            case 1: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 1.25);
                break;
            case 0: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.5);
                break;
            case -1: AmountOfCustomers = Convert.ToInt32(Convert.ToDouble(AmountOfCustomers) * 0.75);
                break;
        }

    }
}