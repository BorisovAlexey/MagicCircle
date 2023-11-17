using UniRx;
using UnityEngine;

namespace Core.Scripts.Data
{
    public interface IElementData
    {
        ReactiveProperty<Vector2> ScreenPosition { get; }
    }
}
