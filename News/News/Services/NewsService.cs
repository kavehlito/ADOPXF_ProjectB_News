#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using News.Models;
using News.ModelsSampleData;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace News.Services
{
    public class NewsService
    {
        //Here is where you lift in your Service code from Part A
        //HttpClient httpClient = new HttpClient();
        readonly string apiKey = "a389335d7a5044a49c01cb03ab5a9267";

        public event EventHandler<string> NewsAvailable;
        protected virtual void OnNewsAvailable(string message)
        {
            NewsAvailable?.Invoke(this, message);
        }
        public async Task<NewsGroup> GetNewsAsync(NewsCategory category)
        {

#if UseNewsApiSample      

#else
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";


            //Recommend to use Newtonsoft Json Deserializer as it works best with Android
            var webclient = new WebClient();
            var json = await webclient.DownloadStringTaskAsync(uri);
            NewsApiData nd = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsApiData>(json);


            // https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

            // your code to get live data
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            nd = await response.Content.ReadFromJsonAsync<NewsApiData>();
#endif
            NewsGroup news = new NewsGroup();
            var date = DateTime.UtcNow;

            NewsCacheKey cacheKey = new NewsCacheKey(category, date);

            if (!cacheKey.CacheExist)
            {
                    NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);
                
                    news.Category = category;
                    news.Articles = nd.Articles.Select(item => new NewsItem
                    {
                        DateTime = item.PublishedAt,
                        Title = item.Title,
                        Description = item.Description,
                        Url = item.Url,
                        UrlToImage = item.UrlToImage

                    }).ToList();

                NewsGroup.Serialize(news, "test.xml");
                NewsCacheKey.Serialize(news, cacheKey.FileName);
                OnNewsAvailable($"New news in category {category} is available");
         
            }
            else
            {
                news = NewsCacheKey.Deserialize(cacheKey.FileName);
                OnNewsAvailable($"XML Cached news in category {category} is available");
            }
            return news;
        }
    }
}

