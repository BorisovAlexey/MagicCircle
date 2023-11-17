using System;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using Zenject;

namespace Core.Scripts.Managers
{
    public class QuadsDataManager : IInitializable, IDisposable
    {
        private QuadsData _quadsData;
        private SignalBus _signalBus;

        public QuadsDataManager(QuadsData quadsData, SignalBus signalBus)
        {
            _quadsData = quadsData;
            _signalBus = signalBus;
        }
    
        public void Initialize()
        {
            _signalBus.Subscribe<QuadSpawnSignal>(OnQuadSpawned);
            _signalBus.Subscribe<QuadPickupSignal>(OnQuadPickuped);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<QuadSpawnSignal>(OnQuadSpawned);
            _signalBus.Unsubscribe<QuadPickupSignal>(OnQuadPickuped);
        }

        private void OnQuadSpawned(QuadSpawnSignal quadSpawnSignal)
        {
            _quadsData.List.Add(quadSpawnSignal.QuadData);
        }

        private void OnQuadPickuped(QuadPickupSignal quadPickupSignal)
        {
            _quadsData.List.Remove(quadPickupSignal.QuadData);
        }
    }
}
