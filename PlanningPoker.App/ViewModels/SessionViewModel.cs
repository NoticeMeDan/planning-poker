using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PlanningPoker.Shared;

namespace PlanningPoker.App.ViewModels
{
    using System.Net.Http;
    using Models;

    public class SessionViewModel : BaseViewModel
    {
        private SessionRepository Repository;
        private readonly string sessionKey;

        private string currentDescription;
        private string currentTitle;
        private string result;


        public ObservableCollection<UserDTO> Players { get; set; }
        public ObservableCollection<UserDTO> PlayersToVote { get; set; }
        public ObservableCollection<VoteDTO> Votes { get; set; }

        public ICommand AddCardCommand { get; }

        public ICommand AddUserCommand { get; }

        public ICommand RevoteCommand { get; }

        public ICommand NextItemCommand { get;  }

        public ICommand LoadPlayersCommand { get; }

        public ICommand LoadVotesCommand { get; }

        public ICommand SendVoteCommand { get; }


        public SessionViewModel()
        {
            this.Repository = new SessionRepository(new HttpClient());
            this.sessionKey = "42";

            this.BaseTitle = "Session";
            this.currentTitle = "test title";

            this.Players = new ObservableCollection<UserDTO>();
            this.PlayersToVote = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();

            this.AddCardCommand = new RelayCommand(_ => this.ExecuteAddCardCommand());
            this.AddUserCommand = new RelayCommand(_ => this.ExecuteAddUserCommand());
            this.LoadPlayersCommand = new RelayCommand(_ => this.ExecuteLoadPlayersCommand());
            this.LoadVotesCommand = new RelayCommand(_ => this.ExecuteLoadVotesCommand());
            this.RevoteCommand = new RelayCommand(_ => this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(_ => this.ExecuteNextItemCommand());
            this.SendVoteCommand = new RelayCommand(this.ExecuteSendVoteCommand);
        }



        /*public string Result
        {
            get => this.Result;
            set => this.SetProperty(ref this.result, value);
        }*/

        public string CurrentTitle
        {
            get => this.CurrentTitle;
            set => this.SetProperty(ref this.currentTitle, value);
        }

        /*public string CurrentDescription
        {
            get => this.CurrentDescription;
            set => this.SetProperty(ref this.currentTitle, value);
        }*/

        private async Task GetPlayers() {
            Debug.WriteLine("Starting testing GetByKeyAsync");
            var session = await this.Repository.GetByKeyAsync(this.sessionKey).ConfigureAwait(false);
            Debug.WriteLine("Hey testing GetByKeyAsync");
            var players = session.Users;
            foreach (var player in players) {
                this.Players.Add(player);
            }
            Debug.WriteLine(session.Users);
        }

        private void ExecuteLoadVotesCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var votes = VotesMockData();

            foreach (var vote in votes)
            {
                this.Votes.Add(vote);
            }

            this.IsBusy = false;
        }

        private void ExecuteNextItemCommand() {
            throw new System.NotImplementedException();
        }

        private void ExecuteRevoteCommand() {
            throw new System.NotImplementedException();
        }

        private void ExecuteAddUserCommand() {
            throw new System.NotImplementedException();
        }

        private void ExecuteAddCardCommand() {
            throw new System.NotImplementedException();
        }

        private void ExecuteSendVoteCommand(object number) {
            // cast to int.
            var voteEstimate = int.Parse(number.ToString());
            Debug.WriteLine(voteEstimate);
            Debug.WriteLine("");
        }

        private async Task ExecuteLoadPlayersCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            await this.GetPlayers();

            this.IsBusy = false;
        }

        // Mock data to test view bindings.
        private static ObservableCollection<UserDTO> PlayersMockData()
        {
            var data = new ObservableCollection<UserDTO>();
            var userOne = new UserDTO {Nickname = "Vidr"};
            var userTwo = new UserDTO {Nickname = "alol"};
            var userThree = new UserDTO {Nickname = "olju"};

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
}
