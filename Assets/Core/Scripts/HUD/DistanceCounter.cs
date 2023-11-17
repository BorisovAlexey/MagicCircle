using System;
using Core.Scripts.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Scripts.HUD
{
    public class DistanceCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _distanceCounterText;
        
        private PawnData _pawnData;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        
        [Inject]
        public void Construct(PawnData pawnData)
        {
            _pawnData = pawnData;
        }

        public void Start()
        {
            _distanceCounterText.text = GetDistanceString(_pawnData.Distance.Value);
            
            IDisposable subscription = _pawnData.Distance.Subscribe(OnDistanceChanged);
            _compositeDisposable.Add(subscription);
        }

        public void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnDistanceChanged(float distance)
        {
            _distanceCounterText.text = GetDistanceString(distance);
        }

        private string GetDistanceString(float distance)
        {
            return $"D: {distance.ToString("F3")}";
        }
    }
}

