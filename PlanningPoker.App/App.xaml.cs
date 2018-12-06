using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PlanningPoker.App.Models;
using PlanningPoker.App.ViewModels;
using PlanningPoker.App.Views.SessionCreation;
using PlanningPoker.App.Views.WelcomeScreen;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PlanningPoker.App
{
    public partial class App : Application
    {
        public static PublicClientApplication publicClientApplication = null;

        public static UIParent UiParent { get; set; }

        private readonly Lazy<IServiceProvider> _lazyProvider;
        private Settings settings = new Settings();

        public IServiceProvider Container => this._lazyProvider.Value;

        public App()
        {
            this.InitializeComponent();

            this._lazyProvider = new Lazy<IServiceProvider>(() => this.ConfigureServices());
            publicClientApplication = new PublicClientApplication(this.settings.ClientId)
            {
                RedirectUri = $"msal{this.settings.ClientId}://auth",
            };

            DependencyResolver.ResolveUsing(this.Container.GetService);

            // Change Screen for faster development. Standard page is WelcomeScreen()
            this.MainPage = new NavigationPage(new WelcomeScreen());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var settings = new Settings();

            var handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler) { BaseAddress = settings.BackendUrl };

            services.AddSingleton(_ => new HttpClient(handler) { BaseAddress = settings.BackendUrl });

            // Adding the ViewModels
            services.AddScoped<LoginViewModel>();
            services.AddScoped<ItemsViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
