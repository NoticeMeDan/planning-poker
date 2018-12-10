using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PlanningPoker.App.Models;
using Xamarin.Forms;

namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using PlanningPoker.App.ViewModels.Interfaces;
    using PlanningPoker.Shared;

    // This class contains data until repositories is setup
    public class ItemsViewModel : BaseViewModel
    {
        // TODO: Use API to get and set items.
        private readonly ISessionRepository sessionRepo;
        public List<ItemCreateUpdateDTO> Items { get; set; }
        public string Session { get; }

        // public ICommand AddCommand { get; set; }
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CreateSessionCommand { get; }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ItemsViewModel(ISessionRepository sessionRepo)
        {
            this.Session = "New session";

            this.sessionRepo = sessionRepo;

            this.Items = new List<ItemCreateUpdateDTO>();

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
            this.SaveCommand = new RelayCommand(_ => this.ExecuteSaveCommand());
            this.CreateSessionCommand = new RelayCommand(_ => this.ExecuteCreateSessionCommand());
        }

        private void ExecuteSaveCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var toCreate = new ItemCreateUpdateDTO
            {
                Title = Title,
                Description = Description
            };

            Items.Add(toCreate);

            Title = string.Empty;
            Description = string.Empty;
            this.LoadCommand.Execute(null);

            IsBusy = false;
        }

        private void ExecuteLoadCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();

            var items = Items;

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.IsBusy = false;
        }

        private async Task ExecuteCreateSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var toCreate = new SessionCreateUpdateDTO
            {
                Items = this.Items
            };

            await sessionRepo.CreateAsync(toCreate);

            this.IsBusy = false;
        }
    }
}
