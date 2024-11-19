using System.Text.Json.Nodes;
using BumboApp.Models.Models;

namespace Bumbo.Data.External;

public class WeatherRepository
{
    private string _baseUri;

    public WeatherRepository()
    {
        this._baseUri = "https://api.open-meteo.com/v1/";
    }

    public List<WeatherDayModel>? GetWeather(int branchId, DateOnly startDate)
    {
        double lat = 51.688;
        double lon = 5.287;
        
        string jsonString;
        List<WeatherDayModel> weatherDayModels;

        try
        {
            jsonString = Task.Run(() => this.GetAsyncWeather(lat, lon, startDate, startDate.AddDays(6))).Result;
            weatherDayModels = ParseJson(jsonString);
        }
        catch (Exception e)
        {
            return null;
        }

        if (jsonString.Contains("error") || jsonString.Contains("null"))
        {
            return null;
        }
        
        
        return weatherDayModels;
    }

    private async Task<string> GetAsyncWeather(double lat, double lon, DateOnly startDate, DateOnly endDate)
    {
        string startDateString = startDate.ToString("yyyy-MM-dd");
        string endDateString = endDate.ToString("yyyy-MM-dd");
        string uri = _baseUri + "forecast?latitude=" + lat + "&longitude=" + lon +
                                "&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,precipitation_probability_max,wind_speed_10m_max&start_date=" + startDateString + "&end_date=" + endDateString;

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

    private List<WeatherDayModel> ParseJson(string jsonString)
    {
        List<WeatherDayModel> dayForecasts = new List<WeatherDayModel>();
        JsonNode json;
        int lenght;
        try
        {
            json = JsonNode.Parse(jsonString)["daily"];
            lenght = JsonNode.Parse(json["time"].ToString()).AsArray().Count;
        }
        catch (Exception e)
        {
            throw e;
        }

        for (int i = 0; i < lenght; i++)
        {
            dayForecasts.Add(new WeatherDayModel(
                json["time"][i].ToString(),
                Convert.ToDouble(json["temperature_2m_min"][i].ToString()),
                Convert.ToDouble(json["temperature_2m_max"][i].ToString()), 
                Convert.ToDouble(json["precipitation_sum"][i].ToString()), 
                Convert.ToInt32(json["precipitation_probability_max"][i].ToString()), 
                Convert.ToDouble(json["wind_speed_10m_max"][i].ToString())));
        }

        return dayForecasts;
    }
}