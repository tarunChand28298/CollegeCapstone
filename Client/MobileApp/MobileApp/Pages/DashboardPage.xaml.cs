using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        HttpClient httpClient;
        private float _TemperatureToSet;

        public float TemperatureToSet
        {
            get { return _TemperatureToSet; }
            set 
            { 

                _TemperatureToSet = 25.0f + (value * 15.0f); 
            }
        }


        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void Prediction_Update_Clicked(object sender, EventArgs e)
        {
            httpClient = new HttpClient();
            Uri address = new Uri("http://192.168.1.102:5000/SensorData/GetPrediction");
            HttpResponseMessage response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                PredictionLable.Text = content;
                if (content == "normal") RecommendationLable.Text = "Nothing to recommend";
                else RecommendationLable.Text = "Please see the doctor immideately";
            }
        }

        private async void Update_Button_Clicked(object sender, EventArgs e)
        {
            httpClient = new HttpClient();
            Uri address = new Uri("http://192.168.1.102:5000/SensorData/GetTemperature/1");
            HttpResponseMessage response = await httpClient.GetAsync(address);
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<TemperatureLog>>(content);
                if(Items.Count > 0)
                {
                    TemperatureLable.Text = $" {Items[0].Temperature}°C";
                }
            }

            httpClient = new HttpClient();
            address = new Uri("http://192.168.1.102:5000/SensorData/GetHumidity/1");
            response = await httpClient.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<HumidityLog>>(content);
                if(Items.Count > 0)
                {
                    HumidityLable.Text = $" {Items[0].Humidity}%";
                }
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            TemperatureToSet = (float)e.NewValue;
            SliderBindedValue.Text = TemperatureToSet.ToString("00.00") + "°C";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            httpClient = new HttpClient();
            Uri address = new Uri($"http://192.168.1.102:5000/SensorData/SetSetpoint/{TemperatureToSet}");
            HttpResponseMessage response = await httpClient.GetAsync(address);
        }
    }
}