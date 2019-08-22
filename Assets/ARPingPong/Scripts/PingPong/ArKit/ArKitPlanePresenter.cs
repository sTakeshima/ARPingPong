namespace Ricoh.TeamHaptics.AR.PingPong.ArKit
{
    using Ricoh.TeamHaptics.AR.PingPong;
    using UnityEngine;


    public class ArKitPlanePresenter: StageObserver
    {
        private MeshRenderer m_MeshRenderer;
        
        protected override void PostAwake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            Debug.Log("ARKit Plane Mesh Renderer: " + m_MeshRenderer);
        }

        internal override void onStageStateChanged(bool found)
        {
            var before = m_MeshRenderer.enabled;
            m_MeshRenderer.enabled = !found;
            Debug.Log("ARKit Plane Mesh Renderer state change: " + before + "->" + m_MeshRenderer.enabled);
        }
    }
}