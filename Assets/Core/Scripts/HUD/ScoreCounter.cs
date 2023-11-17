using System;
using Core.Scripts.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Scripts.HUD
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreCounterText;
        
        private PawnData _pawnData;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        
        [Inject]
        public void Construct(PawnData pawnData)
        {
            _pawnData = pawnData;
        }

        public void Start()
        {
            _scoreCounterText.text = GetScoreString(_pawnData.Score.Value);
            
            IDisposable subscription = _pawnData.Score.Subscribe(OnScoreChanged);
            _compositeDisposable.Add(subscription);
        }

        public void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnScoreChanged(int score)
        {
            _scoreCounterText.text = GetScoreString(score);
        }

        private string GetScoreString(int score)
        {
            return $"S: {score.ToString()}";
        }
    }
}

