using System;
using System.Threading.Tasks;
using RxSharp.ApplicationLayer;
using RxSharp.Infrastructure;

namespace RxSharp
{
    public class Application
    {
        public Task RunAsync()
        {
            var client = new HttpClientMock(5000);

            // Suppose we have some kind of business process that is constantly receiving new data.
            const int amountOfData = 50;
            const int delayBeforeDataCreatedMs = 300;
            IBusinessCapabilityProcess
                process = new LongRunningProcessGenerator(amountOfData, delayBeforeDataCreatedMs);
            var dataObservable = process.BusinessCapabilityItems();

            // Suppose we have some kind of business function that only processes the most recent data.
            var feature = new ApplicationLayerFeature(client);
            feature.HandleRecentItems(dataObservable);

            // Subscribe to the completion of data processing
            var cts = new TaskCompletionSource();
            dataObservable.Subscribe(_ => { }, (error) => { cts.SetException(error); }, () => { cts.SetResult(); });

            return cts.Task;
        }
    }
}