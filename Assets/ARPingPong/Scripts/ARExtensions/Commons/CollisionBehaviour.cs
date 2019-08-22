namespace Ricoh.TeamHaptics.AR.Extensions.Commons
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 衝突時の実際の処理を行うクラスです。
    /// CollisionController により、呼び出しを制御されます。
    /// 複数の CollisionActionBehaviour が呼び出される可能性があり、
    /// 直前の CollisionActionBehaviour の結果を引数として受け取ります。
    /// 次の CollisionActionBehaviour のために、結果を引き継いでください。
    /// </summary>
    public class CollisionActionBehaviour : MonoBehaviour
    {
        internal CollisionActionResult onCollisionAction(CollisionActionResult result)
        {
            return enabled ? OnCollisionAction(result) : result;
        }

        public virtual CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            return result;
        }
    }    
}
