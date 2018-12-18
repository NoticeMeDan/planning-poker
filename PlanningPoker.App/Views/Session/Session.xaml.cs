namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel viewModel;

        public Session()
        {
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.viewModel.NextItemCommand.Execute(null);
        }

        private void OnNitpicker_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnReVote_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnNextItem_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnVoteCard_Clicked(object sender, EventArgs e)
        {
            this.viewModel.SendVoteCommand.Execute(null);
        }

        private void OnVote_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
