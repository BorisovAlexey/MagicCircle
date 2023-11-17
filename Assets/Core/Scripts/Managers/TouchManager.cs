using Core.Scripts.Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Managers
{
    public class TouchManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
           var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ =>
                {
                    _signalBus.Fire(new ClickSignal{ ScreenPosition = Input.mousePosition });
                });
           _compositeDisposable.Add(clickStream);
        }

        private void OnDisable()
        {
            _compositeDisposable.Dispose();
        }
    }
}

