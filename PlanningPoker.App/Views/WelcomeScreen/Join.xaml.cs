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
        private JoinViewModel _vm;

        public Join()
        {
            this.InitializeComponent();

            this.BindingContext = this._vm =
               (Application.Current as App)?.Container.GetRequiredService<JoinViewModel>();
        }

        private async Task HandleClickedAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("try to connection?");
            this._vm.JoinCommand.Execute(e);
            if (_vm.connection == true)
            {
                Debug.WriteLine("Connection!");
                await this.Navigation.PushModalAsync(new Lobby());
            }
        }
    }
}
