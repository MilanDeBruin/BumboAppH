using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using BumboApp.Models.Models;

namespace BumboApp.Models.Repositorys;

public class HolidayRepository
{
    private string _baseUri;
    public HolidayRepository()
    {
        this._baseUri = "https://openholidaysapi.org/PublicHolidays";
    }

    public List<HolidayModel> GetHolidays(string country, DateTime startDate)
    {
        string jsonString;

        try
        {
            jsonString = Task.Run(() => this.GetAsyncHolidays(country, startDate, startDate.AddDays(9))).Result;
        }
        catch (Exception e)
        {
            throw e;
        }

        return this.ParseHolidayJson(jsonString);
    }

    private async Task<string> GetAsyncHolidays(string country, DateTime startDate, DateTime endDate)
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