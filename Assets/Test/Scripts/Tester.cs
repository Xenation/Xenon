using UnityEngine;
using UnityEngine.UI;

using Xenon.Collections;
using Xenon.Processes;

namespace Xenon.Test {
	public class Tester : Singleton<Tester>, IEventSender {

		public Graphic fadeInTest;
		public Graphic fadeOutTest;

		public Mesh testMesh;

		private ProcessRythmer rythmer;

		private Process fadeInProc;
		private Process fadeOutProc;
		private Process timerProc;

		private TestListener testListener;

		public void Awake() {
			rythmer = GetComponent<ProcessRythmer>();
			testListener = new TestListener();

			RefList<Vector3> t = new RefList<Vector3>(8);
			foreach (ref Vector3 v in t) {
				Debug.Log($"{v}");
			}
		}

		public void Start() {
			//Process
			fadeInProc = new FadeInProcess(2f, fadeInTest);
			rythmer.ProcessManager.LaunchProcess(fadeInProc);

			timerProc = new TimedProcess(5f);
			fadeInProc.Attach(timerProc);

			fadeOutProc = new FadeOutProcess(2f, fadeOutTest);
			timerProc.Attach(fadeOutProc);

			//Event
			this.Send(new TestEvent());
		}
		
		public void Update() {
			TimingDebugger.Start("Tester - Update");

			VisualDebug.WireSphere(Vector3.zero, 2f, Color.red);

			VisualDebug.WireMesh(testMesh, Vector3.zero, Quaternion.identity, Vector3.one, Color.red);

			//VisualDebug.DrawLine(Vector3.zero, Vector3.one * 10f, Color.blue, Color.red);
			VisualDebug.Wire(new VisualDebug.WireVertex(Vector3.up, Color.blue), new VisualDebug.WireVertex(Vector3.up * 6f + Vector3.left * 2.5f, Color.red), new VisualDebug.WireVertex(Vector3.up * 1f + Vector3.left * 5f, Color.green), new VisualDebug.WireVertex(Vector3.up, Color.blue));
			VisualDebug.Circle(Vector3.up * 2f, Vector3.up, 1f, Color.yellow);

			VisualDebug.WireCube(Vector3.up * 2f, Vector3.one, Color.cyan);
			VisualDebug.WireSphere(Vector3.right * 2f, 1f, Color.yellow);

			Camera mainCam = Camera.main;
			VisualDebug.Frustum(mainCam.worldToCameraMatrix, mainCam.projectionMatrix, mainCam.nearClipPlane, mainCam.farClipPlane, Color.red, Color.green);

			TimingDebugger.Stop();
		}

		public void OnDestroy() {
			EventManager.I.UnregisterListener(testListener);
		}
	}
}
