using UnityEngine;

namespace Xenon {
	public abstract class MonoBehaviour2D : MonoBehaviour {

		private Transform2D transform2D;
		public new ref Transform2D transform {
			get {
				transform2D = new Transform2D(base.transform);
				return ref transform2D;
			}
		}

	}
}
