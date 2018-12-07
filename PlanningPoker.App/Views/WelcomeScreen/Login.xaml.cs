namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly LoginViewModel loginViewModel;

        public Login()
        {
            this.InitializeComponent();
            this.BindingContext = this.loginViewModel = ((App)Application.Current).Container.GetRequiredService<LoginViewModel>();
        }

        public async void LoginCommand(object sender, EventArgs e)
        {
            var result = await this.loginViewModel.ExecuteLoginCommand();
            if (result)
            {
                await this.Navigation.PushModalAsync(new SessionCreation.CreateSession());
            }
        }
    }
}
