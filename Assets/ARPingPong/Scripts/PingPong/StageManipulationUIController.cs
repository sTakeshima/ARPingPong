namespace Ricoh.TeamHaptics.AR.PingPong
{
    using UnityEngine;
    using UnityEngine.UI;


    public class StageManipulationUIController: StageObserver
    {
        private const float DELTA_POSITION = 0.05f;
        private const float DELTA_SCALE = 0.05f;
        private const float DELTA_DEGREE = 10.0f;

        [SerializeField]
        private GameObject m_ManipulationToggle;
        [SerializeField]
        private GameObject m_ManipulationPanel;
        
        protected override void PostAwake()
        {
            log("awake!");
        }

        internal override void onStageStateChanged(bool found)
        {
            m_ManipulationToggle.SetActive(found);
            log("stage found: " + found);
        }

        public void OnManipulationToggleValueChanged(bool change)
        {
            m_ManipulationPanel.SetActive(change);
        }

        private void log(string message)
        {
            Debug.Log("Stage Manipulation UI: " + message);
        }

        private void addPositionY(float y)
        {
            var position = stage.transform.localPosition;
            position.y += y;
            stage.transform.localPosition = position;
        }
        public void OnClickYPosPlus()
        {
            addPositionY(DELTA_POSITION);
            log("Y-Pos + " + DELTA_POSITION);
        }

        public void OnClickYPosMinus()
        {
            addPositionY(-DELTA_POSITION);
            log("Y-Pos - " + DELTA_POSITION);
        }

        private void addScale(float scale)
        {
            var localScale = stage.transform.localScale;
            stage.transform.localScale = localScale + new Vector3(scale, scale, scale);
        }
        public void OnClickScalePlus()
        {
            addScale(DELTA_SCALE);
            log("Scale + " + DELTA_SCALE);
        }

        public void OnClickScaleMinus()
        {
            addScale(-DELTA_SCALE);
            log("Scale - " + DELTA_SCALE);
        }

        private void addDegree(float degree)
        {
            stage.transform.Rotate(0.0f, degree, 0.0f);
        }
        public void OnClickRotationPlus()
        {
            addDegree(DELTA_DEGREE);
            log("Rotation + " + DELTA_DEGREE);
        }

        public void OnClickRotationMinus()
        {
            addDegree(-DELTA_DEGREE);
            log("Rotation - " + DELTA_DEGREE);
        }
    }
}