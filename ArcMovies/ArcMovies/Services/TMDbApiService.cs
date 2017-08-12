using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ArcMovies.Models;
using Xamarin.Forms;
using ArcMovies.Interfaces;
using System.Net.Http.Headers;
using System.IO;
using System.Globalization;
using System.Diagnostics;

[assembly: Dependency(typeof(ArcMovies.Services.TmdbApiService))]
namespace ArcMovies.Services
{
    public class TmdbApiService : IMoviesApi
    {
        private const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";
        private const string ApiUrl = "https://api.themoviedb.org/3";

        private const string SearchMoviesPath = "/search/movie";
        private const string MoviesPath = "/movie";
        private const string GenreListPath = "/genre/list";

        private readonly string _language;

        private readonly HttpClient _httpClient;

        public TmdbApiService()
        {
            _language = CultureInfo.CurrentCulture.Name;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        ~TmdbApiService()
        {
            _httpClient.Dispose();
        }

        public async Task<SearchMovie> SearchMoviesAsync(string searchTerm, int page)
        {            
            var restUrl = $"{ApiUrl}{SearchMoviesPath}?api_key={ApiKey}&query={searchTerm}&page={page}&language={_language}";

            try
            {
                using (var response = await _httpClient.GetAsync(restUrl).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return JsonConvert.DeserializeObject<SearchMovie>(
                                await new StreamReader(responseStream).ReadToEndAsync().ConfigureAwait(false));
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }            

            return null;
        }
                
        public async Task<SearchMovie> GetMoviesByCategoryAsync(int page, Enums.MovieCategory category)
        {
            var restUrl = $"{ApiUrl}{Enums.PathCategoryMovie(category)}?api_key={ApiKey}&page={page}&language={_language}";
            try
            {
                using (var response = await _httpClient.GetAsync(restUrl).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return JsonConvert.DeserializeObject<SearchMovie>(
                                await new StreamReader(responseStream).ReadToEndAsync().ConfigureAwait(false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }            

            return null;
        }
        
        public async Task<MovieDetail> GetMovieDetailAsync(int id)
        {
            var restUrl = $"{ApiUrl}{MoviesPath}/{id}?api_key={ApiKey}&language={_language}";
            try
            {
                using (var response = await _httpClient.GetAsync(restUrl).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return JsonConvert.DeserializeObject<MovieDetail>(
                                await new StreamReader(responseStream).ReadToEndAsync().ConfigureAwait(false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }            

            return null;
        }
        
        public async Task<List<Genre>> GetGenresAsync()
        {            
            var restUrl = $"{ApiUrl}{GenreListPath}?api_key={ApiKey}&language={_language}";
            try
            {
                using (var response = await _httpClient.GetAsync(restUrl).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            var genreList = JsonConvert.DeserializeObject<GenreList>(
                                await new StreamReader(responseStream).ReadToEndAsync().ConfigureAwait(false));
                            return genreList?.Genres;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }            

            return null;
        }

        private void ReportError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }    
}