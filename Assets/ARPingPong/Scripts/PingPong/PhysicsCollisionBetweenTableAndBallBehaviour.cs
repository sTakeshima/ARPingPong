namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 🏓の台とボールの物理演算用の CollisionBehaviour です
    /// </summary>
    public class PhysicsCollisionBetweenTableAndBallBehaviour : CollisionActionBehaviour
    {
        /// <summary>
        /// テーブルはServer管理していないので独自の物理計算はせずにPhysics Materialで対応する方向で・・・テーブルは常時isKinematicオンでOK
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            if (Prefab.Ball.IsClone(result.Other.gameObject))
            {
                //Debug.Log("A ball hit the table tennis table.");
                /*var body = result.Other.gameObject.GetComponent<Rigidbody>();
                Debug.Log("T_Before"+body.velocity);
                if (Mathf.Abs(body.velocity.y) <= 0.5f)
                {
                    result.Other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, Mathf.Abs(body.velocity.z) * body.mass * 1f / 1.414f, 0f), ForceMode.Impulse);
                    Debug.Log("modeA");
                }
                else
                {
                    result.Other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, Mathf.Abs(body.velocity.y) * body.mass * 0.8f, 0f), ForceMode.Impulse);
                    Debug.Log("modeB");
                }*/
            }
            return result;
        }
    }    
}
