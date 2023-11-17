using System;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using Zenject;

namespace Core.Scripts.Managers
{
    public class PawnScoreManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private IPawnData _pawnData;


        public PawnScoreManager(SignalBus signalBus, IPawnData pawnData)
        {
            _signalBus = signalBus;
            _pawnData = pawnData;
        }
    
        public void Initialize()
        {
            _signalBus.Subscribe<QuadPickupSignal>(OnQuadPickuped);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<QuadPickupSignal>(OnQuadPickuped);
        }

        private void OnQuadPickuped(QuadPickupSignal quadPickupSignal)
        {
            _pawnData.Score.Value++;
        }
    }
}
