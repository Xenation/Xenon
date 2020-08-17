using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Xenon {
	public static class XDebug {

		private enum DrawType {
			WireCube,
			Cube,
			WireSphere,
			Sphere
		}

		private struct DrawCall {
			public DrawType type;
			public Color color;
			public object[] parameters;
		}

		private static bool initialized = false;

		private static Queue<DrawCall> drawCalls = new Queue<DrawCall>();

#if UNITY_EDITOR
		private static void RegisterSceneDelegate() {
			if (initialized) return;
			initialized = true;
			Debug.Log("Loading delegate at " + EditorApplication.timeSinceStartup);
			SceneView.duringSceneGui += OnSceneGUI;
			System.AppDomain.CurrentDomain.DomainUnload += UnregisterSceneDelegate;
		}

		private static void UnregisterSceneDelegate(object sender, System.EventArgs args) {
			Debug.Log("Unloading delegate at " + EditorApplication.timeSinceStartup);
			SceneView.duringSceneGui -= OnSceneGUI;
		}
#endif

		private static void RegisterDrawCall(DrawType type, Color color, params object[] parameters) {
#if UNITY_EDITOR
			RegisterSceneDelegate();
			DrawCall call = new DrawCall() { type = type, color = color, parameters = parameters };
			drawCalls.Enqueue(call);
#endif
		}

		public static void DrawWireCube(Vector3 center, Vector3 size, Color color) {
			RegisterDrawCall(DrawType.WireCube, color, center, size);
		}

		public static void DrawCube(Vector3 center, Vector3 size, Color color) {
			RegisterDrawCall(DrawType.Cube, color, center, size);
		}

		public static void DrawWireSphere(Vector3 center, float radius, Color color) {
			RegisterDrawCall(DrawType.WireSphere, color, center, radius);
		}

		public static void DrawSphere(Vector3 center, float radius, Color color) {
			RegisterDrawCall(DrawType.Sphere, color, center, radius);
		}

#if UNITY_EDITOR
		private static void OnSceneGUI(SceneView view) {
			Debug.Log("SceneGUI - draw calls: " + drawCalls.Count);
			while (drawCalls.Count > 0) {
				ExecuteDrawCall(drawCalls.Dequeue());
			}
		}

		private static void ExecuteDrawCall(DrawCall call) {
			Handles.color = call.color;
			switch (call.type) {
				case DrawType.WireCube:
					Handles.DrawWireCube((Vector3) call.parameters[0], (Vector3) call.parameters[1]);
					break;
				case DrawType.Cube:
					//Handles.DrawCube((Vector3) call.parameters[0], (Vector3) call.parameters[1]);
					break;
				case DrawType.WireSphere:
					Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.up, (float) call.parameters[1]);
					Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.right, (float) call.parameters[1]);
					Handles.DrawWireDisc((Vector3) call.parameters[0], new Vector3(1, 0, 1).normalized, (float) call.parameters[1]);
					Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.forward, (float) call.parameters[1]);
					Handles.DrawWireDisc((Vector3) call.parameters[0], new Vector3(-1, 0, 1).normalized, (float) call.parameters[1]);
					break;
				case DrawType.Sphere:
					//Handles.DrawSphere((Vector3) call.parameters[0], (float) call.parameters[1]);
					break;
			}
		}
#endif

	}
}
