namespace Ricoh.TeamHaptics.AR.Extensions.Commons
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CollisionActionResult
    {
        public bool IsCollisionModifiable {get; private set;}

        public bool IsCollisionHapticable {get; private set;}
        public GameObject Me {get; private set;}
        public Collision Other {get; private set;}

        public AudioClip Sound {get; set;}

        public CollisionActionResult(GameObject me, Collision other, bool isModifiable, bool isHapticable)
        {
            Me = me;
            Other = other;
            IsCollisionModifiable = isModifiable;
            IsCollisionHapticable = isHapticable;
        }
    }    
}
