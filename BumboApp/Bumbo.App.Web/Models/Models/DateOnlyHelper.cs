using System.Runtime.InteropServices.JavaScript;

namespace Bumbo.App.Web.Models.Models;

public class DateOnlyHelper
{
    public static DateOnly GetFirstDayOfWeek(DateOnly date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Tuesday: date = date.AddDays(-1);
                break;
            case DayOfWeek.Wednesday: date = date.AddDays(-2);
                break;
            case DayOfWeek.Thursday: date = date.AddDays(-3);
                break;
            case DayOfWeek.Friday: date = date.AddDays(-4);
                break;
            case DayOfWeek.Saturday: date = date.AddDays(-5);
                break;
            case DayOfWeek.Sunday: date = date.AddDays(-6);
                break;
        }

        return date;
    }
}