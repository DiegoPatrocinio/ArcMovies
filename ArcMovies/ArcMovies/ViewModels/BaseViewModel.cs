using Prism.Mvvm;
using ArcMovies.Interfaces;

using Xamarin.Forms;

namespace ArcMovies.ViewModels
{
    public class BaseViewModel : BindableBase
    {

        protected IMoviesApi ApiService => DependencyService.Get<IMoviesApi>();

        bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        string _title = string.Empty;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }        
    }
}
