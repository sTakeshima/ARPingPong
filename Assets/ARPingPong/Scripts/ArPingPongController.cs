//-----------------------------------------------------------------------
// <copyright file="CloudAnchorsExampleController.cs" company="Google">
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
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Networking;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Controller for the AR Ping Pong Game. Handles the AR(ARCore) lifecycle.
    /// </summary>
    public class ArPingPongController : MonoBehaviour
    {
        [Header("UI")]

        /// <summary>
        /// The UI Controller.
        /// </summary>
        public NetworkManagerUIController UIController;

        [Header("Native AR")]
        /// <summary>
        /// Native AR Session wrapper.
        /// 他からの参照が簡単になるように分離します.
        /// </summary>
        public ArSession ArSession;

        /// <summary>
        /// Indicates whether the Origin of the new World Coordinate System, i.e. the Cloud Anchor, was placed.
        /// </summary>
        private bool m_IsOriginPlaced = false;

        /// <summary>
        /// Indicates whether the Anchor was already instantiated.
        /// </summary>
        private bool m_AnchorAlreadyInstantiated = false;

        /// <summary>
        /// Indicates whether the Cloud Anchor finished hosting.
        /// </summary>
        private bool m_AnchorFinishedHosting = false;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The last placed anchor.
        /// </summary>
        private Component m_LastPlacedAnchor = null;

        /// <summary>
        /// The current cloud anchor mode.
        /// </summary>
        private ApplicationMode m_CurrentMode = ApplicationMode.Ready;

        /// <summary>
        /// Enumerates modes the example application can be in.
        /// </summary>
        public enum ApplicationMode
        {
            Ready,
            Hosting,
            Resolving,
        }

        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            // A Name is provided to the Game Object so it can be found by other Scripts instantiated as prefabs in the
            // scene.
            gameObject.name = "ArPingPongController";
            ArSession.SetArActive(false);
            _ResetStatus();
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // If we are neither in hosting nor resolving mode then the update is complete.
            if (m_CurrentMode != ApplicationMode.Hosting && m_CurrentMode != ApplicationMode.Resolving)
            {
                return;
            }

            // If the origin anchor has not been placed yet, then update in resolving mode is complete.
            if (m_CurrentMode == ApplicationMode.Resolving && !m_IsOriginPlaced)
            {
                return;
            }

            // If the player has not touched the screen then the update is complete.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            var lastPlacedAnchor = ArSession.Raycast(touch.position.x, touch.position.y);
            if (lastPlacedAnchor != null)
            {
                m_LastPlacedAnchor = lastPlacedAnchor;
            }

            // If there was an anchor placed, then instantiate the corresponding object.
            if (m_LastPlacedAnchor != null)
            {
                // The first touch on the Hosting mode will instantiate the origin anchor. Any subsequent touch will
                // instantiate a star, both in Hosting and Resolving modes.
                if (_CanPlaceStars())
                {
                    _InstantiateStage();
                }
                else if (!m_IsOriginPlaced && m_CurrentMode == ApplicationMode.Hosting)
                {
                    SetWorldOrigin(m_LastPlacedAnchor.transform);
                    _InstantiateAnchor();
                    OnAnchorInstantiated(true);
                }
            }
        }

        /// <summary>
        /// �ǉ��@Anchor�z�u�����g���C
        /// </summary>
        public void OnEnterRetryButtonClick()
        {
            Debug.Log("Enter Retry");
            if (m_LastPlacedAnchor != null)
            {
                Destroy(m_LastPlacedAnchor.gameObject);
            }
            else { return; }
            m_LastPlacedAnchor = null;
            m_AnchorAlreadyInstantiated = false;
            m_AnchorFinishedHosting = false;
            m_CurrentMode = ApplicationMode.Hosting;
            ResetWorldOrigin();
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>().ResetAnchor();
        }

        /// <summary>
        /// �ǉ��@���]���o�[���͌��_���W�����Z�b�g���邾���Ȃ̂ł����������o
        /// </summary>
        public void ResetWorldOrigin()
        {
            m_IsOriginPlaced = false;
            ArSession.ResetWorldOrigin();
        }

        /// <summary>
        /// �ǉ��@�|�C���g�A�b�v����
        /// </summary>
        public void OnEnterLeftPointUpButtonClick()
        {
            Debug.Log("On Enter Left-Side Point up");           
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>().NotifyLeftPointUp();//�|�C���g�A�b�v�𑊎��ɓ`����
        }

        public void OnEnterRightPointUpButtonClick()
        {
            Debug.Log("On Enter Right-Side Point up");
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>().NotifyRightPointUp();//�|�C���g�A�b�v�𑊎��ɓ`����
        }

        /// <summary>
        /// Sets the apparent world origin so that the Origin of Unity's World Coordinate System coincides with the
        /// Anchor. This function needs to be called once the Cloud Anchor is either hosted or resolved.
        /// </summary>
        /// <param name="anchorTransform">Transform of the Cloud Anchor.</param>
        public void SetWorldOrigin(Transform anchorTransform)
        {
            if (m_IsOriginPlaced)
            {
                Debug.LogWarning("The World Origin can be set only once.");
                return;
            }

            m_IsOriginPlaced = true;

            ArSession.SetWorldOrigin(anchorTransform);
        }

        /// <summary>
        /// Ball Make�{�^���������ꂽ���̏���
        /// </summary>
        public void OnEnterBallMakeClick()
        {
            Vector3 p0 = ArSession.ArCamera.transform.TransformPoint(0, 0.0f, 1.0f);

            var player = GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>();

            player.CmdSpawnBall(p0, Quaternion.identity, player.netId);
            Debug.Log("ball created");
        }

        /// <summary>
        /// Handles user intent to enter a mode where they can place an anchor to host or to exit this mode if
        /// already in it.
        /// </summary>
        public void OnEnterHostingModeClick()
        {
            if (m_CurrentMode == ApplicationMode.Hosting)
            {
                m_CurrentMode = ApplicationMode.Ready;
                _ResetStatus();
                return;
            }

            m_CurrentMode = ApplicationMode.Hosting;
            _SetPlatformActive();
        }

        /// <summary>
        /// Handles a user intent to enter a mode where they can input an anchor to be resolved or exit this mode if
        /// already in it.
        /// </summary>
        public void OnEnterResolvingModeClick()
        {
            if (m_CurrentMode == ApplicationMode.Resolving)
            {
                m_CurrentMode = ApplicationMode.Ready;
                _ResetStatus();
                return;
            }

            m_CurrentMode = ApplicationMode.Resolving;
            _SetPlatformActive();
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was instantiated and the host request was made.
        /// </summary>
        /// <param name="isHost">Indicates whether this player is the host.</param>
        public void OnAnchorInstantiated(bool isHost)
        {
            if (m_AnchorAlreadyInstantiated)
            {
                return;
            }

            m_AnchorAlreadyInstantiated = true;
            UIController.OnAnchorInstantiated(isHost);
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was hosted.
        /// </summary>
        /// <param name="success">If set to <c>true</c> indicates the Cloud Anchor was hosted successfully.</param>
        /// <param name="response">The response string received.</param>
        public void OnAnchorHosted(bool success, string response)
        {
            m_AnchorFinishedHosting = success;
            UIController.OnAnchorHosted(success, response);
        }

        /// <summary>
        /// Callback indicating that the Cloud Anchor was resolved.
        /// </summary>
        /// <param name="success">If set to <c>true</c> indicates the Cloud Anchor was resolved successfully.</param>
        /// <param name="response">The response string received.</param>
        public void OnAnchorResolved(bool success, string response)
        {
            UIController.OnAnchorResolved(success, response);
        }

        /// <summary>
        /// Instantiates the anchor object at the pose of the m_LastPlacedAnchor Anchor. This will host the Cloud
        /// Anchor.
        /// </summary>
        private void _InstantiateAnchor()
        {
            // The anchor will be spawned by the host, so no networking Command is needed.
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                      .SpawnAnchor(Vector3.zero, Quaternion.identity, m_LastPlacedAnchor);
        }

        /// <summary>
        /// Instantiates a star object that will be synchronized over the network to other clients.
        /// </summary>
        private void _InstantiateStar()
        {
            // Star must be spawned in the server so a networking Command is used.
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                      .CmdSpawnStar(m_LastPlacedAnchor.transform.position, m_LastPlacedAnchor.transform.rotation);
        }

        /// <summary>
        /// Instantiates a star object that will be synchronized over the network to other clients.
        /// </summary>
        private void _InstantiateStage()
        {
            // Star must be spawned in the server so a networking Command is used.
            GameObject.Find("LocalPlayer").GetComponent<LocalPlayerController>()
                      .CmdSpawnStage(m_LastPlacedAnchor.transform.position, m_LastPlacedAnchor.transform.rotation);
        }

        /// <summary>
        /// Sets the corresponding platform active.
        /// </summary>
        private void _SetPlatformActive()
        {
            ArSession.SetArActive(true);
        }

        /// <summary>
        /// Indicates whether a star can be placed.
        /// </summary>
        /// <returns><c>true</c>, if stars can be placed, <c>false</c> otherwise.</returns>
        private bool _CanPlaceStars()
        {
            if (m_CurrentMode == ApplicationMode.Resolving)
            {
                return m_IsOriginPlaced;
            }

            if (m_CurrentMode == ApplicationMode.Hosting)
            {
                return m_IsOriginPlaced && m_AnchorFinishedHosting;
            }

            return false;
        }

        /// <summary>
        /// Resets the internal status.
        /// </summary>
        private void _ResetStatus()
        {
            // Reset internal status.
            m_CurrentMode = ApplicationMode.Ready;
            if (m_LastPlacedAnchor != null)
            {
                Destroy(m_LastPlacedAnchor.gameObject);
            }

            m_LastPlacedAnchor = null;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            var sleepTimeout = SleepTimeout.NeverSleep;

#if !UNITY_IOS
            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                sleepTimeout = lostTrackingSleepTimeout;
            }
#endif

            Screen.sleepTimeout = sleepTimeout;

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
