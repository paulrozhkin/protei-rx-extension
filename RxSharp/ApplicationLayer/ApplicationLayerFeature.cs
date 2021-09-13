using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using RxSharp.Infrastructure;
using RxSharp.Infrastructure.Extensions;

namespace RxSharp.ApplicationLayer
{
    public class ApplicationLayerFeature
    {
        private readonly IHttpClient _client;

        public ApplicationLayerFeature(IHttpClient client)
        {
            _client = client;
        }

        public void HandleRecentItems(IObservable<int> observable)
        {
            observable
                .Select(x => _client.SendAsync<int, int>(x).ToObservable())
                .Concat()
                .TimeInterval()
                .SubscribeConsole("HandleRecentItems");
        }
    }
}