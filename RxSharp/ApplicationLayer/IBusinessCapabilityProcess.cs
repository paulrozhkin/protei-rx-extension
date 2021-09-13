using System;

namespace RxSharp.ApplicationLayer
{
    public interface IBusinessCapabilityProcess
    {
        IObservable<int> BusinessCapabilityItems();
    }
}