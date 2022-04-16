using News.Models;
using News.Services;
using System.Text;
using System.Threading.Tasks;

namespace News.Consoles


// mer async?!
{
    //Your can move your Console application Main here. Rename Main to myMain and make it NOT static and async
    class Program
    {
        #region used by the Console
        Views.ConsolePage theConsole;
        StringBuilder theConsoleString;
        public Program(Views.ConsolePage myConsole)
        {
            //used for the Console
            theConsole = myConsole;
            theConsoleString = new StringBuilder();
        }
        #endregion
        NewsService service;

        #region Console Demo program
        //This is the method you replace with your async method renamed and NON static Main
        public async Task MyMain()
        {
            service.NewsAvailable += ReportNewsAvailabe;

            Task<NewsGroup> t1 = null;

            for (NewsCategory c = NewsCategory.business; c < NewsCategory.technology + 1; c++)
            {
                await service.GetNewsAsync(c);
                t1 = service.GetNewsAsync(c);
                theConsoleString.AppendLine($"News in {t1.Result.Category}:");

                foreach (var item in t1.Result.Articles)
                {
                    theConsoleString.AppendLine($"    -{item.DateTime}: {item.Title}");
                }
            }
            theConsole.WriteLine(theConsoleString.ToString());
            theConsoleString.Clear();


            Task<NewsGroup> t2 = null;

            for (NewsCategory c = NewsCategory.business; c < NewsCategory.technology + 1; c++)
            {
                await service.GetNewsAsync(c);
                t2 = service.GetNewsAsync(c);
                theConsoleString.AppendLine($"News in {t2.Result.Category}:");

                foreach (var item in t2.Result.Articles)
                {
                    theConsoleString.AppendLine($"    -{item.DateTime}: {item.Title}");
                }
            }
            theConsole.WriteLine(theConsoleString.ToString());
            theConsoleString.Clear();

            void ReportNewsAvailabe(object sender, string message)
            {
                theConsole.WriteLine($"Event message from news service: {message}");
            }

            /*
            theConsole.WriteLine("Demo program output");

            //Write an output to the Console
            theConsole.Write("One ");
            theConsole.Write("Two ");
            theConsole.WriteLine("Three and end the line");

            //As theConsole.WriteLine return trips are quite slow in UWP, use instead of myConsoleString to build the the more complex output
            //string using several myConsoleString.AppendLine instead of several theConsole.WriteLine. 
            foreach (char c in "Hello World from my Console program")
            {
                theConsoleString.Append(c);
            }

            //Once the string is complete Write it to the Console
            theConsole.WriteLine(theConsoleString.ToString());

            theConsole.WriteLine("Wait for 2 seconds...");
            await Task.Delay(2000);

            //Finally, demonstrating getting some data async
            theConsole.WriteLine("Download from https://dotnet.microsoft.com/...");
            theConsoleString.Clear();
            using (var w = new WebClient())
            {
                string str = await w.DownloadStringTaskAsync("https://dotnet.microsoft.com/");
                theConsoleString.Append($"Nr of characters downloaded: {str.Length}");
            }
            theConsole.WriteLine(theConsoleString.ToString());
        }

        //If you have any event handlers, they could be placed here
        void myEventHandler(object sender, string message)
        {
            theConsole.WriteLine($"Event message: {message}"); //theConsole is a Captured Variable, don't use myConsoleString here
        }
            */
            #endregion
        }
    }
}
