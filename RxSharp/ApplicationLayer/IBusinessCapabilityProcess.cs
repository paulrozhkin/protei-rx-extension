using System;
using RxSharp.Models;

namespace RxSharp.ApplicationLayer
{
    public interface IBusinessCapabilityProcess
    {
        IObservable<FeatureData<int>> BusinessCapabilityItems();
    }
}