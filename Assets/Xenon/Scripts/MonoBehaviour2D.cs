using UnityEngine;

namespace Xenon {
	public abstract class MonoBehaviour2D : MonoBehaviour {

		public new Transform2D transform;

		protected virtual void Awake() {
			transform = new Transform2D(base.transform);
		}

	}
}
