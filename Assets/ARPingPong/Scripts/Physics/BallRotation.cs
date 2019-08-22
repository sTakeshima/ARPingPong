namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BallRotation : MonoBehaviour
    {
        /************summary (suzuki) **************
         * ボールが当たった直後のラケットの速度ベクトルを抽出
         * ラケットの速度ベクトルから(ざっくり)90°傾けたベクトルをボールの回転軸にする
         * ラケットの速度からボールに与える角速度を決定
         * 角速度をトルクに変換してボールのリジットボディにaddforce
        ********************************************/

        public GameObject Ball;
        private Rigidbody rb;  //ボールのRigidbody
        private Rigidbody p_rb;  //プレイヤーのRigidbody
        Vector3[] position;  //ラケットのポジショントラッキング用
        Vector3[] positionB;
        private int index;
        private int maxIndex;  //1秒間にスイングを計測する回数
        private float maxOmega = 1004.8f; //プロプレーヤークラスの角速度
        private float userLevel = 2.0f; //アマチュアレベルに角速度を落とすための係数
        private float time = 0f;
        private int i;
        private bool makeRotation = false;
        private Vector3 playerMoveDis; //ボールをヒットする前後0.5secのラケット移動距離
        private Vector3 playerVel; //ラケットの速度
        private Vector3 rotationSpeed; //ボールに与える角速度
        private Queue<Vector3> que = new Queue<Vector3>() { };
        private Vector3 rotationAxis = Vector3.zero;
        private int k = 0;
        private Vector3 vel;

        // Start is called before the first frame update
        void Start()
        {
            rb = Ball.GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 2000;
            position = new Vector3[10];
            positionB = new Vector3[6];
            index = 0;
            maxIndex = 6;  //計算の都合上、偶数で！！
            i = (maxIndex - 1);
            playerMoveDis = Vector3.zero;
            playerVel = Vector3.zero;
            vel = Vector3.zero;
        }

        // Update is called once per frame
        //プレーヤーの位置は常時監視。一定間隔でqueに入れておく。
        //衝突をトリガーにindexを加算していき、"所定のindexがたまったら" = "所定の時間が経過したら"回転をかける計算をする
        void Update()
        {
            time += Time.deltaTime;

            if (time >= 1.0f / maxIndex)  //一定時間がたったらPlayerのPositionを取得
            {
                que.Enqueue(transform.position);
                if (que.Count > maxIndex)
                {
                    que.Dequeue();
                }
                if (makeRotation)
                {
                    index += 2;
                }
                time = 0f;
            }

            if (index >= maxIndex)
            {
                CalculateRotation();

                //reset
                index = 0;
                i = (maxIndex - 1);
                playerMoveDis = Vector3.zero;
                playerVel = Vector3.zero;
                makeRotation = false;
            }
        }

        /**衝突時の計算のトリガー**/
        void OnCollisionEnter(Collision collision)
        {
            string objName = collision.gameObject.name;
            if (objName == "Ball(Clone)")  //ラケットに衝突したのがボールだったら回転付加を開始
            {
                //makeRotation = true;
                makeRotation = false;
            }
        }

        /**直近0.5secのラケットスピードを計算**/
        public float RacketSpeed()
        {
            int j = 0;
            foreach (Vector3 posB in que)
            {
                positionB[j] = posB;
                j++;
            }

            vel = (positionB[5] - positionB[3]) / (2 * Time.deltaTime);
            return vel.z;
        }

        /**衝突前後0.5secずつのラケットの動きからボールに掛かる回転を計算**/
        public int playModeRotation()
        {
            if (vel.y < -0.1f)
            {
                rotationAxis = new Vector3(0f, 1f, 0f);
                k = 1;
            }
            else if(vel.y > 0.1f)
            {
                rotationAxis = new Vector3(0f, -1f, 0f);
                k = 2;
            }
            else
            {
                rotationAxis = new Vector3(0f, 0f, 0f);
                k = 0;
            }
            var omega = rotationAxis*maxOmega;  //omega = "ボールに与える角速度"
            var R = transform.rotation;  //ここから下で角速度をトルクに変換
            var RI = Quaternion.Inverse(transform.rotation);
            var Id = rb.inertiaTensor;
            var Ir = rb.inertiaTensorRotation;
            var IrI = Quaternion.Inverse(Ir);
            var torque = R * Ir * Vector3.Scale(Id, IrI * RI * omega);
            rb.AddTorque(torque, ForceMode.Impulse);  //ボールにトルクを与えて回転

            Debug.Log(k);
            return k;
        }

        /**衝突前後0.5secずつのラケットの動きからボールに掛かる回転を計算**/
        public void CalculateRotation()
        {
            foreach (Vector3 pos in que)  //計算するためにqueを配列に入れなおす。
            {
                position[i] = pos;
                i--;
            }

            for (int i = 1; i < index; i++)
            {
                playerMoveDis += (position[i] - position[i - 1]);  //ボールヒット前後のラケットの移動距離を算出
            }
            playerVel = playerMoveDis / (1.0f / maxIndex * (index - 1)); //距離を速度に変換
            rb.angularVelocity = Vector3.zero;  //ボールにかかっている回転を一度ストップ
            if (playerVel.magnitude > 0.14f) //ラケットの速度ベクトルが一定以上であればボールに回転をかける
            {
                float rot = playerVel.magnitude * maxOmega / userLevel;
                if (rot > maxOmega)  //ボールにかける角速度が計算上プロレベル(maxOmega)を超える場合は上限張り付け
                {
                    rot = maxOmega;
                }
                Vector2 smpPlayerVec = new Vector2(playerVel.x, playerVel.y); //ボールのトルク軸を決めるためにラケットの速度ベクトルを単純化(2D化)
                rotationSpeed = Quaternion.Euler(0, 0, 90) * (smpPlayerVec.normalized * rot / 4f); //トルク軸をラケット速度ベクトルから90°傾ける
                var omega = rotationSpeed;  //omega = "ボールに与える角速度"
                var R = transform.rotation;  //ここから下で角速度をトルクに変換
                var RI = Quaternion.Inverse(transform.rotation);
                var Id = rb.inertiaTensor;
                var Ir = rb.inertiaTensorRotation;
                var IrI = Quaternion.Inverse(Ir);
                var torque = R * Ir * Vector3.Scale(Id, IrI * RI * omega);
                rb.AddTorque(torque, ForceMode.Impulse);  //ボールにトルクを与えて回転させる
                Debug.Log("Successied" + rot);
            }
            else
            {
                Debug.Log("Need more speed!!");
            }
        }
    }
}