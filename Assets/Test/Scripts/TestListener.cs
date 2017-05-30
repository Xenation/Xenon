using UnityEngine;

namespace Xenon.Test {
	public class TestListener : IEventListener {

		public TestListener() {
			EventManager.I.RegisterListener(this);
		}

		public void OnTest(IEventSender sender, Event ev) {
			Debug.Log("EVENT CAUGHT!");
		}

	}
}
