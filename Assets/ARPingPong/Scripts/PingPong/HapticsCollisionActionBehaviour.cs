namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using Ricoh.TeamHaptics.AR.Extensions.Haptics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Haptics 機能の ON/OFF によって振動を制御する.
    /// </summary>
    public class HapticsCollisionActionBehaviour : SimpleHapticCollisionActionBehaviour
    {
        private HapticConfig m_HapticConfig;
        public void Awake()
        {
            m_HapticConfig = GameObject.FindObjectOfType<HapticConfig>();
            Debug.Log("HapticConfig: " + m_HapticConfig);
        }
        protected override bool isHapticable(CollisionActionResult result)
        {
            var baseResult = base.isHapticable(result);
            var haptic = m_HapticConfig == null || m_HapticConfig.IsHapticEnabled;
            Debug.Log("base: " + baseResult + ", haptic: " + haptic);
            return baseResult && haptic;
        }
    }    
}
