namespace Ricoh.TeamHaptics.AR.Extensions.Commons
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 衝突時の処理を制御します。
    /// そのため、RigidBody や Collider がアタッチされた GameObject と一緒にアタッチしてください。
    /// OnCollisionEnter を受け取れるようにしておいてください。
    /// </summary>
    public class CollisionController : MonoBehaviour
    {
        /// <summary>
        /// Collision を変更できるかどうか.
        /// </summary>
        /// <value></value>
        public bool isCollisionModifiable = false;

        public bool isCollisionHapticable = false;

        void OnCollisionEnter(Collision other)
        {
            if (disabled)
            {
                return;
            }
            var result = new CollisionActionResult(gameObject, other, isCollisionModifiable, isCollisionHapticable);
            foreach (var action in GetComponents<CollisionActionBehaviour>())
            {
                result = action.onCollisionAction(result);
            }
            StartCoroutine("wait");
        }

        private bool disabled = false;
        IEnumerator wait()
        {
            disabled = true;
            yield return new WaitForSeconds(1.0f);
            disabled = false;
        }
    }   
}