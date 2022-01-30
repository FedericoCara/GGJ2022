using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Mimic.LolFramework
{
    public class FadeTween : GeneralTween
    {

        [Header("Fade Tween"), SerializeField]
        private float target = 0;

        public float Target {
            get => target;
            set => target = value;
        }

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            return GetComponent<Graphic>().DOFade(target, duration).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}