using Newtonsoft.Json;
using server.Data;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace server
{
    internal class WeatherForecastWriter
    {
        private readonly ApplicationDbContext _context;
        public WeatherForecastWriter()
        {
            _context = new ApplicationDbContext();
        }

        public async Task Run()
        {
            await WriteToDb();
        }
        private async Task WriteToDb()
        {
            var weather = await GetWeatherFromAPI();
            if (weather is null)
                throw new NullReferenceException(nameof(weather));

            _context.Weathers.Add(weather);
            _context.SaveChanges();
            return;
        }

        private async Task<Weather> GetWeatherFromAPI()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openweathermap.org/data/2.5/weather?q=Lviv&appid=f7e902c5bfac898fe9ff3bb88feaf88b");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(json)!;

            Weather weather = new Weather()
            {
                Description = jsonObject.weather[0].description,
                Temperature = jsonObject.main.temp - 273.15,
                Preassure = jsonObject.main.pressure,
                WindSpeed = jsonObject.wind.speed,
            };

            return weather;
        }
    }
}
