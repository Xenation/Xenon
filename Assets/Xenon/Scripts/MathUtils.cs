using UnityEngine;

namespace Xenon {
	public static class MathUtils {

		//// int \\\\
		public static int Clamp(int v, int min, int max) {
			return (v > max) ? max : ((v < min) ? min : v);
		}

		//// float \\\\
		public static float Remap(float v, float from1, float to1, float from2, float to2) {
			return (v - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public static float Snap(float v, float snapPos, float snapDist) {
			return ((v - snapPos).Abs() < snapDist) ? snapPos : v;
		}

		//// Vector2 \\\\
		public static float MaxComponent(Vector2 v) {
			return Mathf.Max(v.x, v.y);
		}

		public static Vector2 Abs(Vector2 v) {
			return new Vector2(v.x.Abs(), v.y.Abs());
		}

		public static Vector2 Ceil(Vector2 v) {
			return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
		}

		public static Vector2Int CeilToInt(Vector2 v) {
			return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
		}

		public static Vector2 Floor(Vector2 v) {
			return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
		}

		public static Vector2Int FloorToInt(Vector2 v) {
			return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		}

		public static Vector2 Round(Vector2 v) {
			return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
		}

		public static Vector2Int RoundToInt(Vector2 v) {
			return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public static Vector2 Clamp(Vector2 v, float min, float max) {
			return new Vector2(v.x.Clamp(min, max), v.y.Clamp(min, max));
		}

		public static Vector2 ClampMagnitude(Vector2 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector2 Remap(Vector2 v, float from1, float to1, float from2, float to2) {
			return new Vector2(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2));
		}

		public static Vector2 RemapMagnitude(Vector2 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector2 Snap(Vector2 v, Vector2 snapPos, float snapDist) {
			return ((v - snapPos).magnitude < snapDist) ? snapPos : v;
		}

		public static Vector2 Rotate(Vector2 v, float angle) {
			float s = Mathf.Sin(angle * Mathf.Deg2Rad);
			float c = Mathf.Cos(angle * Mathf.Deg2Rad);
			return new Vector2(c * v.x - s * v.y, s * v.x + c * v.y);
		}

		//// Vector2Int \\\\
		public static int MaxComponent(Vector2Int v) {
			return Mathf.Max(v.x, v.y);
		}

		public static Vector2Int Max(Vector2Int a, Vector2Int b) {
			return new Vector2Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2Int Abs(Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y);
		}

		public static Vector2Int Clamp(Vector2Int v, int min, int max) {
			return new Vector2Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y));
		}

		public static Vector2Int Clamp(Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x > rect.max.x) ? rect.max.x : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y > rect.max.y) ? rect.max.y : v.y));
		}

		//// Vector3 \\\\
		public static float MaxComponent(Vector3 v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
		}

		public static Vector3 Max(Vector3 a, Vector3 b) {
			return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3 Abs(Vector3 v) {
			return new Vector3(v.x.Abs(), v.y.Abs(), v.z.Abs());
		}

		public static Vector3 Ceil(Vector3 v) {
			return new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
		}

		public static Vector3Int CeilToInt(Vector3 v) {
			return new Vector3Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));
		}

		public static Vector3 Floor(Vector3 v) {
			return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
		}

		public static Vector3Int FloorToInt(Vector3 v) {
			return new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		}

		public static Vector3 Round(Vector3 v) {
			return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
		}

		public static Vector3Int RoundToInt(Vector3 v) {
			return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		public static Vector3 Clamp(Vector3 v, float min, float max) {
			return new Vector3(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max));
		}

		public static Vector3 ClampMagnitude(Vector3 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector3 Remap(Vector3 v, float from1, float to1, float from2, float to2) {
			return new Vector3(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2));
		}

		public static Vector3 RemapMagnitude(Vector3 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector3 Snap(Vector3 v, Vector3 snapPos, float snapDist) {
			return ((v - snapPos).magnitude < snapDist) ? snapPos : v;
		}

		//// Vector3Int \\\\
		public static int MaxComponent(Vector3Int v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
		}

		public static Vector3Int Max(Vector3Int a, Vector3Int b) {
			return new Vector3Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3Int Abs(Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y, (v.z < 0) ? -v.z : v.z);
		}

		public static Vector3Int Clamp(Vector3Int v, int min, int max) {
			return new Vector3Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y), (v.z < min) ? min : ((v.z > max) ? max : v.z));
		}

		public static Vector3Int Clamp(Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x > box.max.x) ? box.max.x : v.x), (v.y < box.min.y) ? box.min.y : ((v.y > box.max.y) ? box.max.y : v.y), (v.z < box.min.z) ? box.min.z : ((v.z > box.max.z) ? box.max.z : v.z));
		}

		//// Vector4 \\\\
		public static float MaxComponent(Vector4 v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), Mathf.Max(v.z, v.w));
		}

		public static Vector4 Max(Vector4 a, Vector4 b) {
			return new Vector4(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z), Mathf.Max(a.w, b.w));
		}

		public static Vector4 Abs(Vector4 v) {
			return new Vector4(v.x.Abs(), v.y.Abs(), v.z.Abs(), v.w.Abs());
		}

		public static Vector4 Ceil(Vector4 v) {
			return new Vector4(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z), Mathf.Ceil(v.w));
		}

		public static Vector4 Floor(Vector4 v) {
			return new Vector4(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z), Mathf.Floor(v.w));
		}

		public static Vector4 Round(Vector4 v) {
			return new Vector4(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z), Mathf.Round(v.w));
		}

		public static Vector4 Clamp(Vector4 v, float min, float max) {
			return new Vector4(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max), v.w.Clamp(min, max));
		}

		public static Vector4 ClampMagnitude(Vector4 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector4 Remap(Vector4 v, float from1, float to1, float from2, float to2) {
			return new Vector4(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2), v.w.Remap(from1, to1, from2, to2));
		}

		public static Vector4 RemapMagnitude(Vector4 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

	}

	public static class MathExt {

		//// int \\\\
		public static int Max(this int a, int b) {
			return (a > b) ? a : b;
		}

		public static int Min(this int a, int b) {
			return (a < b) ? a : b;
		}

		public static int Abs(this int v) {
			return (v < 0) ? -v : v;
		}
		
		public static int Clamp(this int v, int min, int max) {
			return (v > max) ? max : ((v < min) ? min : v);
		}

		//// float \\\\
		public static float Abs(this float v) {
			return (v < 0f) ? -v : v;
		}

		public static float Ceil(this float v) {
			return Mathf.Ceil(v);
		}

		public static int CeilToInt(this float v) {
			return Mathf.CeilToInt(v);
		}

		public static float Floor(this float v) {
			return Mathf.Floor(v);
		}

		public static int FloorToInt(this float v) {
			return Mathf.FloorToInt(v);
		}

		public static float Round(this float v) {
			return Mathf.Round(v);
		}

		public static int RoundToInt(this float v) {
			return Mathf.RoundToInt(v);
		}

		public static float Clamp(this float v, float min, float max) {
			return (v > max) ? max : ((v < min) ? min : v);
		}

		public static float Clamp01(this float v) {
			return (v > 1f) ? 1f : ((v < 0f) ? 0f : v);
		}

		public static float Remap(this float v, float from1, float to1, float from2, float to2) {
			return (v - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public static float Snap(this float v, float snapPos, float snapDist) {
			return ((v - snapPos).Abs() < snapDist) ? snapPos : v;
		}

		//// Vector2 \\\\
		public static float MaxComponent(this Vector2 v) {
			return Mathf.Max(v.x, v.y);
		}

		public static Vector2 Max(this Vector2 a, Vector2 b) {
			return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2 Abs(this Vector2 v) {
			return new Vector2(v.x.Abs(), v.y.Abs());
		}

		public static Vector2 Ceil(this Vector2 v) {
			return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
		}

		public static Vector2Int CeilToInt(this Vector2 v) {
			return new Vector2Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
		}

		public static Vector2 Floor(this Vector2 v) {
			return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
		}

		public static Vector2Int FloorToInt(this Vector2 v) {
			return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		}

		public static Vector2 Round(this Vector2 v) {
			return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
		}

		public static Vector2Int RoundToInt(this Vector2 v) {
			return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public static Vector2 Clamp(this Vector2 v, float min, float max) {
			return new Vector2(v.x.Clamp(min, max), v.y.Clamp(min, max));
		}

		public static Vector2 ClampMagnitude(this Vector2 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector2 Remap(this Vector2 v, float from1, float to1, float from2, float to2) {
			return new Vector2(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2));
		}

		public static Vector2 RemapMagnitude(this Vector2 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector2 Snap(this Vector2 v, Vector2 snapPos, float snapDist) {
			return ((v - snapPos).magnitude < snapDist) ? snapPos : v;
		}

		public static Vector3 ToVec3(this Vector2 v, float z = 0f) {
			return new Vector3(v.x, v.y, z);
		}

		public static Vector3 Unflat(this Vector2 v, float y = 0f) {
			return new Vector3(v.x, y, v.y);
		}

		public static Vector2 Rotate(this Vector2 v, float angle) {
			float s = Mathf.Sin(angle * Mathf.Deg2Rad);
			float c = Mathf.Cos(angle * Mathf.Deg2Rad);
			return new Vector2(c * v.x - s * v.y, s * v.x + c * v.y);
		}

		//// Vector2Int \\\\
		public static int MaxComponent(this Vector2Int v) {
			return Mathf.Max(v.x, v.y);
		}

		public static Vector2Int Max(this Vector2Int a, Vector2Int b) {
			return new Vector2Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2Int Abs(this Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y);
		}

		public static Vector2Int Clamp(this Vector2Int v, int min, int max) {
			return new Vector2Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y));
		}

		public static Vector2Int Clamp(this Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x > rect.max.x) ? rect.max.x : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y > rect.max.y) ? rect.max.y : v.y));
		}

		public static Vector3Int ToVec3Int(this Vector2Int v, int z = 0) {
			return new Vector3Int(v.x, v.y, z);
		}

		public static Vector3Int Unflat(this Vector2Int v, int y = 0) {
			return new Vector3Int(v.x, y, v.y);
		}

		public static Vector2 Float(this Vector2Int v) {
			return new Vector2(v.x, v.y);
		}

		//// Vector3 \\\\
		public static float MaxComponent(this Vector3 v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
		}

		public static Vector3 Max(this Vector3 a, Vector3 b) {
			return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3 Abs(this Vector3 v) {
			return new Vector3(v.x.Abs(), v.y.Abs(), v.z.Abs());
		}

		public static Vector3 Ceil(this Vector3 v) {
			return new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
		}

		public static Vector3Int CeilToInt(this Vector3 v) {
			return new Vector3Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));
		}

		public static Vector3 Floor(this Vector3 v) {
			return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
		}

		public static Vector3Int FloorToInt(this Vector3 v) {
			return new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		}

		public static Vector3 Round(this Vector3 v) {
			return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
		}

		public static Vector3Int RoundToInt(this Vector3 v) {
			return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		public static Vector3 Clamp(this Vector3 v, float min, float max) {
			return new Vector3(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max));
		}

		public static Vector3 ClampMagnitude(this Vector3 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector3 Remap(this Vector3 v, float from1, float to1, float from2, float to2) {
			return new Vector3(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2));
		}

		public static Vector3 RemapMagnitude(this Vector3 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector3 Snap(this Vector3 v, Vector3 snapPos, float snapDist) {
			return ((v - snapPos).magnitude < snapDist) ? snapPos : v;
		}

		public static Vector2 ToVec2(this Vector3 v) {
			return new Vector3(v.x, v.y);
		}

		public static Vector2 Flat(this Vector3 v) {
			return new Vector3(v.x, v.z);
		}

		public static Vector4 ToVec4Point(this Vector3 v) {
			return new Vector4(v.x, v.y, v.z, 1f);
		}

		public static Vector4 ToVec4Dir(this Vector3 v) {
			return new Vector4(v.x, v.y, v.z, 0f);
		}

		//// Vector3Int \\\\
		public static int MaxComponent(this Vector3Int v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
		}

		public static Vector3Int Max(this Vector3Int a, Vector3Int b) {
			return new Vector3Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3Int Abs(this Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y, (v.z < 0) ? -v.z : v.z);
		}

		public static Vector3Int Clamp(this Vector3Int v, int min, int max) {
			return new Vector3Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y), (v.z < min) ? min : ((v.z > max) ? max : v.z));
		}

		public static Vector3Int Clamp(this Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x > box.max.x) ? box.max.x : v.x), (v.y < box.min.y) ? box.min.y : ((v.y > box.max.y) ? box.max.y : v.y), (v.z < box.min.z) ? box.min.z : ((v.z > box.max.z) ? box.max.z : v.z));
		}

		public static Vector2Int ToVec2Int(this Vector3Int v) {
			return new Vector2Int(v.x, v.y);
		}

		public static Vector2Int Flat(this Vector3Int v) {
			return new Vector2Int(v.x, v.z);
		}

		public static Vector3 Float(this Vector3Int v) {
			return new Vector3(v.x, v.y, v.z);
		}

		//// Vector4 \\\\
		public static float MaxComponent(this Vector4 v) {
			return Mathf.Max(Mathf.Max(v.x, v.y), Mathf.Max(v.z, v.w));
		}

		public static Vector4 Max(this Vector4 a, Vector4 b) {
			return new Vector4(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z), Mathf.Max(a.w, b.w));
		}

		public static Vector4 Abs(this Vector4 v) {
			return new Vector4(v.x.Abs(), v.y.Abs(), v.z.Abs(), v.w.Abs());
		}

		public static Vector4 Ceil(this Vector4 v) {
			return new Vector4(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z), Mathf.Ceil(v.w));
		}

		public static Vector4 Floor(this Vector4 v) {
			return new Vector4(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z), Mathf.Floor(v.w));
		}

		public static Vector4 Round(this Vector4 v) {
			return new Vector4(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z), Mathf.Round(v.w));
		}

		public static Vector4 Clamp(this Vector4 v, float min, float max) {
			return new Vector4(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max), v.w.Clamp(min, max));
		}

		public static Vector4 ClampMagnitude(this Vector4 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector4 Remap(this Vector4 v, float from1, float to1, float from2, float to2) {
			return new Vector4(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2), v.w.Remap(from1, to1, from2, to2));
		}

		public static Vector4 RemapMagnitude(this Vector4 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector3 ToVec3(this Vector4 v) {
			return new Vector3(v.x, v.y, v.z);
		}

	}
}
