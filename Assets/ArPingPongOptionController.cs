namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ArPingPongOptionController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_OptionPanel;
        private bool m_IsOptionVisible {get; set;} = false;

        public void OnClickOptionButton()
        {
            m_IsOptionVisible = !m_IsOptionVisible;
            m_OptionPanel.SetActive(m_IsOptionVisible);
        }
    }
}