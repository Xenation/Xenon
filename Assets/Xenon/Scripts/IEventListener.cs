namespace Xenon {
	public interface IEventListener {

	}

	public static class IEventListenerExt {

		public static void RegisterListener(this IEventListener listener) {
			EventManager.I.RegisterListener(listener);
		}

		public static void UnregisterListener(this IEventListener listener) {
			EventManager.I.UnregisterListener(listener);
		}

	}
}
