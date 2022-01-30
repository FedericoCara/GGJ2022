using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Mimic.LolFramework
{
    public class RawImageTween : GeneralTween
    {
        [Header("Raw Image Tween"), SerializeField]
        private RawImage rawImage;
        [SerializeField]
        private Vector2 movement;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            Rect targetRect = new Rect(rawImage.uvRect);
            targetRect.center += movement;
            return DOTween.To(() => rawImage.uvRect, (Rect rect) => rawImage.uvRect = rect, targetRect, duration).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}
