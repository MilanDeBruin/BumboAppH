using System.ComponentModel.DataAnnotations;

namespace Bumbo.App.Web.Models.ViewModels.SchoolSchedule
{
    public class SchoolScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public List<DailySchoolSchedule> DailySchoolSchedules { get; set; } = new List<DailySchoolSchedule>();
    }

    public class DailySchoolSchedule : IValidatableObject
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
}
