using System;
using System.Collections;
using System.Collections.Generic;

namespace Xenon.Collections {
	/// <summary>
	/// A List that allows the use of refs for method params and returns. It does not implement ICollection as that would prevent the use of a ref Enumerator.
	/// </summary>
	/// <typeparam name="T">The type of the items/elements of the list</typeparam>
	public class RefList<T> {

		public struct Enumerator : IEnumerator<T> {

			public T Current => list[currentIndex];
			object IEnumerator.Current => list[currentIndex];

			private RefList<T> list;
			private int currentIndex;

			public Enumerator(RefList<T> list) {
				this.list = list;
				currentIndex = -1;
			}

			public bool MoveNext() {
				currentIndex++;
				return currentIndex < list.Count;
			}

			public void Reset() {
				currentIndex = -1;
			}

			public void Dispose() {
				
			}
		}


		private T[] array;
		private int count = 0;

		public int Count => count;

		public bool IsReadOnly => false;

		public ref T this[int index] {
			get {
				return ref array[index];
			}
		}

		public ref T this[uint index] {
			get {
				return ref array[index];
			}
		}

		public RefList() {
			array = new T[16];
		}

		public RefList(int capacity) {
			array = new T[capacity];
		}

		public RefList(uint capacity) {
			array = new T[capacity];
		}

		public void Add(T item) {
			Add(ref item);
		}

		public void Add(ref T item) {
			if (count == array.Length) {
				Grow();
			}
			array[count++] = item;
		}

		private void Grow() {
			T[] nArray = new T[array.Length * 2];
			Array.Copy(array, nArray, array.Length);
			array = nArray;
		}

		public void RemoveAt(int index) {
			ShiftLeft((uint) index);
		}

		public void RemoveAt(uint index) {
			ShiftLeft(index);
		}

		private void ShiftLeft(uint index) {
			for (uint i = index; i < count - 1; i++) {
				array[i] = array[i + 1];
			}
			count--;
		}

		public void Clear() {
			Array.Clear(array, 0, array.Length);
		}

		public bool Contains(T item) {
			for (uint i = 0; i < array.Length; i++) {
				if (EqualityComparer<T>.Default.Equals(array[i], item)) return true;
			}
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex) {
			Array.Copy(this.array, 0, array, arrayIndex, this.array.Length);
		}

		public bool Remove(T item) {
			for (uint i = 0; i < array.Length; i++) {
				if (EqualityComparer<T>.Default.Equals(array[i], item)) {
					ShiftLeft(i);
					return true;
				}
			}
			return false;
		}

#if NET_STANDARD_2_1 || NETSTANDARD2_1 || UNITY_2021_2_OR_NEWER
		public Span<T> ToSpan() {
			return new Span<T>(array, 0, count);
		}

		public ReadOnlySpan<T> ToReadOnlySpan() {
			return new ReadOnlySpan<T>(array, 0, count);
		}

		public Span<T>.Enumerator GetEnumerator() {
			return ToSpan().GetEnumerator();
		}
#else
		// Slower Alternative for enumeration but in previous versions this is all we have
		public Enumerator GetEnumerator() {
			return new Enumerator(this);
		}
#endif

	}
}
