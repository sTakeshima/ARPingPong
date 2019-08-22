namespace Ricoh.TeamHaptics.AR.PingPong
{
    using GoogleARCore;
    using UnityEngine;
    using System;

    public class ArSession : MonoBehaviour
    {
        [Header("ARCore")]
        [SerializeField]
        private GameObject m_ArCoreRoot;

        [SerializeField]
        private ARCoreWorldOriginHelper m_ArCoreHelper;

        [Header("ARKit")]
        [SerializeField]
        private GameObject m_ArKitRoot;

        [SerializeField]
        private Camera m_ArKitCamera;

        private ARKitHelper m_ArKitHelper = new ARKitHelper();

        public GameObject ArRoot
        {
            get
            {
                return select(m_ArCoreRoot, m_ArKitRoot);
            }
        }

        public Camera ArCamera 
        {
            get
            {
                return select(Camera.main, m_ArKitCamera);
            }
        }

        private T select<T>(T arcore, T arkit)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return arkit;
            }
            else
            {
                return arcore;
            }
        }

        public void SetArActive(bool active)
        {
            m_ArCoreRoot.SetActive(false);
            m_ArKitRoot.SetActive(false);

            ArRoot.SetActive(active);
        }

        /// <summary>
        /// Raycast against the location the player touched to search for planes.
        /// 
        /// </summary>
        /// <param name="touchX"></param>
        /// <param name="touchY"></param>
        /// <returns>Return component if the raycast had a hit, otherwise null.</returns>
        public Component Raycast(float touchX, float touchY)
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                TrackableHit hit;
                if (m_ArCoreHelper.Raycast(touchX, touchY,
                        TrackableHitFlags.PlaneWithinPolygon, out hit))
                {
                    return hit.Trackable.CreateAnchor(hit.Pose);
                }
            }
            else
            {
                Pose hitPose;
                if (m_ArKitHelper.RaycastPlane(ArCamera, touchX, touchY, out hitPose))
                {
                    return m_ArKitHelper.CreateAnchor(hitPose);
                }
            }

            return null;
        }

        public void SetWorldOrigin(Transform anchorTransform)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_ArKitHelper.SetWorldOrigin(anchorTransform);
            }
            else
            {
                m_ArCoreHelper.SetWorldOrigin(anchorTransform);
            }
        }

        public void ResetWorldOrigin()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return;
            }
            else
            {
                m_ArCoreHelper.ResetWorldOrigin();
            }
        }

    }
}