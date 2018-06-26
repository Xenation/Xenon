using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class Essentials : XenonWindow<Essentials> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 275f; } }
		protected override string titleStr { get { return "Essentials"; } }

		public Color colorX;
		public Color colorY;
		public Color colorZ;

		private SerializedObject serializedObject;

		public List<EssentialsModule> modules = new List<EssentialsModule>();
		public ShortcutsModules shortcutsModules;
		public SurfaceMoveModule surfaceMoveModule;
		public CameraToolsModule cameraToolsModule;

		// General
		public bool ShortcutsEnabled { get { return shortcutsModules.Active; } }
		public bool ctrl = false;
		public bool shift = false;
		public GameObject selected = null;

		public void InitModules() {
			shortcutsModules = new ShortcutsModules(this);
			surfaceMoveModule = new SurfaceMoveModule(this);
			cameraToolsModule = new CameraToolsModule(this);
			modules.Add(shortcutsModules);
			modules.Add(surfaceMoveModule);
			modules.Add(cameraToolsModule);
		}

		public void ResetModifiers() {
			ctrl = false;
			shift = false;
		}

		private void OnEnable() {
			InitModules();

			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			colorX = GetPrefColor("Scene/X Axis");
			colorY = GetPrefColor("Scene/Y Axis");
			colorZ = GetPrefColor("Scene/Z Axis");
			
			serializedObject = new SerializedObject(this);

			// Modules Init
			foreach (EssentialsModule module in modules) {
				module.OnEnable(serializedObject);
			}
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
			foreach (EssentialsModule module in modules) {
				module.InspectorGUI();
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void OnSceneGUI(SceneView scene) {
			GameObject nSelected = Selection.activeGameObject;
			if (selected != nSelected) {
				selected = nSelected;
				SelectionChanged();
			}

			Event e = Event.current;
			shift = e.shift;
			ctrl = e.control;

			foreach (EssentialsModule module in modules) {
				module.OnSceneGUI(scene);
			}

			switch (e.type) {
				case EventType.Repaint:
					SceneRepaint(scene);
					break;
			}
		}

		private void SceneRepaint(SceneView view) {
			// Nothing
		}

		private void SelectionChanged() {
			// Nothing
		}

	}
}
