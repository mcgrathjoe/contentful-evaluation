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
        client.BaseAddress = new Uri("https://cdn.contentful.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        var response = client.GetAsync("spaces/cfexampleapi?access_token=b4c0n73n7fu1").Result;
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
