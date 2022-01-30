using UnityEngine;
using DG.Tweening;

namespace Mimic.LolFramework
{
    public class LinearTween : GeneralTween
    {
        [Header("Linear Tween"), SerializeField]
        private Vector3 localMovement;
        public Vector3 LocalMovement { get { return localMovement; } set { localMovement = value; } }

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            return transform.DOLocalMove(localMovement, duration).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}