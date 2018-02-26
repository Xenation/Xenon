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
			FillGrid(TimingDebugger.root, grid);
			grid.DisplayGUI(position.width);

			if (GUILayout.Button("Reset")) {
				TimingDebugger.ClearAll();
				grid.Clear();
			}
		}

		public void DrawGUI(Timing timing, float width) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(timing.id, GUILayout.MaxWidth(width / 2f - 10f));
			EditorGUILayout.LabelField("avg: " + timing.GetAverageMilliseconds() + "ms" + " - max: " + timing.GetMaxMilliseconds() + "ms", TimingDebugger.alignRight, GUILayout.MaxWidth(width / 2f));
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;
			foreach (Timing tmg in timing.childs.Values) {
				DrawGUI(tmg, width);
			}
			EditorGUI.indentLevel--;
		}

		public void FillGrid(Timing timing, GridGUI grid) {
			int row = 0;
			FillGrid(timing, grid, ref row, 0);
		}

		public void FillGrid(Timing timing, GridGUI grid, ref int row, int indent) {
			grid[row, 0] = "";
			for (int i = 0; i < indent; i++) {
				grid[row, 0] += "  ";
			}
			grid[row, 0] += timing.id;
			grid[row, 1] = timing.GetAverageMilliseconds() + "ms";
			grid[row, 2] = timing.GetMaxMilliseconds() + "ms";
			row++;
			foreach (Timing tmg in timing.childs.Values) {
				FillGrid(tmg, grid, ref row, indent + 1);
			}
		}

	}
}
