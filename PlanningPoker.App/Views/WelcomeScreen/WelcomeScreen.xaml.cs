namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
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
            this.welcomeViewModel.Join.Execute(e);
            await this.Navigation.PushModalAsync(new NavigationPage(new Lobby(this.welcomeViewModel.Key)));
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
