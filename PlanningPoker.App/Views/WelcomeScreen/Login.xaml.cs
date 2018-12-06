namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public Login()
        {
            _loginViewModel = new LoginViewModel();
            InitializeComponent();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            this._loginViewModel.ExecuteLoginCommand();
            this.Navigation.PushAsync(new SessionCreation.CreateSession());
        }
    }
}
