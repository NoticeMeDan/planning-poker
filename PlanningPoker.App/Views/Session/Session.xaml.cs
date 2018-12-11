namespace PlanningPoker.App.Views.Session
{
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using System;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel ViewModel;

        public Session()
        {
            this.ViewModel = new SessionViewModel();
            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();

            this.InitializeComponent();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e)
        {
            Votes.GetEstimate();
            this.Navigation.PushAsync(new ItemEstimation());
        }
    }
}
