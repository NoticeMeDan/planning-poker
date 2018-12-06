namespace PlanningPoker.App.Views.Session
{
    using System;
    using Xamarin.Forms;

    public partial class ItemEstimation : ContentPage
    {
        public ItemEstimation()
        {
            InitializeComponent();
        }

        private void Vote_Clicked(object sender, EventArgs e)
        {
            this.DisplayAlert("Send Vote?", string.Empty, "Yes", "No");
        }
    }
}
