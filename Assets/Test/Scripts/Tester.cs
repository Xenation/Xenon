using UnityEngine;
using UnityEngine.UI;
using Xenon.Processes;

namespace Xenon.Test {
	public class Tester : Singleton<Tester>, IEventSender {

		public Graphic fadeInTest;
		public Graphic fadeOutTest;

		private ProcessRythmer rythmer;

		private Process fadeInProc;
		private Process fadeOutProc;
		private Process timerProc;

		private TestListener testListener;

		public void Awake() {
			rythmer = GetComponent<ProcessRythmer>();
			testListener = new TestListener();
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
			TimingDebugger.StartTiming("Tester - Update");

			TimingDebugger.EndTiming();
		}

		public void OnDestroy() {
			EventManager.I.UnregisterListener(testListener);
		}
	}
}
