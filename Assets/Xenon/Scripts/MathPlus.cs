using UnityEngine;

namespace Xenon {
	public static class MathPlus {

		// TODO recheck that there are no copies of existing unity function

		//// int \\\\
		public static float Clamp(this int val, int min, int max) {
			if (val > max) {
				return max;
			} else if (val < min) {
				return min;
			}
			return val;
		}

		//// float \\\\
		public static float Clamp(this float val, float min, float max) {
			if (val > max) {
				return max;
			} else if (val < min) {
				return min;
			}
			return val;
		}

		public static float Remap(this float val, float from1, float to1, float from2, float to2) {
			return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		//// Vector2 \\\\
		public static Vector2 ClampMagnitude(this Vector2 vec, float minMag, float maxMag) {
			float mag = vec.magnitude;
			if (mag > maxMag) {
				return vec.normalized * maxMag;
			} else if (mag < minMag) {
				return vec.normalized * minMag;
			}
			return vec;
		}

		public static Vector2 RemapMagnitude(this Vector2 vec, float from1, float to1, float from2, float to2) {
			float mag = vec.magnitude.Remap(from1, to1, from2, to2);
			return vec.normalized * mag;
		}

		//// Vector3 \\\\
		public static Vector3 ClampMagnitude(this Vector3 vec, float minMag, float maxMag) {
			float mag = vec.magnitude;
			if (mag > maxMag) {
				return vec.normalized * maxMag;
			} else if (mag < minMag) {
				return vec.normalized * minMag;
			}
			return vec;
		}

		public static Vector3 RemapMagnitude(this Vector3 vec, float from1, float to1, float from2, float to2) {
			float mag = vec.magnitude.Remap(from1, to1, from2, to2);
			return vec.normalized * mag;
		}

		//// Vector4 \\\\
		public static Vector4 ClampMagnitude(this Vector4 vec, float minMag, float maxMag) {
			float mag = vec.magnitude;
			if (mag > maxMag) {
				return vec.normalized * maxMag;
			} else if (mag < minMag) {
				return vec.normalized * minMag;
			}
			return vec;
		}

		public static Vector4 RemapMagnitude(this Vector4 vec, float from1, float to1, float from2, float to2) {
			float mag = vec.magnitude.Remap(from1, to1, from2, to2);
			return vec.normalized * mag;
		}

	}
}
