using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Domain.Models.Schedueling;

public class ScheduleModel
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string Day { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Required]
    public string Department { get; set; }
}
