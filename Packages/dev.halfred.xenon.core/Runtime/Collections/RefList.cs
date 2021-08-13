using System;
using System.Collections;
using System.Collections.Generic;

namespace Xenon.Collections {
	public class RefList<T> : ICollection<T>, IEnumerable<T> {

		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable {
			public T Current => throw new NotImplementedException();

			object IEnumerator.Current => throw new NotImplementedException();

			public bool MoveNext() {
				throw new NotImplementedException();
			}

			public void Reset() {
				throw new NotImplementedException();
			}

			public void Dispose() {
				throw new NotImplementedException();
			}
		}

		private T[] array;

		public uint Count {
			get {
				return 0;
			}
		}

		int ICollection<T>.Count => throw new System.NotImplementedException();

		public bool IsReadOnly => throw new System.NotImplementedException();

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

		}

		public RefList(int capacity) {

		}

		public RefList(uint capacity) {

		}

		public void Add(T item) {

		}

		public void RemoveAt(int index) {

		}

		public void RemoveAt(uint index) {

		}

		public void Clear() {
			throw new System.NotImplementedException();
		}

		public bool Contains(T item) {
			throw new System.NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex) {
			throw new System.NotImplementedException();
		}

		public bool Remove(T item) {
			throw new System.NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator() {
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			throw new System.NotImplementedException();
		}
	}
}
