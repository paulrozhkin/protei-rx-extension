using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using RxSharp.Models;

namespace RxSharp.ApplicationLayer
{
    public class LongRunningProcessGenerator : IBusinessCapabilityProcess
    {
        private readonly int _amount;
        private readonly int _delayBeforeGenerateMs;

        public LongRunningProcessGenerator(int amount, int delayBeforeGenerateMs)
        {
            _amount = amount;
            _delayBeforeGenerateMs = delayBeforeGenerateMs;
        }

        public IObservable<FeatureData<int>> BusinessCapabilityItems()
        {
            return Observable.Create<FeatureData<int>>((o, ctObservable) =>
            {
                return Task.Run(() =>
                {
                    foreach (var data in Generate(_amount, _delayBeforeGenerateMs))
                    {
                        ctObservable.ThrowIfCancellationRequested();
                        var featureData = new FeatureData<int>(data.value)
                        {
                            DebugData =
                            {
                                Created = DateTime.Now,
                                IsLast = data.isLastValue
                            }
                        };

                        o.OnNext(featureData);
                    }

                    ctObservable.ThrowIfCancellationRequested();
                    o.OnCompleted();
                }, ctObservable);
            });
        }

        private IEnumerable<(int value, bool isLastValue)> Generate(int amount, int delayBeforeGenerateMs)
        {
            for (var i = 0; i < amount; i++)
            {
                Thread.Sleep(delayBeforeGenerateMs);
                var isLast = i == amount - 1;
                yield return (i, isLast);
            }
        }
    }
}