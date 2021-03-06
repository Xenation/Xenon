﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class SurfaceMoveModule : EssentialsModule {

		private enum SurfaceMoveMode {
			Pivot,
			BoxCollider,
			BoxRenderer
		}

		private const int HIT_ARRAY_SIZE = 20;

		protected override string Title { get { return "Surface Move"; } }
		
		private float range = 1000f;
		private bool rotateWithSurfaceNormal = false;
		private SurfaceMoveMode PlacementMode = SurfaceMoveMode.Pivot;
		private bool useCustomLayer = false;
		private string[] availableLayersNames;
		private int selectedLayersNamesMask = 0;
		private int customLayerMask = 0;
		private bool surfaceMove = false;
		private RaycastHit[] surfaceHits = new RaycastHit[HIT_ARRAY_SIZE];
		private List<Collider> ignoredColliders = new List<Collider>();
		private float selectedLowestBound = 0f;

		public SurfaceMoveModule(Essentials ess) : base(ess) { }

		public override void OnEnable(SerializedObject serializedObject) {
			
		}

		protected override void OnInspectorGUI() {
			range = EditorGUILayout.FloatField(new GUIContent("Range", "Range of the raycast"), range);
			PlacementMode = EdGUIPlus.EnumButtonsField("Placement Mode", PlacementMode, "Pivot", "BoxCol", "BoxRend");
			rotateWithSurfaceNormal = EditorGUILayout.ToggleLeft("Rotate With Surface Normal", rotateWithSurfaceNormal);
			useCustomLayer = EditorGUILayout.ToggleLeft("Use Custom LayerMask", useCustomLayer);
			if (useCustomLayer) {
				EditorGUI.indentLevel = 2;
				availableLayersNames = GetAllLayerNames();
				selectedLayersNamesMask = EditorGUILayout.MaskField("Layer Mask", selectedLayersNamesMask, availableLayersNames);
				customLayerMask = GetSelectedLayerMask();
			}
		}

		public override void OnSceneGUI(SceneView scene) {
			OnHandlesUpdate(scene);
		}

		private void OnHandlesUpdate(SceneView view) {

			if (essentials.ShortcutsEnabled && Active) { // Surface Move
				if (surfaceMove) {
					if (!essentials.shift || essentials.selected == null || Tools.current != Tool.Move) {
						surfaceMove = false;
						Tools.hidden = false;
					}
				} else {
					if (essentials.shift && essentials.selected != null && Tools.current == Tool.Move) {
						surfaceMove = true;
						Tools.hidden = true;
						// Update Ignored Colliders
						essentials.selected.GetComponentsInChildren(ignoredColliders);
						switch (PlacementMode) {
							case SurfaceMoveMode.Pivot:
								break;
							case SurfaceMoveMode.BoxCollider:
								selectedLowestBound = GetLowestCollisionBound(essentials.selected);
								break;
							case SurfaceMoveMode.BoxRenderer:
								selectedLowestBound = GetLowestRenderingBound(essentials.selected);
								break;
						}
					}
				}

				if (surfaceMove) {
					Vector3 pos = essentials.selected.transform.position;
					float arrowSize = HandleUtility.GetHandleSize(pos);
					float rectSize = arrowSize * .2f;

					EditorGUI.BeginChangeCheck();
					Handles.color = Color.cyan;
					pos = Handles.FreeMoveHandle(pos, Quaternion.identity, rectSize, Vector3.zero, Handles.RectangleHandleCap);
					Handles.color = essentials.colorX;
					pos = Handles.Slider(pos, Vector3.right, arrowSize, Handles.ArrowHandleCap, 0f);
					Handles.color = essentials.colorZ;
					pos = Handles.Slider(pos, Vector3.forward, arrowSize, Handles.ArrowHandleCap, 0f);
					Handles.color = essentials.colorY;
					pos = Handles.Slider(pos, Vector3.up, arrowSize, Handles.ArrowHandleCap, 0f);
					if (EditorGUI.EndChangeCheck()) {
						if (PlacementMode != SurfaceMoveMode.Pivot) {
							pos.y += selectedLowestBound;
						}
						Ray ray = view.camera.ViewportPointToRay(view.camera.WorldToViewportPoint(pos));
						int mask = ~0;
						if (useCustomLayer) {
							mask = customLayerMask;
						}
						int surfaceHitsCount = Physics.RaycastNonAlloc(ray, surfaceHits, range, mask);
						if (surfaceHitsCount != 0) {
							Vector3 hitPoint = essentials.selected.transform.position;
							Vector3 hitNormal = essentials.selected.transform.up;
							float curDistance = range;
							for (int i = 0; i < surfaceHitsCount; i++) {
								if (ignoredColliders.Contains(surfaceHits[i].collider)) continue;
								if (surfaceHits[i].distance < curDistance) {
									curDistance = surfaceHits[i].distance;
									hitPoint = surfaceHits[i].point;
									hitNormal = surfaceHits[i].normal;
								}
							}

							if (PlacementMode != SurfaceMoveMode.Pivot) {
								hitPoint.y -= selectedLowestBound;
							}

							Undo.RecordObject(essentials.selected.transform, "Surface Move");
							essentials.selected.transform.position = hitPoint;
							if (rotateWithSurfaceNormal) {
								essentials.selected.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
							}
							essentials.Repaint();
						}
					}
				}
			}

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

		private float GetLowestCollisionBound(GameObject gameObject) {
			float lowestBound = 0f;
			float currentBound = 0f;
			List<Collider> colliders = new List<Collider>();
			gameObject.GetComponentsInChildren(colliders);
			foreach (Collider collider in colliders) {
				currentBound = collider.bounds.min.y - gameObject.transform.position.y;
				if (currentBound < lowestBound) {
					lowestBound = currentBound;
				}
			}
			return lowestBound;
		}

		private float GetLowestRenderingBound(GameObject gameObject) {
			float lowestBound = 0f;
			float currentBound = 0f;
			List<Renderer> renderers = new List<Renderer>();
			gameObject.GetComponentsInChildren(renderers);
			foreach (Renderer renderer in renderers) {
				currentBound = renderer.bounds.min.y - gameObject.transform.position.y;
				if (currentBound < lowestBound) {
					lowestBound = currentBound;
				}
			}
			return lowestBound;
		}

	}
}
