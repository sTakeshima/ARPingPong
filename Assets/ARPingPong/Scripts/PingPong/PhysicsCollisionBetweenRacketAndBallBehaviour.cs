namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// 🏓の物理演算用の CollisionBehaviour です
    /// </summary>
    public class PhysicsCollisionBetweenRacketAndBallBehaviour : CollisionActionBehaviour
    {
        private GameObject stage = null;

        /// <summary>
        /// 追加　プレイヤーに当たったオブジェクトがボールだった場合は振動して上方向に力を加える
        /// LocalPlayerController に書かれていた処理を持ってきました。
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override CollisionActionResult OnCollisionAction(CollisionActionResult result)
        {
            Debug.Log("Hit:" + result.Other.gameObject.name); // ぶつかった相手の名前を取得
            //float speedZ = 1.414f; //physics version.1用
            float speedZ = 0.414f;
            float speedY = 1f;
            float speedX = 0f;
            float speedP = 12f;
            float addSpeedP = 0f;
            //float coefficient = 1.0f;

            if (Prefab.Ball.IsClone(result.Other.gameObject)) // if (collision.gameObject.name == "Ball(Clone)")
            {
                var body = result.Other.gameObject.GetComponent<Rigidbody>();
                body.useGravity = true;
                Vector3 Force;
                Vector3 stagePosition;

                if (stage != null)
                {
                    stage = Prefab.Stage.FindClone();
                    stagePosition = stage.transform.position;
                }

                BallRotation br = GetComponent<BallRotation>();  //ラケットの速度を取得
                if (br.RacketSpeed() <= 0.5f)  //ラケットが遅いとき
                {
                    addSpeedP = 0.5f;
                }
                else if (br.RacketSpeed() > 2.0f)  //ラケットが速すぎるとき
                {
                    addSpeedP = 2f;
                }
                else
                {
                    addSpeedP = (br.RacketSpeed() - 0.5f) * 2f / 1.5f;  //ラケットがいい感じの時
                }

                /*if (br.playModeRotation() == 1)
                {
                    speedX = -0.3f;
                }
                else if (br.playModeRotation() == 2)
                {
                    speedX = 0.3f;
                }
                else
                {
                    speedX = 0.0f;
                }*/

                /**physics version.1
                Force = new Vector3(0f, speedY, speedZ).normalized * v0;
                body.velocity = Vector3.zero;
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(Force, ForceMode.Impulse);
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x,0f,0f)*v0, ForceMode.Impulse);
                **/

                if (stage == null)
                {
                    stagePosition = Vector3.zero;
                }
                else if (stage != null)
                {
                    float Dif = transform.position.y - stage.transform.position.y;
                    if (Dif <= 0.2)
                    {
                        speedP = 12f;
                    }
                    else if (Dif > 0.2 && Dif <= 0.4)
                    {
                        speedP = 11f;
                    }
                    else if (Dif > 0.4 && Dif <= 0.6)
                    {
                        speedP = 10f;
                    }
                    else
                    {
                        speedP = 9f;
                    }
                }


                float speed = (speedP + addSpeedP) * 1000f / 3600f;
                float v0 = body.mass * speed;
                //physics version.2
                Force = new Vector3(0f, transform.forward.y + speedY, transform.forward.z + speedZ).normalized * v0;
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(Force, ForceMode.Impulse);
                result.Other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x + speedX, 0f, 0f) * v0, ForceMode.Impulse);
            }
            return result;
        }

    }
}