using client.Models;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Text.Json;

namespace client
{
    public partial class Form1 : Form
    {
        private NamedPipeClientStream client;
        private StreamReader reader;
        private CancellationTokenSource cts;
        public Form1()
        {
            InitializeComponent();
            client = new NamedPipeClientStream("WeatherPipe");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!client.IsConnected)
            {
                reader = new StreamReader(client);
                client.Connect();
                cts = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(2000);
                        if (cts.IsCancellationRequested)
                            return;
                        var json = reader.ReadLine();
                        Weather weather = JsonSerializer.Deserialize<Weather>(json);
                        string[] weatherStringArray = [
                            weather.Id.ToString(),
                            weather.Temperature.ToString(),
                            weather.Preassure.ToString(),
                            weather.WindSpeed.ToString(),
                            weather.Description];
                        dataGridView1.Invoke(() =>
                        {
                            dataGridView1.Rows.Add(weatherStringArray);
                        });

                    }
                }, cts.Token);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cts is not null)
                cts.Cancel();

            //Server.StopServer();
            if (client.IsConnected)
            {
                client.Close();
                client = new NamedPipeClientStream("WeatherPipe");
                reader.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2_Click(this, null);
            Application.Exit();
        }
    }
}