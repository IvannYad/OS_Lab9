using Microsoft.EntityFrameworkCore;
using server;
using server.Data;
using server.Models;
using server.StaticData;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace server {
    public class Server {
        private static ApplicationDbContext _context = new ApplicationDbContext();
        private static WeatherForecastReader _weatherForecastReader;
        private static WeatherForecastWriter _weatherForecastWriter;
        public static async Task StartServer() {
            var t = Task.Factory.StartNew(async () => {
                _weatherForecastWriter = new WeatherForecastWriter();
                _weatherForecastReader = new WeatherForecastReader();
                while (true) {
                    await _weatherForecastWriter.Run();
                    _weatherForecastReader.Run();
                    await Task.Delay(1000);
                }

            });

            var res = await t;
            await res;
        }

        public static void StopServer() {
            _weatherForecastReader.PipeDisconnect();
        }
    }
}