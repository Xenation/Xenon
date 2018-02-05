using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class TimingDebuggerWindow : XenonWindow<TimingDebuggerWindow> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 300f; } }
		protected override string titleStr { get { return "Timings Debugger"; } }

		private void OnEnable() {
			EditorApplication.playModeStateChanged += PlayModeChange;
		}

		private void PlayModeChange(PlayModeStateChange stateChange) {
			if (stateChange == PlayModeStateChange.EnteredPlayMode) {
				TimingDebugger.ClearAll();
			}
		}

		private void OnGUI() {

			TimingDebugger.root.DrawGUI(position.width);

			if (Time.frameCount > TimingDebugger.prevFrameCount) {
				TimingDebugger.EndFrame();
				TimingDebugger.prevFrameCount = Time.frameCount;
			}
		}

	}
}
