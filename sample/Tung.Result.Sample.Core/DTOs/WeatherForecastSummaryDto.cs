using Tung.Result.Sample.Core.Model;
using System;

namespace Tung.Result.Sample.Core.DTOs
{
    public class WeatherForecastSummaryDto
    {
        public DateTime Date { get; set; }
        public string Summary { get; set; }

        public WeatherForecastSummaryDto(DateTime date, string summary)
        {
            Date = date;
            Summary = summary;
        }

        public static WeatherForecastSummaryDto MapFrom(WeatherForecast weatherForecast) =>
            new WeatherForecastSummaryDto(weatherForecast.Date, weatherForecast.Summary);
    }
}
