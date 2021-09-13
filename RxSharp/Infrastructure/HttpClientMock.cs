using System;
using System.Threading;
using System.Threading.Tasks;
using RxSharp.Models;

namespace RxSharp.Infrastructure
{
    public class HttpClientMock : IHttpClient
    {
        private readonly int _delayRequestMs;
        private long _countOfRequests;

        public HttpClientMock(int delayRequestMs)
        {
            _delayRequestMs = delayRequestMs;
        }

        public async Task<FeatureData<TRes>> SendAsync<TReq, TRes>(FeatureData<TReq> content,
            CancellationToken ct = default)
        {
            var requestNumber = Interlocked.Add(ref _countOfRequests, 1);
            content.DebugData.Sent = DateTime.Now;
            await Task.Delay(_delayRequestMs, ct).ConfigureAwait(false);
            var response = GenerateRandomResponse<TReq, TRes>(content.Data);
            content.DebugData.Received = DateTime.Now;
            content.DebugData.CountRequest = requestNumber;
            return new FeatureData<TRes>(response, content.DebugData);
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