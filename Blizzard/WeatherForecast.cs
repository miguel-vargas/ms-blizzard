using System;

namespace MVargas.Blizzard
{
    /// <summary>
    /// A Weather Forecast Objects.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The date of the forecast.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The temperature in Celsius of the forecast.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// The temperature in Fahrenheit of the forecast.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// A breif summary of the weather.
        /// </summary>
        public string Summary { get; set; }
    }
}
