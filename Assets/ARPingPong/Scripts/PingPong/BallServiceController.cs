namespace Ricoh.TeamHaptics.AR.PingPong
{
    using UnityEngine;
    using UnityEngine.UI;
    public class BallServiceController: SubjectMonoBehaviour, IObserver
    {
        void Start()
        {
            Subjects.Add(Subject.Ball, this);
        }

        public void OnClick()
        {
            setInteractable(false);
        }

        private Button button {
            get
            {
                return gameObject.GetComponent<Button>();
            }
        }

        private void setInteractable(bool interactable)
        {
            button.interactable = interactable;
            var text = button.GetComponentInChildren<Text>();
            var color = text.color;
            text.color = new Color(color.r, color.g, color.b, interactable ? 1.0f: 0.0f);
        }

        /// <summary>
        /// Ball Subject Notification
        /// </summary>
        public void OnUpdate()
        {
            var ball = Prefab.Ball.FindClone();
            if (ball == null)
            {
                return;
            }
            var available = ball.GetComponent<BallState>().Available;
            Debug.Log("Notified. Ball: " + (available ? "Available" : "Not Available"));
            // Ball が有効の間は非表示、無効になったら表示する
            setInteractable(!available);

        }
    }
}