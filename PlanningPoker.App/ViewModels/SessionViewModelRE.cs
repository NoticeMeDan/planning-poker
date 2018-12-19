namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;
    using Xamarin.Forms;

    class SessionViewModelRE : BaseViewModel
    {
        private readonly ISessionClient client;

        public ObservableCollection<UserDTO> Players { get; set; }

        public ObservableCollection<Card> Cards { get; set; }

        public ObservableCollection<VoteDTO> Votes { get; set; }

        public SessionDTO Session { get; set; }

        public ICommand sendVote { get; }

        public ICommand fetchVote { get; }

        public ICommand fetchPlayers { get; }

        public ICommand newRound { get; }

        private string itemTitle;
        private string sessionKey;
        private int estimate;
        private bool loading;

        private ItemDTO CurrentItem { get; set; }

        private RoundDTO CurrentRound { get; set; }

        public SessionViewModelRE(ISessionClient client)
        {
            this.client = client;
            this.Players = new ObservableCollection<UserDTO>();
            this.Cards = new ObservableCollection<Card>();
            this.Votes = new ObservableCollection<VoteDTO>();

            this.sendVote = new RelayCommand(async _ => await this.Vote(this.estimate));
            this.fetchVote = new RelayCommand(async _ => await this.FetchVotes());
            this.fetchPlayers = new RelayCommand(async _ => await this.FetchPlayers());
            this.newRound = new RelayCommand(async _ => await this.StartNewRound());
        }

        private async Task StartNewRound()
        {
            await this.client.NextRoundAsync(this.sessionKey);
            this.Votes.Clear();
            await this.UpdateCurrentRound();
        }

        private async Task UpdateSession()
        {
            this.Session = await this.client.GetByKeyAsync(this.sessionKey);
        }

        private async Task UpdateCurrentItem()
        {
            this.CurrentItem = await this.client.GetCurrentItem(this.sessionKey);
        }

        private async Task UpdateCurrentRound()
        {
            this.CurrentRound = await this.client.GetCurrentRound(this.sessionKey);
        }

        private async Task Vote(int estimate)
        {
        }

        public async Task FetchVotes()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            await this.UpdateCurrentRound();

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    this.UpdateVoteCollection(this.CurrentRound.Votes);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            this.loading = false;
        }

        public async Task FetchPlayers()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            await this.UpdateSession();

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    this.UpdatePlayersCollection(this.Session.Users);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            this.loading = false;
        }

        private void UpdatePlayersCollection(ICollection<UserDTO> users)
        {
            this.Players.Clear();
            users.ToList().ForEach(p =>
            {
                this.Players.Add(p);
            });
        }

        private void UpdateVoteCollection(ICollection<VoteDTO> votes)
        {
            this.Votes.Clear();
            votes.ToList().ForEach(v =>
            {
                this.Votes.Add(v);
            });
        }

        public string ItemTitle
        {
            get => this.itemTitle;
            set => this.SetProperty(ref this.itemTitle, value);
        }

        public string SesssionKey
        {
            get => this.sessionKey;
            set => this.SetProperty(ref this.sessionKey, value);
        }

        public int Estimate
        {
            get => this.estimate;
            set => this.SetProperty(ref this.estimate, value);
        }

        internal class Card
        {
            public string Name { get; set; }

            public int Estimate { get; set; }
        }
    }
}
