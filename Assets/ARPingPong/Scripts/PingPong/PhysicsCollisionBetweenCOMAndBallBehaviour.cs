namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 🏓の物理演算用の CollisionBehaviour です
    /// </summary>
    public class PhysicsCollisionBetweenCOMAndBallBehaviour : CollisionActionBehaviour
    {
        private GameObject stage = null;

        void Start()
        {
            stage = Prefab.Stage.FindClone();
        }

        /// <summary>
        /// 追加　プレイヤーに当たったオブジェクトがボールだった場合は振動して上方向に力を加える
        /// LocalPlayerController に書かれていた処理を持ってきました。
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            Debug.Log("Hit:" + result.Other.gameObject.name); // ぶつかった相手の名前を取得
            float speedZ = -1.414f;//前方向
            float speedY = 1f;//上方向
            float speed = 12f;

            if (stage == null)
            {
                stage = Prefab.Stage.FindClone();
            }

            if (Prefab.Ball.IsClone(result.Other.gameObject)) // if (collision.gameObject.name == "Ball(Clone)")
            {
                var body = result.Other.gameObject.GetComponent<Rigidbody>();
                Vector3 Force = new Vector3(0, speedY, speedZ).normalized * body.mass * speed * 1000f / 3600f;
                Vector3 Force2 = (stage.transform.position - body.transform.position) * body.mass * speed * 1000f / 3600f;
                body.velocity = Vector3.zero; //ボールの速度をゼロにする
                body.angularVelocity = Vector3.zero;  //ボールにかかっている回転を一度ストップ
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(Force, ForceMode.Impulse);
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Force2.x * 0.5f,0f,0f), ForceMode.Impulse);
            }
            return result;
        }
    }    
}
