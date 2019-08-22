//-----------------------------------------------------------------------
// <copyright file="LocalPlayerController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace Ricoh.TeamHaptics.AR.PingPong
{

    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using Ricoh.TeamHaptics.AR.Extensions.Haptics;

    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.UI;

    /// <summary>
    /// Local player controller. Handles the spawning of the networked Game Objects.
    /// </summary>
#pragma warning disable 618
    public class LocalPlayerController : NetworkBehaviour
#pragma warning restore 618
    {
        /// <summary>
        /// The Star model that will represent networked objects in the scene.
        /// </summary>
        public GameObject StarPrefab;

        /// <summary>
        /// The Anchor model that will represent the anchor in the scene.
        /// </summary>
        public GameObject AnchorPrefab;

        public GameObject StagePrefab;//追加

        public GameObject BallPrefab;//追加
        
        private static volatile int s_LeftPoint = 0;
        private static volatile int s_RightPoint = 0;

        private ArSession m_ArSession;

        /// <summary>
        /// 追加　プレイヤーをカメラの位置に配置
        /// </summary>
        void Awake()
        {
            m_ArSession = GameObject.FindObjectOfType<ArSession>();

            Debug.Log("Awake Player, Camera: " + m_ArSession.ArCamera + m_ArSession.ArCamera.transform.position);
        }

        void Start()
        {
            var collisionController = GetComponent<CollisionController>();
            collisionController.isCollisionHapticable = isLocalPlayer;
            collisionController.isCollisionModifiable = isServer;
            Debug.Log("Start Player(" + netId + "): server?" + isServer + ", local?" + isLocalPlayer);
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                transform.position = m_ArSession.ArCamera.transform.position;
                transform.rotation = m_ArSession.ArCamera.transform.rotation;
            }
        }

        /// <summary>
        /// 追加　ポイントアップを通知して相手側のラケットの点数を書き換える
        /// SyncVarだと値がめちゃくちゃになる(UNETのバグ？)ので、Command/RPCで対応
        /// </summary>
        public void NotifyLeftPointUp()
        {
            if(isServer)
            {
                RpcLeftPointUp();
            }
            else
            {
                CmdLeftPointUp();
            }
        }

        public void NotifyRightPointUp()
        {
            if(isServer)
            {
                RpcRightPointUp();
            }
            else
            {
                CmdRightPointUp();
            }
        }

        private void leftPointUp()
        {
            s_LeftPoint++;
            if (s_LeftPoint > 99)
            {
                s_LeftPoint = 0;
            }
            Debug.Log("rpc left pointup" + s_LeftPoint);
            
            GameObject.FindGameObjectWithTag("LeftPoint").GetComponent<Text>().text = s_LeftPoint.ToString();
        }

        private void rightPointUp()
        {
            s_RightPoint++;
            if (s_RightPoint > 99)
            {
                s_RightPoint = 0;
            }
            Debug.Log("rpc right pointup" + s_RightPoint);
            GameObject.FindGameObjectWithTag("RightPoint").GetComponent<Text>().text = s_RightPoint.ToString();
        }

        /// <summary>
        /// サーバー側で点数アップした場合は、全クライアントに通知
        /// </summary>
        [ClientRpc]
        public void RpcLeftPointUp()
        {
            leftPointUp();   
        }

        [ClientRpc]
        public void RpcRightPointUp()
        {
            rightPointUp();
        }

        /// <summary>
        /// クライアント側で通知された場合は、サーバーに通知
        /// </summary>
        /// <param name="point"></param>
        [Command]
        public void CmdLeftPointUp()
        {
            if (isServer)
            {
                RpcLeftPointUp();
            }
        }

        [Command]
        public void CmdRightPointUp()
        {
            if (isServer)
            {
                RpcRightPointUp();
            }
        }

        /// <summary>
        /// The Unity OnStartLocalPlayer() method.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // A Name is provided to the Game Object so it can be found by other Scripts, since this is instantiated as
            // a prefab in the scene.
            gameObject.name = "LocalPlayer";
            
            var origin = Prefab.Anchor.FindClone();
            if (origin != null)
            {
                transform.parent = origin.transform;
            }

            Debug.Log("Start Local Player:" + netId);
        }

        /// <summary>
        /// Will spawn the origin anchor and host the Cloud Anchor. Must be called by the host.
        /// </summary>
        public void ResetAnchor()
        {
            var anchor = Prefab.Anchor.FindClone(); // GameObject.Find("Anchor(Clone)");//.GetComponent<AnchorController>();
            if (anchor != null) {
                NetworkServer.Destroy(anchor.gameObject);
            }

            /*var xpanchor = GameObject.Find("XPAnchor");//.GetComponent<AnchorController>();
            if (anchor != null)
            {
                Destroy(xpanchor);
            }*/

            var stage = Prefab.Stage.FindClone(); // GameObject.Find("Stage(Clone)");//.GetComponent<>();
            if (stage != null) {
                NetworkServer.Destroy(stage.gameObject);
            }
        }

        /// <summary>
        /// Will spawn the origin anchor and host the Cloud Anchor. Must be called by the host.
        /// </summary>
        /// <param name="position">Position of the object to be instantiated.</param>
        /// <param name="rotation">Rotation of the object to be instantiated.</param>
        /// <param name="anchor">The ARCore Anchor to be hosted.</param>
        public void SpawnAnchor(Vector3 position, Quaternion rotation, Component anchor)
        {
            // Instantiate Anchor model at the hit pose.
            var anchorObject = Instantiate(AnchorPrefab, position, rotation);

            Debug.Log("LocalPlayerController SpawnAnchor set  : " + anchorObject.transform.position);

            // Anchor must be hosted in the device.
            anchorObject.GetComponent<AnchorController>().HostLastPlacedAnchor(anchor);

            // my parent
            transform.parent = anchorObject.transform;

            // Host can spawn directly without using a Command because the server is running in this instance.
#pragma warning disable 618
            NetworkServer.Spawn(anchorObject);
#pragma warning restore 618
        }

        /// <summary>
        /// 追加　ステージ配置用
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
#pragma warning disable 618
        [Command]
#pragma warning restore 618
        public void CmdSpawnStage(Vector3 position, Quaternion rotation)
        {
            GameObject[] stages = GameObject.FindGameObjectsWithTag("Stage");

            if (stages.Length > 0)
            {
                return;
            }

            var stage = (GameObject)Instantiate(StagePrefab, position, Quaternion.identity);
            
            // world origin anchor's child
            var origin = Prefab.Anchor.FindClone();
            stage.transform.parent = origin.transform;

            var controller = stage.GetComponent<CollisionController>();
            if (controller != null)
            {
                controller.isCollisionModifiable = isServer;
                Debug.Log("server only controll stage physics.");
            }
            // Spawn the object in all clients.
#pragma warning disable 618
            NetworkServer.Spawn(stage);
#pragma warning restore 618
        }

        /// <summary>
        /// 追加　ボール配置用
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        [Command]
        public void CmdSpawnBall(Vector3 position, Quaternion rotation, NetworkInstanceId playerId)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Moving object");

            foreach (GameObject b in balls)
            {
                NetworkServer.Destroy(b);
            }

            var ball = Instantiate(BallPrefab, position, rotation);

            LocalPlayerController requestedPlayer = null;
            foreach (var player in FindObjectsOfType<LocalPlayerController>())
            {
                if (player.netId == playerId)
                {
                    requestedPlayer = player;
                }
            }
            var body = ball.GetComponent<Rigidbody>();
            Debug.Log("Ball mass: " + body.mass + " kg");

            var origin = Prefab.Anchor.FindClone();
            if (origin == null)
            {
                Debug.Log("CmdSpawnBall origin is null");
                ball.GetComponent<Rigidbody>().AddForce(m_ArSession.ArCamera.transform.TransformDirection(0, body.mass / 1.0f * 1.5f, body.mass / 1.0f * -3f), ForceMode.Impulse);    
            }
            else
            {
                Debug.Log("CmdSpawnBall origin is NOT null");
                ball.transform.parent = origin.transform;
                body.useGravity = false;
                var heading = transform.position - origin.transform.position;
                var heading2 = heading + transform.forward.normalized * heading.magnitude * 0.1f;
                //ball.transform.position = new Vector3(heading.x * 0.9f, heading.y * 0.9f, heading.z * 0.9f);
                ball.transform.position = heading2;
            }
            // Spawn the object in all clients.
#pragma warning disable 618
            NetworkServer.Spawn(ball);
#pragma warning restore 618
        }
        
        /// <summary>
        /// A command run on the server that will spawn the Star prefab in all clients.
        /// </summary>
        /// <param name="position">Position of the object to be instantiated.</param>
        /// <param name="rotation">Rotation of the object to be instantiated.</param>
        [Command]
        public void CmdSpawnStar(Vector3 position, Quaternion rotation)
        {
            // Instantiate Star model at the hit pose.
            var starObject = Instantiate(StarPrefab, position, rotation);

            // Spawn the object in all clients.
#pragma warning disable 618
            NetworkServer.Spawn(starObject);
#pragma warning restore 618
        }

        /// <summary>
        /// 
        /// </summary>
        [Command]
        public void CmdDestroyBall(GameObject ball)
        {
            NetworkServer.Destroy(ball);
            Debug.Log("Destroy Ball");
        }
    }
}
