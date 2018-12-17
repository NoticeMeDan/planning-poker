namespace PlanningPoker.App.ViewModels
{
    using System.Collections.Generic;
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

        public ObservableCollection<UserDTO> Users { get; set; }

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

            var session = await this.repository.GetByKeyAsync(this.Key);

            this.UpdateUserCollection(session.Users);

            this.loading = false;
        }

        private void UpdateUserCollection(ICollection<UserDTO> users)
        {
            this.Users.Clear();

            users.ToList().ForEach(u =>
            {
                    this.Users.Add(u);
            });
        }

        public string Key
        {
            get => this.key;
            set => this.SetProperty(ref this.key, value);
        }
    }
}
