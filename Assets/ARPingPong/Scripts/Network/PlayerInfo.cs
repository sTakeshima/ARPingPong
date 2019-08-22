namespace Ricoh.TeamHaptics.AR.PingPong
{
    using UnityEngine;
    using UnityEngine.Networking;
    
    public static class PlayerType
    {
        public const int Player = 0;
        public const int Spectator = 1;
    }

    // プレイヤー生成時に必要な情報をクライアントからサーバーへ送るための入れ物
    public class PlayerInfoMessage : MessageBase
    {
        public int type;
    }

    // プレイヤー情報クラス。
    // 今何が選択されているかを管理する。
    public class PlayerInfo : MonoBehaviour
    {
        public static int type = PlayerType.Player;
        
        public void OnPlayModeToggleClicked(bool value)
        {
            if (value)
                type = PlayerType.Player;
            else
                type = PlayerType.Spectator;
            Debug.Log("PlayerType"+type);
        }        
    }
}
