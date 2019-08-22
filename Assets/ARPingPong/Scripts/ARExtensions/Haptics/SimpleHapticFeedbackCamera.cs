namespace Ricoh.TeamHaptics.AR.Extensions.Haptics
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    public class SimpleHapticFeedbackCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Camera;
        [SerializeField]
        private PhysicMaterial m_PhysicMaterial;

        public void Start()
        {
            Debug.Log("Start " + this);
            var rigidBody = m_Camera.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            // setNoKinematic(rigidBody);
            setKinematic(rigidBody);
            var collider = m_Camera.AddComponent<BoxCollider>();
            collider.material = m_PhysicMaterial;
            collider.size = new Vector3(1.0f, 1.0f, 0.1f);
            // m_Camera.AddComponent<SimpleHapticFeedback>();
        }

        /// <summary>
        /// 物理演算を利用しない場合はこっち。
        /// </summary>
        private void setKinematic(Rigidbody rigidBody)
        {
            rigidBody.isKinematic = true;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }


        /// <summary>
        /// 物理演算を利用する場合は、こちら。
        /// </summary>
        private void setNoKinematic(Rigidbody rigidBody)
        {
            rigidBody.isKinematic = false;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }
}