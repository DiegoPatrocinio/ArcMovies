using Prism.Navigation;
using System.Threading.Tasks;
using ArcMovies.Models;

namespace ArcMovies.ViewModels
{
    public class MovieDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private Movie _movie;
        public Movie Movie
        {
            get => _movie;
            set => SetProperty(ref _movie, value);
        }        
  
        private async Task LoadMovieDetailAsync(int movieId)
        {
            var movieDetail = await ApiService.GetMovieDetailAsync(movieId).ConfigureAwait(false);
            if (movieDetail != null)
            {
                Movie = movieDetail;
            }            
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            Movie = parameters.GetValue<Movie>("movie");            
            Title = Movie.Title;
            await LoadMovieDetailAsync(Movie.Id).ConfigureAwait(false);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }
    }
}