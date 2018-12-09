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
            this.ViewModel = new ItemsViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.ViewModel.LoadCommand.Execute(null);
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new NewItem());
        }


    }
}
