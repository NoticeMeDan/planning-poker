using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using PlanningPoker.Shared;

namespace PlanningPoker.App.ViewModels
{
    using System.Net.Http;
    using Models;

    public class SessionViewModel : BaseViewModel
    {
        private SessionRepository Repository;
        private string currentDescription;
        private string currentTitle;
        private string result;

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

            this.BaseTitle = "Session";
            this.currentTitle = "test title";

            this.PlayersToVote = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();

            this.AddCardCommand = new RelayCommand(_ => this.ExecuteAddCardCommand());
            this.AddUserCommand = new RelayCommand(_ => this.ExecuteAddUserCommand());
            this.LoadPlayersCommand = new RelayCommand(_ => this.ExecuteLoadPlayersCommand());
            this.LoadVotesCommand = new RelayCommand(_ => this.ExecuteLoadVotesCommand());
            this.RevoteCommand = new RelayCommand(_ => this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(_ => this.ExecuteNextItemCommand());
            this.SendVoteCommand = new RelayCommand(number => this.ExecuteSendVoteCommand(number));
        }

        private void ExecuteSendVoteCommand(object number) {
            // cast to int.
            var voteEstimate = int.Parse(number.ToString());
            Debug.WriteLine(voteEstimate);
            Debug.WriteLine("");
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

        private void ExecuteLoadPlayersCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var players = PlayersMockData();

            foreach (var player in players)
            {
                this.PlayersToVote.Add(player);
            }

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
