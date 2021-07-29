using UnityEngine;

namespace Xenon.Test {
	public class TestLogger : MonoBehaviour {

		private void Awake() {
			Log.Info(Log.Cat("TEST"), "An awake log yay!");
			Log.Warn(Log.Cat("TEST", "MORE_TEST"), "A warning!");
			Log.Error(Log.Cat("TEST"), "ERROR");
			Log.Assert(false, Log.Cat("TEST"), "ASSERT!!!");
		}

	}
}
