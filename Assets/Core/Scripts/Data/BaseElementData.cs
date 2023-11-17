using UniRx;
using UnityEngine;

namespace Core.Scripts.Data
{
    public abstract class BaseElementData
    {
        public ReactiveProperty<Vector2> ScreenPosition { get; }

        
        protected BaseElementData(Vector2 screenPosition)
        {
            ScreenPosition = new ReactiveProperty<Vector2>(screenPosition);
        }
    }
}

