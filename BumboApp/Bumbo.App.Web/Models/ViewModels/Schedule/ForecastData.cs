using Bumbo.Domain.Enums;

namespace Bumbo.App.Web.Models.ViewModels.Schedule
{
    public class ForecastData
    {
        public DateOnly Date { get; set; }
        public DepartmentEnum Department { get; set; }
        public double ManHours { get; set; }
    }
}
