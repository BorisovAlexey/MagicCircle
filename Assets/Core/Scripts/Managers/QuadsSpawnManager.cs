using System;
using System.Threading;
using Core.Scripts.Configs;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Managers
{
    public class QuadsSpawnManager : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private QuadData.Pool _quadsPool;
        private GameConfig _gameConfig;
        private QuadsData _quadsData;
        private FieldData _fieldData;
        private IPawnData _pawnData;
        
        private float _cachedPawnRadiusSqr;
        private float _cachedMinDistanceSqr;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new ();
        

        public QuadsSpawnManager(SignalBus signalBus, QuadData.Pool quadsPool, GameConfig gameConfig, QuadsData quadsData,
            FieldData fieldData, IPawnData pawnData)
        {
            _signalBus = signalBus;
            _quadsPool = quadsPool;
            _gameConfig = gameConfig;
            _quadsData = quadsData;
            _fieldData = fieldData;
            _pawnData = pawnData;
        }

        public void Initialize()
        {
            _cachedPawnRadiusSqr = _pawnData.Radius * _pawnData.Radius;
            _cachedMinDistanceSqr = _gameConfig.QuadRect.height * _gameConfig.QuadRect.height;
            
            SpawnProcess();
        }

        public void Dispose()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        private async UniTaskVoid SpawnProcess()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                float secondsCount = UnityEngine.Random.Range(_gameConfig.QuadSpawnDelayRange.x, _gameConfig.QuadSpawnDelayRange.y);
                TimeSpan secondsTimeSpan = TimeSpan.FromSeconds(secondsCount);
                await TrySpawnQuad(secondsTimeSpan);
            }
        }

        private async UniTask TrySpawnQuad(TimeSpan secondsTimeSpan)
        {
            await UniTask.Delay(secondsTimeSpan, cancellationToken: _cancellationTokenSource.Token);

            if (_quadsData.List.Count >= _gameConfig.QuadsMaxCount)
            {
                return;
            }

            int triesCount = 5;
            Vector2 randomScreenPosition = GetRandomScreenPosition();
            bool hasIntersections = false;
            while (!hasIntersections && triesCount > 0)
            {
                hasIntersections = HasIntersections(randomScreenPosition);
                
                if (!hasIntersections)
                {
                    break;
                }
                
                randomScreenPosition = GetRandomScreenPosition();
                triesCount--;
                await UniTask.Yield();
            }

            CreateQuadData(randomScreenPosition);
        }

        private bool HasIntersections(Vector2 screenPosition)
        {
            foreach (QuadData quadData in _quadsData.List)
            {
                if (Vector2.SqrMagnitude(quadData.Rect.center - screenPosition) < _cachedMinDistanceSqr)
                {
                    return true;
                }
            }

            if (Vector2.SqrMagnitude(_pawnData.ScreenPosition.Value - screenPosition) < _cachedPawnRadiusSqr)
            {
                return true;
            }

            return false;
        }

        private void CreateQuadData(Vector2 screenPosition)
        {
            QuadData newQuadData = _quadsPool.Spawn(screenPosition, _gameConfig.QuadRect);
            _signalBus.Fire(new QuadSpawnSignal{ QuadData = newQuadData});
        }

        private Vector2 GetRandomScreenPosition()
        {
            float screenPadding = _gameConfig.QuadSpawnScreenPadding;
            Vector2 randomScreenPosition = new Vector2(
                UnityEngine.Random.Range(screenPadding, _fieldData.Rect.width - screenPadding),
                UnityEngine.Random.Range(screenPadding, _fieldData.Rect.height - screenPadding)
            );

            return randomScreenPosition;
        }
    }
}
