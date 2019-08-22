namespace Ricoh.TeamHaptics.AR.PingPong
{
    using UnityEngine;
    using System.Collections;

    public enum Prefab {
        Ball,
        Stage,
        Anchor,
        Table,
    }

    static public class PrefabHelper
    {

        /// <summary>
        /// 暫定対応、Stageで検索されたものを Table に置き換えます.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        static private string name(this Prefab prefab)
        {
            var name = prefab.ToString();
            if (Prefab.Stage == prefab)
            {
                name = Prefab.Table.ToString();
            }
            return name + "(Clone)";
        }

        static public bool IsClone(this Prefab prefab, GameObject obj)
        {
            return prefab.name() == obj.name;
        }

        static public GameObject FindClone(this Prefab prefab)
        {
            return GameObject.Find(prefab.name());
        } 
    }

}