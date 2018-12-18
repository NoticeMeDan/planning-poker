namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class JoinHelper
    {
        private readonly UserCreateDTO user;

        private readonly ISessionClient client;

        public ICommand Join { get; }

        public string Key { get; set; }

        public string Token { get; set; }

        public JoinHelper(ISessionClient client, string key, UserCreateDTO user)
        {
            this.Key = key;
            this.user = user;
            this.client = client;
            this.Join = new RelayCommand(async _ => await this.ExecuteJoinCommand());
        }

        public bool Loading { get; private set; }

        private async Task ExecuteJoinCommand()
        {
            if (this.Loading)
            {
                return;
            }

            this.Loading = true;

            await this.JoinSession();

            this.Loading = false;
        }

        public async Task JoinSession()
        {
            try
            {
                var response = await this.client.Join(this.Key, this.user);
                this.Token = response.Token;
            }
            catch (Exception e)
            {
                this.Key = string.Empty;
                Debug.WriteLine("JoinSession failed. Caught exception: " + e.GetType());
            }
        }
    }
}
