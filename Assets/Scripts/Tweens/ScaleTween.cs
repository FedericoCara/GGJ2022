using DG.Tweening;
using UnityEngine;

namespace Mimic.LolFramework
{
    public class ScaleTween : GeneralTween
    {

        [Header("Scale Tween")]
        [SerializeField]
        private bool setStartingValue = false;

        [SerializeField, ConditionalField("setStartingValue")]
        private Vector3 startingScale;
        public Vector3 StartingScale { get { return startingScale; } set { startingScale = value; } }

        [SerializeField]
        private Vector3 scale;

        public Vector3 Scale { get { return scale; } set { scale = value; } }

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            if (setStartingValue) {
                transform.localScale = StartingScale;
            }
            return transform.DOScale(Scale, duration).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}