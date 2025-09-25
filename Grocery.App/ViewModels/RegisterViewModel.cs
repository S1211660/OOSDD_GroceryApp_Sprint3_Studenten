using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace Grocery.App.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IClientService _clientService;

        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string confirmPassword = "";

        [ObservableProperty]
        private string registrationMessage = "";

        public RegisterViewModel(IClientService clientService)
        {
            _clientService = clientService;
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                RegistrationMessage = "";

                if (!ValidateInput())
                    return;

                var client = _clientService.Register(Name, Email, Password);

                RegistrationMessage = "Registratie succesvol!";

                await Task.Delay(1000);
                await Shell.Current.GoToAsync("//Login");
            }
            catch (InvalidOperationException ex)
            {
                RegistrationMessage = ex.Message;
            }
            catch (Exception)
            {
                RegistrationMessage = "Registratie mislukt. Probeer opnieuw.";
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("//Login");
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                RegistrationMessage = "Naam is verplicht.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
            {
                RegistrationMessage = "Geldig email adres is verplicht.";
                return false;
            }

            if (!IsValidPassword(Password))
            {
                RegistrationMessage = "Wachtwoord moet minimaal 8 karakters lang zijn en speciale tekens bevatten.";
                return false;
            }

            if (Password != ConfirmPassword)
            {
                RegistrationMessage = "Wachtwoorden komen niet overeen.";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            return password.Any(c => !char.IsLetterOrDigit(c));
        }
    }
}