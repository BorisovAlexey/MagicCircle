using UnityEngine;
using Zenject;

namespace Core.Scripts.Data
{
    public class QuadData : BaseElementData
    {
        public Rect Rect { get; private set; }

        
        public QuadData() : base(default)
        {
        }
        
        public class Pool : MemoryPool<Vector2, Rect, QuadData>
        {
            protected override void Reinitialize(Vector2 screenPosition, Rect rect, QuadData item)
            {
                item.ScreenPosition.Value = screenPosition;
                item.Rect = rect;
            }
        }
    }
}

