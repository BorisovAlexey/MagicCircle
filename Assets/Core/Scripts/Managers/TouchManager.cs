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
                    Vector2 screenPosition = ClampScreenPosition(Input.mousePosition);
                    
                    _signalBus.Fire(new ClickSignal{ ScreenPosition = screenPosition });
                });
           _compositeDisposable.Add(clickStream);
        }

        private Vector2 ClampScreenPosition(Vector2 screenPosition)
        {
            return new Vector2(
                x: Mathf.Clamp(screenPosition.x, 0, Screen.width),
                y: Mathf.Clamp(screenPosition.y, 0, Screen.height)
            );
        }

        private void OnDisable()
        {
            _compositeDisposable.Dispose();
        }
    }
}

