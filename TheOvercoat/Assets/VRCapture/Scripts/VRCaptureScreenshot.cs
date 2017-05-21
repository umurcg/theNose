using UnityEngine;
using System.Collections;

namespace VRCapture {

    [RequireComponent(typeof(Camera))]
    public class VRCaptureScreenshot : MonoBehaviour {

        public Material transformMaterial;
        public int delaySeconds = 10;

        Camera videoCamera;
        Texture2D frameTexture;
        RenderTexture frameRenderTexture;
        Cubemap frameCubemap;

        void Awake() {
            videoCamera = GetComponent<Camera>();

            frameRenderTexture = new RenderTexture(4096, 2048, 24);
            frameRenderTexture.antiAliasing = 4;
            frameRenderTexture.wrapMode = TextureWrapMode.Clamp;
            frameRenderTexture.filterMode = FilterMode.Trilinear;
            frameRenderTexture.anisoLevel = 0;
            frameRenderTexture.hideFlags = HideFlags.HideAndDontSave;
            frameRenderTexture.Create();

            videoCamera.targetTexture = frameRenderTexture;
            videoCamera.aspect = 1.0f;
            videoCamera.fieldOfView = 90;

            frameCubemap = new Cubemap(1024, TextureFormat.RGB24, false);

            frameTexture = new Texture2D(4096, 2048, TextureFormat.RGB24, false);
            frameTexture.hideFlags = HideFlags.HideAndDontSave;
            frameTexture.wrapMode = TextureWrapMode.Clamp;
            frameTexture.filterMode = FilterMode.Trilinear;
            frameTexture.hideFlags = HideFlags.HideAndDontSave;
            frameTexture.anisoLevel = 0;

            Time.maximumDeltaTime = Time.fixedDeltaTime;

            if (delaySeconds > 0) {
                StartCoroutine(AutoCapture(delaySeconds));
            }
        }

        IEnumerator AutoCapture(int seconds) {
            Debug.Log("AutoCapture");
            yield return new WaitForSeconds(seconds);
            Capture();
        }

        public void Capture() {
            Debug.Log("Capture");
            int width = 1024;
            int height = 1024;

            CubemapFace[] faces = new CubemapFace[] {
                CubemapFace.PositiveX,
                CubemapFace.NegativeX,
                CubemapFace.PositiveY,
                CubemapFace.NegativeY,
                CubemapFace.PositiveZ,
                CubemapFace.NegativeZ
            };
            Vector3[] faceAngles = new Vector3[] {
                new Vector3(0.0f, 90.0f, 0.0f),
                new Vector3(0.0f, -90.0f, 0.0f),
                new Vector3(-90.0f, 0.0f, 0.0f),
                new Vector3(90.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 180.0f, 0.0f)
            };
            videoCamera.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            // Create cubemap face render texture.
            RenderTexture faceTexture = new RenderTexture(width, height, 24);
            faceTexture.antiAliasing = 4;
#if !(UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3)
            faceTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
#endif
            faceTexture.hideFlags = HideFlags.HideAndDontSave;
            // For intermediate saving
            Texture2D swapTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            swapTexture.hideFlags = HideFlags.HideAndDontSave;
            // Prepare for target render texture.
            videoCamera.targetTexture = faceTexture;

            Color[] mirroredPixels = new Color[swapTexture.height * swapTexture.width];
            for (int i = 0; i < faces.Length; i++) {
                videoCamera.transform.eulerAngles = faceAngles[i];
                videoCamera.Render();
                RenderTexture.active = faceTexture;
                swapTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
                // Mirror vertically to meet the standard of unity cubemap.
                Color[] OrignalPixels = swapTexture.GetPixels();
                for (int y1 = 0; y1 < height; y1++) {
                    for (int x1 = 0; x1 < width; x1++) {
                        mirroredPixels[y1 * width + x1] = OrignalPixels[((height - 1 - y1) * width) + x1];
                    }
                }
                frameCubemap.SetPixels(mirroredPixels, faces[i]);
            }
            frameCubemap.SmoothEdges();
            frameCubemap.Apply();
            // Convert to equirectangular projection.
            Graphics.Blit(frameCubemap, frameRenderTexture, transformMaterial);
            // Bind texture.
            RenderTexture.active = frameRenderTexture;
            // TODO, remove expensive step of copying pixel data from GPU to CPU.
            frameTexture.ReadPixels(new Rect(0, 0, 4096, 2048), 0, 0, false);
            frameTexture.Apply();
            // Save frameTexture to file.
            try {
                // Encode the texture and save it to disk
                byte[] bytes = frameTexture.EncodeToPNG();
                string path = VRCaptureUtils.SaveFolder + VRCommonUtils.GetPngFileName();
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log("Equirectangular file created " + path);
            }
            catch (System.Exception e) {
                Debug.LogError("Failed to save equirectangular file since " + e.ToString());
                return;
            }
            // Restore RenderTexture states.
            RenderTexture.active = null;

            RenderTexture.active = null;
            videoCamera.targetTexture = null;

            // Clean temp texture.
            DestroyImmediate(swapTexture);
            DestroyImmediate(faceTexture);

        }
    }
}