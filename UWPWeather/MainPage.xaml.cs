using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWeather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

       

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                var position = await LocationManager.GetPosition();
                var lat = position.Coordinate.Latitude;
                var lon = position.Coordinate.Longitude;
                RootObject myWeather = await OpenWeatherMapProxy.GetWeather(34.67, -90.12);
                var uri = string.Format("http://uwpweatherservicer.azurewebsites.net/?lat={0}&lon={1}", 34.67, -90.56);
                //update tile
                var tileContent = new Uri(uri);
                var requestedInterval = PeriodicUpdateRecurrence.HalfHour;
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.StartPeriodicUpdate(tileContent, requestedInterval);


                //ResultTextBlock.Text = myWeather.name + "--" + ((int)myWeather.main.temp).ToString() + "--" + myWeather.weather[0].description;
                string icon = String.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
                ImageControl.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
                TempTextBlock.Text = ((int)myWeather.main.temp).ToString();
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;
            }
            catch
            {
                LocationTextBlock.Text = "unable to get weather information..";

            }
        }
    }
}
