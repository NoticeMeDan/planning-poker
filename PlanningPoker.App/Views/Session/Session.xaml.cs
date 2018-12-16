using System.Diagnostics;

namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
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

        protected override void OnAppearing()
        {
            this.ViewModel.LoadPlayersCommand.Execute(null);

            //TODO: LoadItemsCommand should be executed when all players have played a card
            this.ViewModel.LoadVotesCommand.Execute(null);
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
            this.ViewModel.SendVoteCommand.Execute(null);

        }

        private void OnVote_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
