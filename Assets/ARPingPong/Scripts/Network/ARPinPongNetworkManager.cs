namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking; // ←NetworkManagerなどのために必要

    public class ARPinPongNetworkManager : NetworkManager
    {
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] GameObject SpectatorPrefab;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {

            Debug.Log("NewOnServerAddPlayer");

            PlayerInfoMessage msg = extraMessageReader.ReadMessage<PlayerInfoMessage>();
            int playerType = msg.type;

            GameObject playerPrefab;

            playerPrefab = (playerType == PlayerType.Player)?PlayerPrefab:SpectatorPrefab;

            GameObject player;
            Transform startPos = GetStartPosition();
            if (startPos != null)
            {
                player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
            }
            else
            {
                player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            }

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            // クライアント上でシーンの準備が完了したらこれを呼ぶ必要がある
            ClientScene.Ready(conn);

            // PlayerInfoMessageオブジェクトを作成し、プレイヤーの情報を格納する
            PlayerInfoMessage msg = new PlayerInfoMessage();
            msg.type = PlayerInfo.type;
            Debug.Log("megType"+ msg.type);
            // サーバーにAddPlayerメッセージを送信する。
            // その際、第3引数に追加情報（PlayerInfoMessage）を付与する。
            ClientScene.AddPlayer(conn, 0, msg);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            print("OnClientConnect");
            //base.OnClientConnect(conn);
            // A custom identifier we want to transmit from client to server on connection

            PlayerInfoMessage msg = new PlayerInfoMessage();
            msg.type = PlayerInfo.type;

            Debug.Log("megType" + msg.type);

            // Call Add player and pass the message
            ClientScene.AddPlayer(conn, 0, msg);
        }
    }
}