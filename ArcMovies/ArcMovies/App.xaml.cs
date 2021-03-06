﻿
using Prism.Unity;
using ArcMovies.Views;
using Xamarin.Forms;

namespace ArcMovies
{
    public partial class App : PrismApplication
    {
        public App() : base(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<SearchMoviesPage>();
            Container.RegisterTypeForNavigation<MovieDetailPage>();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
