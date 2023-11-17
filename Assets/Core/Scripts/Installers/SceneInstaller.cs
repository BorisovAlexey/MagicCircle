using Core.Scripts.Configs;
using Core.Scripts.Controllers;
using Core.Scripts.Data;
using Core.Scripts.Managers;
using Core.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<QuadSpawnSignal>().RunAsync();
            Container.DeclareSignal<QuadPickupSignal>().RunAsync();
            Container.DeclareSignal<ClickSignal>();
            
            Container.Bind<QuadsData>().AsSingle();
            Container.BindMemoryPool<QuadData, QuadData.Pool>().WithInitialSize(_gameConfig.QuadsMaxCount);
            Rect fieldScreenRect = CalculateScreenRect();
            Container.BindInstance(new FieldData { Rect = fieldScreenRect });

            PawnData pawnData = new PawnData
            (
                screenPosition: 0.5f * fieldScreenRect.size, 
                radius: _gameConfig.PawnRadius
            );
            Container.BindInstance(pawnData).AsSingle();
            Container.BindInterfacesTo<PawnController>().AsSingle();
            
            Container.BindInterfacesTo<QuadsSpawnManager>().AsSingle();
            Container.BindInterfacesTo<QuadsPickupManager>().AsSingle();
            Container.BindInterfacesTo<QuadsDataManager>().AsSingle();
            Container.BindInterfacesTo<PawnScoreManager>().AsSingle();
            
            Container.Bind<GameConfig>().FromScriptableObject(_gameConfig).AsSingle();
        }

        private Rect CalculateScreenRect()
        {
            Rect screenRect = new Rect();
            screenRect.max = new Vector2(Screen.width, Screen.height);

            return screenRect;
        }
    }
}
