using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.Availability;

public class AvailabilityViewModel
{
    public int EmployeeId { get; set; }
    public List<DailyAvailability> DailyAvailabilities { get; set; } = new List<DailyAvailability>();
}

public class DailyAvailability : IValidatableObject
{
    public DateOnly Weekday { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartTime.HasValue && EndTime.HasValue && StartTime > EndTime)
        {
            yield return new ValidationResult(
                "Starttijd kan niet later zijn dan eindtijd",
                [nameof(StartTime), nameof(EndTime)]);
        }
    }
}
