using UnityEngine;

namespace Xenon {
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class VisualDebugProxy : MonoBehaviour {

		public delegate void CallbackDelegate();
		public event CallbackDelegate onPostRender;

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		private void OnPostRender() {
			onPostRender?.Invoke();

		}

	}
}
