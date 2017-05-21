using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace VRCapture {
    /// <summary>
    /// VRReplay is a replay system for Unity, its can replay game object's
    /// postion and rotation and device's input. It will create a replay file,
    /// and that file can be used for game playback.
    /// 
    /// Its useful for creating 360 gameplay video; Save a replay file will
    /// not hit performance issue, and generate 360 video based on the replay.
    /// </summary>
    public class VRReplay : MonoBehaviour {
        /// <summary>
        /// VRReplay instance reference.
        /// </summary>
        public static VRReplay Instance { get; private set; }
        /// <summary>
        /// Replay mode type.
        /// </summary>
        public enum ModeType {
            /// <summary>
            /// Passthru mode, VRReplay take no effect on game play.
            /// </summary>
            Passthru,
            /// <summary>
            /// Record mode, VRReplay will record VR devices' input and any
            /// object added by AddTransform, will record its
            /// position and rotation.
            /// </summary>
            Record,
            /// <summary>
            /// Playback mode, VR devices' input and recorded object will
            /// be replayed.
            /// </summary>
            Playback,
        }

        /// <summary>
        /// To be notified when the replay is complete, register a delegate 
        /// using this signature by calling RegisterCompleteDelegate.
        /// </summary>
        public delegate void CompleteDelegate();

        /// <summary>
        /// Register a delegate to be invoked when the replay is complete.
        /// </summary>
        /// <param name='del'>
        /// The delegate to be invoked when replay complete.
        /// </param>
        public void RegisterCompleteDelegate(CompleteDelegate del) {
            completeDelegate += del;
        }

        /// <summary>
        /// Unregister a previously registered session complete delegate.
        /// </summary>
        /// <param name='del'>
        /// The delegate to be unregistered.
        /// </param>
        public void UnregisterCompleteDelegate(CompleteDelegate del) {
            completeDelegate -= del;
        }
        /// <summary>
        /// The replay session complete delegate variable.
        /// </summary>
        CompleteDelegate completeDelegate;

        /// <summary>
        /// Return the VRReplay Impl instance.
        /// </summary>
        /// <value>The impl.</value>
        public VRReplayImpl Impl { get { return impl; } }

        /// <summary>
        /// The mode of replay, include passthru, record and playback.
        /// </summary>
        /// <value>The mode.</value>
        public ModeType Mode { get; private set; }

        /// <summary>
        /// Whether replay trace is recording.
        /// </summary>
        /// <value><c>true</c> if is recording; otherwise, <c>false</c>.</value>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Whether is replaying game using replay trace file.
        /// </summary>
        /// <value><c>true</c> if is replaying; otherwise, <c>false</c>.</value>
        public bool IsReplaying { get; private set; }

        /// <summary>
        /// The VRReplay impl instance.
        /// </summary>
        VRReplayImpl impl;
        /// <summary>
        /// Holding the list of objects which require record transform info.
        /// </summary>
        Dictionary<string, Transform> traceTransPool;
        /// <summary>
        /// If upload the replay file to remote server for rendering.
        /// </summary>
        [Tooltip("If upload the replay file to remote server for rendering.")]
        [SerializeField]
        private bool renderingOnServer;
        public bool RenderingOnServer {
            get {
                return renderingOnServer;
            }
            set {
                renderingOnServer = value;
            }
        }
        /// <summary>
        /// The replay game identifier.
        /// </summary>
        [Tooltip("The replay game id.")]
        [SerializeField]
        private int gameID;
        public int GameID {
            get {
                return gameID;
            }
            set {
                gameID = value;
            }
        }
        /// <summary>
        /// Show debug message.
        /// </summary>
        [Tooltip("Show debug info message.")]
        public bool debug = false;

        /// <summary>
        /// Get/Set the name of the replay file.
        /// </summary>
        /// <value>The name of the replay file.</value>
        public string ReplayFileName {
            get {
                if (replayFileName != null)
                    return replayFileName;
                replayFileName = VRCommonUtils.GetTxtFileName();
                return replayFileName;
            }
            set {
                replayFileName = value;
            }
        }

        /// <summary>
        /// The name of the replay file.
        /// </summary>
        string replayFileName = null;

        /// <summary>
        /// Get/Set the replay file path.
        /// </summary>
        /// <value>The replay file path.</value>
        public string ReplayFilePath {
            get {
                if (replayFilePath != null)
                    return replayFilePath;
                replayFilePath = VRReplayUtils.SaveFolder + ReplayFileName;
                return replayFilePath;
            }
            set {
                replayFilePath = value;
            }
        }

        /// <summary>
        /// The replay file saved path.
        /// </summary>
        string replayFilePath = null;

        /// <summary>
        /// Start record replay trace.
        /// </summary>
        /// <returns><c>true</c>, if record was started, <c>false</c> otherwise.</returns>
        public bool StartRecord() {
            if (!Directory.Exists(VRReplayUtils.SaveFolder)) {
                Directory.CreateDirectory(VRReplayUtils.SaveFolder);
            }
            if (IsRecording) {
                Debug.LogWarning(
                    "VRReplay: StartRecord called, but its " +
                    "alreay recording."
                );
                return false;
            }
            if (!Impl.StartRecord(ReplayFilePath)) {
                Debug.LogWarning("VRReplay: StartRecord failed!");
                return false;
            }
            if (debug)
                Debug.Log("VRReplay: StartRecord success!");
            Mode = ModeType.Record;
            IsRecording = true;
            return true;
        }

        /// <summary>
        /// Stop replay trace record.
        /// </summary>
        /// <returns><c>true</c>, if record was stoped, <c>false</c> otherwise.</returns>
        public bool StopRecord() {
            if (!IsRecording) {
                Debug.LogWarning(
                    "VRReplay: StopRecord called, but its " +
                    "not start record yet."
                );
                return false;
            }
            if (!Impl.StopRecord()) {
                Debug.LogWarning("VRReplay: StopRecord failed!");
                return false;
            }
            if (debug)
                Debug.Log("VRReplay: StopRecord success!");
            Mode = ModeType.Passthru;
            IsRecording = false;
            if (renderingOnServer) {
                StartCoroutine(UploadReplay());
            }
            return true;
        }

        /// <summary>
        /// Load the replay file into native plugin.
        /// </summary>
        /// <returns><c>true</c>, if replay was loaded, <c>false</c> otherwise.</returns>
        public bool LoadReplay() {
            if (IsReplaying) {
                Debug.LogWarning(
                    "VRReplay: LoadReplay called, but current " +
                    "is processing a replaying."
                );
                return false;
            }
            if (!Impl.LoadReplay(ReplayFilePath)) {
                Debug.LogWarning("VRReplay: LoadReplay failed, try to load file " + ReplayFilePath);
                return false;
            }
            if (debug)
                Debug.Log("VRReplay: LoadReplay success!");
            return true;
        }

        /// <summary>
        /// Start replay using replay trace file.
        /// </summary>
        /// <returns><c>true</c>, if replay was started, <c>false</c> otherwise.</returns>
        public bool StartReplay() {
            if (IsReplaying) {
                Debug.LogWarning(
                    "VRReplay: StartReplay called, but its " +
                    "alreay replaying."
                );
                return false;
            }
            Mode = ModeType.Playback;
            IsReplaying = true;
            return true;
        }

        /// <summary>
        /// Stop the replay process.
        /// </summary>
        /// <returns><c>true</c>, if replay was stoped, <c>false</c> otherwise.</returns>
        public bool StopReplay() {
            if (!IsReplaying) {
                Debug.LogWarning(
                    "VRReplay: StopReplay called, but its " +
                    "not start replay yet."
                );
                return false;
            }
            Mode = ModeType.Passthru;
            IsReplaying = false;
            if (completeDelegate != null) {
                completeDelegate();
            }
            if (debug)
                Debug.Log("VRReplay: StopReplay success!");
            return true;
        }

        /// <summary>
        /// Upload the replay to server for rendering.
        /// </summary>
        public IEnumerator UploadReplay() {
            byte[] replayFile = File.ReadAllBytes(ReplayFilePath);
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", replayFile, ReplayFileName, "text/plain");
            form.AddField("game", GameID);
            WWW www = new WWW(VRReplayUtils.UPLOAD_ADDRESS, form);
            yield return www;
            if (www.error != null) {
                Debug.LogWarning("VRReplay: UploadReplay called, but create " +
                "WWW with error: ");
                Debug.LogWarning(www.error);
            }
            else {
                if (debug)
                    Debug.Log("VRReplay: UploadReplay success!");
            }
        }

        /// <summary>
        /// Record the index of the device.
        /// </summary>
        /// <returns><c>true</c>, if device index was recorded, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <param name="index">Index.</param>
        public bool RecordDeviceIndex(string name, int index) {
            return Impl.RecordDeviceIndex(name, index);
        }

        /// <summary>
        /// Query the index of the device.
        /// </summary>
        /// <returns>The device index.</returns>
        /// <param name="name">Name.</param>
        public int QueryDeviceIndex(string name) {
            return Impl.QueryDeviceIndex(name);
        }

        /// <summary>
        /// Record bool value VR input.
        /// </summary>
        /// <returns><c>true</c>, if VR Input bool was recorded, <c>false</c> otherwise.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public bool RecordVRInputBool(int deviceID, string input, VRDeviceInputType key, bool value) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return false;
            }
            VRDeviceInputInfo inputInfo = new VRDeviceInputInfo(deviceID, input);
            switch (key) {
                case VRDeviceInputType.Key:
                    inputInfo.key = value;
                    break;
                case VRDeviceInputType.KeyUp:
                    inputInfo.keyUp = value;
                    break;
                case VRDeviceInputType.KeyDown:
                    inputInfo.keyDown = value;
                    break;
                case VRDeviceInputType.Button:
                    inputInfo.button = value;
                    break;
                case VRDeviceInputType.ButtonUp:
                    inputInfo.buttonUp = value;
                    break;
                case VRDeviceInputType.ButtonDown:
                    inputInfo.buttonDown = value;
                    break;
                case VRDeviceInputType.Press:
                    inputInfo.press = value;
                    break;
                case VRDeviceInputType.PressUp:
                    inputInfo.pressUp = value;
                    break;
                case VRDeviceInputType.PressDown:
                    inputInfo.pressDown = value;
                    break;
                case VRDeviceInputType.Touch:
                    inputInfo.touch = value;
                    break;
                case VRDeviceInputType.TouchDown:
                    inputInfo.touchDown = value;
                    break;
                case VRDeviceInputType.TouchUp:
                    inputInfo.touchUp = value;
                    break;
                case VRDeviceInputType.HairTrigger:
                    inputInfo.hairTrigger = value;
                    break;
                case VRDeviceInputType.HairTriggerDown:
                    inputInfo.hairTriggerDown = value;
                    break;
                case VRDeviceInputType.HairTriggerUp:
                    inputInfo.hairTriggerUp = value;
                    break;
                default:
                    return false;
            }
            if (value == false) {
                return false;
            }
            return Impl.RecordVRInput(inputInfo);
        }

        /// <summary>
        /// Query bool value VR input.
        /// </summary>
        /// <returns>VR input bool value.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        public bool QueryVRInputBool(int deviceID, string input, VRDeviceInputType key) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return false;
            }
            VRDeviceInputInfo inputInfo = Impl.QueryVRInput(deviceID, input);
            if (inputInfo == null) {
                return false;
            }
            switch (key) {
                case VRDeviceInputType.Key:
                    return inputInfo.key;
                case VRDeviceInputType.KeyUp:
                    return inputInfo.keyUp;
                case VRDeviceInputType.KeyDown:
                    return inputInfo.keyDown;
                case VRDeviceInputType.Button:
                    return inputInfo.button;
                case VRDeviceInputType.ButtonUp:
                    return inputInfo.buttonUp;
                case VRDeviceInputType.ButtonDown:
                    return inputInfo.buttonDown;
                case VRDeviceInputType.Press:
                    return inputInfo.press;
                case VRDeviceInputType.PressUp:
                    return inputInfo.pressUp;
                case VRDeviceInputType.PressDown:
                    return inputInfo.pressDown;
                case VRDeviceInputType.Touch:
                    return inputInfo.touch;
                case VRDeviceInputType.TouchDown:
                    return inputInfo.touchDown;
                case VRDeviceInputType.TouchUp:
                    return inputInfo.touchUp;
                case VRDeviceInputType.HairTrigger:
                    return inputInfo.hairTrigger;
                case VRDeviceInputType.HairTriggerDown:
                    return inputInfo.hairTriggerDown;
                case VRDeviceInputType.HairTriggerUp:
                    return inputInfo.hairTriggerUp;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Record float value VR input.
        /// </summary>
        /// <returns><c>true</c>, if VRInput float was recorded, <c>false</c> otherwise.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public bool RecordVRInputFloat(int deviceID, string input, VRDeviceInputType key, float value) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return false;
            }
            VRDeviceInputInfo inputInfo = new VRDeviceInputInfo(deviceID, input);
            switch (key) {
                case VRDeviceInputType.AxisFloat:
                    inputInfo.axisFloat = value;
                    break;
                default:
                    return false;
            }
            return Impl.RecordVRInput(inputInfo);
        }

        /// <summary>
        /// Query float value VR input.
        /// </summary>
        /// <returns>The VR input float value.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        public float QueryVRInputFloat(int deviceID, string input, VRDeviceInputType key) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return 0;
            }
            VRDeviceInputInfo inputInfo = Impl.QueryVRInput(deviceID, input);
            if (inputInfo == null) {
                return 0;
            }
            switch (key) {
                case VRDeviceInputType.AxisFloat:
                    return inputInfo.axisFloat;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Record vector2 value VR input.
        /// </summary>
        /// <returns><c>true</c>, if VRInput vec2 was recorded, <c>false</c> otherwise.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        /// <param name="vec2">Vec2.</param>
        public bool RecordVRInputVec2(int deviceID, string input, VRDeviceInputType key, Vector2 vec2) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return false;
            }
            VRDeviceInputInfo inputInfo = new VRDeviceInputInfo(deviceID, input);
            switch (key) {
                case VRDeviceInputType.AxisVec2:
                    inputInfo.axisVec2 = new SerializableVector2(vec2);
                    break;
                default:
                    return false;
            }
            return Impl.RecordVRInput(inputInfo);
        }

        /// <summary>
        /// Query vector2 value VR input.
        /// </summary>
        /// <returns>The VR input vec2 x value.</returns>
        /// <param name="deviceID">Device identifier.</param>
        /// <param name="input">Input.</param>
        /// <param name="key">Key.</param>
        public Vector2 QueryVRInputVec2(int deviceID, string input, VRDeviceInputType key) {
            if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                return new Vector2();
            }
            VRDeviceInputInfo inputInfo = Impl.QueryVRInput(deviceID, input);
            if (inputInfo == null) {
                return new Vector2();
            }
            switch (key) {
                case VRDeviceInputType.AxisVec2:
                    return new Vector2(inputInfo.axisVec2.x, inputInfo.axisVec2.y);
                default:
                    return new Vector2();
            }
        }

        /// <summary>
        /// Record sequence float data.
        /// </summary>
        /// <returns><c>true</c>, if sequence float was recorded, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public bool RecordSequenceFloat(string name, float value) {
            return Impl.RecordSequenceFloat(name, value);
        }

        /// <summary>
        /// Record sequence int data.
        /// </summary>
        /// <returns><c>true</c>, if sequence int was recorded, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public bool RecordSequenceInt(string name, int value) {
            return Impl.RecordSequenceInt(name, value);
        }

        /// <summary>
        /// Query the sequence float.
        /// </summary>
        /// <returns>The sequence float.</returns>
        /// <param name="name">Name.</param>
        public float QuerySequenceFloat(string name) {
            // Auto move to next sequence data.
            if (!Impl.NextSequenceFloat(name)) {
                return 0f;
            }
            return Impl.QuerySequenceFloat(name);
        }

        /// <summary>
        /// Query the sequence int.
        /// </summary>
        /// <returns>The sequence int.</returns>
        /// <param name="name">Name.</param>
        public int QuerySequenceInt(string name) {
            // Auto move to next sequence data.
            if (!Impl.NextSequenceInt(name)) {
                return 0;
            }
            return Impl.QuerySequenceInt(name);
        }

        /// <summary>
        /// Record store string data.
        /// </summary>
        /// <returns><c>true</c>, if store string was recorded, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public bool RecordStoreString(string name, string value) {
            return Impl.RecordStoreString(name, value);
        }

        /// <summary>
        /// Query store string data.
        /// </summary>
        /// <returns>The store string.</returns>
        /// <param name="name">Name.</param>
        public string QueryStoreString(string name) {
            return Impl.QueryStoreString(name);
        }

        /// <summary>
        /// Add object for transform tracing.
        /// </summary>
        /// <param name="trans">Object transform.</param>
        public void AddTransform(Transform trans) {
            if (traceTransPool.ContainsKey(trans.name)) {
                Debug.LogWarning(
                    "VRReplay: TraceTransform called, " +
                    trans.name + " already in the traceTransPool. " +
                    "Update instead of adding a new object."
                );
                traceTransPool[trans.name] = trans;
                return;
            }
            traceTransPool.Add(trans.name, trans);
        }

        /// <summary>
        /// Remove object for transform tracing.
        /// </summary>
        /// <param name="trans">Object transform.</param>
        public void RemoveTransform(Transform trans) {
            if (!traceTransPool.ContainsKey(trans.name)) {
                Debug.LogWarning(
                    "VRReplay: UntraceTransform called, " +
                    trans.name + " not in the traceTransPool. " +
                    "Nothing happened."
                );
                return;
            }
            traceTransPool.Remove(trans.name);
        }

        /// <summary>
        /// Initial instance.
        /// </summary>
        void Awake() {
            Instance = this;
            // Initial replay impl instance.
            impl = new VRReplayImpl();
            Mode = ModeType.Passthru;
            traceTransPool = new Dictionary<string, Transform>();
        }
        /// <summary>
        /// Record or Replay in fixed frame rate.
        /// </summary>
        void FixedUpdate() {
            if (Mode == ModeType.Passthru) { // Do nothing.

            }
            else if (Mode == ModeType.Record && IsRecording) { // Record.
                // Allocate a new frame.
                Impl.NewFrame();
                // Record added transforms.
                foreach (string objName in traceTransPool.Keys) {
                    Transform trans = traceTransPool[objName];
                    Impl.RecordTransform(new SerializableTransform(
                        objName,
                        trans.position,
                        trans.rotation
                    ));
                }

            }
            else if (Mode == ModeType.Playback && IsReplaying) {  // Replay.
                if (!Impl.NextFrame()) {
                    // Stop replay if no more trace frame.
                    StopReplay();
                    return;
                }
                // Replay objects position in current frame.
                while (Impl.NextTransform()) {
                    SerializableTransform trans = Impl.QueryTransform();
                    Transform replayTrans = null;
                    if (traceTransPool.ContainsKey(trans.Name)) {
                        replayTrans = traceTransPool[trans.Name];
                    }
                    else {
                        GameObject obj = GameObject.Find(trans.Name);
                        if (obj != null) {
                            replayTrans = obj.transform;
                            AddTransform(obj.transform);
                        }
                    }
                    if (replayTrans != null) {
                        replayTrans.position = new Vector3(
                            trans.GetPostion().x,
                            trans.GetPostion().y,
                            trans.GetPostion().z
                        );
                        replayTrans.rotation = new Quaternion(
                            trans.GetRotation().x,
                            trans.GetRotation().y,
                            trans.GetRotation().z,
                            trans.GetRotation().w
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Check if still recording on application quit.
        /// </summary>
        void OnApplicationQuit() {
            if (IsRecording) {
                StopRecord();
            }
        }

        // VRReplay Serializable Struct Overview:
        //
        // [float, float, ...]    [int, int, ...]                               <- sequence data
        //            \ name          / name
        //            [TraceReplayBuffer]  ------------ [VRDeviceConfigInfo]    <- device config data
        //                /    |    \            \
        //                [TraceFrame]      [TraceFrame]
        //              /      |      \             \
        //   [TraceTransform, VRDeviceInfo] [TraceTransform, VRDeviceInfo]      <- frame data
        //       /                     |               \
        // [VRDeviceInputInfo][VRDeviceInputInfo][VRDeviceInputInfo]
        //
        /// <summary>
        /// Represent trace information in one frame, include VR devices' input 
        /// and objects' transform.
        /// </summary>
        [Serializable]
        class TraceFrame {
            /// <summary>
            /// Save registered objects' transform.
            /// </summary>
            List<SerializableTransform> traceTransforms;
            /// <summary>
            /// From device index (int) to find VR device info.
            /// </summary>
            Dictionary<int, VRDeviceInfo> traceVRDeviceInfos;
            /// <summary>
            /// Constructor.
            /// </summary>
            public TraceFrame() {
                traceTransforms = new List<SerializableTransform>();
                traceVRDeviceInfos = new Dictionary<int, VRDeviceInfo>();
            }
            /// <summary>
            /// Get the traced transforms.
            /// </summary>
            /// <returns>The transforms.</returns>
            public List<SerializableTransform> GetTransforms() {
                return traceTransforms;
            }
            /// <summary>
            /// Get the traced VR Device infos.
            /// </summary>
            /// <returns>The VR Device infos.</returns>
            public Dictionary<int, VRDeviceInfo> GetVRDeviceInfos() {
                return traceVRDeviceInfos;
            }
        }
        /// <summary>
        /// Contain the trace frame data and sequence data required for replaying;
        /// Can be serialized to or restore from disk file.
        /// </summary>
        [Serializable]
        class TraceReplayBuffer {
            /// <summary>
            /// Contain all recorded trace frames.
            /// </summary>
            List<TraceFrame> frames;
            /// <summary>
            /// The device config information.
            /// </summary>
            VRDeviceConfigInfo deviceConfig;
            /// <summary>
            /// Float sequence data.
            /// </summary>
            Dictionary<string, List<float>> floatSequences;
            /// <summary>
            /// Int sequence data.
            /// </summary>
            Dictionary<string, List<int>> intSequences;
            /// <summary>
            /// String store data.
            /// </summary>
            Dictionary<string, string> stringStores;
            /// <summary>
            /// Constructor.
            /// </summary>
            public TraceReplayBuffer() {
                frames = new List<TraceFrame>();
                deviceConfig = new VRDeviceConfigInfo();
                floatSequences = new Dictionary<string, List<float>>();
                intSequences = new Dictionary<string, List<int>>();
                stringStores = new Dictionary<string, string>();
            }
            /// <summary>
            /// Get frames reference.
            /// </summary>
            /// <returns>The frames.</returns>
            public List<TraceFrame> GetFrames() {
                return frames;
            }
            /// <summary>
            /// Get deviceConfig reference.
            /// </summary>
            /// <returns>The VR Device config.</returns>
            public VRDeviceConfigInfo GetVRDeviceConfig() {
                return deviceConfig;
            }
            /// <summary>
            /// Get sequence floats map reference.
            /// </summary>
            /// <returns>The float sequences.</returns>
            public Dictionary<string, List<float>> GetFloatSequences() {
                return floatSequences;
            }
            /// <summary>
            /// Get sequence floats by name.
            /// </summary>
            /// <returns>The float sequence.</returns>
            /// <param name="name">Name.</param>
            public List<float> GetFloatSequence(string name) {
                if (!GetFloatSequences().ContainsKey(name)) {
                    return null;
                }
                return GetFloatSequences()[name];
            }
            /// <summary>
            /// Get sequence ints map reference.
            /// </summary>
            /// <returns>The int sequences.</returns>
            public Dictionary<string, List<int>> GetIntSequences() {
                return intSequences;
            }
            /// <summary>
            /// Get sequence ints by name.
            /// </summary>
            /// <returns>The int sequence.</returns>
            /// <param name="name">Name.</param>
            public List<int> GetIntSequence(string name) {
                if (!GetIntSequences().ContainsKey(name)) {
                    return null;
                }
                return GetIntSequences()[name];
            }
            /// <summary>
            /// Get store strings reference.
            /// </summary>
            /// <returns>The string stores.</returns>
            public Dictionary<string, string> GetStringStores() {
                return stringStores;
            }
            /// <summary>
            /// Get string value by name.
            /// </summary>
            /// <returns>The string store.</returns>
            /// <param name="name">Name.</param>
            public string GetStringStore(string name) {
                if (!GetStringStores().ContainsKey(name)) {
                    return null;
                }
                return GetStringStores()[name];
            }
        }
        /// <summary>
        /// TraceReplayBuffer manager.
        /// Manage replay informations based on frame.
        /// </summary>
        public class VRReplayImpl {
            /// <summary>
            /// File path for loading and saving replay trace file.
            /// </summary>
            string replayFilePath;
            /// <summary>
            /// If replay file loaded.
            /// </summary>
            bool replayFileLoaded;
            /// <summary>
            /// The index of recorded trace frame list.
            /// </summary>
            int frameIndex;
            /// <summary>
            /// The iterator of transform list for current frame.
            /// </summary>
            List<SerializableTransform>.Enumerator transformIterator;
            /// <summary>
            /// The index map of float sequences.
            /// </summary>
            Dictionary<string, int> floatSequenceIndexs;
            /// <summary>
            /// The index map of int sequences.
            /// </summary>
            Dictionary<string, int> intSequenceIndexs;
            /// <summary>
            /// Instance of TraceReplayBuffer.
            /// </summary>
            TraceReplayBuffer replayBuffer;
            /// <summary>
            /// Get current frame according to frameIndex.
            /// </summary>
            /// <returns>The frame.</returns>
            TraceFrame CurrentFrame() {
                return replayBuffer.GetFrames()[frameIndex];
            }
            /// <summary>
            /// Get device index map config for current replay.
            /// </summary>
            /// <returns>The VR Device indexs.</returns>
            Dictionary<string, int> GetVRDeviceIndexs() {
                return replayBuffer.GetVRDeviceConfig().GetVRDeviceIndexs();
            }
            /// <summary>
            /// Get all VR device infos for current frame.
            /// </summary>
            /// <returns>The VR Device infos.</returns>
            Dictionary<int, VRDeviceInfo> GetVRDeviceInfos() {
                return CurrentFrame().GetVRDeviceInfos();
            }
            /// <summary>
            /// Get all VR input infos for device ID and current frame.
            /// </summary>
            /// <returns>The VRI nput infos.</returns>
            /// <param name="deviceID">Device identifier.</param>
            Dictionary<string, VRDeviceInputInfo> GetVRInputInfos(int deviceID) {
                if (!GetVRDeviceInfos().ContainsKey(deviceID)) {
                    return null;
                }
                return GetVRDeviceInfos()[deviceID].GetVRInputInfos();
            }
            /// <summary>
            /// Get replay buffer float sequence reference.
            /// </summary>
            /// <returns>The float sequences.</returns>
            Dictionary<string, List<float>> GetFloatSequences() {
                return replayBuffer.GetFloatSequences();
            }
            /// <summary>
            /// Get replay buffer int sequence reference.
            /// </summary>
            /// <returns>The int sequences.</returns>
            Dictionary<string, List<int>> GetIntSequences() {
                return replayBuffer.GetIntSequences();
            }
            /// <summary>
            /// Get sequence floats by name.
            /// </summary>
            /// <returns>The float sequence.</returns>
            /// <param name="name">Name.</param>
            List<float> GetFloatSequence(string name) {
                return replayBuffer.GetFloatSequence(name);
            }
            /// <summary>
            /// Get sequence ints by name.
            /// </summary>
            /// <returns>The int sequence.</returns>
            /// <param name="name">Name.</param>
            List<int> GetIntSequence(string name) {
                return replayBuffer.GetIntSequence(name);
            }
            /// <summary>
            /// Get stored strings.
            /// </summary>
            /// <returns>The string stores.</returns>
            Dictionary<string, string> GetStringStores() {
                return replayBuffer.GetStringStores();
            }
            /// <summary>
            /// Constructor.
            /// </summary>
            public VRReplayImpl() {
                replayBuffer = new TraceReplayBuffer();
                floatSequenceIndexs = new Dictionary<string, int>();
                intSequenceIndexs = new Dictionary<string, int>();
            }
            /// <summary>
            /// Called before record trace, reset replay state.
            /// </summary>
            /// <returns><c>true</c>, if record was prepared, <c>false</c> otherwise.</returns>
            public bool PrepareRecord() {
                frameIndex = -1;
                return true;
            }
            /// <summary>
            /// Called after load from trace file, reset replay state.
            /// </summary>
            /// <returns><c>true</c>, if replay was prepared, <c>false</c> otherwise.</returns>
            public bool PrepareReplay() {
                // Frame index.
                frameIndex = -1;
                // Float sequence data index.
                foreach (string key in GetFloatSequences().Keys) {
                    if (!floatSequenceIndexs.ContainsKey(key)) {
                        floatSequenceIndexs.Add(key, -1);
                    }
                    else {
                        floatSequenceIndexs[key] = -1;
                    }
                }
                // Int sequence data index.
                foreach (string key in GetIntSequences().Keys) {
                    if (!intSequenceIndexs.ContainsKey(key)) {
                        intSequenceIndexs.Add(key, -1);
                    }
                    else {
                        intSequenceIndexs[key] = -1;
                    }
                }
                return true;
            }
            /// <summary>
            /// Record device name index mapping.
            /// </summary>
            /// <returns><c>true</c>, if device index was recorded, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            /// <param name="index">Index.</param>
            public bool RecordDeviceIndex(string name, int index) {
                if (GetVRDeviceIndexs().ContainsKey(name)) {
                    return false;
                }
                GetVRDeviceIndexs()[name] = index;
                return true;
            }
            /// <summary>
            /// Query device index by name.
            /// </summary>
            /// <returns>The device index.</returns>
            /// <param name="name">Name.</param>
            public int QueryDeviceIndex(string name) {
                if (!replayFileLoaded ||
                    !GetVRDeviceIndexs().ContainsKey(name)) {
                    return 0;
                }
                return GetVRDeviceIndexs()[name];
            }
            /// <summary>
            /// Record transform information for this frame.
            /// </summary>
            /// <returns><c>true</c>, if transform was recorded, <c>false</c> otherwise.</returns>
            /// <param name="trans">Trans.</param>
            public bool RecordTransform(SerializableTransform trans) {
                CurrentFrame().GetTransforms().Add(trans);
                return true;
            }
            /// <summary>
            /// Move to next transform based on current frame.
            /// </summary>
            /// <returns><c>true</c>, if transform was nexted, <c>false</c> otherwise.</returns>
            public bool NextTransform() {
                if (!replayFileLoaded) {
                    return false;
                }
                return transformIterator.MoveNext();
            }
            /// <summary>
            /// Query current transform for current frame.
            /// </summary>
            /// <returns>The transform.</returns>
            public SerializableTransform QueryTransform() {
                if (!replayFileLoaded) {
                    return null;
                }
                return transformIterator.Current;
            }
            /// <summary>
            /// Record VR device input information for this frame.
            /// </summary>
            /// <returns><c>true</c>, if VR Input was recorded, <c>false</c> otherwise.</returns>
            /// <param name="info">Info.</param>
            public bool RecordVRInput(VRDeviceInputInfo info) {
                if (info.deviceID < 0 || info.deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                    return false;
                }
                if (!info.press && !info.pressUp && !info.pressDown && !info.touch && !info.touchUp && !info.touchDown) {
                    return false;
                }
                if (!CurrentFrame().GetVRDeviceInfos().ContainsKey(info.deviceID)) {
                    CurrentFrame().GetVRDeviceInfos().Add(info.deviceID, new VRDeviceInfo());
                }
                if (!CurrentFrame().GetVRDeviceInfos()[info.deviceID].GetVRInputInfos().ContainsKey(info.inputName)) {
                    CurrentFrame().GetVRDeviceInfos()[info.deviceID].GetVRInputInfos().Add(info.inputName, info);
                }
                else {

                    CurrentFrame().GetVRDeviceInfos()[info.deviceID].GetVRInputInfos()[info.inputName] = info;
                }
                return true;
            }
            /// <summary>
            /// Query VR device input information by device ID for current frame.
            /// </summary>
            /// <returns>The VR Input.</returns>
            /// <param name="deviceID">Device identifier.</param>
            /// <param name="inputName">Input name.</param>
            public VRDeviceInputInfo QueryVRInput(int deviceID, string inputName) {
                if (deviceID < 0 || deviceID >= VRReplayUtils.MAX_DEVICE_NUM) {
                    return null;
                }
                if (GetVRInputInfos(deviceID) == null || !GetVRInputInfos(deviceID).ContainsKey(inputName)) {
                    return null;
                }
                return GetVRInputInfos(deviceID)[inputName];
            }
            /// <summary>
            /// Record float sequence data.
            /// </summary>
            /// <returns><c>true</c>, if sequence float was recorded, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            /// <param name="value">Value.</param>
            public bool RecordSequenceFloat(string name, float value) {
                if (!GetFloatSequences().ContainsKey(name)) {
                    GetFloatSequences().Add(name, new List<float>());
                }
                GetFloatSequence(name).Add(value);
                return true;
            }
            /// <summary>
            /// Record int sequence data.
            /// </summary>
            /// <returns><c>true</c>, if sequence int was recorded, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            /// <param name="value">Value.</param>
            public bool RecordSequenceInt(string name, int value) {
                if (!GetIntSequences().ContainsKey(name)) {
                    GetIntSequences().Add(name, new List<int>());
                }
                GetIntSequence(name).Add(value);
                return true;
            }
            /// <summary>
            /// Move to next float data in sequence.
            /// </summary>
            /// <returns><c>true</c>, if sequence float was nexted, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            public bool NextSequenceFloat(string name) {
                int index = floatSequenceIndexs[name] + 1;
                floatSequenceIndexs[name] = index;
                if (!replayFileLoaded ||
                    index >= GetFloatSequence(name).Count) {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Move to next int data in sequence.
            /// </summary>
            /// <returns><c>true</c>, if sequence int was nexted, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            public bool NextSequenceInt(string name) {
                int index = intSequenceIndexs[name] + 1;
                intSequenceIndexs[name] = index;
                if (!replayFileLoaded ||
                    index >= GetIntSequence(name).Count) {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Query sequence float data.
            /// </summary>
            /// <returns>The sequence float.</returns>
            /// <param name="name">Name.</param>
            public float QuerySequenceFloat(string name) {
                if (!replayFileLoaded ||
                    !floatSequenceIndexs.ContainsKey(name)) {
                    return 0f;
                }
                return GetFloatSequence(name)[floatSequenceIndexs[name]];
            }
            /// <summary>
            /// Query sequence int data.
            /// </summary>
            /// <returns>The sequence int.</returns>
            /// <param name="name">Name.</param>
            public int QuerySequenceInt(string name) {
                if (!replayFileLoaded ||
                    !intSequenceIndexs.ContainsKey(name)) {
                    return 0;
                }
                return GetIntSequence(name)[intSequenceIndexs[name]];
            }
            /// <summary>
            /// Record store string data.
            /// </summary>
            /// <returns><c>true</c>, if store string was recorded, <c>false</c> otherwise.</returns>
            /// <param name="name">Name.</param>
            /// <param name="value">Value.</param>
            public bool RecordStoreString(string name, string value) {
                if (GetStringStores().ContainsKey(name)) {
                    GetStringStores()[name] = value;
                }
                else {
                    GetStringStores().Add(name, value);
                }
                return true;
            }
            /// <summary>
            /// Query store string data.
            /// </summary>
            /// <returns>The store string.</returns>
            /// <param name="name">Name.</param>
            public string QueryStoreString(string name) {
                if (!replayFileLoaded ||
                    !GetStringStores().ContainsKey(name)) {
                    return null;
                }
                return GetStringStores()[name];
            }
            /// <summary>
            /// Get TraceReplayBuffer struct.
            /// </summary>
            /// <returns>The replay buffer.</returns>
            //public TraceReplayBuffer GetReplayBuffer() {
            //    return replayBuffer;
            //}
            /// <summary>
            /// Create a new record frame and add to frame list.
            /// </summary>
            /// <returns><c>true</c>, if frame was newed, <c>false</c> otherwise.</returns>
            public bool NewFrame() {
                TraceFrame frame = new TraceFrame();
                replayBuffer.GetFrames().Add(frame);
                frameIndex++;
                return true;
            }
            /// <summary>
            /// Move to next frame based on frame index.
            /// </summary>
            /// <returns><c>true</c>, if frame was nexted, <c>false</c> otherwise.</returns>
            public bool NextFrame() {
                frameIndex++;
                if (!replayFileLoaded ||
                    frameIndex >= replayBuffer.GetFrames().Count) {
                    return false;
                }
                // Reset iterator for current frame transform list.
                transformIterator = CurrentFrame().GetTransforms().GetEnumerator();
                return true;
            }
            /// <summary>
            /// Save the replay trace file.
            /// </summary>
            /// <returns><c>true</c>, if trace file was saved, <c>false</c> otherwise.</returns>
            bool SaveTraceFile() {
                // Serialize
                using (Stream stream = File.Open(replayFilePath, FileMode.Create)) {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter =
                        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binFormatter.Serialize(stream, replayBuffer);
                }
                return true;
            }
            /// <summary>
            /// Load the replay trace file.
            /// </summary>
            /// <returns><c>true</c>, if trace file was loaded, <c>false</c> otherwise.</returns>
            bool LoadTraceFile() {
                if (!File.Exists(replayFilePath)) {
                    Debug.LogWarning("VRReplayImpl: LoadTraceFile called, but replay " +
                                     "file not found!");
                    return false;
                }
                // Deserialize
                using (Stream stream = File.Open(replayFilePath, FileMode.Open)) {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormatter =
                        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    replayBuffer = (TraceReplayBuffer)binFormatter.Deserialize(stream);
                }
                return true;
            }
            /// <summary>
            /// Start replay trace record.
            /// </summary>
            /// <returns><c>true</c>, if record was started, <c>false</c> otherwise.</returns>
            /// <param name="filePath">File path.</param>
            public bool StartRecord(string filePath) {
                replayFilePath = filePath;
                return PrepareRecord();
            }
            /// <summary>
            /// Stop replay trace record.
            /// </summary>
            /// <returns><c>true</c>, if record was stoped, <c>false</c> otherwise.</returns>
            public bool StopRecord() {
                if (!SaveTraceFile()) {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Load replay using trace file.
            /// </summary>
            /// <returns><c>true</c>, if replay was loaded, <c>false</c> otherwise.</returns>
            /// <param name="filePath">File path.</param>
            public bool LoadReplay(string filePath) {
                replayFilePath = filePath;
                if (!LoadTraceFile()) {
                    return false;
                }
                replayFileLoaded = true;
                return PrepareReplay();
            }
        }
    }
}
