using UnityEngine;
using DG.Tweening;
using Mimic;

namespace Mimic.LolFramework{
    public abstract class GeneralTween : MonoBehaviour {


        [SerializeField]
        protected float duration = 1;
        public float Duration { get { return duration; } }
        [SerializeField]
        protected float delay = 0;
        public float Delay { get { return delay; } }
        [SerializeField, ConditionalField(fieldToCheck: "delay", inverse: true, compareValues: new object[] { 0f })]
        protected bool randomizeDelay = false;

        [SerializeField]
        protected Ease easing = Ease.Linear;

        [SerializeField]
        protected bool relative = false;

        [SerializeField]
        protected bool isIndependentUpdate = false;

        [Header("Loop"), SerializeField]
        protected bool loopEnabled = false;
        [SerializeField, ConditionalField("loopEnabled")]
        protected int loopAmount = -1;
        [SerializeField, ConditionalField("loopEnabled")]
        protected LoopType loopType = LoopType.Incremental;

        public delegate void OnCompleteTween();
        public abstract event OnCompleteTween OnCompleteListeners;

        private bool activated = false;
        public bool Activated => activated;

        protected virtual bool ParentSetsLoopsDelayAndEasing {
            get { return true; }
        }

        private Tween mainTween;

        void Start() {
            Activate();
        }

        public void Activate(OnCompleteTween listener) {
            OnCompleteListeners += listener;
            Activate();
        }

        public void Activate() {
            if (activated)
                return;
            activated = true;
            mainTween = StartTween();
            if (ParentSetsLoopsDelayAndEasing) {
                mainTween.SetDelay(delay * (randomizeDelay ? Random.value : 1)).SetEase(easing).SetRelative(relative).SetUpdate(isIndependentUpdate);
                if (loopEnabled)
                    mainTween.SetLoops(loopAmount, loopType);
            }
            mainTween.onKill = () => {
                mainTween = null;
                activated = false;
            };
        }

        protected abstract Tween StartTween();

        public virtual void Stop(bool complete = false) {
            if (mainTween != null && !mainTween.IsComplete()) {
                if (mainTween.Loops() < 0 && complete) {
                    mainTween.Rewind(false);
                    mainTween.Kill(false);
                } else {
                    mainTween.Kill(complete);
                }
                activated = false;
            }
        }

        private void OnDestroy() {
            if (mainTween != null && mainTween.IsActive()) {
                mainTween.Kill(false);
            }
        }
    }
}