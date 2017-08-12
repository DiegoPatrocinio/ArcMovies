using Plugin.Connectivity;
using ArcMovies.Interfaces;

using Xamarin.Forms;

namespace ArcMovies.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ItemsListView.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}
