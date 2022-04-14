using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using News.Models;
using News.Services;

namespace News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        NewsService service;
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
            
            NewsCategory category = (NewsCategory)Enum.Parse(typeof(NewsCategory), Title, true);
            await service.GetNewsAsync(category);
            Task<NewsGroup> t1 = service.GetNewsAsync(category);
            var items = t1.Result.Articles;
            NewsList.ItemsSource = items;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            progBar.IsVisible = true;
            await progBar.ProgressTo(1, 4000, Easing.Linear);
            await LoadNews();
            progBar.IsVisible = false;
        }

        private async void NewsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ArticleView articleView = (ArticleView)e.Item;
            await Navigation.PushAsync(articleView);
            ((ListView)sender).SelectedItem = null;
        }
    }
}