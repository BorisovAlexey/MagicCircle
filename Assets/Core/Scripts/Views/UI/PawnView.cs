using System;
using Core.Scripts.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Views.UI
{
    public class PawnView : BaseElementView, IInitializable, IDisposable
    {
        private PawnData _pawnData;
        private Camera _camera;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        
        [Inject]
        private void Construct(PawnData pawnData, Camera camera)
        {
            _pawnData = pawnData;
            _camera = camera;
        }

        public void Initialize()
        {
            RectTransform.sizeDelta = _pawnData.Radius * Vector2.one;
            
            IDisposable subscription = _pawnData.ScreenPosition.Subscribe(newScreenPosition =>
            {
                Vector3 worldPosition = _camera.ScreenToWorldPoint(newScreenPosition);
                SetPosition(worldPosition);
            });
            _compositeDisposable.Add(subscription);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
