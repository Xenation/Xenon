using UnityEngine;

namespace Xenon.Test {
	public class TestLogger : MonoBehaviour {

		private static ulong __marker = Log.Marker("TEST");

		static TestLogger() {
			Log.Marker("TEST");
		}

		private void Awake() {
			Log.Marker("TEST_METHOD");
			Log.Info("A log with auto categories");
			//Log.Info(Log.Cat("TEST"), "An awake log yay!");
			//Log.Warn(Log.Cat("TEST", "MORE_TEST"), "A warning!");
			//Log.Error(Log.Cat("TEST"), "ERROR");
			//Log.Assert(false, Log.Cat("TEST"), "ASSERT!!!");
		}

	}
}
