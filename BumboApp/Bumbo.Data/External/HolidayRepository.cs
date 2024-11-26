using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using BumboApp.Models.Models;

namespace Bumbo.Data.External;

public class HolidayRepository
{
    private string _baseUri;
    public HolidayRepository()
    {
        this._baseUri = "https://openholidaysapi.org/PublicHolidays";
    }

    public HolidayModel? GetHoliday(int branchId, DateOnly startDate)
    {
        string country = "NL";
        string jsonString;
        HolidayModel holidayModel;

        try
        {
            jsonString = Task.Run(() => this.GetAsyncHolidays(country, startDate, startDate.AddDays(9))).Result;
            holidayModel = ParseHolidayJson(jsonString)[0];
        }
        catch (Exception e)
        {
            return null;
        }

        return holidayModel;
    }

    private async Task<string> GetAsyncHolidays(string country, DateOnly startDate, DateOnly endDate)
    {
        string startDateString = startDate.ToString("yyyy-MM-dd");
        string endDateString = endDate.ToString("yyyy-MM-dd");
        string uri = this._baseUri + "?countryIsoCode=" + country + "&languageIsoCode=" + country + "&validFrom=" + startDateString + "&validTo=" + endDateString;
        
        try
        {
            HttpResponseMessage response = await StaticHttpClient.Client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private List<HolidayModel> ParseHolidayJson(string jsonString)
    {
        List<HolidayModel> holidays = new List<HolidayModel>();
        
        JsonArray array = JsonNode.Parse(jsonString).AsArray();
        foreach (var node in array)
        {
            HolidayModel? model = JsonSerializer.Deserialize<HolidayModel>(node, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (model != null)
            {
                holidays.Add(model);
            }
        }
        
        return holidays;
    }
}