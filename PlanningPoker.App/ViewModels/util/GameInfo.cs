namespace PlanningPoker.App.ViewModels.util
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class GameInfo
    {
        private string SessionKey { get; set; }

        private SessionDTO Session { get; set; }

        private readonly ISessionClient client;

        public ObservableCollection<UserDTO> Users { get; }

        public UserState UserState { get; set; }

        public string PlayerNickname { get; set; }

        public GameInfo(ISessionClient client, string sessionKey)
        {
            this.client = client;
            this.SessionKey = sessionKey;
            this.Users = new ObservableCollection<UserDTO>();
        }

        public async Task Initialize()
        {
            Debug.WriteLine("ini Gameinfo");
            this.Session = await this.GetSession();
            Debug.WriteLine("game ini session: " + this.Session);
            if (this.Session != null)
            {
                this.SetUserS(this.Session.Users);
                Debug.WriteLine("ini Gameinfo setUsers complete");
                this.UserState = await this.GetWhoAmI();
                this.PlayerNickname = this.GetNickname();
            }
            Debug.WriteLine("ini Gameinfo success");
        }

        public async Task<ItemDTO> GetCurrentItem()
        {
            return await this.client.GetCurrentItem(this.SessionKey);
        }

        public async Task<RoundDTO> GetCurrentRound()
        {
            return await this.client.GetCurrentRound(this.SessionKey);
        }

        private string GetNickname()
        {
            if (this.Users != null && this.UserState != null)
            {
                return this.Users.ToList().Where(u => u.Id == this.UserState.Id).Select(u => u.Nickname).FirstOrDefault();
            }

            return null;
        }

        private async Task<UserState> GetWhoAmI()
        {
            return await this.client.WhoAmI(this.SessionKey);
        }

        private async Task<SessionDTO> GetSession()
        {
            return await this.client.GetByKeyAsync(this.SessionKey);
        }

        private void SetUserS(ICollection<UserDTO> users)
        {
            if (users != null)
            {
                users.ToList().ForEach(u => this.Users.Add(u));
            }
        }
    }
}
