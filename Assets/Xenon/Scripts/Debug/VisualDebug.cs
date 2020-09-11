using System.Collections.Generic;

using Unity.Collections;
//#if UNITY_EDITOR
using UnityEditor;
//#endif
using UnityEngine;
using UnityEngine.Rendering;

namespace Xenon {
	public static class VisualDebug {

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
		public struct WireVertex {
			public Vector3 position;
			public Color32 color;

			public WireVertex(Vector3 position, Color32 color) {
				this.position = position;
				this.color = color;
			}
		}

		private enum DrawType {
			WireCube,
			Cube,
			WireSphere,
			Sphere,
			WireMesh
		}

		private struct DrawCall {
			public DrawType type;
			public Color color;
			public object[] parameters;
		}

		private static bool initialized = false;
		private static VisualDebugProxy proxy;
		private static bool gameFrameCompleted = false;
		private static int previousFrameCount = 0;

		private static Queue<DrawCall> drawCalls = new Queue<DrawCall>();

		private const int WIRE_MESH_VERTEX_CAPACITY = 65536;
		private static int wireMeshVertexCount = 0;
		private const int WIRE_MESH_INDICES_INCREMENT = 512;
		private static int wireMeshIndexCapacity = WIRE_MESH_INDICES_INCREMENT;
		private static int wireMeshIndexCount = 0;
		private static bool wireMeshIndexOverCap = false;
		private static VertexAttributeDescriptor[] wireMeshAttr = new VertexAttributeDescriptor[] {
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4)
		};
		private static Mesh wireMesh;

		private static NativeArray<WireVertex> wireVertices;
		private static NativeArray<ushort> wireIndices;

//#if UNITY_EDITOR
		private static void Initialize() {
			if (initialized) return;
			initialized = true;

			// Register the delegates
			Debug.Log("Loading delegate at " + EditorApplication.timeSinceStartup);
			SceneView.duringSceneGui += OnSceneGUI;
			System.AppDomain.CurrentDomain.DomainUnload += UnregisterSceneDelegate;

			// Create Proxy GameObject
			GameObject proxyObj = new GameObject("VisualDebugProxy");
			proxy = proxyObj.AddComponent<VisualDebugProxy>();
			proxyObj.hideFlags = HideFlags.HideAndDontSave;
			proxy.onPostRender += OnPostRender;

			// Initialize Meshes
			wireMesh = new Mesh();
			wireMesh.hideFlags = HideFlags.HideAndDontSave;
			wireVertices = new NativeArray<WireVertex>(WIRE_MESH_VERTEX_CAPACITY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			wireIndices = new NativeArray<ushort>(wireMeshIndexCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			PrepareMeshes();
		}

		private static void UnregisterSceneDelegate(object sender, System.EventArgs args) {
			// Unregister delegates
			Debug.Log("Unloading delegate at " + EditorApplication.timeSinceStartup);
			SceneView.duringSceneGui -= OnSceneGUI;
			
			// Destroy Meshes
			Object.DestroyImmediate(wireMesh);
			wireVertices.Dispose();
			wireIndices.Dispose();

			// Destroy Proxy GameObject
			Object.DestroyImmediate(proxy.gameObject);
		}

		private static void OnPostRender() {
			gameFrameCompleted = true;
		}
//#endif

		private static void RegisterDrawCall(DrawType type, Color color, params object[] parameters) {
#if UNITY_EDITOR
			Initialize();
			DrawCall call = new DrawCall() { type = type, color = color, parameters = parameters };
			drawCalls.Enqueue(call);
			//drawCalls.Add(call);
#else
			Debug.LogWarning("Trying to register a debug draw on a non editor build!");
#endif
		}

		#region Draw Functions

		/*
		 * WIRE
		 * - Line
		 * - PolyLine
		 * - Ray
		 * - Circle
		 * - Cube/Box
		 * - Sphere
		 * - Cylinder
		 * - Cone
		 * - Frustum
		 * - (Mesh)
		 * SEMI SOLID
		 * - Disc
		 * - Cube/Box
		 * - Sphere
		 * - Cylinder
		 * - Cone
		 * - Frustum
		 * - (Mesh)
		 */

		private static uint drawCallCount = 0;

		public static void Wire(params WireVertex[] points) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			if (points.Length < 2) return;
			AddWireVertex(points[0]);
			for (int i = 1; i < points.Length; i++) {
				AddWireVertex(points[i]);
				AddWireIndex(wireMeshVertexCount - 2);
				AddWireIndex(wireMeshVertexCount - 1);
			}
		}

		public static void Line(Vector3 from, Vector3 to, Color color) {
			Line(from, to, color, color);
		}

		public static void Line(Vector3 from, Vector3 to, Color fromColor, Color toColor) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			AddWireVertex(from, fromColor);
			AddWireVertex(to, toColor);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 1);
		}

		public static void PolyLine(Color color, params Vector3[] points) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			if (points.Length < 2) return;
			AddWireVertex(points[0], color);
			for (int i = 1; i < points.Length; i++) {
				AddWireVertex(points[i], color);
				AddWireIndex(wireMeshVertexCount - 2);
				AddWireIndex(wireMeshVertexCount - 1);
			}
		}

		public static void Ray(Ray ray, float length, Color color) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			AddWireVertex(ray.origin, color);
			AddWireVertex(ray.origin + ray.direction * length, color);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 1);
		}

		public static void Circle(Vector3 center, Vector3 normal, float radius, Color color) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			Vector3 right;
			if (Vector3.Dot(Vector3.up, normal) > 0.95f) {
				right = Vector3.Cross(normal, Vector3.forward);
			} else {
				right = Vector3.Cross(normal, Vector3.up);
			}
			right.Normalize();
			Vector3 up = Vector3.Cross(right, normal);
			up.Normalize();
			const int subDivs = 32;
			for (int i = 0; i < subDivs; i++) {
				float perim = (i / (float) subDivs) * Mathf.PI * 2f;
				AddWireVertex(center + Mathf.Cos(perim) * radius * right + Mathf.Sin(perim) * radius * up, color);
			}
			for (int i = 0; i < subDivs - 1; i++) {
				AddWireIndex(wireMeshVertexCount - i - 1);
				AddWireIndex(wireMeshVertexCount - i - 2);
			}
			AddWireIndex(wireMeshVertexCount - subDivs);
			AddWireIndex(wireMeshVertexCount - 1);
		}

		public static void WireCube(Vector3 center, Vector3 size, Color color) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			Vector3 extents = size * 0.5f;
			AddWireVertex(new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z), color);
			AddWireVertex(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z), color);
			AddWireVertex(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z), color);
			AddWireVertex(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z), color);
			AddWireVertex(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z), color);
			AddWireVertex(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z), color);
			AddWireVertex(new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z), color);
			AddWireVertex(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z), color);
			AddWireIndex(wireMeshVertexCount - 8);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 8);
			AddWireIndex(wireMeshVertexCount - 4);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 1);
			AddWireIndex(wireMeshVertexCount - 1);
			AddWireIndex(wireMeshVertexCount - 4);
			AddWireIndex(wireMeshVertexCount - 8);
			AddWireIndex(wireMeshVertexCount - 4);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 1);
		}

		public static void WireSphere(Vector3 center, float radius, Color color) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			Circle(center, Vector3.up, radius, color);
			Circle(center, Vector3.right, radius, color);
			Circle(center, Vector3.forward, radius, color);
			Circle(center, new Vector3(0.7071067f, 0f, 0.7071067f), radius, color);
			Circle(center, new Vector3(-0.7071067f, 0f, 0.7071067f), radius, color);
		}

		public static void Frustum(Matrix4x4 viewMatrix, Matrix4x4 projMatrix, float near, float far, Color color) {
			Frustum(viewMatrix, projMatrix, near, far, color, color);
		}

		public static void Frustum(Matrix4x4 viewMatrix, Matrix4x4 projMatrix, float near, float far, Color nearColor, Color farColor) {
			Initialize();
			if (gameFrameCompleted) return;
			drawCallCount++;
			Matrix4x4 inv = viewMatrix.inverse * projMatrix.inverse;
			AddWireVertex(inv * new Vector4(-near, -near, -near, near), nearColor);
			AddWireVertex(inv * new Vector4(-near, near, -near, near), nearColor);
			AddWireVertex(inv * new Vector4(near, near, -near, near), nearColor);
			AddWireVertex(inv * new Vector4(near, -near, -near, near), nearColor);
			AddWireVertex(inv * new Vector4(-far, -far, far, far), farColor);
			AddWireVertex(inv * new Vector4(-far, far, far, far), farColor);
			AddWireVertex(inv * new Vector4(far, far, far, far), farColor);
			AddWireVertex(inv * new Vector4(far, -far, far, far), farColor);
			// Near
			AddWireIndex(wireMeshVertexCount - 8);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 8);
			// Far
			AddWireIndex(wireMeshVertexCount - 4);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 1);
			AddWireIndex(wireMeshVertexCount - 1);
			AddWireIndex(wireMeshVertexCount - 4);
			// Edges
			AddWireIndex(wireMeshVertexCount - 8);
			AddWireIndex(wireMeshVertexCount - 4);
			AddWireIndex(wireMeshVertexCount - 7);
			AddWireIndex(wireMeshVertexCount - 3);
			AddWireIndex(wireMeshVertexCount - 6);
			AddWireIndex(wireMeshVertexCount - 2);
			AddWireIndex(wireMeshVertexCount - 5);
			AddWireIndex(wireMeshVertexCount - 1);
		}

		public static void WireMesh(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, Color color) {
			RegisterDrawCall(DrawType.WireMesh, color, mesh, position, rotation, scale);
		}

		public static void WireDisk(Vector3 center, Vector3 normal, float radius, Color color) {

		}

		public static void WireCone(Vector3 topPos, Vector3 basePos, float baseRadius, Color color) {

		}

		public static void Cube(Vector3 center, Vector3 size, Color color) {
			RegisterDrawCall(DrawType.Cube, color, center, size);
		}

		public static void Sphere(Vector3 center, float radius, Color color) {
			RegisterDrawCall(DrawType.Sphere, color, center, radius);
		}
		#endregion

		#region Mesh Management
		private static void PrepareMeshes() {
			drawCallCount = 0;
			wireMeshVertexCount = 0;
			int wireMeshPrevIndexCapacity = wireMeshIndexCapacity;
			wireMeshIndexCapacity = (wireMeshIndexCount / WIRE_MESH_INDICES_INCREMENT + 1) * WIRE_MESH_INDICES_INCREMENT;
			wireMeshIndexCount = 0;
			wireMeshIndexOverCap = false;
			//Debug.Log($"Preparing wireMesh: i_cap{wireMeshIndexCapacity} prev_i_cap{wireMeshPrevIndexCapacity}");

			if (wireMeshPrevIndexCapacity != wireMeshIndexCapacity) {
				wireIndices.Dispose();
				wireIndices = new NativeArray<ushort>(wireMeshIndexCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
		}

		private static void AddWireVertex(Vector3 pos, Color color) {
			if (wireMeshVertexCount >= WIRE_MESH_VERTEX_CAPACITY) {
				Debug.LogWarning($"[Xenon.VisualDebug] Too much vertices are being generated, cap is at {WIRE_MESH_VERTEX_CAPACITY}");
				return;
			}
			//wireVertices.ReinterpretStore(wireMeshVertexCount * 4, pos.x);
			//wireVertices.ReinterpretStore(wireMeshVertexCount * 4 + 1, pos.y);
			//wireVertices.ReinterpretStore(wireMeshVertexCount * 4 + 2, pos.z);
			//wireVertices.ReinterpretStore(wireMeshVertexCount * 4 + 3, (Color32) color);
			
			wireVertices[wireMeshVertexCount++] = new WireVertex() { position = pos, color = color };
		}

		private static void AddWireVertex(WireVertex vertex) {
			if (wireMeshVertexCount >= WIRE_MESH_VERTEX_CAPACITY) {
				Debug.LogWarning($"[Xenon.VisualDebug] Too much vertices are being generated, cap is at {WIRE_MESH_VERTEX_CAPACITY}");
				return;
			}
			wireVertices[wireMeshVertexCount++] = vertex;
		}

		private static void AddWireIndex(int index) {
			if (wireMeshIndexOverCap) return;
			if (index >= WIRE_MESH_VERTEX_CAPACITY) {
				wireMeshIndexOverCap = true;
				return;
			}
			if (wireMeshIndexCount >= wireMeshIndexCapacity) {
				wireMeshIndexCount++;
				return;
			}
			wireIndices[wireMeshIndexCount++] = (ushort) index;
		}

		private static void FinalizeMeshes() {
			//Debug.Log($"Finalizing the wireMesh: v{wireMeshVertexCount} i{wireMeshIndexCount} i_cap{wireMeshIndexCapacity}");
			wireMesh.SetVertexBufferParams(wireMeshVertexCount, wireMeshAttr);
			wireMesh.SetVertexBufferData(wireVertices, 0, 0, wireMeshVertexCount);
			int effectiveIndexCount = Mathf.Min(wireMeshIndexCount, wireMeshIndexCapacity);
			wireMesh.SetIndexBufferParams(effectiveIndexCount, IndexFormat.UInt16);
			wireMesh.SetIndexBufferData(wireIndices, 0, 0, effectiveIndexCount, MeshUpdateFlags.DontValidateIndices);
			wireMesh.SetSubMesh(0, new SubMeshDescriptor(0, effectiveIndexCount, MeshTopology.Lines));

			Debug.Log($"Finilizing wireMesh with {drawCallCount} draw calls");
		}
		#endregion

		//#if UNITY_EDITOR
		private static void OnSceneGUI(SceneView view) {
			//Debug.Log("SceneGUI - draw calls: " + drawCalls.Count);
			// HACK for some reason duringSceneGui callback is called twice per frame (per sceneview) and the first call does not render anything so we need to skip it
			if (previousFrameCount != Time.frameCount) {
				previousFrameCount = Time.frameCount;
				return;
			}
			previousFrameCount = Time.frameCount;

			FinalizeMeshes();

			Material mat = new Material(Shader.Find("Hidden/Xenon/DebugWire"));
			mat.SetPass(0);
			Graphics.DrawMeshNow(wireMesh, Matrix4x4.identity);
			while (drawCalls.Count > 0) {
				ExecuteDrawCall(drawCalls.Dequeue());
			}

			PrepareMeshes();
			gameFrameCompleted = false;
		}

		private static void ExecuteDrawCall(DrawCall call) {
			return;
			//Handles.color = call.color;
			//switch (call.type) {
			//	case DrawType.WireCube:
			//		Handles.DrawWireCube((Vector3) call.parameters[0], (Vector3) call.parameters[1]);
			//		break;
			//	case DrawType.Cube:
			//		// TODO implement
			//		//Handles.DrawCube((Vector3) call.parameters[0], (Vector3) call.parameters[1]);
			//		break;
			//	case DrawType.WireSphere:
			//		Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.up, (float) call.parameters[1]);
			//		Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.right, (float) call.parameters[1]);
			//		Handles.DrawWireDisc((Vector3) call.parameters[0], new Vector3(1, 0, 1).normalized, (float) call.parameters[1]);
			//		Handles.DrawWireDisc((Vector3) call.parameters[0], Vector3.forward, (float) call.parameters[1]);
			//		Handles.DrawWireDisc((Vector3) call.parameters[0], new Vector3(-1, 0, 1).normalized, (float) call.parameters[1]);
			//		break;
			//	case DrawType.Sphere:
			//		// TODO implement
			//		//Handles.DrawSphere((Vector3) call.parameters[0], (float) call.parameters[1]);
			//		break;
			//	case DrawType.WireMesh:
			//		Graphics.DrawMeshNow((Mesh) call.parameters[0], (Vector3) call.parameters[1], (Quaternion) call.parameters[2]);
			//		break;
			//}
		}
//#endif

	}
}
