namespace Ricoh.TeamHaptics.AR.Extensions.Haptics
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Runtime.InteropServices;
    public class SimpleHapticFeedback : IHapticFeedback
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void light();
#endif


        public void Rumble(HapticData hapticData)
        {
            rumble(hapticData.pattern, hapticData.amplitudes, hapticData.repeat);
        }
        
        private void rumble(long[] pattern, int[] amplitudes, int repeat)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject> ("currentActivity"))
                {
                    using (var haptic = new AndroidJavaObject("com.ricoh.teamhaptics.ar.extensions.haptics.Haptic", activity))
                    {
                        haptic.Call("rumble", pattern, amplitudes, repeat);
                    }
                }
            }
#if UNITY_IOS && !UNITY_EDITOR
            light();
#endif            
        } 
    }
}