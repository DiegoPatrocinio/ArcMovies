using System.Collections.Generic;
using System.Threading.Tasks;
using ArcMovies.Models;

namespace ArcMovies.Interfaces
{
    public interface IMoviesApi
    {

        Task<SearchMovie> SearchMoviesAsync(string searchTerm, int page);

        Task<SearchMovie> GetMoviesByCategoryAsync(int page, Enums.MovieCategory sortBy);

        Task<MovieDetail> GetMovieDetailAsync(int id);

        Task<List<Genre>> GetGenresAsync();
    }
}