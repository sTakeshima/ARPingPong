namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.PingPong;
    using UnityEngine;

    /// <summary>
    /// Selects the mesh corresponding to the runtime platform by its name. To use Unity's Networking API, the same
    /// prefab will be spawn across all clients. To allow different meshes for different platforms (which might be
    /// useful for light estimation for example), this script selects the corresponding mesh for the runtime platform.
    /// </summary>
    public class StageObserver : SubjectMonoBehaviour, IObserver
    {
        /// <summary>
        /// The Unity Start() method.
        /// Stage の監視を開始し, Start 時点での状態を通知する.
        /// </summary>
        public void Start()
        {
            Subjects.Add(Subject.Stage, this);
            onStageStateChanged(isStageFound);
        }

        public void OnUpdate()
        {
            onStageStateChanged(isStageFound);
            Debug.Log("Stage State: Updated -> " + isStageFound);
        }

        /// <summary>
        /// Stage の状態が変化した時に通知される.
        /// </summary>
        /// <param name="found">Stage が生成された時は true, それ以外は false.</param>
        internal virtual void onStageStateChanged(bool found)
        {

        }

        internal GameObject stage {
            get
            {
                return Prefab.Stage.FindClone();
            }
        }

        /// <summary>
        /// Stage が存在するかどうか.
        /// </summary>
        /// <value>Stage が存在する場合は true, それ以外は false.</value>
        bool isStageFound {
            get
            {
                return stage != null;
            }
        }
    }
}
