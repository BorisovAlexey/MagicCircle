using System;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using Zenject;
using UniRx;
using UnityEngine;

namespace Core.Scripts.Managers
{
    public class QuadsPickupManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private PawnData _pawnData;
        private QuadsData _quadsData;
        private float _cachedPickupDistanceSqr;
        
        private readonly CompositeDisposable _compositeDisposable = new();
        
        
        public QuadsPickupManager(SignalBus signalBus, PawnData pawnData, QuadsData quadsData)
        {
            _signalBus = signalBus;
            _pawnData = pawnData;
            _quadsData = quadsData;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<QuadSpawnSignal>(OnQuadSpawned);

            _cachedPickupDistanceSqr = _pawnData.Radius * _pawnData.Radius;
            
            IDisposable subscription = _pawnData.ScreenPosition.Subscribe(OnScreenPositionChanged);
            _compositeDisposable.Add(subscription);
        }

        private void OnScreenPositionChanged(Vector2 newScreenPosition)
        {
            foreach (QuadData quadData in _quadsData.List)
            {
                float sqrDistance = Vector2.SqrMagnitude(quadData.ScreenPosition.Value - newScreenPosition);
                if (sqrDistance < _cachedPickupDistanceSqr)
                {
                    PickupQuad(quadData);
                }
            }
        }

        private void PickupQuad(QuadData quadData)
        {
            _signalBus.Fire(new QuadPickupSignal{QuadData = quadData});
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<QuadSpawnSignal>(OnQuadSpawned);
            _compositeDisposable.Dispose();
        }

        private void OnQuadSpawned(QuadSpawnSignal quadSpawnSignal)
        {
        }
    }
}
