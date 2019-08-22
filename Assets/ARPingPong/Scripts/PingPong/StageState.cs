namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class StageState : SubjectMonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // notify
            Subjects.NotifyUpdate(Subject.Stage);
        }

        void Destory()
        {
            // notify
            Subjects.NotifyUpdate(Subject.Stage);
        }
    }
}