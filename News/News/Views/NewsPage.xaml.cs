using News.Models;
using News.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        NewsService service;
        string URL;
        public NewsPage()
        {
            InitializeComponent();
            service = new NewsService();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Code here will run right before the screen appears
            //You want to set the Title or set the City
            lblHeader.Text = $"Todays {Title} Headlines";

            //This is making the first load of data
            MainThread.BeginInvokeOnMainThread(async () => { await LoadNews(); });
        }

        private async Task LoadNews()
        {
            try
            {
                NewsCategory category = (NewsCategory)Enum.Parse(typeof(NewsCategory), Title, true);
                if (category == NewsCategory.technology)
                {
                    await Task.Delay(5000); // Makes technology load slower, giving the activity indicator time to run. "Fake slow connection"
                }
                await service.GetNewsAsync(category);
                Task<NewsGroup> t1 = service.GetNewsAsync(category);
                var items = t1.Result.Articles;
                NewsList.ItemsSource = items;
                URL = items[0].Url;
            }
            catch (Exception)
            {
                await DisplayAlert("Have you tried turning it off and on again?",
                    "Seems like the connection to the server has been lost!\nPlease check your internet connection or try again later!",
                    "Fine...");
            }
            actIndicator.IsRunning = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            progBar.IsVisible = true;
            actIndicator.IsRunning = true;
            await progBar.ProgressTo(1, 3000, Easing.Linear);
            await LoadNews();
            progBar.IsVisible = false;
            progBar.Progress = 0;
        }

        private async void NewsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ArticleView(URL));
        }


    }
}