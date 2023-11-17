using UnityEngine;

namespace Core.Scripts.Configs
{
    [CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Configs/" + nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        [Header("Pawns")]
        [SerializeField] private float _pawnRadius = 1;
        [Space]
        [SerializeField] private float _smoothDuration = 0.25f;
        [SerializeField] private AnimationCurve _smoothCurve;
        [Header("Quads")]
        [SerializeField] private int _quadsMaxCount = 5;
        [SerializeField] private Vector2 _quadSpawnDelayRange = new (1, 3);
        [SerializeField] private float quadSpawnScreenPadding = 0.2f;
        [SerializeField] private Rect _quadRect;

        public int QuadsMaxCount => _quadsMaxCount;
        public float SmoothDuration => _smoothDuration;
        public AnimationCurve SmoothCurve => _smoothCurve;
        public float PawnRadius => _pawnRadius;
        public Vector2 QuadSpawnDelayRange => _quadSpawnDelayRange;
        public float QuadSpawnScreenPadding => quadSpawnScreenPadding;
        public Rect QuadRect => _quadRect;
    }
}
