using Core.Scripts.Configs;
using Core.Scripts.Data;
using Core.Scripts.HUD;
using Core.Scripts.Managers;
using Core.Scripts.Views.UI;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Installers
{
    public class UiViewInstaller : MonoInstaller
    {
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private Camera _uiCamera;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _pawnPrefab;
        [SerializeField] private GameObject _quadPrefab;
        
        private IPawnData _pawnData;
        private GameConfig _gameConfig;

        
        [Inject]
        public void Construct(IPawnData pawnData, GameConfig gameConfig)
        {
            _pawnData = pawnData;
            _gameConfig = gameConfig;
        }
        
        public override void InstallBindings()
        {
            Container.BindInstance(_uiCamera).AsSingle();
            
            Container.BindInterfacesTo<PawnView>()
                .FromComponentInNewPrefab(_pawnPrefab)
                .UnderTransform(_parentRectTransform)
                .AsSingle();
            
            Container.BindMemoryPool<QuadView, QuadView.Pool>()
                .WithInitialSize(_gameConfig.QuadsMaxCount)
                .FromComponentInNewPrefab(_quadPrefab)
                .UnderTransform(_parentRectTransform);

            Container.BindInterfacesTo<QuadsViewManager>()
                .AsSingle();
            
            Container.Bind<TouchManager>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName(nameof(TouchManager))
                .AsSingle()
                .NonLazy();
        }
    }
}

