namespace Ricoh.TeamHaptics.AR.Extensions.Haptics
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class SimpleHapticCollisionActionBehaviour : CollisionActionBehaviour
    {
        private IHapticFeedback m_HapticFeedback = new SimpleHapticFeedback();
        private bool m_IsHapticable = true;
        public override CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            if (isHapticable(result))
            {
                Debug.Log("Feedback Haptic: " + result.Other.gameObject.name);
                // Rumble(500); // 500 ms
                // pattern: off, on, off, ...(ms), repeat: -1 is oneshot. 
                m_HapticFeedback.Rumble(HapticData.CreateFrom(result.Sound));
                // StartCoroutine("Restore");
            }
            return result;
        }

        IEnumerator Restore()
        {
            m_IsHapticable = false;
            yield return new WaitForSeconds(1.0f);
            m_IsHapticable = true;
        }

        protected virtual bool isHapticable(CollisionActionResult result)
        {
            return m_IsHapticable && result.IsCollisionHapticable && result.Other.gameObject.GetComponent<Hapticable>() != null;
        }
    }
}