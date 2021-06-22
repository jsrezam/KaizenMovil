using Kaizen.Models;
using Kaizen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kaizen.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistryPage : ContentPage
    {
        private readonly ApiService apiService;
        public RegistryPage()
        {
            InitializeComponent();
            this.apiService = new ApiService();
        }

        private async void SignUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidForm()) {
                    await DisplayAlert("Error", "Something missing in the form, please double check all entries", "Alright");
                    return;
                }

                if (!this.enTPassword.Text.Equals(this.enTConfirmPassword.Text))
                {
                    await DisplayAlert("Error", "The password must be match", "Alright");
                    return;
                }                
                
                await apiService.SignUp(new UserCredentials
                {
                    FirstName = this.enTFirstName.Text,
                    LastName = this.enTLastName.Text,
                    IdentificationCard = this.enTIdentificationCard.Text,
                    Email = this.enTEmail.Text,
                    PhoneNumber = this.enTCellPhone.Text,
                    UserName = this.enTEmail.Text,
                    Password = this.enTPassword.Text
                });
                
                await Navigation.PushAsync(new LoginPage());                               
                    
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Alright");
            }
        }

        private bool IsValidForm() 
        {
            if (string.IsNullOrEmpty(this.enTFirstName.Text) || string.IsNullOrEmpty(this.enTFirstName.Text)) return false;
            if (string.IsNullOrEmpty(this.enTIdentificationCard.Text) || string.IsNullOrEmpty(this.enTCellPhone.Text)) return false;
            if (string.IsNullOrEmpty(this.enTIdentificationCard.Text) || string.IsNullOrEmpty(this.enTCellPhone.Text)) return false;
            if (string.IsNullOrEmpty(this.enTEmail.Text) ) return false;
            if (string.IsNullOrEmpty(this.enTPassword.Text) || string.IsNullOrEmpty(this.enTConfirmPassword.Text)) return false;
            return true;
        }
    }
}