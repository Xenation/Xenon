using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	[System.Serializable]
	public class CameraToolsModule : EssentialsModule {

		protected override string Title { get { return "Camera Tools"; } }
		
		private bool useCustomCamera = false;
		[SerializeField]
		private Camera customCameraToFit;
		private SerializedProperty customCameraToFitProp; // SerializedProp used here only to allow simple camera only field (does not save selection)
		private bool applyCameraParameters;
		private float sceneCameraNearPlane;
		private float sceneCameraFarPlane;
		private SceneView sceneOfPlanes;

		public CameraToolsModule(Essentials ess) : base(ess) { }

		public override void OnEnable(SerializedObject serializedObject) {
			customCameraToFitProp = serializedObject.FindProperty("cameraToolsModule.customCameraToFit");
		}

		protected override void OnInspectorGUI() {
			useCustomCamera = EditorGUILayout.ToggleLeft("Use Custom Camera", useCustomCamera);
			if (useCustomCamera) {
				EditorGUI.indentLevel = 2;
				EditorGUILayout.PropertyField(customCameraToFitProp);
			}
			EditorGUI.indentLevel = 1;
			applyCameraParameters = EditorGUILayout.ToggleLeft("Apply Camera Parameters", applyCameraParameters);
			if (EdGUIPlus.Button("Fit Camera", EditorStyles.miniButton)) {
				if (useCustomCamera) {
					PlaceCameraFromSceneView(SceneView.lastActiveSceneView, customCameraToFit, applyCameraParameters);
				} else {
					PlaceCameraFromSceneView(SceneView.lastActiveSceneView, Camera.main, applyCameraParameters);
				}
			}

			if (sceneOfPlanes != null) {
				Rect totalRect = EditorGUILayout.GetControlRect();
				Rect fieldRect = EditorGUI.PrefixLabel(totalRect, new GUIContent("Scene Cam"));
				EditorGUI.indentLevel = 0;
				float floatFieldWidth = (fieldRect.width - 50f) / 2f;
				Rect curRect = new Rect(fieldRect.x, fieldRect.y, 32f, fieldRect.height);
				GUI.Label(curRect, "Near");
				curRect.x += 32f;
				curRect.width = floatFieldWidth - 32f;
				float nSceneCameraNearPlane = EditorGUI.DelayedFloatField(curRect, sceneOfPlanes.camera.nearClipPlane);
				curRect.x += floatFieldWidth - 17f;
				curRect.width = 25f;
				GUI.Label(curRect, "Far");
				curRect.x += 25f;
				curRect.width = floatFieldWidth - 40f;
				float nSceneCameraFarPlane = EditorGUI.DelayedFloatField(curRect, sceneOfPlanes.camera.farClipPlane);
				curRect.x += floatFieldWidth - 40f;
				curRect.width = 50f;
				if (nSceneCameraNearPlane != sceneOfPlanes.camera.nearClipPlane || nSceneCameraFarPlane != sceneOfPlanes.camera.farClipPlane) {
					//sceneOfPlanes.camera.nearClipPlane = 0.1f;
					//sceneOfPlanes.camera.farClipPlane = 20000f;
					sceneOfPlanes.camera.projectionMatrix = Matrix4x4.Perspective(sceneOfPlanes.camera.fieldOfView, sceneOfPlanes.camera.aspect, nSceneCameraNearPlane, nSceneCameraFarPlane);
					//sceneOfPlanes.camera.worldToCameraMatrix = Matrix4x4.Perspective(sceneOfPlanes.camera.fieldOfView, sceneOfPlanes.camera.aspect, nSceneCameraNearPlane, nSceneCameraFarPlane);
					Debug.Log(sceneOfPlanes.camera.nearClipPlane);
					Debug.Log(sceneOfPlanes.camera.farClipPlane);
				}
			}
		}

		public override void OnSceneGUI(SceneView scene) {
			float nSceneCameraNearPlane = scene.camera.nearClipPlane;
			float nSceneCameraFarPlane = scene.camera.farClipPlane;
			if (nSceneCameraNearPlane != sceneCameraNearPlane || nSceneCameraFarPlane != sceneCameraFarPlane) {
				sceneCameraNearPlane = nSceneCameraNearPlane;
				sceneCameraFarPlane = nSceneCameraFarPlane;
				sceneOfPlanes = scene;
				essentials.Repaint();
			}
		}

		private void PlaceCameraFromSceneView(SceneView view, Camera cam, bool applyCamParams) {
			Object[] toUndo;
			if (applyCamParams) {
				toUndo = new Object[2];
				toUndo[0] = cam.transform;
				toUndo[1] = cam;
			} else {
				toUndo = new Object[1];
				toUndo[0] = cam.transform;
			}
			Undo.RecordObjects(toUndo, "Camera Fit");
			cam.transform.position = view.camera.transform.position;
			cam.transform.rotation = view.camera.transform.rotation;
			if (applyCamParams) {
				cam.fieldOfView = view.camera.fieldOfView;
			}
		}

	}
}
