using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PlanningPoker.App.Models;
using PlanningPoker.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlanningPoker.App.Views;
using PlanningPoker.App.Views.WelcomeScreen;
using Xamarin.Android.Net;
using Xamarin.Forms.Internals;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PlanningPoker.App
{
    public partial class App : Application
    {
        public static PublicClientApplication publicClientApplication = null;
        public static UIParent UiParent { get; set; }
        private readonly Lazy<IServiceProvider> _lazyProvider;

        public IServiceProvider Container => _lazyProvider.Value;

        public App()
        {
            InitializeComponent();
            var settings = new Settings();

            _lazyProvider = new Lazy<IServiceProvider>(() => ConfigureServices());
            MainPage = new MainPage();
            publicClientApplication = new PublicClientApplication(settings.ClientId)
            {
                RedirectUri = $"msal{settings.ClientId}://auth",
            };

            DependencyResolver.ResolveUsing(type => Container.GetService(type));

            MainPage = new WelcomeScreen();
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

            var handler = new AndroidClientHandler();

            var httpClient = new HttpClient(handler) { BaseAddress = settings.BackendUrl };

            //services.AddSingleton<IPublicClientApplication>(publicClientApplication);
            services.AddSingleton(_ => new HttpClient(handler) { BaseAddress = settings.BackendUrl});

            return services.BuildServiceProvider();
        }
    }
}
