using System.Threading;
using System.Threading.Tasks;

namespace RxSharp.Infrastructure
{
    public interface IHttpClient
    {
        Task<TRes> SendAsync<TReq, TRes>(TReq content, CancellationToken ct = default);
    }
}