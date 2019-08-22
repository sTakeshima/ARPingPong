namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SubjectMonoBehaviour : MonoBehaviour
    {
        protected Subjects Subjects {get; private set;}
        void Awake()
        {
            Subjects = GameObject.FindObjectOfType<Subjects>();
            PostAwake();
        }

        protected virtual void PostAwake()
        {

        }
    }
}
