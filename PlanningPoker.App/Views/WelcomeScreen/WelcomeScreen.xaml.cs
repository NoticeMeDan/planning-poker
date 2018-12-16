namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeScreen : ContentPage
    {
        private readonly WelcomeViewModel welcomeViewModel;

        public WelcomeScreen()
        {
            this.BindingContext = this.welcomeViewModel =
                (Application.Current as App)?.Container.GetRequiredService<WelcomeViewModel>();
            this.InitializeComponent();
        }

        public void JoinCommand(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Lobby());
        }

        public async void LoginCommand(object sender, EventArgs e)
        {
            var result = await this.welcomeViewModel.ExecuteLoginCommand();
            if (result)
            {
                await this.Navigation.PushModalAsync(new SessionCreation.CreateSession());
            }
            else
            {
                await this.Navigation.PushModalAsync(new WelcomeScreen());
            }
        }
    }
}
