using System.Net;

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
        public string sessionKey;
        private string currentItemTitle;
        private JobScheduler jobSchedulerVotes;
        private JobScheduler jobSchedulerRounds;
        private RoundDTO currentRound;
        private bool finishedSession;

        public SessionViewModel(ISessionClient client)
        {
            this.client = client;

            // TODO: Get sessionkey from constructor argument
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
            this.jobSchedulerRounds = new JobScheduler(
                TimeSpan.FromSeconds(5),
                new Action(async () => await this.ShouldFetchRounds()));

            this.jobSchedulerVotes = new JobScheduler(
                TimeSpan.FromSeconds(5),
                new Action(async () => await this.ShouldShowVotes()));

            this.finishedSession = false;
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

        public string CurrentItemTitle
        {
            get => this.currentItemTitle;
            set => this.SetProperty(ref this.currentItemTitle, value);
        }

        private async Task ShouldFetchRounds()
        {
            while (this.jobSchedulerVotes != null) {
                try {
                    this.jobSchedulerVotes.Stop();
                    this.jobSchedulerVotes = null;
                }
                catch (Exception e) {
                    //No handling
                }
            }

            // New Round. Stop the thread and setup new Item estimation
            Debug.WriteLine("Pulling rounds");
            var round = this.client.GetCurrentRound(this.sessionKey).Result;

            // New Round?
            if (round.Id != this.currentRound.Id)
            {
                while (this.jobSchedulerRounds != null) {
                    try {
                        this.jobSchedulerRounds.Stop();
                        this.jobSchedulerRounds = null;
                    }
                    catch (Exception e) {
                        //No handling
                    }
                }
                this.jobSchedulerRounds = null;

                this.currentRound = round;
                await this.SetCurrentTitle();

                // Listen for incoming votes
                Debug.WriteLine("ShouldFetchRounds");
                this.StartJobSchedulerVotes();
            }
        }

        private async Task ShouldShowVotes()
        {
            Debug.WriteLine("Pulling Votes");
            var currentVotes = await this.client.GetCurrentRound(this.sessionKey);
            var result = currentVotes.Votes.Count == this.Players.Count;

            if (result)
            {
                while (this.jobSchedulerVotes != null) {
                    try {
                        this.jobSchedulerVotes.Stop();
                        this.jobSchedulerVotes = null;
                    }
                    catch (Exception e) {
                        //No handling
                    }
                }
                await this.ExecuteLoadVotesCommand();
            }
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
            var response = await this.client.NextItemAsync(this.sessionKey);
            if (response == null) {
                this.finishedSession = true;
            }
            await this.SetCurrentTitle();
            Debug.WriteLine(this.currentItemTitle);

            this.IsBusy = false;
            await this.ExecuteLoadSessionCommand();
        }

        private async Task ExecuteRevoteCommand()
        {
            while (this.jobSchedulerRounds != null) {
                try {
                    this.jobSchedulerRounds.Stop();
                    this.jobSchedulerRounds = null;
                }
                catch (Exception e) {
                    //No handling
                }
            }

            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            this.VoteCards.Clear();

            Debug.WriteLine("Revote clicked");

            await this.client.NextRoundAsync(this.sessionKey);

            this.IsBusy = false;

            Debug.WriteLine("ExecuteRevote Method");
            this.StartJobSchedulerVotes();
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
            Debug.WriteLine("ExecuteLoadSession");
            this.StartJobSchedulerVotes();
        }

        private async Task ExecuteLoadVotesCommand()
        {
            try
            {
                this.jobSchedulerVotes = null;
            }
            catch (Exception e)
            {
                e.ToString();
            }

            if (this.IsBusy)
            {
                return;
            }

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

            try
            {
                this.StartJobSchedulerRounds();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine("Too bad, try again");
                this.StartJobSchedulerRounds();
            }
        }

        private void StartJobSchedulerRounds()
        {
            this.jobSchedulerRounds = new JobScheduler(
                TimeSpan.FromSeconds(5),
                new Action(async () => await this.ShouldFetchRounds()));
            this.jobSchedulerRounds.Start();
        }

        private void StartJobSchedulerVotes()
        {
            this.jobSchedulerVotes = new JobScheduler(
                TimeSpan.FromSeconds(5),
                new Action(async () => await this.ShouldShowVotes()));
            this.jobSchedulerVotes.Start();
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

        /*
         * Following are the methods called by the ExecuteCommands
         */

        private async Task SetCurrentTitle()
        {
            Debug.WriteLine("Updating Item Title");

            var title = await this.client.GetCurrentItem(this.sessionKey);

            this.CurrentItemTitle = title.Title;
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

        public bool CheckSessionStatus()
        {
            //return await this.client.GetCurrentItem(this.sessionKey);
            return this.finishedSession;
        }

        public class Card
        {
            public string Name { get; set; }

            public int Estimate { get; set; }
        }

        public async Task<int> GetId()
        {
            var session = await this.client.GetByKeyAsync(this.sessionKey);

            return session.Id;
        }
    }
}
