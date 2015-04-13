using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Contentful.NET;
using Contentful.NET.DataModels;

namespace ContentDeliveryApiEvaluation
{
  class Program
  {
    private const string AccessTokenForPreview = "aa0524027dd0df60cc0d87be6bb6f2b3280d7e9e741b4e42ee491ee1d99d38a0";
    private const string SpaceIdForConditionsSpace = "zccritvyeajf";

    static void Main(string[] args)
    {
      Console.WriteLine("This is to test getting content via the Contentful Content Delivery API.");

      using (var httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri("https://preview.contentful.com/");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var uri = string.Format("spaces/{0}/entries/q6GJPH3JBuym6iWUq6yAo?access_token={1}",
          SpaceIdForConditionsSpace,
          AccessTokenForPreview);

        var response = httpClient.GetAsync(uri).Result;
        if (response.IsSuccessStatusCode)
        {
          var json = response.Content.ReadAsStringAsync().Result;
          Console.WriteLine(json);
        }
      }

      Console.Read();

      Console.WriteLine("Now to try ryan-codingintrigue's Contentful.Net package");

      var contenfulClient = new ContentfulClient(AccessTokenForPreview, SpaceIdForConditionsSpace, true);

      var areaEntry = contenfulClient.GetAsync<Entry>(new CancellationToken(), "q6GJPH3JBuym6iWUq6yAo").Result;

      Console.WriteLine("Title = " + areaEntry.GetString("Title"));
      Console.WriteLine("Content = " + areaEntry.GetString("Content"));

      Console.Read();
    }
  }
}
