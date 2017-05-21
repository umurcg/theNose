using UnityEngine;

namespace VRCapture {
    /// <summary>
    /// Simulate PC Standalone input, replace Unity Input with StandaloneInput
    /// class will record all user input, such as keyboard, mouse button state, 
    /// mouse position, etc.
    /// 
    /// Example usage:
    /// StandaloneInput.GetKey(KeyCode.LeftArrow)
    /// </summary>
    public class StandaloneInput {
        /// <summary>
        /// Return the Standalone key press state.
        /// </summary>
        /// <returns><c>true</c>, if key was pressed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public static bool GetKey(KeyCode key) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, key.ToString(), VRDeviceInputType.Key);
            }
            bool state = Input.GetKey(key);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, key.ToString(), VRDeviceInputType.Key, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone key up press state.
        /// </summary>
        /// <returns><c>true</c>, if key up was pressed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public static bool GetKeyUp(KeyCode key) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, key.ToString(), VRDeviceInputType.KeyUp);
            }
            bool state = Input.GetKeyUp(key);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, key.ToString(), VRDeviceInputType.KeyUp, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone key down press state.
        /// </summary>
        /// <returns><c>true</c>, if key down was pressed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public static bool GetKeyDown(KeyCode key) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, key.ToString(), VRDeviceInputType.KeyDown);
            }
            bool state = Input.GetKeyDown(key);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, key.ToString(), VRDeviceInputType.KeyDown, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone button press state.
        /// </summary>
        /// <returns><c>true</c>, if button was pressed, <c>false</c> otherwise.</returns>
        /// <param name="buttonName">Button name.</param>
        public static bool GetButton(string buttonName) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, buttonName, VRDeviceInputType.Button);
            }
            bool state = Input.GetButton(buttonName);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, buttonName, VRDeviceInputType.Button, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone button up press state.
        /// </summary>
        /// <returns><c>true</c>, if button up was pressed, <c>false</c> otherwise.</returns>
        /// <param name="buttonName">Button name.</param>
        public static bool GetButtonUp(string buttonName) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, buttonName, VRDeviceInputType.ButtonUp);
            }
            bool state = Input.GetButtonUp(buttonName);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, buttonName, VRDeviceInputType.ButtonUp, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone button down press state.
        /// </summary>
        /// <returns><c>true</c>, if button down was pressed, <c>false</c> otherwise.</returns>
        /// <param name="buttonName">Button name.</param>
        public static bool GetButtonDown(string buttonName) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputBool(0, buttonName, VRDeviceInputType.ButtonDown);
            }
            bool state = Input.GetButtonDown(buttonName);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if (state)
                    VRReplay.Instance.RecordVRInputBool(0, buttonName, VRDeviceInputType.ButtonDown, state);
            }
            return state;
        }

        /// <summary>
        /// Return the Standalone button axis value.
        /// </summary>
        /// <returns>The axis.</returns>
        /// <param name="buttonName">Button name.</param>
        public static float GetAxis(string buttonName) {
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                return VRReplay.Instance.QueryVRInputFloat(0, buttonName, VRDeviceInputType.AxisFloat);
            }
            float value = Input.GetAxis(buttonName);
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                if ((Mathf.Abs(value) < VRCommonUtils.EPSILON)) {
                    VRReplay.Instance.RecordVRInputFloat(0, buttonName, VRDeviceInputType.Button, value);
                }
            }
            return value;
        }
    }
}