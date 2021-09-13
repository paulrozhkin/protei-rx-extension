using System;
using System.Threading.Tasks;

namespace RxSharp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await ApplicationBuilder.RunAsync();
        }
        
        private static Application ApplicationBuilder => new Application();
    }
}