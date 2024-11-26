using Bumbo.Domain.Models.Forecast;

namespace Bumbo.Domain.Services.Forecast;

public interface IGenerateForecastService
{
    public void GenerateForecast(GenerateForecastModel forecastModel);
}