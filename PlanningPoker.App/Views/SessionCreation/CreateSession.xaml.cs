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
        private readonly ItemsViewModel itemsViewModel;

        public CreateSession()
        {
            this.BindingContext = this.itemsViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.itemsViewModel.LoadCommand.Execute(null);
        }

        private void CreateSessionClicked(object sender, EventArgs e)
        {
            //TODO: Hook up with API when ready

            this.itemsViewModel.CreateSessionCommand.Execute(null);
            this.Navigation.PushModalAsync(new NavigationPage(
                new Lobby(this.itemsViewModel.Key)));
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.itemsViewModel.AddItemCommand.Execute(null);
        }
    }
}
