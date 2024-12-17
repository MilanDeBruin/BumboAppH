using Bumbo.Domain.Enums;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Bumbo.Data.SqlRepository;
using BumboApp.Models.Models;

namespace Bumbo.Domain.Models.Forecast;

public class GenerateForecastModel
{
    
    public GenerateForecastModel(int branchId, HolidayModel? nextHoliday, List<GenerateForecastDayModel> forecastDays)
    {
        BranchId = branchId;
        NextHoliday = nextHoliday;
        ForecastDays = forecastDays;
    }
    
    public int BranchId { get; }
    private HolidayModel? NextHoliday { get; }
    private List<GenerateForecastDayModel> ForecastDays { get; }

    public List<Data.Models.Forecast> GenerateForecast(List<Norm> norms)
    {
        List<Data.Models.Forecast> forecasts = new List<Data.Models.Forecast>();

        foreach (var forecastDay in ForecastDays)
        {
            forecastDay.CalculateAmountOfCustomers(NextHoliday);
            forecasts.Add(new Data.Models.Forecast()
            {
                BranchId = BranchId,
                Date = forecastDay.Date,
                Department = DepartmentEnum.Dkw.ToString(),
                ManHours = CalculateShelfStackerHours(
                    norms.FirstOrDefault(n => n.SupermarketActivity == "Coli uitladen"),
                    norms.FirstOrDefault(n => n.SupermarketActivity == "Vakken vullen"),
                    forecastDay.AmountOfCollies
                    )
                
            });
            forecasts.Add(new Data.Models.Forecast()
            {
                BranchId = BranchId,
                Date = forecastDay.Date,
                Department = DepartmentEnum.Kassa.ToString(),
                ManHours = CalculateCheckoutHours(
                    norms.FirstOrDefault(n => n.SupermarketActivity == "Kassa"),
                    forecastDay.AmountOfCustomers
                )
                
            });
            forecasts.Add(new Data.Models.Forecast()
            {
                BranchId = BranchId,
                Date = forecastDay.Date,
                Department = DepartmentEnum.Vers.ToString(),
                ManHours = CalculateFreshHours(
                    norms.FirstOrDefault(n => n.SupermarketActivity == "Vers"),
                    forecastDay.AmountOfCustomers
                )
                
            });
        }

        return forecasts;
    }

    private int CalculateShelfStackerHours(Norm? unloadNorm, Norm? shelfStackingNorm, int amountOfCollies)
    {
        if (unloadNorm == null || shelfStackingNorm == null)
        {
            throw new InvalidOperationException("Could not find norms for ShelfStacker");
        }

        var hours = (int)Math.Ceiling((double)amountOfCollies / unloadNorm.NormPerHour + 
                                      amountOfCollies) / shelfStackingNorm.NormPerHour;

        return hours == 0 ? 1 : hours;
    }

    private int CalculateCheckoutHours(Norm? checkoutNorm, int amountOfCustomers)
    {
        if (checkoutNorm == null)
        {
            throw new InvalidOperationException("Could not find norm for Checkout");
        }

        var hours = (int)Math.Ceiling((double)amountOfCustomers) / checkoutNorm.NormPerHour;
        
        return hours == 0 ? 1 : hours;
    }

    private int CalculateFreshHours(Norm? freshNorm, int amountOfCustomers)
    {
        if (freshNorm == null)
        {
            throw new InvalidOperationException("Could not find norm for Fresh");
        }

        var hours = (int)Math.Ceiling((double)amountOfCustomers) / freshNorm.NormPerHour;

        return hours == 0 ? 1 : hours;
    }
}