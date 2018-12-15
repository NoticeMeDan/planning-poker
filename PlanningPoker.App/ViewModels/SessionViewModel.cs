using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<UserDTO> Players { get; set; }

        public ObservableCollection<VoteDTO> Votes { get; set; }


        public SessionViewModel() {
            this.Repository = new SessionRepository(new HttpClient());

            this.BaseTitle = "Session";
            this.currentTitle = "test title";

            this.Players = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();

            this.AddCardCommand = new RelayCommand(_ => this.ExecuteAddCardCommand());
            this.AddUserCommand = new RelayCommand(_ => this.ExecuteAddUserCommand());
            this.LoadPlayersCommand = new RelayCommand(_ => this.ExecuteLoadPlayersCommand());
            this.LoadItemsCommand = new RelayCommand(_ => this.ExecuteLoadItemsCommand());
            this.RevoteCommand = new RelayCommand(_ => this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(_ => this.ExecuteNextItemCommand());
        }

        private void ExecuteLoadItemsCommand() {
            throw new System.NotImplementedException();
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


        public ICommand AddCardCommand { get; }

        public ICommand AddUserCommand { get; }

        public ICommand RevoteCommand { get; }

        public ICommand NextItemCommand { get;  }

        public ICommand LoadPlayersCommand { get; }

        public ICommand LoadItemsCommand { get; }

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
                this.Players.Add(player);
            }

            this.IsBusy = false;
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
            var voteThree = new VoteDTO {Estimate = 1, UserId = 3};

            data.Add(voteOne);
            data.Add(voteTwo);
            data.Add(voteThree);

            return data;
        }
    }
}
