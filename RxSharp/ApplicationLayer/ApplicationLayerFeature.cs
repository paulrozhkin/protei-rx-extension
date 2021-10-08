using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using RxSharp.Infrastructure;
using RxSharp.Infrastructure.Extensions;
using RxSharp.Models;

namespace RxSharp.ApplicationLayer
{
    public class ApplicationLayerFeature
    {
        private readonly IHttpClient _client;
        private readonly Subject<Unit> _allDataHandledSubject = new();

        public ApplicationLayerFeature(IHttpClient client)
        {
            _client = client;
        }

        public IObservable<Unit> AllDataHandled => _allDataHandledSubject.AsObservable();

        public void SubscribeForHandleRecentData(IObservable<FeatureData<int>> observable)
        {
            observable
                .Select(x => Observable.FromAsync(() => HandleRecentDataAsync(x)))
                .MergeBounded()
                .TimeInterval()
                .SubscribeConsole("HandleRecentItems");
        }

        private async Task<FeatureData<int>> HandleRecentDataAsync(FeatureData<int> dataItem)
        {
            var response = await _client.SendAsync<int, int>(dataItem).ConfigureAwait(false);
            await Task.Delay(100);
            response.DebugData.Handled = DateTime.Now;

            if (response.DebugData.IsLast)
            {
                _allDataHandledSubject.OnNext(Unit.Default);
                _allDataHandledSubject.OnCompleted();
            }

            return response;
        }
    }
}