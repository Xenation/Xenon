using System.IO;

using UnityEditor;

using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Xenon.Screenshot {
	public class ScreenshotTool : EditorWindow {

		private static readonly string winTitle = "Screenshot Tool";
		private static readonly Vector2 winMinSize = new Vector2(300f, 300f);
		private static readonly Vector2Int maxTextureSize = new Vector2Int(8192, 8192);

		[MenuItem("Window/General/Screenshot Tool &#s", priority = 20)]
		public static void ShowWindow() {
			ScreenshotTool win = GetWindow<ScreenshotTool>();
			if (win == null) {
				Debug.LogWarning("Could not create Window!");
				return;
			}
			win.titleContent = new GUIContent(winTitle);
			win.minSize = winMinSize;
		}


		private bool useSceneCamera = false;
		private Camera targetCamera = null;
		private Vector2Int resolution = new Vector2Int(1920, 1080);
		private int superSize = 1;
		private bool saveToClipboard = false;
		private string directory = "Temp";
		private string filename = "screenshot.png";

		private void OnGUI() {
			// Camera
			useSceneCamera = EditorGUILayout.Toggle("Use Scene Camera", useSceneCamera);
			EditorGUI.BeginDisabledGroup(useSceneCamera);
			targetCamera = EditorGUILayout.ObjectField("Camera", targetCamera, typeof(Camera), true) as Camera;
			EditorGUI.EndDisabledGroup();
			// Resolution
			resolution = EditorGUILayout.Vector2IntField("Resolution", resolution);
			superSize = EditorGUILayout.IntSlider("Super Size", superSize, 1, 8);
			// Save
			saveToClipboard = EditorGUILayout.Toggle("Save To Clipboard", saveToClipboard);
			EditorGUI.BeginDisabledGroup(saveToClipboard);
			EditorGUILayout.BeginHorizontal();
			directory = EditorGUILayout.TextField("Directory", directory);
			if (GUILayout.Button("...")) {
				directory = EditorUtility.OpenFolderPanel("Screenshot Save Directory", directory, "");
			}
			EditorGUILayout.EndHorizontal();
			filename = EditorGUILayout.TextField("Filename", filename);
			EditorGUI.EndDisabledGroup();

			if (GUILayout.Button("Take Screenshot")) {
				string filepath = Path.Combine(directory, filename);

				Camera camera = targetCamera;
				bool isSceneView = false;
				if (useSceneCamera) {
					camera = SceneView.lastActiveSceneView.camera;
					isSceneView = true;
				} else if (camera == null) {
					Debug.Log("No Target Camera given! Falling back to main camera or any available.");
					camera = FindCamera();
					if (camera == null) {
						Debug.Log("No available camera found! Falling back to scene view camera.");
						camera = SceneView.lastActiveSceneView.camera;
						isSceneView = true;
					}
				}

				camera = CopyCamera(camera, isSceneView);

				if (camera != null) {
					TakeScreenshotFromCamera(camera, filepath);
				}

				DestroyImmediate(camera.gameObject);
			}
		}

		private void TakeScreenshotFromCamera(Camera camera, string filepath) {
			// Create the render texture used for each tile
			//RenderTextureDescriptor texDesc = new RenderTextureDescriptor(resolution.x, resolution.y, GraphicsFormat.R8G8B8_UNorm, 0, 0);
			RenderTextureDescriptor texDesc = new RenderTextureDescriptor(resolution.x, resolution.y, RenderTextureFormat.ARGB32, 0, 0);
			RenderTexture texture = RenderTexture.GetTemporary(texDesc);

			// Create the final texture
			Texture2D finalTexture = new Texture2D(texDesc.width * superSize, texDesc.height * superSize, TextureFormat.RGB24, false);

			// Render from the camera
			camera.targetTexture = texture;
			RenderTexture.active = texture;
			//camera.aspect = texDesc.width / texDesc.height;
			for (int y = 0; y < superSize; y++) {
				for (int x = 0; x < superSize; x++) {
					camera.projectionMatrix = ComputeTileProjectionMatrix(new Vector2Int(x, y), Vector2Int.one * superSize, camera.fieldOfView * Mathf.Deg2Rad, camera.nearClipPlane, camera.farClipPlane, camera.aspect);
					camera.Render();
					// Copy to the final texture
					finalTexture.ReadPixels(new Rect(Vector2.zero, resolution), texDesc.width * x, texDesc.height * y, false);
					finalTexture.Apply();
				}
			}
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(texture);

			// Save screenshot
			SaveScreenshot(finalTexture, filepath);
		}

		private void SaveScreenshot(Texture2D texture, string filepath) {
			if (saveToClipboard) {
				byte[] bmpBytes = Bitmap.GetDIBBytes(texture);
				Clipboard.SetImage(bmpBytes);
				Debug.Log($"Screenshot saved to clipboard");
			} else {
				byte[] pngBytes = texture.EncodeToPNG();
				File.WriteAllBytes(filepath, pngBytes);
				Debug.Log($"Screenshot saved: {filepath}");
			}
		}

		private Camera FindCamera() {
			Camera[] cameras = FindObjectsOfType<Camera>(true);
			if (cameras.Length == 0) return null;
			// Search for a main camera
			Camera firstActive = null;
			foreach (Camera camera in cameras) {
				if (camera.tag.Equals("main")) {
					return camera;
				}
				if (firstActive == null && camera.gameObject.activeInHierarchy) {
					firstActive = camera;
				}
			}
			// No main camera pick the first active, if there is one, otherwise the first in the array
			if (firstActive != null) {
				return firstActive;
			} else {
				return cameras[0];
			}
		}

		private Camera CopyCamera(Camera camera, bool isSceneView) {
			GameObject tmpCameraObj = GameObject.Instantiate(camera.gameObject);
			Camera tmpCamera = tmpCameraObj.GetComponent<Camera>();
			if (isSceneView) {
				tmpCamera.worldToCameraMatrix = camera.worldToCameraMatrix;
			}
			return tmpCamera;
		}

		private Matrix4x4 ComputeTileProjectionMatrix(Vector2Int tilePos, Vector2Int gridSize, float fov, float near, float far, float aspect) {
			float top = Mathf.Tan(fov * 0.50f) * near;
			float right = top * aspect;

			float shiftX = right * 2 / gridSize.x;
			float shiftY = top * 2 / gridSize.y;

			return Matrix4x4.Frustum(-right + shiftX * tilePos.x, -right + shiftX * (tilePos.x + 1), -top + shiftY * tilePos.y, -top + shiftY * (tilePos.y + 1), near, far);
		}

	}
}
