using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class ShortcutsModules : EssentialsModule {

		protected override string Title { get { return "Shortcuts"; } }

		public ShortcutsModules(Essentials ess) : base(ess) { }

		public override void OnEnable(SerializedObject serializedObject) {
			
		}

		protected override void OnInspectorGUI() {
			EditorGUILayout.LabelField("[<]\t: Deselect GameObject", EditorStyles.label);
			EditorGUILayout.LabelField("[ctrl+w]\t: Switch wireframe/shaded mode", EditorStyles.label);
			EditorGUILayout.LabelField("[shift]\t: Surface move handle", EditorStyles.label);
		}

		protected override void OnDesactivated() {
			essentials.ResetModifiers();
		}

		public override void OnSceneGUI(SceneView scene) {
			Event e = Event.current;
			if (!Active && (e.type == EventType.KeyDown || e.type == EventType.KeyUp)) {
				return;
			}

			switch (e.type) {
				case EventType.KeyDown: // DOWN
					//Debug.Log(e.keyCode); // Pressed debug
					switch (e.keyCode) {
						case KeyCode.W: // Wireframe switch
							if (essentials.ctrl) {
								SwitchRenderMode(scene);
							}
							break;
						case KeyCode.Backslash: // Deselector
							DeselectAll();
							break;
					}
					break;

				case EventType.KeyUp: // UP
					break;
			}
		}

		private void SwitchRenderMode(SceneView sceneView) {
			SceneView.CameraMode camMode;
			camMode = sceneView.cameraMode;
			switch (camMode.drawMode) {
				default:
				case DrawCameraMode.Textured:
					camMode.drawMode = DrawCameraMode.TexturedWire;
					break;
				case DrawCameraMode.TexturedWire:
					camMode.drawMode = DrawCameraMode.Textured;
					break;
			}
			sceneView.cameraMode = camMode;
			sceneView.Repaint();
		}

		private void DeselectAll() {
			Selection.activeGameObject = null;
			essentials.selected = null;
		}

	}
}
