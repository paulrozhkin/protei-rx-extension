using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RxSharp.ApplicationLayer
{
    public class LongRunningProcessGenerator: IBusinessCapabilityProcess
    {
        private readonly int _amount;
        private readonly int _delayBeforeGenerateMs;

        public LongRunningProcessGenerator(int amount, int delayBeforeGenerateMs)
        {
            _amount = amount;
            _delayBeforeGenerateMs = delayBeforeGenerateMs;
        }
        
        public IObservable<int> BusinessCapabilityItems()
        {
            return Observable.Create<int>((o, ctObservable) =>
            {
                return Task.Run(() =>
                {
                    foreach (var value in Generate(_amount, _delayBeforeGenerateMs))
                    {
                        ctObservable.ThrowIfCancellationRequested();
                        o.OnNext(value);
                    }

                    ctObservable.ThrowIfCancellationRequested();
                    o.OnCompleted();
                }, ctObservable);
            });
        }

        private IEnumerable<int> Generate(int amount, int delayBeforeGenerateMs)
        {
            for (var i = 0; i < amount; i++)
            {
                Thread.Sleep(delayBeforeGenerateMs);
                yield return i;
            }
        }
    }
}