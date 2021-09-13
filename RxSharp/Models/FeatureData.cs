using RxSharp.Infrastructure.Extensions;

namespace RxSharp.Models
{
    public class FeatureData<T>
    {
        private FeatureData()
        {
            DebugData = new DebugInfo();
        }

        public FeatureData(T data) : this()
        {
            Data = data;
        }

        public FeatureData(T data, DebugInfo debugData)
        {
            Data = data;
            DebugData = debugData;
        }

        public T Data { get; }

        public DebugInfo DebugData { get; }

        public override string ToString()
        {
            return this.Dump();
        }
    }
}