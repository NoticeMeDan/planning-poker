namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Models;
    using OpenJobScheduler;
    using Shared;
    using Xamarin.Forms;

    public class SessionViewModel : BaseViewModel
    {
        private readonly ISessionClient client;
        private readonly string sessionKey;
        private string currentItemTitle;
        private JobScheduler jobScheduler;
        private RoundDTO currentRound;

        public SessionViewModel(ISessionClient client)
        {
            this.client = client;

            // TODO: Get sessionkey from constructor argument
            this.sessionKey = "B2NH14M";
            this.BaseTitle = "Session: " + this.sessionKey;
            this.CurrentItemTitle = string.Empty;
            this.currentRound = null;

            // Lists
            this.Players = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();
            this.VoteCards = new ObservableCollection<Card>();

            // Setup
            this.LoadSessionCommand = new RelayCommand(async _ => await this.ExecuteLoadSessionCommand());
            this.LoadVotesCommand = new RelayCommand(async _ => await this.ExecuteLoadVotesCommand());
            this.RevoteCommand = new RelayCommand(async _ => await this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(async _ => await this.ExecuteNextItemCommand());
            this.SendVoteCommand = new RelayCommand(async number => await this.ExecuteSendVoteCommand(number));
            this.SendNitpickerCommand = new RelayCommand(_ => this.ExecuteNitpickerCommand());

            // Threads
            this.StopThreadsCommand = new RelayCommand(_ => this.ExecuteStopThreadsCommand());
            this.StartVotesPull = new RelayCommand(_ => this.ExecuteStartVotesPull());
            this.StartRoundsPull = new RelayCommand(_ => this.ExecuteStartRoundsPull());
        }

        public ObservableCollection<UserDTO> Players { get; set; }

        public ObservableCollection<Card> VoteCards { get; set; }

        public ObservableCollection<VoteDTO> Votes { get; set; }

        public ICommand RevoteCommand { get; }

        public ICommand NextItemCommand { get; }

        public ICommand LoadSessionCommand { get; }

        public ICommand LoadVotesCommand { get; }

        public ICommand SendVoteCommand { get; }

        public ICommand SendNitpickerCommand { get; }

        // Threads
        public ICommand StartVotesPull { get; }

        public ICommand StartRoundsPull { get; }

        public ICommand StopThreadsCommand { get; }

        public string CurrentItemTitle
        {
            get => this.currentItemTitle;
            set => this.SetProperty(ref this.currentItemTitle, value);
        }

        private void ExecuteStartRoundsPull()
        {
            this.jobScheduler =
                new JobScheduler(TimeSpan.FromSeconds(5), new Action(async () => await this.FetchRounds()));
            this.jobScheduler.Start();
        }

        private async Task FetchRounds()
        {
            var round = this.client.GetCurrentRound(this.sessionKey).Result;

            if (round.Id != this.currentRound.Id)
            {
                this.currentRound = round;
                this.ExecuteStopThreadsCommand();
                await this.SetCurrentTitle();
                this.ExecuteStartVotesPull();
            }
        }

        private void ExecuteStartVotesPull()
        {
            this.jobScheduler = new JobScheduler(
                TimeSpan.FromSeconds(5),
                new Action(async () => await this.ShouldShowVotes()));

            this.jobScheduler.Start();
        }

        /*
         * Following is the ExecuteCommands
         */
        private async Task ExecuteNextItemCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            this.VoteCards.Clear();
            Debug.WriteLine("Next Item clicked");
            await this.client.NextItemAsync(this.sessionKey);
            await this.SetCurrentTitle();
            Debug.WriteLine(this.currentItemTitle);

            this.IsBusy = false;
            await this.ExecuteLoadSessionCommand();
        }

        private async Task ExecuteRevoteCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            this.VoteCards.Clear();

            Debug.WriteLine("Revote clicked");

            await this.client.NextRoundAsync(this.sessionKey);

            // Resets votes because there are none in a new round
            this.LoadVotesCommand.Execute(null);

            this.IsBusy = false;
        }

        private async Task ExecuteSendVoteCommand(object number)
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            Debug.WriteLine("SendVote clicked");

            // cast to int.
            var voteEstimate = int.Parse(number.ToString());
            Debug.WriteLine(voteEstimate);

            // Call repository.Vote with new VoteDTO
            await this.client?.Vote(this.sessionKey, new VoteDTO { Estimate = voteEstimate });

            this.IsBusy = false;
        }

        private async Task ExecuteLoadSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            Debug.WriteLine("LoadSession clicked");

            var session = await this.client.GetByKeyAsync(this.sessionKey);
            this.Players.Clear();
            var players = session.Users;

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    foreach (var player in players)
                    {
                        this.Players.Add(player);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            this.IsBusy = false;
        }

        private async Task ExecuteLoadVotesCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.ExecuteStopThreadsCommand();

            this.IsBusy = true;
            this.VoteCards.Clear();

            Debug.WriteLine("Load votes clicked");

            this.currentRound = await this.client.GetCurrentRound(this.sessionKey);
            var votes = this.currentRound.Votes;

            ObservableCollection<Card> voteCards = this.VotesToCards(votes);

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    foreach (var voteCard in voteCards)
                    {
                        this.VoteCards.Add(voteCard);
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            this.IsBusy = false;

            this.ExecuteStartRoundsPull();
        }

        private void ExecuteNitpickerCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            Debug.WriteLine("Nitpicker clicked");

            this.IsBusy = false;
        }

        private void ExecuteStopThreadsCommand()
        {
            this.jobScheduler.Stop();
        }

        /*
         * Following are the methods called by the ExecuteCommands
         */

        private async Task SetCurrentTitle()
        {
            Debug.WriteLine("Updating Item Title");

            var title = await this.client.GetCurrentItem(this.sessionKey);

            this.CurrentItemTitle = title.Title;
        }

        private async Task ShouldShowVotes()
        {
            this.ExecuteStopThreadsCommand();
            Debug.WriteLine("Pulling");
            var currentVotes = await this.client.GetCurrentRound(this.sessionKey);
            var result = currentVotes.Votes.Count == this.Players.Count;

            if (result)
            {
                await this.ExecuteLoadVotesCommand();
            }
            else
            {
                this.ExecuteStartVotesPull();
            }
        }

        private ObservableCollection<Card> VotesToCards(ICollection<VoteDTO> votes)
        {
            var cards = new ObservableCollection<Card>();
            foreach (var vote in votes)
            {
                foreach (var user in this.Players)
                {
                    if (vote.UserId == user.Id)
                    {
                        var card = new Card { Name = user.Nickname, Estimate = vote.Estimate };
                        cards.Add(card);
                    }
                }
            }

            return cards;
        }

        public class Card
        {
            public string Name { get; set; }

            public int Estimate { get; set; }
        }
    }
}
