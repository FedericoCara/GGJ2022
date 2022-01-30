using UnityEngine;
using DG.Tweening;

namespace Mimic.LolFramework
{
    public class RotateTween : GeneralTween
    {
        [Header("Rotate Config"), SerializeField]
        private float degrees = 90;

        [SerializeField]
        private Vector3 rotationVector = Vector3.forward;

        [SerializeField]
        private RotateMode rotateMode = RotateMode.Fast;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            float randomValueForDirection = Random.value;
            return transform.DORotate(rotationVector * degrees, duration, rotateMode).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}