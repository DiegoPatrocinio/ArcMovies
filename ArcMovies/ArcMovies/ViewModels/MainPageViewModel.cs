using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArcMovies.Helpers;
using ArcMovies.Models;
using Xamarin.Forms;

namespace ArcMovies.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {        
        private int _currentPage = 1;
        private int _totalPage = 0;

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        private List<Genre> _genres;        
        public ObservableRangeCollection<Movie> Movies { get; set; }

        public DelegateCommand LoadUpcomingMoviesCommand { get; }
        public DelegateCommand ShowSearchMoviesCommand { get; }        
        public DelegateCommand<Movie> ShowMovieDetailCommand { get; }
        public DelegateCommand<Movie> ItemAppearingCommand { get; }

        private readonly INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            Title = "Upcoming Movies";
            this._navigationService = navigationService;
            Movies = new ObservableRangeCollection<Movie>();

            LoadUpcomingMoviesCommand = new DelegateCommand(async () => await ExecuteLoadUpcomingMoviesCommand().ConfigureAwait(false));
            ShowSearchMoviesCommand = new DelegateCommand(async () => await ExecuteShowSearchMoviesCommand().ConfigureAwait(false));
            ShowMovieDetailCommand = new DelegateCommand<Movie>(async (Movie movie) => await ExecuteShowMovieDetailCommand(movie).ConfigureAwait(false));
            ItemAppearingCommand = new DelegateCommand<Movie>(async (Movie movie) => await ExecuteItemAppearingCommand(movie).ConfigureAwait(false));

            LoadUpcomingMoviesCommand.Execute();
        }        

        private async Task ExecuteLoadUpcomingMoviesCommand()
        {
            IsConnected = CrossConnectivity.Current.IsConnected;

            if (IsBusy || !IsConnected)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                _currentPage = 1;
                await LoadMoviesAsync(_currentPage, Enums.MovieCategory.Upcoming).ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteShowSearchMoviesCommand()
        {            
            await _navigationService.NavigateAsync("SearchMoviesPage").ConfigureAwait(false);
        }

        private async Task ExecuteShowMovieDetailCommand(Movie movie)
        {            
            var parameters = new NavigationParameters();
            parameters.Add(nameof(movie), movie);
            await _navigationService.NavigateAsync("MovieDetailPage", parameters).ConfigureAwait(false);
        }

        private async Task ExecuteItemAppearingCommand(Movie movie)
        {
            int itemLoadNextItem = 2;
            int viewCellIndex = Movies.IndexOf(movie);
            if (Movies.Count - itemLoadNextItem <= viewCellIndex)
            {                
                await NextPageUpcomingMoviesAsync().ConfigureAwait(false);
            }
        }

        private async Task LoadMoviesAsync(int page, Enums.MovieCategory movieCategory)
        {
            try
            {
                var continueOnCapturedContext = Device.RuntimePlatform == Device.Windows;

                _genres = _genres ?? await ApiService.GetGenresAsync().ConfigureAwait(continueOnCapturedContext);
                var searchMovie = await ApiService.GetMoviesByCategoryAsync(page, movieCategory).ConfigureAwait(continueOnCapturedContext);
                if (searchMovie != null)
                {
                    var movies = new List<Movie>();
                    _totalPage = searchMovie.TotalPages;
                    foreach (var movie in searchMovie.Movies)
                    {                                                
                        GenreListToString(movie);                        
                        movies.Add(movie);
                    }
                    Movies.AddRange(movies);
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task NextPageUpcomingMoviesAsync()
        {
            _currentPage++;
            if (_currentPage <= _totalPage)
            {                
                await LoadMoviesAsync(_currentPage, Enums.MovieCategory.Upcoming).ConfigureAwait(false);
            }
        }

        private void GenreListToString(Movie movie)
        {
            var genresMovie = _genres.Where(genre => movie.GenreIds.Any(genreId => genreId == genre.Id));
            movie.GenresNames = genresMovie.Select(g => g.Name).Aggregate((first, second) => $"{first}, {second}");            
        }
    }
}