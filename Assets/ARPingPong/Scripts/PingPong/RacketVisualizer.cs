namespace Ricoh.TeamHaptics.AR.PingPong
{
    using UnityEngine;
    public class RacketVisualizer: MonoBehaviour
    {
        public void OnDebugVisualChanged(bool changed)
        {
            var rackets = GameObject.FindGameObjectsWithTag("DebugRacket");
            Debug.Log("debug racket found: " + rackets);
            if (rackets == null)
            {
                return;
            }
            foreach (var racket in rackets)
            {
                if (racket != null)
                {
                    racket.GetComponent<MeshRenderer>().enabled = changed;
                }      
            }
        }
    }
}