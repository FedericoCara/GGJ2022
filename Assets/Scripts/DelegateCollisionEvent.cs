using UnityEngine;
using System.Collections;

namespace Mimic.Animations{
	public class DelegateCollisionEvent : MonoBehaviour
	{
		public GameObject target;

        private void OnTriggerEnter2D(Collider2D collision) {
            target.SendMessage(nameof(OnTriggerEnter2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            target.SendMessage(nameof(OnTriggerExit2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerStay2D(Collider2D collision) {
            target.SendMessage(nameof(OnTriggerStay2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerEnter(Collider collision) {
            target.SendMessage(nameof(OnTriggerEnter), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerExit(Collider collision) {
            target.SendMessage(nameof(OnTriggerExit), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerStay(Collider collision) {
            target.SendMessage(nameof(OnTriggerStay), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            target.SendMessage(nameof(OnCollisionEnter2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            target.SendMessage(nameof(OnCollisionExit2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionStay2D(Collision2D collision) {
            target.SendMessage(nameof(OnCollisionStay2D), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionEnter(Collision collision) {
            target.SendMessage(nameof(OnCollisionEnter), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionExit(Collision collision) {
            target.SendMessage(nameof(OnCollisionExit), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionStay(Collision collision) {
            target.SendMessage(nameof(OnCollisionStay), collision, SendMessageOptions.DontRequireReceiver);
        }

        void OnAnimationEvent(string methodName){
            if (target != null)
                target.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
            else
                Debug.LogWarning("target is null",gameObject);
		}
	}
}