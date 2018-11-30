using System;
using PlanningPoker.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views.WelcomeScreen {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly LoginViewModel _loginViewModel;

        public Login()
        {
            InitializeComponent();
            this.BindingContext = this._loginViewModel = new LoginViewModel();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            this._loginViewModel.ExecuteLoginCommand();
        }
    }
}
