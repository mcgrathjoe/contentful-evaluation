using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ContentDeliveryApiEvaluation
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("This is to test getting content via the Contentful Content Delivery API.");

      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri("https://preview.contentful.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = client.GetAsync(
          "spaces/zccritvyeajf/entries?content_type=69BaRY3fLGsA0sKo6wAYEQ&access_token=aa0524027dd0df60cc0d87be6bb6f2b3280d7e9e741b4e42ee491ee1d99d38a0").Result;
        if (response.IsSuccessStatusCode)
        {
          var json = response.Content.ReadAsStringAsync().Result;
          Console.WriteLine(json);
        }
      }

      Console.Read();
    }
  }
}
