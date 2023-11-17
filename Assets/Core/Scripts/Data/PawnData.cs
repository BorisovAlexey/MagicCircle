using UniRx;
using UnityEngine;

namespace Core.Scripts.Data
{
    public class PawnData : BaseElementData
    {
        private const string _pawnScoreKey = "Pawn.Score";
        private const string _pawnDistanceKey = "Pawn.Distance";
        
        private float _radius;
        
        public float Radius => _radius;
        
        public ReactiveProperty<int> Score { get; private set;  }
        public ReactiveProperty<float> Distance { get; private set;  }


        public PawnData(Vector2 screenPosition, float radius) : base(screenPosition)
        {
            _radius = radius;

            SubscribeToScore();
            SubscribeToDistance();
        }

        private void SubscribeToScore()
        {
            int savedScore = PlayerPrefs.GetInt(_pawnScoreKey, 0);
            Score = new ReactiveProperty<int>(savedScore);
            Score.Subscribe(newScoreValue =>
            {
                PlayerPrefs.SetInt(_pawnScoreKey, newScoreValue);
            });
        }

        private void SubscribeToDistance()
        {
            float savedDistance = PlayerPrefs.GetFloat(_pawnDistanceKey, 0);
            Distance = new ReactiveProperty<float>(savedDistance);
            Distance.Subscribe(newDistanceValue =>
            {
                PlayerPrefs.SetFloat(_pawnDistanceKey, newDistanceValue);
            });
        }
    }
}

