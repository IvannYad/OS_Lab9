using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Data;
using server.Models;
using server.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static server.StaticData.SD;

namespace ServerSide {
    internal class WeatherForecastReader {
        private ApplicationDbContext _context;
        private NamedPipeServerStream _pipe;
        private StreamWriter _writer;
        public WeatherForecastReader() {
            _context = new ApplicationDbContext();
            _pipe = new NamedPipeServerStream("WeatherPipe");
            _writer = new StreamWriter(_pipe);

        }

        public void Run() {
            try {
                if (!_pipe.IsConnected)
                    _pipe.WaitForConnection();

                var weather = GetWeatherFromDB();
                var line = JsonSerializer.Serialize<Weather>(weather);
                _writer.WriteLine(line);
                _writer.Flush();

            } catch {
                _pipe.Disconnect();
                _pipe.Dispose();
                _pipe = new NamedPipeServerStream("WeatherPipe");
                _writer = new StreamWriter(_pipe);
            }

        }

        private Weather GetWeatherFromDB() {
            var weather = _context.Weathers.AsNoTracking().AsEnumerable().MaxBy(w => w.Id)!;
            return weather;
        }

        public void PipeDisconnect() {
            if (_pipe.IsConnected)
                _pipe.Disconnect();
        }
    }
}