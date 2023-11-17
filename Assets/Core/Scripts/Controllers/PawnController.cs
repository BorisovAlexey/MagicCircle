using System;
using Core.Scripts.Configs;
using Core.Scripts.Data;
using Core.Scripts.Signals;
using DG.Tweening;
using Zenject;
using Vector2 = UnityEngine.Vector2;

namespace Core.Scripts.Controllers
{
    public class PawnController : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private PawnData _pawnData;
        private GameConfig _gameConfig;

        private float _cachedPawnRadiusSqr;
        private Vector2 _startPosition;
        private Tweener _repositionTweener;
        
        
        public PawnController(SignalBus signalBus, PawnData pawnData, GameConfig gameConfig)
        {
            _signalBus = signalBus;
            _pawnData = pawnData;
            _gameConfig = gameConfig;
        }

        public void Initialize()
        {
            _cachedPawnRadiusSqr = _pawnData.Radius * _pawnData.Radius;
            
            _signalBus.Subscribe<ClickSignal>(OnClick);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ClickSignal>(OnClick);
            
            TryKillRepositionTweener();
        }

        private void OnClick(ClickSignal clickSignal)
        {
            TryKillRepositionTweener();

            if (IsTouchContainPawn(clickSignal))
            {
                return;
            }

            _startPosition = _pawnData.ScreenPosition.Value;
            
            _repositionTweener = DOVirtual.Vector3(
                    from: _startPosition, 
                    to: clickSignal.ScreenPosition,
                    duration: _gameConfig.SmoothDuration,
                    (pos) =>
                    {
                        _pawnData.ScreenPosition.Value = pos;
                        RecalculatePawnDistance();
                    }
                )
                .SetEase(_gameConfig.SmoothCurve);
        }

        private void RecalculatePawnDistance()
        {
            _pawnData.Distance.Value += CalculateDistance();

            _startPosition = _pawnData.ScreenPosition.Value;
        }

        private float CalculateDistance()
        {
            return Vector2.Distance(_pawnData.ScreenPosition.Value, _startPosition);
        }

        private bool IsTouchContainPawn(ClickSignal clickSignal)
        {
            if (Vector2.SqrMagnitude(clickSignal.ScreenPosition - _pawnData.ScreenPosition.Value) < _cachedPawnRadiusSqr)
            {
                return true;
            }

            return false;
        }

        private void TryKillRepositionTweener()
        {
            if (_repositionTweener == null)
            {
                return;
            }

            RecalculatePawnDistance();
            
            _repositionTweener.Kill();
        }
    }
}