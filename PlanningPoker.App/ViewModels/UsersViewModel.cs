namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Shared;

    // This class contains testdata until repositories is setup
    public class UsersViewModel : BaseViewModel
    {
        // TODO: Use API to get and set items.
        // Connect to repositories and get data

        // public ICommand AddCommand { get; set; }
        public ICommand LoadCommand { get; }

        // Only for testing until repositories are ready
        public ObservableCollection<UserDTO> Users { get; }

        public UsersViewModel()
        {
            this.BaseTitle = "Users";

            this.Users = new ObservableCollection<UserDTO>();

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
        }

        private static ObservableCollection<UserDTO> MockDataUsers()
        {
            var data = new ObservableCollection<UserDTO>();

            var item1 = new UserDTO { Nickname = "Franz" };
            var item2 = new UserDTO { Nickname = "Isabel" };
            var item3 = new UserDTO { Nickname = "Magnus" };

            data.Add(item1);
            data.Add(item2);
            data.Add(item3);

            return data;
        }

        private void ExecuteLoadCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Users.Clear();

            var users = MockDataUsers();

            foreach (var user in users)
            {
                this.Users.Add(user);
            }

            this.IsBusy = false;
        }
    }
}
