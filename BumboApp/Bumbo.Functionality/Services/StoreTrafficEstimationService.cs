using Bumbo.Data.Models;
using BumboApp.Models.Models;
using BumboApp.Models.Repositorys;
using Microsoft.IdentityModel.Tokens;

namespace Bumbo.Functionality.Services;

public class StoreTrafficEstimationService
{
    private List<StoreTraffic> _defaultTraffic;

    public StoreTrafficEstimationService()
    {
        _defaultTraffic = new List<StoreTraffic>
        {
            new StoreTraffic() { Amount = 1000 },
            new StoreTraffic() { Amount = 1100 },
            new StoreTraffic() { Amount = 1300 },
            new StoreTraffic() { Amount = 1100 },
            new StoreTraffic() { Amount = 1300 },
            new StoreTraffic() { Amount = 1500 },
            new StoreTraffic() { Amount = 800 }
        };
    }


    public List<StoreTraffic> EstimateStoreTraffic(int branchId, DateOnly firstDayOfWeek)
    {
        List<StoreTraffic> estimatedStoreTraffic = new List<StoreTraffic>();
        DateTime firstDayOfWeekDT = firstDayOfWeek.ToDateTime(new TimeOnly());
        for (int i = 0; i < 7; i++)
        {
            DateTime date = firstDayOfWeekDT.AddDays(i);
            estimatedStoreTraffic.Add(new StoreTraffic() { DateTime = firstDayOfWeekDT});
        }

        try
        {
            estimatedStoreTraffic = ApplyWeatherToTraffic(estimatedStoreTraffic, firstDayOfWeekDT);
        }
        catch (Exception e)
        {
            throw e;
        }
        estimatedStoreTraffic = ApplyHolidaysToTraffic(estimatedStoreTraffic, firstDayOfWeekDT);

        return estimatedStoreTraffic;
    }

    private List<StoreTraffic> ApplyWeatherToTraffic(List<StoreTraffic> traffic, DateTime firstDayOfWeek)
    {
        double lat = 51.688;
        double lon = 5.287;
        
        WeatherRepository weatherRepository = new WeatherRepository();
        List<WeatherDayModel> weatherForecast;
        try
        {
            weatherForecast = weatherRepository.GetWeather(lat, lon, firstDayOfWeek);
        }
        catch (Exception e)
        {
            throw e;
        }

        for (int i = 0; i < 7; i++)
        {
            traffic[i].Amount = ApplyWeatherToDayTraffic(_defaultTraffic[i], weatherForecast[i]);
        }
        
        return traffic;
    }

    private int ApplyWeatherToDayTraffic(StoreTraffic baseTraffic, WeatherDayModel dayWeather)
    {
        double temp = dayWeather.MinTemp;
        int traffic = baseTraffic.Amount;
        switch (temp)
        {
            case var expression when temp < 0: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 0.8);
                break;
            case var expression when temp < 10: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 0.9);
                break;
            case var expression when temp < 15: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 0.95);
                break;
            case var expression when temp < 20: break;
            case var expression when temp < 25: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 1.1);
                break;
            case var expression when temp < 30: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 1.15);
                break;
            default: traffic = Convert.ToInt32(Convert.ToDouble(traffic) * 0.9);
                break;
        }
        return traffic;
    }
    
    private List<StoreTraffic> ApplyHolidaysToTraffic(List<StoreTraffic> traffic, DateTime firstDayOfWeek)
    {
        HolidayRepository holidayRepository = new HolidayRepository();
        List<HolidayModel> holidays = holidayRepository.GetHolidays("NL", firstDayOfWeek);
        if (holidays.IsNullOrEmpty())
        {
            return traffic;
        }

        foreach (var dayTraffic in traffic)
        {
            dayTraffic.Amount = GetDaysAwayFromHoliday(dayTraffic, holidays[0]);
        }
        return traffic;
    }

    private int GetDaysAwayFromHoliday(StoreTraffic day, HolidayModel holiday)
    {
        return (holiday.Date.Date - day.DateTime.Date).Days;
    }
}