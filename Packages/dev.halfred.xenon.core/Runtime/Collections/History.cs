
namespace Xenon {
	public class History<T> {

		public ref T this[int index] {
			get {
				return ref buffer[((startIndex - index) % (int) capacity + (int) capacity) % (int) capacity];
			}
		}
		public ref T this[uint index] {
			get {
				return ref buffer[((startIndex - (int) index) % (int) capacity + (int) capacity) % (int) capacity];
			}
		}

		public readonly uint capacity;
		public uint count { get; private set; }

		private T[] buffer;
		private int startIndex;

		public History(uint capacity) {
			buffer = new T[capacity];
			this.capacity = capacity;
			startIndex = 0;
			count = 0;
		}

		public void Add(ref T item) {
			buffer[startIndex++] = item;
			startIndex = startIndex % (int) capacity;
			count += (count >= capacity) ? 0u : 1u;
		}

		public void Clear() {
			buffer = new T[capacity];
			startIndex = 0;
			count = 0;
		}

	}
}
