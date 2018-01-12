using System;
using System.Net.Http;
using Refit;
using Newtonsoft.Json;
using Akavache;

namespace Heartstone
{
    public class HeartstoneRestService : IHeartstoneApi
    {

        readonly IHeartstoneApi restService;

        static HeartstoneRestService instance;
        static public HeartstoneRestService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HeartstoneRestService();
                }

                return instance;
            }
        }

        public HeartstoneRestService()
        {
            var client = new HttpClient() //new HeartstoneHttpClientHandler()
            {
                BaseAddress = new Uri(App.ApiURL)
            };
            client.DefaultRequestHeaders.Add("X-Mashape-key", App.ApiKey);
            restService = RestService.For<IHeartstoneApi>(client, new RefitSettings()
            {
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            });

        }

        public IObservable<string> GetCards()
        {
            throw new NotImplementedException();
        }

        public IObservable<CardList> GetCardSet(string set)
        {
            //return restService.GetCardSet(set);

            return BlobCache.UserAccount.GetAndFetchLatest($"card_set_${set}",
            () => restService.GetCardSet(set),
            (DateTimeOffset arg) => (DateTimeOffset.Now - arg).Minutes >= 60);
        }
    }
}
