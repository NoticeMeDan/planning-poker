namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.ViewModels.util;
    using PlanningPoker.Shared;

    class SessionViewModel : BaseViewModel
    {
        private readonly ISessionClient client;

        private GameInfo GameInfo { get; set; }

        private int RoundId { get; set; }

        public bool Waiting { get; set; }

        public bool End { get; set; }

        public string sessionKey;

        public string title;

        public ObservableCollection<UserDTO> PlayerList { get; set; }

        public ObservableCollection<Cards> Votes { get; set; }

        public bool IsBehind { get; private set; }

        public ICommand Vote { get; }

        public ICommand NewRound { get; }

        public ICommand NextItem { get; }

        public ICommand LoadCommand { get; }

        public SessionViewModel(ISessionClient client)
        {
            Debug.WriteLine("create VM");
            this.client = client;
            this.End = false;
            this.PlayerList = new ObservableCollection<UserDTO>();
            this.Votes = new ObservableCollection<Cards>();
            this.Vote = new RelayCommand(async estimate => await this.Sendvote(estimate));
            this.NewRound = new RelayCommand(async _ => await this.StartNewRound());
            this.NextItem = new RelayCommand(async _ => await this.StartNextItem());
            this.LoadCommand = new RelayCommand(async _ => await this.Initialize());
            Debug.WriteLine("create success");
        }

        private async Task StartNextItem()
        {
            if (this.IsBusy)
            {
                return;
            }

            Debug.WriteLine("StartNextItem");
            this.IsBusy = true;

            var item = await this.client.NextItemAsync(this.sessionKey);
            if (item == null)
            {
                this.End = true;
            }

            await this.StartNewRound();

            this.IsBusy = false;
        }

        private async Task StartNewRound()
        {
            if (this.IsBusy)
            {
                return;
            }

            Debug.WriteLine("StartNewRound");
            this.IsBusy = true;

            var newRound = await this.client.NextRoundAsync(this.SessionKey);

            if (newRound == null)
            {
                this.End = true;
                return;
            }

            await this.UpdateRound(newRound);

            this.IsBusy = false;
        }

        public async Task FetchVotes()
        {
            if (this.IsBusy)
            {
                return;
            }

            Debug.WriteLine("FetchVotes");
            this.IsBusy = true;

            await this.CheckCurrentRound();

            if (this.PlayerList.Count == this.Votes.Count || this.End)
            {
                return;
            }

            await this.UpdateVoteCollection();

            this.IsBusy = false;
        }

        private async Task UpdateVoteCollection()
        {
            Debug.WriteLine("UpdateVotes");
            this.Votes.Clear();

            var round = await this.GameInfo.GetCurrentRound();
            if (round.Id != this.RoundId)
            {
                await this.UpdateRound(round);
                return;
            }

            var votes = round.Votes;
            votes.ToList().ForEach(v =>
            {
                this.Votes.Add(new Cards
                {
                    Estimate = v.Estimate,
                    Nickname = this.GameInfo.PlayerNickname
                });
            });
        }

        private async Task UpdateRound(RoundDTO newRound)
        {
            Debug.WriteLine("UpdateRound");
            this.IsBusy = true;
            this.Votes.Clear();

            var item = await this.GameInfo.GetCurrentItem();
            this.Title = item.Title;
            this.RoundId = newRound.Id;
            this.Waiting = false;
        }

        private async Task Sendvote(object estimate)
        {
            Debug.WriteLine("SendVote");
            var vote = new VoteDTO
            {
                Estimate = (int)estimate,
                UserId = this.GameInfo.UserState.Id
            };

            await this.client.Vote(this.SessionKey, vote);
            
            this.Waiting = true;
        }

        public async Task Initialize()
        {
            Debug.WriteLine("start ini");

            this.GameInfo = new GameInfo(this.client, this.SessionKey);
            Debug.WriteLine("gameinfo success");

            await this.GameInfo.Initialize();
            Debug.WriteLine("gameinfo ini success");

            this.PlayerList = this.GameInfo.Users;
            var currentItem = await this.GameInfo.GetCurrentItem();
            this.Title = currentItem.Title;
        }

        public string SessionKey
        {
            get => this.sessionKey;
            set => this.SetProperty(ref this.sessionKey, value);
        }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        public async Task<RoundDTO> CheckCurrentRound()
        {
            Debug.WriteLine("ChecCurrentRound");
            var currentRound = await this.GameInfo.GetCurrentRound();

            return currentRound;
        }

        public class Cards
        {
            public string Nickname { get; set; }

            public int Estimate { get; set; }
        }
    }
}
