namespace PlanningPoker.App.Views.SessionCreation
{
    using System;
    using Components;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly ItemsViewModel ViewModel;

        public CreateSession()
        {
            this.ViewModel = new ItemsViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();

            this.InitializeComponent();
        }

        private static int GenerateKey(int min, int max)
        {
            var random = new Random();

            var result = random.Next(min, max);

            return result;
        }

        private void CreateSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(
                new Lobby());
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Items());
        }
    }
}
