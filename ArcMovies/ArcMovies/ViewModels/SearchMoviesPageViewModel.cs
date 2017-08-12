using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ArcMovies.Helpers;
using ArcMovies.Models;
using Xamarin.Forms;

namespace ArcMovies.ViewModels
{
    public class SearchMoviesPageViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        private int _totalPage;        

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                SetProperty(ref _searchTerm, value);                
                SearchResults.Clear();
            }
        }

        public ObservableRangeCollection<Movie> SearchResults { get; set; }

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand<Movie> ShowMovieDetailCommand { get; }
        public DelegateCommand<Movie> ItemAppearingCommand { get; }

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        public SearchMoviesPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            Title = "Search Movies";
            this._navigationService = navigationService;
            this._pageDialogService = pageDialogService;
            SearchResults = new ObservableRangeCollection<Movie>();
            
            SearchCommand = new DelegateCommand(async () => await ExecuteSearchCommand().ConfigureAwait(false));
            ShowMovieDetailCommand = new DelegateCommand<Movie>(async (Movie movie) => await ExecuteShowMovieDetailCommand(movie).ConfigureAwait(false));
            ItemAppearingCommand = new DelegateCommand<Movie>(async (Movie movie) => await ExecuteItemAppearingCommand(movie).ConfigureAwait(false));
        }

        private async Task ExecuteSearchCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                SearchResults.Clear();
                _currentPage = 1;
                await LoadAsync(_currentPage).ConfigureAwait(true);
            }
            finally
            {
                IsBusy = false;
            }

            if (SearchResults.Count == 0)
            {
                await _pageDialogService.DisplayAlertAsync("The Movie", "No results found.", "Ok").ConfigureAwait(false);                
            }
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
            int viewCellIndex = SearchResults.IndexOf(movie);
            if (SearchResults.Count - itemLoadNextItem <= viewCellIndex)
            {
                await NextPageAsync().ConfigureAwait(false);
            }
        }

        private async Task NextPageAsync()
        {
            _currentPage++;
            if (_currentPage <= _totalPage)
            {
                await LoadAsync(_currentPage).ConfigureAwait(false);
            }
        }

        private async Task LoadAsync(int page)
        {
            try
            {
                var continueOnCapturedContext = Device.RuntimePlatform == Device.Windows;

                var searchMovies = await ApiService.SearchMoviesAsync(_searchTerm, page).ConfigureAwait(continueOnCapturedContext);

                if (searchMovies != null)
                {
                    _totalPage = searchMovies.TotalPages;                    
                    SearchResults.AddRange(searchMovies.Movies);                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }        
    }
}