using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RxSharp.ApplicationLayer;
using RxSharp.Infrastructure;

namespace RxSharp
{
    public class Application
    {
        public Task RunAsync()
        {
            const int delayClientMs = 2000;
            var client = new HttpClientMock(delayClientMs);

            // Suppose we have some kind of business process that is constantly receiving new data.
            const int amountOfData = 5;
            const int delayBeforeDataCreatedMs = 250;
            IBusinessCapabilityProcess
                process = new LongRunningProcessGenerator(amountOfData, delayBeforeDataCreatedMs);
            var dataObservable = process.BusinessCapabilityItems();

            // Suppose we have some kind of business function that only processes the most recent data.
            var feature = new ApplicationLayerFeature(client);
            feature.SubscribeForHandleRecentData(dataObservable);

            // Subscribe to the completion of data processing
            var cts = new TaskCompletionSource();
            dataObservable.CombineLatest(feature.AllDataHandled)
                .Subscribe(next => { }, (error) => { cts.SetException(error); }, async () =>
                {
                    await Task.Delay(100).ConfigureAwait(false);
                    cts.SetResult();
                });

            return cts.Task;
        }
    }
}