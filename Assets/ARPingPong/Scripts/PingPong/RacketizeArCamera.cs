namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using Ricoh.TeamHaptics.AR.Extensions.Haptics;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 指定された AR カメラを ラケット化します.
    /// </summary>
    public class RacketizeArCamera : MonoBehaviour
    {
        /// <summary>
        /// 対象の AR カメラ
        /// </summary>
        [SerializeField]
        private GameObject m_ArCamera;
        
        /// <summary>
        /// 🏓の物理マテリアル.
        /// 指定がなければ、Kinematic を有効にします.
        /// </summary>
        [SerializeField]
        private PhysicMaterial m_PhysicMaterial;

        /// <summary>
        /// 打球音を指定してください.
        /// </summary>
        [SerializeField]
        private AudioClip m_HittingSound;

        /// <summary>
        /// ラケットの幅
        /// </summary>
        [SerializeField]
        private float m_Width = 1.0f;

        /// <summary>
        /// ラケットの高さ
        /// </summary>
        [SerializeField]
        private float m_Height = 1.0f;

        /// <summary>
        /// ラケットの厚さ
        /// </summary>
        [SerializeField]
        private float m_Thickness = 0.1f;

        /// <summary>
        /// ラケットの重さ(kg)
        /// </summary>
        [SerializeField]
        private float m_Mass = 0.185f;

        public void Awake()
        {
            var rigidBody = m_ArCamera.AddComponent<Rigidbody>();
            rigidBody.mass = m_Mass;
            rigidBody.useGravity = true;
            rigidBody.isKinematic = true;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            var collider = m_ArCamera.AddComponent<BoxCollider>();
            collider.material = m_PhysicMaterial;
            collider.size = new Vector3(m_Width, m_Height, m_Thickness);

            m_ArCamera.AddComponent<CollisionController>();
            m_ArCamera.AddComponent<PhysicsCollisionBetweenRacketAndBallBehaviour>();
            var soundAction = m_ArCamera.AddComponent<SimpleSoundCollisionActionBehaviour>();
            soundAction.SetAudioClip(m_HittingSound);
            m_ArCamera.AddComponent<SimpleHapticCollisionActionBehaviour>();
        }
    }
}