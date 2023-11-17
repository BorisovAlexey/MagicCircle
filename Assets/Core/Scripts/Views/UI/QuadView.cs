using Core.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Views.UI
{
    public class QuadView : BaseElementView
    {
        private Camera _camera;
        private GameConfig _gameConfig;
        
        
        [Inject]
        private void Construct(Camera camera, GameConfig gameConfig)
        {
            _camera = camera;
            _gameConfig = gameConfig;
        }

        private void SetScreenPosition(Vector2 screenPosition)
        {
            Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            SetPosition(worldPosition);
        }

        private void SetQuadSize()
        {
            RectTransform.sizeDelta = _gameConfig.QuadRect.size;
        }
        
        
        public class Pool : MonoMemoryPool<Vector2, QuadView>
        {
            protected override void Reinitialize(Vector2 screenPosition, QuadView quadView)
            {
                quadView.SetScreenPosition(screenPosition);
                quadView.SetQuadSize();
            }
        }
    }
}
