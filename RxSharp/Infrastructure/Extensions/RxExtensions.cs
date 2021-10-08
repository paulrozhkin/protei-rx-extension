using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RxSharp.Infrastructure.Extensions
{
    public static class RxExtensions
    {
        public static IDisposable SubscribeConsole<T>(
            this IObservable<T> observable,
            string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }

        private class ConsoleObserver<T> : IObserver<T>
        {
            private readonly string _name;

            public ConsoleObserver(string name)
            {
                _name = name;
            }

            public void OnCompleted()
            {
                Console.WriteLine($"{_name} - OnCompleted:");
            }

            public void OnError(Exception error)
            {
                Console.WriteLine($"{_name} - OnError:");
                Console.WriteLine($"\t {error}");
            }

            public void OnNext(T value)
            {
                Console.WriteLine($"{_name} - OnNext ({value})");
            }
        }

        /// <summary>
        /// Concatenates most recent inner observable sequence when previous completes.
        /// Similar to Concat, but it ignores out of date inner observable sequences.
        /// Similar to Exhaust, but it preserves latest inner observable.
        /// </summary>
        public static IObservable<T> ConcatExhaust<T>(this IObservable<IObservable<T>> source)
        {
            return Observable.Defer(() =>
            {
                IObservable<T> latest = default;
                return source
                    .Select(inner =>
                    {
                        latest = inner;
                        return Observable.Defer(() => latest == inner ? inner : Observable.Empty<T>());
                    }).Concat();
            });
        }

        // <summary>
        /// Merges elements from all inner observable sequences into a single observable
        /// sequence, limiting the number of concurrent subscriptions to inner sequences.
        /// The unsubscribed inner sequences are stored in a buffer with the specified
        /// maximum capacity. When the buffer is full, the oldest inner sequence in the
        /// buffer is dropped and ignored in order to make room for the latest inner
        /// sequence.
        /// </summary>
        public static IObservable<T> MergeBounded<T>(
            this IObservable<IObservable<T>> source,
            int maximumConcurrency = 1, int boundedCapacity = 1)
        {
            if (boundedCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(boundedCapacity));

            return Observable.Defer(() =>
            {
                var queue = new Queue<IObservable<T>>(boundedCapacity);
                return source
                    .Select(inner =>
                    {
                        var oldestDropped = false;
                        lock (queue)
                        {
                            if (queue.Count == boundedCapacity)
                            {
                                queue.Dequeue();
                                oldestDropped = true;
                            }

                            queue.Enqueue(inner);
                        }

                        if (oldestDropped) return null;
                        return Observable.Defer(() =>
                        {
                            lock (queue) return queue.Dequeue();
                        });
                    })
                    .Where(inner => inner != null)
                    .Merge(maximumConcurrency);
            });
        }
    }
}