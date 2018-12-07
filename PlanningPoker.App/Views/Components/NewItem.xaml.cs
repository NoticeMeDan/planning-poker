using PlanningPoker.App.Views.Session;

namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using ViewModels;
    using Xamarin.Forms;

    public partial class NewItem : ContentPage {
        private readonly ItemViewModel ViewModel;

        public NewItem()
        {
            this.InitializeComponent();

            this.ViewModel = new ItemViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemViewModel>();
        }

        private void SaveItemClicked(object sender, EventArgs e) {
            this.DisplayAlert("Save?", "Wanna add the item to list of items?", "Yes", "No");
        }
    }
}
