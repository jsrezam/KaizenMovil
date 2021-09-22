using Kaizen.Models;
using Kaizen.Services;
using System;
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

                if (!ValidateEcuadorianId(this.enTIdentificationCard.Text)) 
                {
                    await DisplayAlert("Error", "Identification card not valid", "Alright");
                    return;
                }

                if (this.enTCellPhone.Text.Length != 10) 
                {
                    await DisplayAlert("Error", "Cell phone number not valid", "Alright");
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
            if (string.IsNullOrEmpty(this.enTFirstName.Text) || string.IsNullOrEmpty(this.enTLastName.Text)) return false;
            if (string.IsNullOrEmpty(this.enTIdentificationCard.Text) || string.IsNullOrEmpty(this.enTCellPhone.Text)) return false;
            //if (string.IsNullOrEmpty(this.enTIdentificationCard.Text) || string.IsNullOrEmpty(this.enTCellPhone.Text)) return false;
            if (string.IsNullOrEmpty(this.enTEmail.Text) ) return false;
            if (string.IsNullOrEmpty(this.enTPassword.Text) || string.IsNullOrEmpty(this.enTConfirmPassword.Text)) return false;
            return true;
        }

        private bool ValidateEcuadorianId(string idCard)
        {
            int isNumeric;
            var total = 0;
            const int lengthIdCard = 10;
            int[] coefficients = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            const int regionNumber = 24;
            const int thirdDigit = 6;

            if (int.TryParse(idCard, out isNumeric) && idCard.Length == lengthIdCard)
            {
                var region = Convert.ToInt32(string.Concat(idCard[0], idCard[1], string.Empty));
                var Digitthree = Convert.ToInt32(idCard[2] + string.Empty);

                if ((region > 0 && region <= regionNumber) && Digitthree < thirdDigit)
                {
                    var checkDigitRecived = Convert.ToInt32(idCard[9] + string.Empty);

                    for (int i = 0; i < coefficients.Length; i++)
                    {
                        var value = Convert.ToInt32(coefficients[i] + string.Empty) *
                            Convert.ToInt32(idCard[i] + string.Empty);
                        total = value >= 10 ? total + (value - 9) : total + value;
                    }

                    var checkDigitObtained = total >= 10 ? (total % 10) != 0 ? 10 - (total % 10) : (total % 10) : total;

                    return checkDigitObtained == checkDigitRecived;
                }
            }
            return false;
        }
    }
}