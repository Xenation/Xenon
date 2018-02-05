using UnityEngine;

namespace Xenon.Test {
	public class TestListener : IEventListener {

		public TestListener() {
			EventManager.I.RegisterListener(this);
		}

		public void OnTest(IEventSender sender, XEvent ev) {
			Debug.Log("EVENT CAUGHT!");
		}

	}
}
