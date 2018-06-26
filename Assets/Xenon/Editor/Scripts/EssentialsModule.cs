using UnityEditor;

namespace Xenon.Editor {
	public abstract class EssentialsModule {

		protected abstract string Title { get; }

		protected Essentials essentials;
		public bool Active { get; private set; }

		public EssentialsModule(Essentials ess, bool active = true) {
			essentials = ess;
			this.Active = active;
		}

		public abstract void OnEnable(SerializedObject serializedObject);

		public void InspectorGUI() {
			EditorGUI.indentLevel = 0;
			bool nActive = EditorGUILayout.ToggleLeft(Title, Active, EditorStyles.boldLabel);
			if (nActive != Active) {
				Active = nActive;
				if (nActive) {
					OnActivated();
				} else {
					OnDesactivated();
				}
			}
			if (Active) {
				EditorGUI.indentLevel = 1;
				OnInspectorGUI();
			}
		}

		protected virtual void OnActivated() { }
		protected virtual void OnDesactivated() { }

		protected abstract void OnInspectorGUI();

		public abstract void OnSceneGUI(SceneView scene);

	}
}
