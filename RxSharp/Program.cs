using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using RxSharp.ApplicationLayer;

namespace RxSharp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await ApplicationBuilder.RunAsync();
            
            // var source = Observable.Interval(TimeSpan.FromMilliseconds(300))
            //     .Take(5)
            //     .Do(val => Console.WriteLine($"Source: {val}"));
            //
            //
            // source.Select(val => Observable.FromAsync(() => LoadAsync(val)))
            //     .Wait();
        }

        private static async Task LoadAsync(long val)
        {
            Console.WriteLine($"Load started: {val}");
            await Task.Delay(1500);
            Console.WriteLine($"Load completed: {val}");

        }
        
        private static Application ApplicationBuilder => new Application();
    }
}