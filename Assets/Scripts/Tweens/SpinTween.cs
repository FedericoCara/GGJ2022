using UnityEngine;
using DG.Tweening;

namespace Mimic.LolFramework
{
    public class SpinTween : GeneralTween
    {
        [Header("Spin Config"), SerializeField]
        private float revolutionsPerSec = 1;

        [SerializeField]
        private bool randomizeSpeed = false;

        [SerializeField]
        private Vector2 randomSpeedRange = new Vector2(0.5f, 1f);

        [SerializeField]
        private bool randomizeDirection = false;

        [SerializeField]
        private bool randomizeSense = false;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            float randomValueForDirection = Random.value;
            return transform.DORotate((randomizeDirection ? (randomValueForDirection < 1 / 3f ? Vector3.forward : randomValueForDirection < 2 / 3f ? Vector3.right : Vector3.up) : Vector3.forward) * 360 * (randomizeSense ? (Random.value >= 0.5f ? 1 : -1) : 1), 1 / revolutionsPerSec * (randomizeSpeed ? Random.Range(randomSpeedRange.x, randomSpeedRange.y) : 1), RotateMode.FastBeyond360).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}