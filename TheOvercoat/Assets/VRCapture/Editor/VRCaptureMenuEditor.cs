using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VRCapture.Editor {
    // TODO, set VR device sdk here.
    public class VRCaptureMenuEditor {
        // Add a menu item with multiple levels of nesting
        [MenuItem("VRCapture/FFmpeg/Build FFmpeg for Win")]
        private static void BuildFFmpegForWin() {
            CopyBuildFile(
                VRCommonUtils.DATA_PATH + "/VRCapture/FFmpeg/Win/",
                VRCommonUtils.STREAMING_ASSETS_PATH + "/VRCapture/FFmpeg/Win/"
            );
        }

        [MenuItem("VRCapture/FFmpeg/Build FFmpeg for Mac")]
        private static void BuildFFmpegForMac() {
            CopyBuildFile(
                VRCommonUtils.DATA_PATH + "/VRCapture/FFmpeg/Mac/",
                VRCommonUtils.STREAMING_ASSETS_PATH + "/VRCapture/FFmpeg/Mac/"
            );
        }

        [MenuItem("VRCapture/Capture/Prepare for Panorama Capture")]
        private static void PrepareForPanoramaCapture() {
            // Change to gamma color space.
            // https://docs.unity3d.com/Manual/LinearLighting.html
            PlayerSettings.colorSpace = ColorSpace.Gamma;
        }

        [MenuItem("VRCapture/Capture/Prepare for Normal Capture")]
        private static void PrepareForNormalCapture() {
            PlayerSettings.colorSpace = ColorSpace.Linear;
        }

        /// <summary>
        /// Copy required file along with prod build.
        /// </summary>
        private static void CopyBuildFile(string sourcePath, string destPath) {
            if (Directory.Exists(sourcePath)) {
                if (!Directory.Exists(destPath)) {
                    Directory.CreateDirectory(destPath);
                }
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c => {
                    string destFile = Path.Combine(destPath, Path.GetFileName(c));
                    if (File.Exists(destFile)) {
                        File.Delete(destFile);
                    }
                    File.Copy(c, destFile);
                });
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c => {
                    string destDir = Path.Combine(destPath, Path.GetFileName(c));
                    CopyBuildFile(c, destDir);
                });
            }
        }
    }
}