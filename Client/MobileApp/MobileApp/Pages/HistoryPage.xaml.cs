using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        ObservableCollection<Microcharts.ChartEntry> temperatureEntries = new ObservableCollection<Microcharts.ChartEntry>();
        ObservableCollection<Microcharts.ChartEntry> humidityEntries = new ObservableCollection<Microcharts.ChartEntry>();
        ObservableCollection<Microcharts.ChartEntry> pulseEntries = new ObservableCollection<Microcharts.ChartEntry>();
        ObservableCollection<Microcharts.ChartEntry> spo2Entries = new ObservableCollection<Microcharts.ChartEntry>();
        private HttpClient httpClient;

        public HistoryPage()
        {
            InitializeComponent();

        }

        private async void Update_Charts_Button_Clicked(object sender, EventArgs e)
        {

            //Temperature
            httpClient = new HttpClient();
            Uri address = new Uri("http://192.168.1.102:5000/SensorData/GetTemperature/10");
            HttpResponseMessage response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<TemperatureLog>>(content);
                foreach(var log in Items)
                {
                    temperatureEntries.Add(new Microcharts.ChartEntry(30) 
                    { 
                        Color = SkiaSharp.SKColor.Parse("#F55B47"), 
                        Label = $"{log.TimeStamp}", 
                        ValueLabel = $"{log.Temperature}°C"
                    });
                }

                TemperatureChart.Chart = new Microcharts.LineChart() { Entries = temperatureEntries };
            }

            //Humidity
            httpClient = new HttpClient();
            address = new Uri("http://192.168.1.102:5000/SensorData/GetHumidity/10");
            response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<HumidityLog>>(content);
                foreach (var log in Items)
                {
                    humidityEntries.Add(new Microcharts.ChartEntry(30)
                    {
                        Color = SkiaSharp.SKColor.Parse("#78AAF5"),
                        Label = $"{log.TimeStamp}",
                        ValueLabel = $"{log.Humidity}%"
                    });
                }

                HumidityChart.Chart = new Microcharts.LineChart() { Entries = humidityEntries };
            }

            //HeartRate:
            httpClient = new HttpClient();
            address = new Uri("http://192.168.1.102:5000/SensorData/GetHeartRate/10");
            response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<HeartRateLog>>(content);
                foreach (var log in Items)
                {
                    pulseEntries.Add(new Microcharts.ChartEntry(30)
                    {
                        Color = SkiaSharp.SKColor.Parse("#A85348"),
                        Label = $"{log.TimeStamp}",
                        ValueLabel = $"{log.HeartRate}BPM"
                    });
                }

                PulseChart.Chart = new Microcharts.LineChart() { Entries = pulseEntries };
            }

            //o2 saturation:
            httpClient = new HttpClient();
            address = new Uri("http://192.168.1.102:5000/SensorData/GetOxygenLevel/10");
            response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<O2SaturationLog>>(content);
                foreach (var log in Items)
                {
                    spo2Entries.Add(new Microcharts.ChartEntry(30)
                    {
                        Color = SkiaSharp.SKColor.Parse("#A85348"),
                        Label = $"{log.TimeStamp}",
                        ValueLabel = $"{log.O2Saturation}%"
                    });
                }

                Spo2Chart.Chart = new Microcharts.LineChart() { Entries = spo2Entries };
            }
        }
    }
}