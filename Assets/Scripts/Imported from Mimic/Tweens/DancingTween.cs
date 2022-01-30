using DG.Tweening;
using UnityEngine;

namespace Mimic.LolFramework
{
    public class DancingTween : GeneralTween
    {
        [Header("DancingTween")]
        [SerializeField]
        private float loopableDelay = 1;
        [Header("Shake Rotation")]
        [SerializeField]
        private float strength = 90;
        [SerializeField]
        private int vibrato = 10;
        [SerializeField]
        private float randomness = 90;
        [SerializeField]
        private bool fadeOut = true;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOShakeRotation(duration, new Vector3(0, 0, strength), vibrato, randomness, fadeOut).SetDelay(loopableDelay));
            return seq.OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}