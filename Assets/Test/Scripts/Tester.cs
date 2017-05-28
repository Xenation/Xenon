using UnityEngine;
using UnityEngine.UI;
using Xenon.Processes;

namespace Xenon.Test {
	public class Tester : MonoBehaviour {

		public Graphic fadeInTest;
		public Graphic fadeOutTest;

		private ProcessRythmer rythmer;

		private Process fadeInProc;
		private Process fadeOutProc;
		private Process timerProc;

		public void Awake() {
			rythmer = GetComponent<ProcessRythmer>();
		}

		public void Start() {
			fadeInProc = new FadeInProcess(2f, fadeInTest);
			rythmer.ProcessManager.LaunchProcess(fadeInProc);

			timerProc = new TimedProcess(5f);
			fadeInProc.Attach(timerProc);

			fadeOutProc = new FadeOutProcess(2f, fadeOutTest);
			timerProc.Attach(fadeOutProc);
		}
		
		public void Update() {

		}
	}
}
