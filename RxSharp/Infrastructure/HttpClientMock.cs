using System;
using System.Threading;
using System.Threading.Tasks;

namespace RxSharp.Infrastructure
{
    public class HttpClientMock : IHttpClient
    {
        private readonly int _delayRequestMs;

        public HttpClientMock(int delayRequestMs)
        {
            _delayRequestMs = delayRequestMs;
        }

        public async Task<TRes> SendAsync<TReq, TRes>(TReq content, CancellationToken ct = default)
        {
            Console.WriteLine($"Start delay {content}");
            await Task.Delay(_delayRequestMs, ct).ConfigureAwait(false);
            Console.WriteLine($"End delay {content}");
            return GenerateRandomResponse<TReq, TRes>(content);
        }

        private TRes GenerateRandomResponse<TReq, TRes>(TReq content)
        {
            if (typeof(TRes) == typeof(TReq))
            {
                dynamic response = content;
                return response;
            }
            
            return default;
        }
    }
}