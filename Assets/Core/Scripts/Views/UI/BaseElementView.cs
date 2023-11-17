using UnityEngine;

namespace Core.Scripts.Views.UI
{
    public class BaseElementView : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = (RectTransform)transform;
                }
                return _rectTransform;
            }
        }
        
        
        public void SetPosition(Vector2 position)
        {
            RectTransform.position = position;
        }
    }
}
