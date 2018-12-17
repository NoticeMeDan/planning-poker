namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Summary : ContentPage
    {
        private readonly SummaryViewModel ViewModel;

        public Summary()
        {
            this.BindingContext = this.ViewModel =
               (Application.Current as App)?.Container.GetRequiredService<SummaryViewModel>();

            this.InitializeComponent();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e)
        {
            //ViewModel.Estimate(Votes.GetEstimate());

            //This should get the current vote
            this.Navigation.PushAsync(new Summary());
        }
    }
}
