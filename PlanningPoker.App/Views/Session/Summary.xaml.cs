namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Summary : ContentPage
    {
        private readonly SummaryViewModel viewModel;

        public Summary()
        {
            this.BindingContext = this.viewModel =
               (Application.Current as App)?.Container.GetRequiredService<SummaryViewModel>();

            this.InitializeComponent();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Summary());
        }
    }
}
