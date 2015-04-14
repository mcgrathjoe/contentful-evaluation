using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Contentful.NET;
using Contentful.NET.DataModels;
using Contentful.NET.Search;
using Contentful.NET.Search.Filters;

namespace ContentDeliveryApiEvaluation
{
  class Program
  {
    private static AppSettingsReader reader = new AppSettingsReader();

    private static readonly string AccessTokenForPreview = (string)reader.GetValue("AccessTokenForPreview", typeof(string));
    private static readonly string SpaceIdForConditionsSpace = (string)reader.GetValue("SpaceIdForConditionsSpace", typeof(string));
    private static readonly string EntryIdForAnArea = (string)reader.GetValue("EntryIdForAnArea", typeof(string));

    static void Main(string[] args)
    {
      Console.WriteLine("This is to test getting content via the Contentful Content Delivery API.");

      using (var httpClient = new HttpClient())
      {
        httpClient.BaseAddress = new Uri("https://preview.contentful.com/");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var uri = string.Format("spaces/{0}/entries/{1}?access_token={2}",
          SpaceIdForConditionsSpace,
          EntryIdForAnArea,
          AccessTokenForPreview);

        var response = httpClient.GetAsync(uri).Result;
        if (response.IsSuccessStatusCode)
        {
          var json = response.Content.ReadAsStringAsync().Result;
          Console.WriteLine(json);
        }
      }

      Console.WriteLine("Now to try ryan-codingintrigue's Contentful.Net package");

      var contenfulClient = new ContentfulClient(AccessTokenForPreview, SpaceIdForConditionsSpace, true);

      var areaSearchResult = contenfulClient.SearchAsync<Entry>(
        new CancellationToken(),
        new[] { new EqualitySearchFilter(BuiltInProperties.SysId, EntryIdForAnArea) }).Result;

      var areaEntry = areaSearchResult.Items.SingleOrDefault();

      if (areaEntry != null)
      {
        Console.WriteLine("Title = " + areaEntry.GetString("Title"));
        Console.WriteLine("Content = " + areaEntry.GetString("Content"));

        var imageGalleryItemEntries = areaEntry.GetType<IEnumerable<Link>>("Images");

        foreach (
          var imageGalleryItemEntry in imageGalleryItemEntries.Select(
            image => areaSearchResult.Includes.Entries.SingleOrDefault(
              e => e.SystemProperties.Id == image.SystemProperties.Id)))
        {
          Console.WriteLine("Image Title = " + imageGalleryItemEntry.GetString("imageTitle"));
          Console.WriteLine("Description = " + imageGalleryItemEntry.GetString("Description"));

          var images = imageGalleryItemEntry.GetType<IEnumerable<Asset>>("Images");

          foreach (
            var imageAsset in images.Select(
              image => areaSearchResult.Includes.Assets.SingleOrDefault(
                a => a.SystemProperties.Id == image.SystemProperties.Id)))
          {
            Console.WriteLine("Url = " + imageAsset.Details.File.Url);
          }

          
        }

        Console.WriteLine("Type = " + areaEntry.GetArray("Type"));
      }

      Console.Read();
    }
  }
}
