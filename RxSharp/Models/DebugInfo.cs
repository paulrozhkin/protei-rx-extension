using System;

namespace RxSharp.Models
{
    public class DebugInfo
    {
        public DateTime Created { get; set; }

        public DateTime Sent { get; set; }

        public DateTime Received { get; set; }

        public DateTime Handled { get; set; }

        public bool IsLast { get; set; }

        public long CountRequest { get; set; }
    }
}