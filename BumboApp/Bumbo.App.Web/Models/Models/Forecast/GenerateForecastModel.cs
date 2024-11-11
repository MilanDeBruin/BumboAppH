using Bumbo.App.Web.Models.Enums;
using Bumbo.App.Web.Models.Repositorys;
using Bumbo.Data.Context;
using Bumbo.Data.Models;
using BumboApp.Models.Models;

namespace Bumbo.App.Web.Models.Models.Forecast;

public class GenerateForecastModel
{
    private readonly ForecastRepository _repository;
    private readonly BumboDbContext _dbContext;
    
    public GenerateForecastModel(int branchId, HolidayModel? nextHoliday, List<GenerateForecastDayModel> forecastDays, BumboDbContext dbContext)
    {
        _repository = new ForecastRepository(dbContext);
        _dbContext = dbContext;
        BranchId = branchId;
        NextHoliday = nextHoliday;
        ForecastDays = forecastDays;
    }
    
    private int BranchId { get; }
    private HolidayModel? NextHoliday { get; }
    private List<GenerateForecastDayModel> ForecastDays { get; }

    public void GenerateForecast()
    {
        List<Norm> norms = _dbContext.Norms.Where(n => n.BranchId == BranchId).ToList();
        List<Data.Models.Forecast> forecasts = new List<Data.Models.Forecast>();

        foreach (var forecastDay in ForecastDays)
        {
            forecastDay.CalculateAmountOfCustomers(NextHoliday);
            forecasts.Add(new Data.Models.Forecast()
            {
                BranchId = BranchId,
                Date = forecastDay.Date,
                Department = DepartmentEnum.Shelf.ToString(),
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
        _repository.SetWeekForecast(forecasts);
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