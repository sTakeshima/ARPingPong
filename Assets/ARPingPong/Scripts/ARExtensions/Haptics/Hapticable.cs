namespace Ricoh.TeamHaptics.AR.Extensions.Haptics
{
    using UnityEngine;
    public class Hapticable: MonoBehaviour
    {
        public void Awake()
        {
            Debug.Log(gameObject.name + " becoming Haptic object");
        }
    } 
}