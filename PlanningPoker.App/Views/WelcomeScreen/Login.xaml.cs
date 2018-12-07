namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly LoginViewModel viewModel;

        public Login()
        {
            this.viewModel = new LoginViewModel();
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<LoginViewModel>();

            this.InitializeComponent();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            this.viewModel.ExecuteLoginCommand();
            this.Navigation.PushAsync(new SessionCreation.CreateSession());
        }
    }
}
