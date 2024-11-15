using Bumbo.App.Web.Models.Models.Forecast;
using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.App.Web.Models.Services;

public class GenerateForecastService : IGenerateForecastService
{
    private readonly IForecastRepository _forecastRepository;
    private readonly INormRepository _normRepository;

    public GenerateForecastService(IForecastRepository forecastRepository, INormRepository normRepository)
    {
        _forecastRepository = forecastRepository;
        _normRepository = normRepository;
    }
    
    
    public void GenerateForecast(GenerateForecastModel forecastModel)
    {
        List<Norm> branchNorms = _normRepository.GetAllFromBranch(forecastModel.BranchId);
        _forecastRepository.SetWeekForecast(forecastModel.GenerateForecast(branchNorms));
    }
}