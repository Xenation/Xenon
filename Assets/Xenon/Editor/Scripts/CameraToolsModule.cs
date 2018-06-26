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
		}

		public override void OnSceneGUI(SceneView scene) {
			
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
