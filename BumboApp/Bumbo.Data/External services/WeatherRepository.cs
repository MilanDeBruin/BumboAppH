using System.Text.Json.Nodes;
using BumboApp.Models.Models;

namespace BumboApp.Models.Repositorys;

public class WeatherRepository
{
    private string _baseUri;

    public WeatherRepository()
    {
        this._baseUri = "https://api.open-meteo.com/v1/";
    }

    public List<WeatherDayModel> GetWeather(double lat, double lon, DateTime startDate)
    {
        string jsonString;

        try
        {
            jsonString = Task.Run(() => this.GetAsyncWeather(lat, lon, startDate, startDate.AddDays(6))).Result;
        }
        catch (Exception e)
        {
            throw e;
        }

        if (jsonString.Contains("error"))
        {
            throw new Exception(jsonString);
        }
        
        return this.ParseJson(jsonString);
    }

    private async Task<string> GetAsyncWeather(double lat, double lon, DateTime startDate, DateTime endDate)
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
        Console.WriteLine(jsonString);
        JsonNode json = JsonNode.Parse(jsonString)["daily"];
        int lenght = JsonNode.Parse(json["time"].ToString()).AsArray().Count;

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