namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Items : ContentPage
    {
        private readonly ItemsViewModel ViewModel;

        public Items()
        {
            InitializeComponent();

            this.ViewModel = new ItemsViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new NewItem());
        }

        protected override void OnAppearing()
        {
            this.ViewModel.LoadCommand.Execute(null);
        }
    }
}
