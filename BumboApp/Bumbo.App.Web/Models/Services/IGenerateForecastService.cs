using Bumbo.App.Web.Models.Models.Forecast;

namespace Bumbo.App.Web.Models.Services;

public interface IGenerateForecastService
{
    public void GenerateForecast(GenerateForecastModel forecastModel);
}