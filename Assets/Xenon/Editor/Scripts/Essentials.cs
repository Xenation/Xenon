using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class Essentials : XenonWindow<Essentials> {

		private const int HIT_ARRAY_SIZE = 20;

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 275f; } }
		protected override string titleStr { get { return "Essentials"; } }

		private Color colorX;
		private Color colorY;
		private Color colorZ;

		private SerializedObject serializedObject;

		// General
		private bool shortcutsEnabled = true;
		private bool ctrl = false;
		private bool shift = false;
		private GameObject selected = null;

		// Surface Move
		private bool allowSurfaceMove = true;
		[SerializeField]
		private float surfaceMoveRange = 1000f;
		private SerializedProperty surfaceMoveRangeProp;
		private bool surfaceMoveRotateWithNormal = false;
		private bool surfaceMoveUseCustomLayer = false;
		private string[] availableLayersNames;
		private int selectedLayersNamesMask = 0;
		private int customSurfaceMoveLayerMask = 0;
		private bool surfaceMove = false;
		private RaycastHit[] surfaceHits = new RaycastHit[HIT_ARRAY_SIZE];
		private List<Collider> surfaceMoveIgnoredColliders = new List<Collider>();
		private GameObject surfaceMoveIgnoredGO = null;

		// Camera Fit
		private bool allowCameraFit = true;
		private Camera cameraToFit;
		private bool cameraFitUseCustomCamera = false;
		[SerializeField]
		private Camera customCameraToFit;
		private SerializedProperty cameraToFitProp;
		private bool applyCameraParameters;

		public void ResetModifiers() {
			ctrl = false;
			shift = false;
		}

		private void OnEnable() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			colorX = GetPrefColor("Scene/X Axis");
			colorY = GetPrefColor("Scene/Y Axis");
			colorZ = GetPrefColor("Scene/Z Axis");

			cameraToFit = Camera.main;
			customCameraToFit = Camera.main;

			// Serialized Properties
			serializedObject = new SerializedObject(this);
			surfaceMoveRangeProp = serializedObject.FindProperty("surfaceMoveRange");
			cameraToFitProp = serializedObject.FindProperty("customCameraToFit");
		}

		private Color GetPrefColor(string key) {
			Color c = new Color();
			string colStr = EditorPrefs.GetString(key, "NOT_FOUND");
			if (colStr != "NOT_FOUND") {
				string[] colVals = colStr.Split(';');
				c.r = float.Parse(colVals[1]);
				c.g = float.Parse(colVals[2]);
				c.b = float.Parse(colVals[3]);
				c.a = float.Parse(colVals[4]);
			}
			return c;
		}

		private void OnDestroy() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		private void OnGUI() {
			EditorGUILayout.LabelField("Shortcuts", EditorStyles.boldLabel);
			EditorGUI.indentLevel = 1;
			EditorGUILayout.LabelField("[<]\t: Deselect GameObject", EditorStyles.label);
			EditorGUILayout.LabelField("[ctrl+w]\t: Switch wireframe/shaded mode", EditorStyles.label);
			EditorGUILayout.LabelField("[shift]\t: Surface move handle", EditorStyles.label);

			EditorGUI.indentLevel = 0;
			EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
			EditorGUI.indentLevel = 1;
			shortcutsEnabled = EditorGUILayout.ToggleLeft("Shortcuts Enabled", shortcutsEnabled);
			if (!shortcutsEnabled) {
				ResetModifiers();
			}

			EditorGUI.indentLevel = 0;
			allowSurfaceMove = EditorGUILayout.ToggleLeft("Surface Move", allowSurfaceMove, EditorStyles.boldLabel);
			if (allowSurfaceMove) {
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(surfaceMoveRangeProp, new GUIContent("Range"));
				surfaceMoveRotateWithNormal = EditorGUILayout.ToggleLeft("Rotate With Surface Normal", surfaceMoveRotateWithNormal);
				surfaceMoveUseCustomLayer = EditorGUILayout.ToggleLeft("Use Custom LayerMask", surfaceMoveUseCustomLayer);
				if (surfaceMoveUseCustomLayer) {
					EditorGUI.indentLevel = 2;
					availableLayersNames = GetAllLayerNames();
					selectedLayersNamesMask = EditorGUILayout.MaskField("Layer Mask", selectedLayersNamesMask, availableLayersNames);
					customSurfaceMoveLayerMask = GetSelectedLayerMask();
				}
			}

			EditorGUI.indentLevel = 0;
			allowCameraFit = EditorGUILayout.ToggleLeft("Camera Fit", allowCameraFit, EditorStyles.boldLabel);
			if (allowCameraFit) {
				EditorGUI.indentLevel = 1;
				cameraFitUseCustomCamera = EditorGUILayout.ToggleLeft("Use Custom Camera", cameraFitUseCustomCamera);
				if (cameraFitUseCustomCamera) {
					EditorGUI.indentLevel = 2;
					EditorGUILayout.PropertyField(cameraToFitProp);
				}
				EditorGUI.indentLevel = 1;
				applyCameraParameters = EditorGUILayout.ToggleLeft("Apply Camera Parameters", applyCameraParameters);
				if (GUILayout.Button("Fit Camera")) {
					if (cameraFitUseCustomCamera) {
						PlaceCameraFromSceneView(SceneView.lastActiveSceneView, customCameraToFit, applyCameraParameters);
					} else {
						PlaceCameraFromSceneView(SceneView.lastActiveSceneView, cameraToFit, applyCameraParameters);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private string[] GetAllLayerNames() {
			List<string> layers = new List<string>();
			for (int i = 0; i < 32; i++) {
				string layerName = LayerMask.LayerToName(i);
				if (layerName.Length > 0) {
					layers.Add(layerName);
				}
			}
			return layers.ToArray();
		}

		private int GetSelectedLayerMask() {
			int mask = 0;
			for (int i = 0; i < 32; i++) {
				if ((selectedLayersNamesMask & (1 << i)) != 0) {
					mask |= 1 << LayerMask.NameToLayer(availableLayersNames[i]);
				}
			}
			return mask;
		}

		private void OnSceneGUI(SceneView scene) {
			GameObject nSelected = Selection.activeGameObject;
			if (selected != nSelected) {
				selected = nSelected;
				SelectionChanged();
			}

			UpdateHandles(scene);

			Event e = Event.current;
			if (!shortcutsEnabled && (e.type == EventType.KeyDown || e.type == EventType.KeyUp)) {
				return;
			}
			shift = e.shift;
			ctrl = e.control;
			switch (e.type) {
				case EventType.KeyDown: // DOWN
					//Debug.Log(e.keyCode); // Pressed debug
					switch (e.keyCode) {
						case KeyCode.W: // Wireframe switch
							if (ctrl) {
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

				case EventType.Repaint:
					SceneRepaint(scene);
					break;
			}
		}
		
		private void UpdateHandles(SceneView view) {
			
			if (shortcutsEnabled && allowSurfaceMove) { // Surface Move
				if (surfaceMove) {
					if (!shift || selected == null || Tools.current != Tool.Move) {
						surfaceMove = false;
						Tools.hidden = false;
					}
				} else {
					if (shift && selected != null && Tools.current == Tool.Move) {
						surfaceMove = true;
						Tools.hidden = true;
						// Update Ignored Colliders
						if (surfaceMoveIgnoredGO != selected) {
							selected.GetComponentsInChildren(surfaceMoveIgnoredColliders);
							surfaceMoveIgnoredGO = selected;
						}
					}
				}

				if (surfaceMove) {
					Vector3 pos = selected.transform.position;
					float arrowSize = HandleUtility.GetHandleSize(pos);
					float rectSize = arrowSize * .2f;

					EditorGUI.BeginChangeCheck();
					Handles.color = Color.cyan;
					pos = Handles.FreeMoveHandle(pos, Quaternion.identity, rectSize, Vector3.zero, Handles.RectangleHandleCap);
					Handles.color = colorX;
					pos = Handles.Slider(pos, Vector3.right, arrowSize, Handles.ArrowHandleCap, 0f);
					Handles.color = colorZ;
					pos = Handles.Slider(pos, Vector3.forward, arrowSize, Handles.ArrowHandleCap, 0f);
					Handles.color = colorY;
					pos = Handles.Slider(pos, Vector3.up, arrowSize, Handles.ArrowHandleCap, 0f);
					if (EditorGUI.EndChangeCheck()) {
						Ray ray = view.camera.ViewportPointToRay(view.camera.WorldToViewportPoint(pos));
						int mask = ~0;
						if (surfaceMoveUseCustomLayer) {
							mask = customSurfaceMoveLayerMask;
						}
						int surfaceHitsCount = Physics.RaycastNonAlloc(ray, surfaceHits, surfaceMoveRange, mask);
						if (surfaceHitsCount != 0) {
							Vector3 hitPoint = selected.transform.position;
							Vector3 hitNormal = selected.transform.up;
							float curDistance = surfaceMoveRange;
							for (int i = 0; i < surfaceHitsCount; i++) {
								if (surfaceMoveIgnoredColliders.Contains(surfaceHits[i].collider)) continue;
								if (surfaceHits[i].distance < curDistance) {
									curDistance = surfaceHits[i].distance;
									hitPoint = surfaceHits[i].point;
									hitNormal = surfaceHits[i].normal;
								}
							}
							Undo.RecordObject(selected.transform, "Surface Move");
							selected.transform.position = hitPoint;
							if (surfaceMoveRotateWithNormal) {
								selected.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
							}
							Repaint();
						}
					}
				}
			}
		}

		private void SceneRepaint(SceneView view) {
			
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
			selected = null;
		}

		private void SelectionChanged() {
			// Nothing
		}

		private void PlaceCameraFromSceneView(SceneView view, Camera cam, bool applyCamParams) {
			UnityEngine.Object[] toUndo;
			if (applyCamParams) {
				toUndo = new UnityEngine.Object[2];
				toUndo[0] = cam.transform;
				toUndo[1] = cam;
			} else {
				toUndo = new UnityEngine.Object[1];
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
