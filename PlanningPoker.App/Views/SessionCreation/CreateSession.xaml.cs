using PlanningPoker.App.Views.Session;

namespace PlanningPoker.App.Views.SessionCreate
{
    using System;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.App.Views.CreateSession;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {

        public CreateSession() {
            this.Title = "Session Creation";
            this.InitializeComponent();
        }

        private static int GenerateKey(int min, int max)
        {
            Random random = new Random();
            var result = 0;


            result = random.Next(min, max);

            return result;
        }

        private void ToolbarItem_OnActivated(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new NewItem());
        }

        private void CreateSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(
                new Lobby());
        }


    }
}

