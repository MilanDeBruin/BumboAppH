using Bumbo.Data.Context;
using Bumbo.Data.Models;
using Bumbo.Domain.Models.Schedueling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Bumbo.Domain.Services.CAO;
using Bumbo.Domain.Enums;

namespace Bumbo.Domain.Services.Scheduling;

public class Scheduling
{
    BumboDbContext Context;
    private readonly ILogger<Scheduling> _logger;
    private readonly ICaoScheduleService icss;
    public Scheduling(BumboDbContext context, ILogger<Scheduling> _Slogger, ICaoScheduleService icss)
    {
        Context = context;
        _logger = _Slogger;
        this.icss = icss;
    }

    public DateTime GetStartOfWeek(int year, int week)
    {
        var jan1 = new DateTime(year, 1, 1);
        var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;

        var firstMonday = jan1.AddDays(daysOffset);
        return firstMonday.AddDays((week - 1) * 7);
    }
    public ScheduleSuccesModel SendDataToDb(ScheduleModel model)
    {
        DateOnly date = DateOnly.FromDateTime(model.Date).AddDays(1);
        TimeOnly startTime = TimeOnly.FromTimeSpan(model.StartTime);
        WorkSchedule checker = Context.WorkSchedules.FirstOrDefault(s => s.EmployeeId == model.EmployeeId && s.Date == date && s.BranchId == 1 && startTime == s.StartTime);
        List<WorkSchedule> dayWorkSchedules = Context.WorkSchedules.Where(s => s.EmployeeId == model.EmployeeId && s.Date == date && s.BranchId == 1).ToList();

        foreach (WorkSchedule schedule in dayWorkSchedules) 
        {
            if (startTime > schedule.StartTime && startTime <= schedule.EndTime) 
            {
                return new ScheduleSuccesModel() { Success = false, Message = "Een kan geen dienst worden ingepland tijdens een andere dienst" };
            }
        }
        if (checker == null)
        {

            WorkSchedule schedule = new WorkSchedule()
            {
                EmployeeId = model.EmployeeId,
                Date = date,
                BranchId = 1,
                StartTime = startTime,
                EndTime = TimeOnly.FromTimeSpan(model.EndTime),
                Department = model.Department,
                WorkStatus = "Default",
                Concept = true,
                IsSick = false
            };

            CaoSheduleValidatorEnum result = icss.ValidateSchedule(schedule);
            if (result == CaoSheduleValidatorEnum.Valid)
            {
                _logger.LogInformation($" deze datum wordt naar de db gestuurd: {date} --------------------------------------------------------------------------------------------------------------------------------------------------- key word");

                Context.WorkSchedules.Add(schedule);
                Context.SaveChanges();
                return new ScheduleSuccesModel() { Success = true, Message = result.ToString() };
            }
            else 
            {
                return new ScheduleSuccesModel() { Success = false, Message = result.ToString() };
            }

        }
        else 
        {
            checker.EndTime = TimeOnly.FromTimeSpan(model.EndTime);
            checker.Department = model.Department;
            CaoSheduleValidatorEnum result = icss.ValidateSchedule(checker);

            if (result == CaoSheduleValidatorEnum.Valid)
            {
                Context.SaveChanges();
                return new ScheduleSuccesModel() { Success = true, Message = result.ToString() };
            }
            else
            {
                return new ScheduleSuccesModel() { Success = false, Message = result.ToString() };
            }
            
        }
    }

    public ScheduleSuccesModel DeleteDataFromDb(ScheduleModel model) 
    {
        WorkSchedule schedule = Context.WorkSchedules
           .FirstOrDefault(s =>
               s.EmployeeId == model.EmployeeId &&
               s.Date == DateOnly.FromDateTime(model.Date) &&
               s.BranchId == 1 &&
               s.StartTime == TimeOnly.FromTimeSpan(model.StartTime)
           );

        if (schedule != null)
        {
            Context.WorkSchedules.Remove(schedule);
            Context.SaveChanges();
            return new ScheduleSuccesModel() { Success = true, Message = "Dienst verwijdert" };
        }
        else 
        {
            return new ScheduleSuccesModel() { Success = false, Message = "Er ging iets miss probeer later opnieuw" };
        }


    }

    public void PublishSchedule(DateOnly StartDate) 
    {
        List<WorkSchedule> schedules = Context.WorkSchedules.Where(s => s.Date >= StartDate && s.Date <= StartDate.AddDays(7)).ToList();
        foreach (WorkSchedule schedule in schedules) 
        {
            schedule.Concept = false;
            _logger.LogInformation($" My date is: {schedule.Date} ------------------------------------- My date is + 1: {schedule.Date.AddDays(1)}------------------------------------------------------------------------------------");
        }
        Context.SaveChanges();
    }
}