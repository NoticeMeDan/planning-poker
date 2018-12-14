namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Cards : ContentPage
    {
        private readonly CardsViewModel ViewModel;

        public Cards()
        {
            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
        }
    }
}
