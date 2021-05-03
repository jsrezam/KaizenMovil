using Kaizen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (!string.IsNullOrEmpty(token))
                    Application.Current.MainPage = new MasterPage();
            }
            catch
            {
                await DisplayAlert("Error", "Something goes wrong", "Alright");
            }
        }

        [Obsolete]
        private async void LogIn_Clicked(object sender, EventArgs e)
        {
            var response = await apiService.Login(EnTEmail.Text, EnTPassword.Text);
            if (!response)
            {
                await DisplayAlert("Error", "Something goes wrong", "Alright");
                return;
            }
            Application.Current.MainPage = new MasterPage();
        }
    }
}