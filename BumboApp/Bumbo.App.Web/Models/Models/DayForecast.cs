using Bumbo.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BumboApp.Models.Models
{
    public class DayForecast
    {
        public List<Forecast> forecasts = new List<Forecast>();
        public DateOnly Date { get; private set; }
        private int Branch_id;
        public DayForecast(DateOnly date, int branch_id)
        {
            Date = date;
            Branch_id = branch_id;
        }
        


        public void AddNewForecast(Department Department,int amount) 
        {
            Forecast forecast = new Forecast();
            forecast.Date = Date;
            forecast.BranchId = Branch_id;
            forecast.Department = Department.ToString();
            forecast.ManHours = amount;
            forecasts.Add(forecast);
        }
    }
}
