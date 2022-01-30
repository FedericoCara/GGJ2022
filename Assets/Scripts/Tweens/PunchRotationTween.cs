using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mimic.LolFramework
{
    public class PunchRotationTween : GeneralTween
    {
        [SerializeField]
        private Vector3 punch = Vector3.forward;
        [SerializeField]
        private int vibrato = 10;
        [SerializeField]
        private float elasticity = 1;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            return transform.DOPunchRotation(punch, duration, vibrato, elasticity).
                OnComplete(() => OnCompleteListeners?.Invoke());
        }
    }
}