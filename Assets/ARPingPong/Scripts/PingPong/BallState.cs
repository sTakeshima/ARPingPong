namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class BallState : SubjectMonoBehaviour
    {

        public bool Available {get; private set;}

        protected override void PostAwake()
        {
            Available = true;
            // notify
            Subjects.NotifyUpdate(Subject.Ball);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!Available)
            {
                return;
            }
            // Debug.Log("#" + gameObject.GetHashCode() + ": pos " + gameObject.transform.position);
            if (gameObject.transform.position.y <= -10.0f)
            {
                Available = false;
                // notify
                Subjects.NotifyUpdate(Subject.Ball);
            }
        }
    }
}