namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Windows.Input;
    using Models;
    using Shared;

    public class SessionViewModel : BaseViewModel
    {
        private readonly SessionClient repository;
        private SessionDTO session;
        private readonly string sessionKey;
        private string currentItemTitle;
        private string token;

        public ObservableCollection<UserDTO> Players { get; set; }

        public ObservableCollection<Card> VoteCards { get; set; }

        public ObservableCollection<VoteDTO> Votes { get; set; }

        public ICommand RevoteCommand { get; }

        public ICommand NextItemCommand { get;  }

        public ICommand LoadSessionCommand { get; }

        public ICommand LoadVotesCommand { get; }

        public ICommand SendVoteCommand { get; }

        public ICommand SendNitpickerCommand { get; }

        public SessionViewModel()
        {
            // Create Repository
            this.repository = new SessionClient(new HttpClient());

            // TODO: Get sessionkey from constructor argument
            this.sessionKey = "42";
            this.BaseTitle = "Session: " + this.sessionKey;
            this.CurrentItemTitle = string.Empty;

            // Lists
            this.Players = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<VoteDTO>();
            this.VoteCards = new ObservableCollection<Card>();

            // Setup
            this.LoadSessionCommand = new RelayCommand(_ => this.ExecuteLoadSessionCommand());
            this.LoadVotesCommand = new RelayCommand(_ => this.ExecuteLoadVotesCommand());
            this.RevoteCommand = new RelayCommand(_ => this.ExecuteRevoteCommand());
            this.NextItemCommand = new RelayCommand(_ => this.ExecuteNextItemCommand());
            this.SendVoteCommand = new RelayCommand(this.ExecuteSendVoteCommand);
            this.SendNitpickerCommand = new RelayCommand(_ => this.ExecuteNitpickerCommand());

            // Initialize Session
            // this.ExecuteSetupCommand();
        }

        public string CurrentItemTitle
        {
            get => this.currentItemTitle;
            set => this.SetProperty(ref this.currentItemTitle, value);
        }

        /*
         * Following is the ExecuteCommands
         */
        private void ExecuteNextItemCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            Debug.WriteLine("Next Item clicked");
            this.NextItem();

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

            //this.Repository.NextRoundAsync(this.sessionKey, "something").Wait();
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
            // this.Repository?.Vote(this.sessionKey, new VoteDTO {Estimate = voteEstimate});

            this.IsBusy = false;

            if (this.ShouldShowVotes())
            {
                this.LoadVotesCommand.Execute(null);
            }
        }

        private void ExecuteLoadSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            Debug.WriteLine("LoadSession clicked");
            this.SetCurrentTitle();
            this.GetPlayers();

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
            this.GetVotes();
            this.VoteToCard(this.Votes);

            this.IsBusy = false;
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
        private void GetPlayers()
        {
            var session = this.repository.GetByKeyAsync(this.sessionKey);

            var players = session.Result.Users;
            foreach (var player in players)
            {
                this.Players.Add(player);
            }
        }

        private void SetCurrentTitle()
        {
            Debug.WriteLine("Updating Item Title");

            //var title =  this.repository.GetCurrentItem(this.sessionKey).Result.Title;
            // This requires AuthToken

           this.CurrentItemTitle = "Item One";
           // Debug.WriteLine(title);
        }

        private void NextItem()
        {
            this.repository.NextItemAsync(this.sessionKey).Wait();
            this.SetCurrentTitle();

            // Maybe this is needed.
            //this.GetPlayers();
        }

        private void GetVotes()
        {
            // Using mockdata untill Token is given.
            var votes = VotesMockData();

            // Correct:
            //var votes = this.Repository.GetCurrentRound(this.sessionKey).Result.Votes;

            foreach (var vote in votes)
            {
                this.Votes.Add(vote);
            }
        }

        private bool ShouldShowVotes()
        {
            //var currentVotes = this.repository.GetCurrentRound(this.sessionKey).Result.Votes;
            return true; //(currentVotes.Count == this.Players.Count);
        }

        private void VoteToCard(ObservableCollection<VoteDTO> votes)
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
