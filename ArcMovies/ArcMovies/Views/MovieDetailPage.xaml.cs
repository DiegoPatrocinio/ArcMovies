using Xamarin.Forms;

namespace ArcMovies.Views
{
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage()
        {
            InitializeComponent();

            GenresListView.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}
