namespace Xenon.Editor {
	public class List2D<T> {

		private T[,] grid;
		
		public T this[int x, int y] {
			get {
				return grid[x, y];
			}
			set {
				grid[x, y] = value;
			}
		}

		public List2D(int sizeX, int sizeY) {
			grid = new T[sizeX, sizeY];
		}

		public List2D() : this(1, 1) { }

	}
}
