using System.Globalization;

namespace BumboApp.Models.Models;

public class WeatherDayModel
{
    private DateTime _date;
    private double _minTemp;
    private double _maxTemp;
    private double _precipitation;
    private int _precipitationProbability;
    private double _windSpeed;

    public WeatherDayModel(string date, double minTemp, double maxTemp, double precipitation, int precipitationProbability, double windSpeed)
    {
        this._date = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        this._minTemp = minTemp;
        this._maxTemp = maxTemp;
        this._precipitation = precipitation;
        this._precipitationProbability = precipitationProbability;
        this._windSpeed = windSpeed;
    }
    
    public DateTime Date { get => _date; }
    
    public double MinTemp { get => _minTemp; }
    
    public double MaxTemp { get => _maxTemp; }
    
    public double Precipitation { get => _precipitation; }

    public int PrecipitationProbability { get => _precipitationProbability; }
    
    public double WindSpeed { get => _windSpeed; }
    
}

