namespace Ricoh.TeamHaptics.AR.Extensions.Commons
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SimpleSoundCollisionActionBehaviour : CollisionActionBehaviour
    {
        [SerializeField]
        private AudioClip m_AudioClip;

        private AudioSource m_AudioSource;

        private bool m_IsPlayable = true;

        public void Start()
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            m_AudioClip = audioClip;
        }

        public override CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            if (isPlay(result))
            {
                m_AudioSource.PlayOneShot(m_AudioClip);
                result.Sound = m_AudioClip;
                Debug.Log("clip[sample: " + m_AudioClip.samples + ", " + m_AudioClip.length + " ms, " + (1.0f * m_AudioClip.samples / m_AudioClip.length / 1000.0f) + "]");
                // StartCoroutine("Restore");
            }
            return result;
        }

        IEnumerator Restore()
        {
            m_IsPlayable = false;
            yield return new WaitForSeconds(1.0f);
            m_IsPlayable = true;
        }

        protected virtual bool isPlay(CollisionActionResult result)
        {
            return m_IsPlayable && result.IsCollisionHapticable && m_AudioClip != null;
        }
    }    
}
