using PlanningPoker.App.Views.Session;

namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using ViewModels;
    using Xamarin.Forms;

    public partial class NewItem : ContentPage {

        private readonly ItemsViewModel viewModel;

        public NewItem()
        {
            this.InitializeComponent();

            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();
        }

        private void SaveItemClicked(object sender, EventArgs e)
        {
            if (viewModel.SaveCommand != null) viewModel.SaveCommand.Execute(null);
            this.Navigation.PopModalAsync();
        }
    }
}
