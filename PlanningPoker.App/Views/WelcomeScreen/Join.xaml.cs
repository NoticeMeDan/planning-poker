namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.App.Views.Session;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Join : ContentPage
    {
        private JoinViewModel viewModel;

        public Join()
        {
            this.InitializeComponent();

            this.BindingContext = this.viewModel =
               (Application.Current as App)?.Container.GetRequiredService<JoinViewModel>();
        }

        private async Task HandleClickedAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("try to connection?");
            this.viewModel.JoinCommand.Execute(e);
            if (this.viewModel.Connection == true)
            {
                Debug.WriteLine("Connection!");
                Debug.WriteLine("Join SessionKey: " + this.viewModel.Key);
                await this.Navigation.PushModalAsync(new Lobby(this.viewModel.Key));
            }
        }
    }
}
