using Kaizen.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService apiService;
        public LoginPage()
        {
            InitializeComponent();
            this.apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            try
            {
                var token = await SecureStorage.GetAsync("token");
                var userName = await SecureStorage.GetAsync("userName");
                if (!string.IsNullOrEmpty(token))
                    Application.Current.MainPage = new MasterPage(userName);
            }
            catch
            {
                await DisplayAlert("Error", "Something goes wrong", "Alright");
            }
        }

        [Obsolete]
        private async void LogIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.activityIndicator.IsRunning = true;
                if (string.IsNullOrEmpty(EnTEmail.Text) || string.IsNullOrEmpty(EnTPassword.Text)) 
                {
                    await DisplayAlert("Info", "Make sure you have entered all the fields", "Alright");
                    return;
                } 
                await apiService.Login(EnTEmail.Text, EnTPassword.Text);
                Application.Current.MainPage = new MasterPage(EnTEmail.Text);
                this.activityIndicator.IsRunning = false;
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", ex.Message, "Alright");
                this.activityIndicator.IsRunning = false;
            }
            finally 
            {
                this.activityIndicator.IsRunning = false;
            }           
            
        }

        private void TapSigUp_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistryPage());
        }
    }
}