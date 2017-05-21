using System;
using System.Collections;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRCapture {
    /// <summary>
    /// Usage:
    /// Place this script to the scene you want to record video to enable auto
    /// replay/capture functionality.
    /// 
    /// In record mode, if not set recordSeconds > 0, you will need to call
    /// StopRecord() function mannually in proper time, e.g:
    /// // At the end of game.
    /// if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
    ///     VRReplay.Instance.StopRecord();
    /// }
    /// </summary>
    public class VRReplayAuto : MonoBehaviour {
        /// <summary>
        /// Replay mode type.
        /// </summary>
        public enum ModeType {
            /// <summary>
            /// Auto start record game state.
            /// </summary>
            Record,
            /// <summary>
            /// Auto start replay game without capture video.
            /// </summary>
            Replay,
            /// <summary>
            /// Auto start replay game with capture video.
            /// </summary>
            Replay_Capture,
            /// <summary>
            /// Nothing will happen in this mode.
            /// </summary>
            None,
        }
        /// <summary>
        /// Current mode.
        /// </summary>
        public ModeType mode = ModeType.None;
        /// <summary>
        /// Delay seconds and start record or replay.
        /// </summary>
        public int delaySeconds;
        /// <summary>
        /// Record seconds, if set 0, never auto end record session.
        /// </summary>
        public int recordSeconds;
        /// <summary>
        /// The track transforms.
        /// </summary>
        public Transform[] trackTransforms;
        /// <summary>
        /// Specify the replay file name.
        /// </summary>
        public string replayFileName;
        /// <summary>
        /// Check should quit application.
        /// </summary>
        bool shouldQuit;

        private void Start() {
            if (mode == ModeType.None) {
                return;
            }
            else if (mode == ModeType.Record) {
                StartCoroutine(StartRecordAsync());
            }
            else if (mode == ModeType.Replay || mode == ModeType.Replay_Capture) {

                // Avoid other window interrupt.
                Application.runInBackground = true;
                if (mode == ModeType.Replay_Capture) {
                    VRCapture.Instance.RegisterCompleteDelegate(HandleCaptureFinish);
                }
                VRReplay.Instance.RegisterCompleteDelegate(HandleReplayFinish);

                StartCoroutine(StartReplayAsync());
            }
        }

        private void Update() {
            if (shouldQuit) {
                QuitApplication();
            }
        }

        private void OnDisable() {
            if (mode == ModeType.Replay || mode == ModeType.Replay_Capture) {

                if (mode == ModeType.Replay_Capture) {
                    VRCapture.Instance.UnregisterCompleteDelegate(HandleCaptureFinish);
                }
                VRReplay.Instance.UnregisterCompleteDelegate(HandleReplayFinish);
            }
        }

        private IEnumerator StartRecordAsync() {

            yield return new WaitForSeconds(delaySeconds);

            // Set replay file path.
            if (!replayFileName.Equals(null) && !replayFileName.Equals("")) {
                VRReplay.Instance.ReplayFileName = replayFileName;
            }

            // Add object to record its transfrom.
            foreach (Transform trans in trackTransforms) {
                VRReplay.Instance.AddTransform(trans);
            }

            // Start record game state.
            VRReplay.Instance.StartRecord();

            // Auto stop record for seconds.
            if (recordSeconds > 0) {
                yield return new WaitForSeconds(recordSeconds);
                VRReplay.Instance.StopRecord();
                shouldQuit = true;
            }
        }

        private IEnumerator StartReplayAsync() {

            yield return new WaitForSeconds(delaySeconds);

            if (!VRReplay.Instance.RenderingOnServer &&
                    (replayFileName.Equals(null) || replayFileName.Equals(""))) {
                Debug.LogError("VRReplayAuto: did not specify replay file name, quit!");
                shouldQuit = true;
            }

            string[] arguments = Environment.GetCommandLineArgs();
            string prevArgument = null;
            string replayFilePathFromCmd = null;
            string videoFilePathFromCmd = null;
            for (int i = 0; i < arguments.Length; i++) {
                if (prevArgument == "-r" || prevArgument == "-replay") {
                    replayFilePathFromCmd = arguments[i];
                }
                else if (prevArgument == "-v" || prevArgument == "-video") {
                    videoFilePathFromCmd = arguments[i];
                }
                prevArgument = arguments[i];
            }
            string replayFilePath = null;
            if (replayFileName.Equals(null) || replayFileName.Equals("")) {
                replayFilePath = replayFilePathFromCmd;
            }
            else {
                replayFilePath = VRReplayUtils.SaveFolder + replayFileName;
            }
            if (replayFilePath.Equals(null) || replayFilePath.Equals("")) {
                Debug.LogError("VRReplayAuto: did not specify replay file path, quit!");
                shouldQuit = true;
            }
            if (!File.Exists(replayFilePath)) {
                Debug.LogError("VRReplayAuto: cannot find replay file " + replayFilePath + ", quit!");
                shouldQuit = true;
            }
            if (videoFilePathFromCmd != null) {
                // TODO, sanity check
                VRCapture.Instance.CaptureVideos[0].DestinationPath = videoFilePathFromCmd;
            }
            VRReplay.Instance.ReplayFilePath = replayFilePath;
            if (VRReplay.Instance.LoadReplay()) {
                if (mode == ModeType.Replay_Capture &&
                    VRCapture.Instance.StartCapture() != VRCapture.StatusCode.Success) {
                    Debug.LogError("VRReplayAuto: capture start failed, quit!");
                    shouldQuit = true;
                }
                VRReplay.Instance.StartReplay();
            }
        }

        private void HandleReplayFinish() {
            if (mode == ModeType.Replay_Capture) {
                VRCapture.Instance.StopCapture();
            }
            else {
                shouldQuit = true;
            }
        }

        private void HandleCaptureFinish() {
            shouldQuit = true;
        }

        private void QuitApplication() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}