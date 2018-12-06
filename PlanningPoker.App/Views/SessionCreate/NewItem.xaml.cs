namespace PlanningPoker.App.Views.CreateSession
{
    using System;
    using Xamarin.Forms;

    public partial class NewItem : ContentPage
    {
        public NewItem()
        {
            InitializeComponent();
        }

        private void SaveItemClicked(object sender, EventArgs e) {
            this.DisplayAlert("Save?", "Do you want to save the item?", "Accept","Cancel");
        }
    }
}
