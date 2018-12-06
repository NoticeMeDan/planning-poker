namespace PlanningPoker.App.Views.Session
{
    using System;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        public Session()
        {
            InitializeComponent();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ItemEstimation());
        }
    }
}
