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
		private string directory = "Temp";
		private string filename = "screenshot.png";

		private void OnGUI() {
			useSceneCamera = EditorGUILayout.Toggle("Use Scene Camera", useSceneCamera);
			targetCamera = EditorGUILayout.ObjectField("Camera", targetCamera, typeof(Camera), true) as Camera;
			resolution = EditorGUILayout.Vector2IntField("Resolution", resolution);
			superSize = EditorGUILayout.IntSlider("Super Size", superSize, 1, 8);
			EditorGUILayout.BeginHorizontal();
			directory = EditorGUILayout.TextField("Directory", directory);
			if (GUILayout.Button("...")) {
				directory = EditorUtility.OpenFolderPanel("Screenshot Save Directory", directory, "");
			}
			EditorGUILayout.EndHorizontal();
			filename = EditorGUILayout.TextField("Filename", filename);

			if (GUILayout.Button("Take Screenshot")) {
				string filepath = Path.Combine(directory, filename);
				Debug.Log(filepath);

				Camera camera = targetCamera;
				if (useSceneCamera) {
					camera = SceneView.lastActiveSceneView.camera;
				} else if (camera == null) {
					Debug.Log("No Target Camera given! Falling back to main camera or any available.");
					camera = FindCamera();
					if (camera == null) {
						Debug.Log("No available camera found! Falling back to scene view camera.");
						camera = SceneView.lastActiveSceneView.camera;
					}
				}

				if (camera != null) {
					TakeScreenshotFromCamera(camera, filepath);
				}
			}
		}

		private void TakeScreenshotFromCamera(Camera camera, string filepath) {
			RenderTexture tmpTex = camera.targetTexture;

			// Create the render texture
			//RenderTextureDescriptor texDesc = new RenderTextureDescriptor(resolution.x, resolution.y, GraphicsFormat.R8G8B8_UNorm, 0, 0);
			RenderTextureDescriptor texDesc = new RenderTextureDescriptor(resolution.x, resolution.y, RenderTextureFormat.ARGB32, 0, 0);
			RenderTexture texture = RenderTexture.GetTemporary(texDesc);

			// Render from the camera
			camera.targetTexture = texture;
			camera.Render();

			// Copy to a classic texture
			Texture2D finalTexture = new Texture2D(texDesc.width, texDesc.height, TextureFormat.RGB24, false);
			RenderTexture.active = texture;
			finalTexture.ReadPixels(new Rect(Vector2.zero, resolution), 0, 0, false);
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(texture);

			// Write file
			byte[] pngBytes = finalTexture.EncodeToPNG();
			File.WriteAllBytes(filepath, pngBytes);

			camera.targetTexture = tmpTex;
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

	}
}
