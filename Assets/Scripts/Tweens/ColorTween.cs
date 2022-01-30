using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Mimic.LolFramework
{
    public class ColorTween : GeneralTween
    {

        [Header("Color Tween"), SerializeField]
        private Color color = Color.white;
        public Color Color {
            get => color;
            set => color = value;
        }

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            return GetComponent<Graphic>().DOColor(color, duration).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}