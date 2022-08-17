using UnityEngine;

namespace Xenon {
	public static class UnityLogger {

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		private static void Dummy() {
			// Dummy method to force static init
		}

		static UnityLogger() {
			Log.onMessageAppended += OnLog;
		}

		private static void OnLog(Log.Message message) {
			switch (message.severity) {
				case Log.Severity.INF:
					Debug.Log(message.ToString(false));
					break;
				case Log.Severity.WAR:
					Debug.LogWarning(message.ToString(false));
					break;
				case Log.Severity.ERR:
					Debug.LogError(message.ToString(false));
					break;
				case Log.Severity.ASS:
					Debug.Assert(false, message.ToString(false));
					Debug.Break();
					break;
			}
		}
	}
}
