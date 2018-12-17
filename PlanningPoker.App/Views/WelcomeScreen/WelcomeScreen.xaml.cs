namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
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
            this.InitializeComponent();

            this.BindingContext = this.welcomeViewModel =
                (Application.Current as App)?.Container.GetRequiredService<WelcomeViewModel>();
        }

        private async Task HandleClickedAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("try to connection?");
            this.welcomeViewModel.JoinCommand.Execute(e);
            if (this.welcomeViewModel.Connection == true)
            {
                await this.Navigation.PushModalAsync(new Lobby(this.welcomeViewModel.Key));
            }
        }

        private async void LoginCommand(object sender, EventArgs e)
        {
            var result = await this.welcomeViewModel.ExecuteLoginCommand();
            if (result)
            {
                await this.Navigation.PushModalAsync(new SessionCreation.CreateSession());
            }
        }
    }
}
