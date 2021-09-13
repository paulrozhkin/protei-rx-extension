using System.Threading;
using System.Threading.Tasks;
using RxSharp.Models;

namespace RxSharp.Infrastructure
{
    public interface IHttpClient
    {
        Task<FeatureData<TRes>> SendAsync<TReq, TRes>(FeatureData<TReq> content, CancellationToken ct = default);
    }
}