namespace Ricoh.TeamHaptics.AR.PingPong
{
    using Ricoh.TeamHaptics.AR.Extensions.Commons;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ComController : MonoBehaviour
    {
        private bool IsWall = true;
        private GameObject ball = null;
        private GameObject stage = null;

        // Start is called before the first frame update
        void Start()
        {
            stage = Prefab.Stage.FindClone();
            ball = Prefab.Ball.FindClone();
        }
        

        // Update is called once per frame
        void Update()
        {
            if (stage == null)
            {
                stage = Prefab.Stage.FindClone();
            }
            if (ball == null)
            {
                ball = Prefab.Ball.FindClone();
            }

            if (stage && ball && IsWall)
            {
                Vector3 p = stage.transform.TransformDirection(0f, 2f, 1.35f);
                //transform.position = new Vector3(ball.transform.position.x,ball.transform.position.y,p.z);
                transform.position = p;
                IsWall = false;
            }
        }
    }
}
