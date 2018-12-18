using System.Threading.Tasks;

namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Windows.Input;
    using Models;
    using OpenJobScheduler;
    using Shared;

    public class SessionViewModel : BaseViewModel
    {
        private readonly ISessionClient client;
        private SessionDTO session;
        private readonly string sessionKey;
        private string currentItemTitle;
        private string token;
        private JobScheduler jobScheduler;
        private RoundDTO currentRound;

        public ObservableCollection<UserDTO> Players { get; set; }

        public ObservableCollection<Card> VoteCards { get; set; }

        public ObservableCollection<VoteDTO> Votes { get; set; }

        public ICommand RevoteCommand { get; }

        public ICommand NextItemCommand { get;  }

        public ICommand LoadSessionCommand { get; }

        public ICommand LoadVotesCommand { get; }

        public ICommand SendVoteCommand { get; }

        public ICommand SendNitpickerCommand { get; }

        // Threads
        public ICommand StartVotesPull { get; }

        public ICommand StartRoundsPull { get; }

        public ICommand StopThreadsCommand { get; }

        public SessionViewModel(ISessionClient client)
        {
            this.client = client;

            // TODO: Get sessionkey from constructor argument
            this.sessionKey = "N99HL5Y";
            this.BaseTitle = "Session: " + this.sessionKey;
            this.CurrentItemTitle = string.Empty;
            this.currentRound = null;


            // Lists
            this.Players = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();
            this.VoteCards = new ObservableCollection<Card>();

            // Setup
            this.LoadSessionCommand = new RelayCommand(_ => this.ExecuteLoadSessionCommand());
            this.LoadVotesCommand = new RelayCommand(_ => this.ExecuteLoadVotesCommand());
            this.RevoteCommand = new RelayCommand(_ => this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(async _ => await this.ExecuteNextItemCommand());
            this.SendVoteCommand = new RelayCommand(this.ExecuteSendVoteCommand);
            this.SendNitpickerCommand = new RelayCommand(_ => this.ExecuteNitpickerCommand());

            // Threads
            this.StopThreadsCommand = new RelayCommand(_ => this.ExecuteStopThreadsCommand());
            this.StartVotesPull = new RelayCommand(_ => this.ExecuteStartVotesPull());
            this.StartRoundsPull = new RelayCommand(_ => this.ExecuteStartRoundsPull());

            // Initialize Session

        }

        private void ExecuteStartRoundsPull()
        {
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(5), this.FetchRounds);
            this.jobScheduler.Start();
        }

        private void FetchRounds()
        {
            var round = this.client.GetCurrentRound(this.sessionKey).Result;

            if (round != this.currentRound)
            {
                this.ExecuteStopThreadsCommand();
                this.SetCurrentTitle();
                this.ExecuteStartVotesPull();
            }
        }

        private void ExecuteStartVotesPull()
        {
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(5), this.ShouldShowVotes);

            this.jobScheduler.Start();
        }

        public string CurrentItemTitle
        {
            get => this.currentItemTitle;
            set => this.SetProperty(ref this.currentItemTitle, value);
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

            Debug.WriteLine("Next Item clicked");
            await this.client.NextItemAsync(this.sessionKey);
            this.SetCurrentTitle();
            Debug.WriteLine(this.currentItemTitle);

            this.IsBusy = false;
        }

        private void ExecuteRevoteCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            Debug.WriteLine("Revote clicked");

            this.client.NextRoundAsync(this.sessionKey).Wait();

            // Resets votes because there are none in a new round
            this.LoadVotesCommand.Execute(null);

            this.IsBusy = false;
        }

        private void ExecuteSendVoteCommand(object number)
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
            this.client?.Vote(this.sessionKey, new VoteDTO {Estimate = voteEstimate});

            this.IsBusy = false;
        }

        private void ExecuteLoadSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            Debug.WriteLine("LoadSession clicked");

            var session = this.client.GetByKeyAsync(this.sessionKey);

            var players = session.Result.Users;
            foreach (var player in players)
            {
                this.Players.Add(player);
            }

            this.IsBusy = false;
        }

        private void ExecuteLoadVotesCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            Debug.WriteLine("Load votes clicked");

            this.currentRound = this.client.GetCurrentRound(this.sessionKey).Result;
            var votes = this.currentRound.Votes;

            this.VotesToCards(votes);

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

        private void SetCurrentTitle()
        {
            Debug.WriteLine("Updating Item Title");

            var title = this.client.GetCurrentItem(this.sessionKey).Result.Title;
            // This requires AuthToken

            // Correct = title;
            this.CurrentItemTitle = title;
            // Debug.WriteLine(title);
        }

        private void ShouldShowVotes()
        {
            Debug.WriteLine("Pulling");
            var currentVotes = this.client.GetCurrentRound(this.sessionKey).Result.Votes;
            var result = (currentVotes.Count == this.Players.Count);

            if (result)
            {
                this.ExecuteStopThreadsCommand();
                this.ExecuteLoadVotesCommand();
            }
        }

        private void VotesToCards(ICollection<VoteDTO> votes)
        {
            foreach (var vote in votes)
            {
                foreach (var user in this.Players)
                {
                    if (vote.UserId == user.Id)
                    {
                        this.VoteCards.Add(new Card {Name = user.Nickname, Estimate = vote.Estimate});
                    }
                }
            }
        }

        // Mock data to test view bindings.
        private static ObservableCollection<UserDTO> PlayersMockData()
        {
            var data = new ObservableCollection<UserDTO>();
            var userOne = new UserDTO {Nickname = "Vidr", Id = 1};
            var userTwo = new UserDTO {Nickname = "alol", Id = 2};
            var userThree = new UserDTO {Nickname = "olju", Id = 3};

            data.Add(userOne);

            data.Add(userTwo);

            data.Add(userThree);

            return data;
        }

        private static ObservableCollection<VoteDTO> VotesMockData()
        {
            var data = new ObservableCollection<VoteDTO>();

            var voteOne = new VoteDTO {Estimate = 1, UserId = 1};
            var voteTwo = new VoteDTO {Estimate = 3, UserId = 2};
            var voteThree = new VoteDTO {Estimate = 2, UserId = 3};

            data.Add(voteOne);
            data.Add(voteTwo);
            data.Add(voteThree);

            return data;
        }
    }

    public class Card
    {
        public string Name { get; set; }

        public int Estimate { get; set; }
    }
}
