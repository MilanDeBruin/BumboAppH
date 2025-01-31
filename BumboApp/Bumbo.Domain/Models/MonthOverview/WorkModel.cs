using Bumbo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Domain.Models.MonthOverview
{
    public class WorkModel
    {
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public int BranchId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Department { get; set; } = null!;
        public string WorkStatus { get; set; } = null!;
        public bool IsSick { get; set; }
        public DayOfWeek DayOfWeek{  get; set; }
    }
}
