namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class LobbyViewModel : BaseViewModel
    {
        private readonly ISessionRepository repository;
        private bool loading;
        private string key;
        private string title;

        public ObservableCollection<UserDTO> Users { get; set; }

        public UserDTO User { get; set; }

        public SessionDTO Session { get; set; }

        public ICommand GetUsersCommand { get; }

        public LobbyViewModel(ISessionRepository repository)
        {
            this.repository = repository;
            this.Users = new ObservableCollection<UserDTO>();
            this.GetUsersCommand = new RelayCommand(async _ => await this.ExecuteGetUsersCommand());
        }

        private async Task ExecuteGetUsersCommand()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            this.Users.Clear();
            /*
            this.Session = await this.repository.GetByKeyAsync(this.Key);

            this.Session.Users.ToList().ForEach(u =>
            {
                if (!this.Users.Contains(u))
                {
                    this.Users.Add(u);
                }
            });*/

            // MOCKDATA VILFRED-STYLE
            this.Users.Add(new UserDTO { Nickname = "mips" });
            this.Users.Add(new UserDTO { Nickname = "alol" });
            this.Users.Add(new UserDTO { Nickname = "vidr" });

            this.loading = false;
        }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, "Session-key: " + value);
        }

        public string Key
        {
            get => this.key;
            set => this.SetProperty(ref this.key, value);
        }
    }
}
