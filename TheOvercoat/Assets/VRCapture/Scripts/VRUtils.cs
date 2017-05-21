using System;
using UnityEngine;

namespace VRCapture {
    ////////////////////////////////////////////////////////////////////////////
    ///                           Common Utils.                              ///
    ////////////////////////////////////////////////////////////////////////////
    public class VRCommonUtils {
        public const float EPSILON = 0.000001f;
        public static string DATA_PATH = Application.dataPath;
        public static string PERSISTENT_DATA_PATH = Application.persistentDataPath;
        public static string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
        public static string MY_DOCUMENTS_PATH = Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);

        static string GetTimeString() {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public static string GetPngFileName() {
            return GetPngFileName(null);
        }

        public static string GetPngFileName(string name) {
            return GetTimeString() + (name == null ? "" : "-") + name + ".png";
        }

        public static string GetMp4FileName() {
            return GetMp4FileName(null);
        }

        public static string GetMp4FileName(string name) {
            return GetTimeString() + (name == null ? "" : "-") + name + ".mp4";
        }

        public static string GetWavFileName() {
            return GetWavFileName(null);
        }

        public static string GetWavFileName(string name) {
            return GetTimeString() + (name == null ? "" : "-") + name + ".wav";
        }

        public static string GetTxtFileName() {
            return GetTxtFileName(null);
        }

        public static string GetTxtFileName(string name) {
            return GetTimeString() + (name == null ? "" : "-") + name + ".txt";
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    ///                    Basic Utils for VRCapture.                        ///
    ////////////////////////////////////////////////////////////////////////////
    public class VRCaptureUtils {

        public static string SaveFolder {
            get {
#if UNITY_ANDROID && !UNITY_EDITOR
                return VRCommonUtils.PERSISTENT_DATA_PATH + "/VRCapture";
#else
                return VRCommonUtils.MY_DOCUMENTS_PATH + "/VRCapture/";
#endif
            }
        }

        public static string FFmpegEditorFolder {
            get {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return VRCommonUtils.DATA_PATH + "/VRCapture/FFmpeg/Win/";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                return VRCommonUtils.DATA_PATH + "/VRCapture/FFmpeg/Mac/";
#else
                return "";
#endif
            }
        }

        public static string FFmpegEditorPath {
            get {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return FFmpegEditorFolder + "ffmpeg.exe";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                return FFmpegEditorFolder + "ffmpeg";
#else
                return "";
#endif
            }
        }

        public static string FFmpegBuildFolder {
            get {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return VRCommonUtils.STREAMING_ASSETS_PATH + "/VRCapture/FFmpeg/Win/";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                return VRCommonUtils.STREAMING_ASSETS_PATH + "/VRCapture/FFmpeg/Mac/";
#else
                return "";
#endif
            }
        }

        // TODO, fix path using Unity Build Pipeline
        public static string FFmpegBuildPath {
            get {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return FFmpegBuildFolder + "ffmpeg.exe";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                return FFmpegBuildFolder + "ffmpeg";
#else
                return "";
#endif
            }
        }

        public static string FFmpegPath {
            get {
#if UNITY_EDITOR
                return FFmpegEditorPath;
#else
                return FFmpegBuildPath;
#endif
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    ///                     Basic Utils for VRReplay.                        ///
    ////////////////////////////////////////////////////////////////////////////
    public class VRReplayUtils {

        public static string SaveFolder {
            get {
#if UNITY_ANDROID && !UNITY_EDITOR
                return VRCommonUtils.PERSISTENT_DATA_PATH + "/VRCapture/Replays/";
#else
                return VRCommonUtils.MY_DOCUMENTS_PATH + "/VRCapture/Replays/";
#endif
            }
        }

        //public const string UPLOAD_ADDRESS = "http://127.0.0.1:8000/replay/upload/";
        public const string UPLOAD_ADDRESS = "http://52.80.2.187:8000/replay/upload/";
        public const int MAX_DEVICE_NUM = 16;
    }

    ////////////////////////////////////////////////////////////////////////////
    ///                     VR device releated Utils.                        ///
    ////////////////////////////////////////////////////////////////////////////
    public class VRDeviceUtils {

        public static bool UseSteamVR {
            get {
                Type steamvr = Type.GetType("SteamVR");
                if (steamvr != null) {
                    return true;
                }
                return false;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    ///                     Serializable data struct.                        ///
    ////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Serializable Vector2 to replace Unity Vector2 struct.
    /// </summary>
    [Serializable]
    public struct SerializableVector2 {
        public float x;
        public float y;

        public SerializableVector2(Vector2 v2) {
            x = v2.x;
            y = v2.y;
        }

        public Vector2 ToVector2() {
            return new Vector3(x, y);
        }
    }
    /// <summary>
    /// Serializable Vector3 to replace Unity Vector3 struct.
    /// </summary>
    [Serializable]
    public struct SerializableVector3 {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 v3) {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public Vector3 ToVector3() {
            return new Vector3(x, y, z);
        }
    }
    /// <summary>
    /// Serializable Quaternion to replace Unity Quaternion struct.
    /// </summary>
    [Serializable]
    public struct SerializableQuaternion {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(Quaternion q) {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        public Quaternion ToQuaternion() {
            return new Quaternion(x, y, z, w);
        }
    }
    /// <summary>
    /// Serializable Transform to replace Unity Transform struct.
    /// </summary>
    [Serializable]
    public class SerializableTransform {
        public string Name { get; private set; }

        SerializableVector3 postion;
        SerializableQuaternion rotation;

        public SerializableTransform(string name, Vector3 pos, Quaternion rot) {
            Name = name;
            postion = new SerializableVector3(pos);
            rotation = new SerializableQuaternion(rot);
        }

        public Vector3 GetPostion() {
            return postion.ToVector3();
        }

        public Quaternion GetRotation() {
            return rotation.ToQuaternion();
        }
    }
}