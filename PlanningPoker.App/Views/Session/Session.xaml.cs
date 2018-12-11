namespace PlanningPoker.App.Views.Session
{
    using System;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        public Session()
        {
            this.InitializeComponent();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e)
        {
            Votes.GetEstimate();
            this.Navigation.PushAsync(new ItemEstimation());
        }
    }
}
