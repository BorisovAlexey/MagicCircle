using UniRx;

namespace Core.Scripts.Data
{
    public interface IPawnData : IElementData
    {
        float Radius { get; }
        ReactiveProperty<int> Score { get; }
        ReactiveProperty<float> Distance { get; }
    }
}
