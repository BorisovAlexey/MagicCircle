using System;
using System.Collections.Generic;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using Core.Scripts.Views.UI;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Managers
{
    public class QuadsViewManager : IInitializable, IDisposable
    {
        private QuadView.Pool _quadsPool;
        private SignalBus _signalBus;
        private readonly Dictionary<QuadData, QuadView> _quadViewConnections = new();


        public QuadsViewManager(QuadView.Pool quadsPool, SignalBus signalBus)
        {
            _quadsPool = quadsPool;
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
            Vector2 screenPosition = quadSpawnSignal.QuadData.ScreenPosition.Value;
            QuadView quadView = _quadsPool.Spawn(screenPosition);

            _quadViewConnections[quadSpawnSignal.QuadData] = quadView;
        }

        private void OnQuadPickuped(QuadPickupSignal quadPickupSignal)
        {
            if (_quadViewConnections.TryGetValue(quadPickupSignal.QuadData, out QuadView quadView))
            {
                _quadsPool.Despawn(quadView);
                _quadViewConnections.Remove(quadPickupSignal.QuadData);
            }
        }
    }
}
