namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using Ricoh.TeamHaptics.AR.Extensions.Haptics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Haptics 機能のON/OFFに合わせて音を制御します
    /// </summary>
    public class SoundHapticsCollisionActionBehaviour : SimpleSoundCollisionActionBehaviour
    {
        private HapticConfig m_HapticConfig;
        public void Awake()
        {
            m_HapticConfig = GameObject.FindObjectOfType<HapticConfig>();
            Debug.Log("HapticConfig: " + m_HapticConfig);
        }
        protected override bool isPlay(CollisionActionResult result)
        {
            var baseResult = base.isPlay(result);
            var haptic = m_HapticConfig == null || m_HapticConfig.IsHapticEnabled;
            Debug.Log("base: " + baseResult + ", haptic: " + haptic);
            return baseResult && haptic;
        }
    }    
}