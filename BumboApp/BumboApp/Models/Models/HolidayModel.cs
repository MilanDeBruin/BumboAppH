using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BumboApp.Models.Models;

public class HolidayModel
{
    public string? Id { get; set; }

    [JsonPropertyName("startDate")]
    public string DateString { private get; set; }

    public DateTime Date
    {
        get { return DateTime.ParseExact(this.DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture); }
    }
}