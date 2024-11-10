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

    public List<WeatherDayModel> GetWeather(double lat, double lon, DateTime startDate, DateTime endDate)
    {
        string jsonString;

        try
        {
            jsonString = Task.Run(() => this.GetAsyncWeather(lat, lon, startDate, endDate)).Result;
        }
        catch (Exception e)
        {
            throw e;
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
    
    // public static List<WeatherDayModel> getWeather()
    // {
    //     JsonNode jsonResponse = Task.Run(() => MakeApiCall()).Result;
    //     
    //     JsonNode weather = jsonResponse["daily"];
    //     List<WeatherDayModel> week = new List<WeatherDayModel>();
    //     for (int i = 0; i < 7; i++)
    //     {
    //         week.Add(new WeatherDayModel(
    //             Convert.ToString(weather["time"][i]), 
    //             Convert.ToDouble(weather["temperature_2m_min"][i].ToString()), 
    //             
    //     }
    //     return week;
    // }
    //
    // private static async Task<JsonNode> MakeApiCall()
    // {
    //     string response = await sharedCLient.GetStringAsync("forecast?latitude=51.765&longitude=5.5181&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,precipitation_probability_max,wind_speed_10m_max&start_date=2024-10-07&end_date=2024-10-13");
    //     return JsonObject.Parse(response);
    // }
}