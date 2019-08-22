namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundCollisionWithBallBehaviour: SimpleSoundCollisionActionBehaviour
    {
        
        protected override bool isPlay(CollisionActionResult result)
        {
            return base.isPlay(result) && Prefab.Ball.IsClone(result.Other.gameObject);
        }
    }    
}
