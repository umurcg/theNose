using System;
using System.Collections.Generic;

namespace VRCapture {
    /// <summary>
    /// VR device input key type.
    /// </summary>
    public enum VRDeviceInputType {
        /// <summary>
        /// For Standalone device.
        /// </summary>
        Key,
        KeyDown,
        KeyUp,

        Button,
        ButtonDown,
        ButtonUp,

        AxisFloat,
        /// <summary>
        /// For HTC Vive device.
        /// </summary>
        Press,
        PressDown,
        PressUp,

        Touch,
        TouchDown,
        TouchUp,
        HairTrigger,
        HairTriggerDown,
        HairTriggerUp,

        AxisVec2,
    }

    /// <summary>
    /// Basic VR device input information.
    /// </summary>
    [Serializable]
    public class VRDeviceInputInfo {
        /// <summary>
        /// Index of VR device.
        /// </summary>
        public int deviceID;
        /// <summary>
        /// Name of VR device input.
        /// </summary>
        public string inputName;
        /// <summary>
        /// Standalone part.
        /// </summary>
        public bool key;
        public bool keyUp;
        public bool keyDown;

        public bool button;
        public bool buttonUp;
        public bool buttonDown;

        public float axisFloat;
        /// <summary>
        /// HTC Vive part.
        /// </summary>
        public bool press;
        public bool pressDown;
        public bool pressUp;

        public bool touch;
        public bool touchDown;
        public bool touchUp;

        public bool hairTrigger;
        public bool hairTriggerDown;
        public bool hairTriggerUp;

        public SerializableVector2 axisVec2;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="name">Name.</param>
        public VRDeviceInputInfo(int id, string name) {
            deviceID = id;
            inputName = name;
        }
    }

    /// <summary>
    /// Basic VR device information.
    /// </summary>
    [Serializable]
    public class VRDeviceInfo {

        Dictionary<string, VRDeviceInputInfo> vrInputInfos;

        public VRDeviceInfo() {
            vrInputInfos = new Dictionary<string, VRDeviceInputInfo>();
        }

        public Dictionary<string, VRDeviceInputInfo> GetVRInputInfos() {
            return vrInputInfos;
        }
    }

    /// <summary>
    /// VR device config information.
    /// </summary>
    [Serializable]
    public class VRDeviceConfigInfo {

        Dictionary<string, int> vrDeviceIndexs;

        public VRDeviceConfigInfo() {
            vrDeviceIndexs = new Dictionary<string, int>();
        }

        public Dictionary<string, int> GetVRDeviceIndexs() {
            return vrDeviceIndexs;
        }
    }
}
