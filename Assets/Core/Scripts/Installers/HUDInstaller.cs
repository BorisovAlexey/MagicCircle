using Core.Scripts.HUD;
using Zenject;

namespace Core.Scripts.Installers
{
    public class HUDInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ScoreCounter>().FromComponentInChildren().AsSingle();
            Container.Bind<DistanceCounter>().FromComponentInChildren().AsSingle();
        }
    }
}

