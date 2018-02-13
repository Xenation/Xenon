using UnityEngine;

namespace Xenon.Test {
	public class TestListener : IEventListener {

		public TestListener() {
			this.RegisterListener();
		}

		public void OnTest(IEventSender sender, XEvent ev) {
			Debug.Log("EVENT CAUGHT!");
		}

	}
}
