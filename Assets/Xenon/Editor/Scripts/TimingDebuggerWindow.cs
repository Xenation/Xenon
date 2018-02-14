using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class TimingDebuggerWindow : XenonWindow<TimingDebuggerWindow> {

		protected override float minWidth { get { return 300f; } }
		protected override float minHeight { get { return 300f; } }
		protected override string titleStr { get { return "Timings Debugger"; } }

		private GridGUI _grid;
		private GridGUI grid {
			get {
				if (_grid == null) {
					_grid = new GridGUI();
					_grid.SetHeader("ID", "Avg", "Max");
				}
				return _grid;
			}
			set {
				_grid = value;
			}
		}

		private void OnEnable() {
			EditorApplication.playModeStateChanged += PlayModeChange;
		}

		private void PlayModeChange(PlayModeStateChange change) {
			if (change == PlayModeStateChange.ExitingEditMode) {
				TimingDebugger.ClearAll();
			}
		}

		private void Update() {
			if (TimingDebugger.hasRecordedThisFrame) {
				Repaint();
			}

			if (Time.frameCount > TimingDebugger.prevFrameCount) {
				TimingDebugger.EndFrame();
				TimingDebugger.prevFrameCount = Time.frameCount;
			}
		}

		private void OnGUI() {
			TimingDebugger.root.FillGrid(grid);
			grid.DisplayGUI(position.width);

			if (GUILayout.Button("Reset")) {
				TimingDebugger.ClearAll();
				grid.Clear();
			}
		}

	}
}
