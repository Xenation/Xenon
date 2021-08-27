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

		public static float Fade(float t) {
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		public static float SmootherStep(float edge0, float edge1, float x) {
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		public static void SwapToMinMax(ref int min, ref int max) {
			if (max < min) {
				int tmp = min;
				min = max;
				max = tmp;
			}
		}

		public static void SwapToMinMax(ref float min, ref float max) {
			if (max < min) {
				float tmp = min;
				min = max;
				max = tmp;
			}
		}

		//// Vector2 \\\\
		public static float MaxComponent(Vector2 v) {
			return Mathf.Max(v.x, v.y);
		}

		public static Vector2 Abs(Vector2 v) {
			return new Vector2(v.x.Abs(), v.y.Abs());
		}

		public static Vector2 Sign(Vector2 v) {
			return new Vector2((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f);
		}

		public static Vector2 Inverse(Vector2 v) {
			return new Vector2(1f / v.x, 1f / v.y);
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

		public static Vector2 Step(Vector2 i, Vector2 edge) {
			return new Vector2((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f);
		}

		public static Vector2 Clamp(Vector2 v, float min, float max) {
			return new Vector2(v.x.Clamp(min, max), v.y.Clamp(min, max));
		}

		public static Vector2 Clamp(Vector2 v, Rect rect) {
			return new Vector2(v.x.Clamp(rect.min.x, rect.max.x), v.y.Clamp(rect.min.y, rect.max.y));
		}

		public static Vector2 ClampMagnitude(Vector2 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector2 Remap(Vector2 v, float from1, float to1, float from2, float to2) {
			return new Vector2(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2));
		}

		public static Vector2 Remap(Vector2 v, Vector2 from1, Vector2 to1, Vector2 from2, Vector2 to2) {
			return new Vector2(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y));
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

		public static int Dot(Vector2Int a, Vector2Int b) {
			return a.x * b.x + b.y * b.y;
		}

		public static Vector2Int Max(Vector2Int a, Vector2Int b) {
			return new Vector2Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2Int Abs(Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y);
		}

		public static Vector2Int Sign(Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -1 : 1, (v.y < 0) ? -1 : 1);
		}

		public static Vector2Int Clamp(Vector2Int v, int min, int max) {
			return new Vector2Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y));
		}

		public static Vector2Int Clamp(Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x > rect.max.x) ? rect.max.x : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y > rect.max.y) ? rect.max.y : v.y));
		}

		public static Vector2Int ClampMaxExcluded(Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x >= rect.max.x) ? rect.max.x - 1 : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y >= rect.max.y) ? rect.max.y - 1 : v.y));
		}

		public static Vector2Int Step(Vector2Int i, Vector2Int edge) {
			return new Vector2Int((i.x < edge.x) ? 0 : 1, (i.y < edge.y) ? 0 : 1);
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

		public static Vector3 Sign(Vector3 v) {
			return new Vector3((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f, (v.z < 0f) ? -1f : 1f);
		}

		public static Vector3 Inverse(Vector3 v) {
			return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
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

		public static Vector3 Step(Vector3 i, Vector3 edge) {
			return new Vector3((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f, (i.z < edge.z) ? 0f : 1f);
		}

		public static Vector3 Clamp(Vector3 v, float min, float max) {
			return new Vector3(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max));
		}

		public static Vector3 Clamp(Vector3 v, Bounds bounds) {
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			return new Vector3(v.x.Clamp(min.x, max.x), v.y.Clamp(min.y, max.y), v.z.Clamp(min.z, max.z));
		}

		public static Vector3 ClampMagnitude(Vector3 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector3 Remap(Vector3 v, float from1, float to1, float from2, float to2) {
			return new Vector3(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2));
		}

		public static Vector3 Remap(Vector3 v, Vector3 from1, Vector3 to1, Vector3 from2, Vector3 to2) {
			return new Vector3(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y), v.z.Remap(from1.z, to1.z, from2.z, to2.z));
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

		public static int Dot(Vector3Int a, Vector3Int b) {
			return a.x * b.x + b.y * b.y + a.z * b.z;
		}

		public static Vector3Int Max(Vector3Int a, Vector3Int b) {
			return new Vector3Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3Int Abs(Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y, (v.z < 0) ? -v.z : v.z);
		}

		public static Vector3Int Sign(Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -1 : 1, (v.y < 0) ? -1 : 1, (v.z < 0) ? -1 : 1);
		}

		public static Vector3Int Clamp(Vector3Int v, int min, int max) {
			return new Vector3Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y), (v.z < min) ? min : ((v.z > max) ? max : v.z));
		}

		public static Vector3Int Clamp(Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x > box.max.x) ? box.max.x : v.x), (v.y < box.min.y) ? box.min.y : ((v.y > box.max.y) ? box.max.y : v.y), (v.z < box.min.z) ? box.min.z : ((v.z > box.max.z) ? box.max.z : v.z));
		}

		public static Vector3Int ClampMaxExcluded(Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x >= box.max.x) ? box.max.x - 1 : v.x), (v.y < box.min.y) ? box.min.y : ((v.y >= box.max.y) ? box.max.y - 1 : v.y), (v.z < box.min.z) ? box.min.z : ((v.z >= box.max.z) ? box.max.z - 1 : v.z));
		}

		public static Vector3Int Step(Vector3Int i, Vector3Int edge) {
			return new Vector3Int((i.x < edge.x) ? 0 : 1, (i.y < edge.y) ? 0 : 1, (i.z < edge.z) ? 0 : 1);
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

		public static Vector4 Sign(Vector4 v) {
			return new Vector4((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f, (v.z < 0f) ? -1f : 1f, (v.w < 0f) ? -1f : 1f);
		}

		public static Vector4 Inverse(Vector4 v) {
			return new Vector4(1f / v.x, 1f / v.y, 1f / v.z, 1f / v.w);
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

		public static Vector4 Step(Vector4 i, Vector4 edge) {
			return new Vector4((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f, (i.z < edge.z) ? 0f : 1f, (i.z < edge.w) ? 0f : 1f);
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

		public static Vector4 Remap(Vector4 v, Vector4 from1, Vector4 to1, Vector4 from2, Vector4 to2) {
			return new Vector4(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y), v.z.Remap(from1.z, to1.z, from2.z, to2.z), v.w.Remap(from1.w, to1.w, from2.w, to2.w));
		}

		public static Vector4 RemapMagnitude(Vector4 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		//// Rect \\\\
		public static float SignedDistance(Rect r, Vector2 p) {
			p = p - r.position;
			Vector2 q = p.Abs() - r.size * 0.5f;
			return q.Max(Vector2.zero).magnitude + Mathf.Min(q.MaxComponent(), 0.0f);
		}

		public static RectInt Floor(Rect r) {
			return new RectInt(r.position.FloorToInt(), r.size.FloorToInt());
		}

		public static RectInt Ceil(Rect r) {
			return new RectInt(r.position.CeilToInt(), r.size.CeilToInt());
		}

		public static RectInt Round(Rect r) {
			return new RectInt(r.position.RoundToInt(), r.size.RoundToInt());
		}

		public static RectInt ToIntConservative(Rect r) {
			return new RectInt(r.position.FloorToInt(), r.size.CeilToInt());
		}

		public static Rect Intersect(Rect r, Rect b) {
			Vector2 nMin = Vector2.Max(r.min, b.min);
			Vector2 nMax = Vector2.Min(r.max, b.max);
			return Rect.MinMaxRect(nMin.x, nMin.y, nMax.x, nMax.y);
		}

		public static Rect Bounding(Rect r, Rect b) {
			Vector2 nMin = Vector2.Min(r.min, b.min);
			Vector2 nMax = Vector2.Max(r.max, b.max);
			return Rect.MinMaxRect(nMin.x, nMin.y, nMax.x, nMax.y);
		}

		public static Vector2 ToNormalizedPoint(Rect r, Vector2 p) {
			return Rect.PointToNormalized(r, p);
		}

		public static Vector2 FromNormalizedPoint(Rect r, Vector2 p) {
			return Rect.NormalizedToPoint(r, p);
		}

		//// RectInt \\\\
		public static Rect Float(RectInt r) {
			return new Rect(r.position.Float(), r.size.Float());
		}

		public static RectInt Intersect(RectInt r, RectInt b) {
			Vector2Int nMin = Vector2Int.Max(r.min, b.min);
			Vector2Int nMax = Vector2Int.Min(r.max, b.max);
			return new RectInt(nMin, nMax - nMin);
		}

		public static RectInt Bounding(RectInt r, RectInt b) {
			Vector2Int nMin = Vector2Int.Min(r.min, b.min);
			Vector2Int nMax = Vector2Int.Max(r.max, b.max);
			return new RectInt(nMin, nMax - nMin);
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

		public static float Dot(this Vector2 a, Vector2 b) {
			return a.x * b.x + a.y * b.y;
		}

		public static Vector2 Max(this Vector2 a, Vector2 b) {
			return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2 Min(this Vector2 a, Vector2 b) {
			return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
		}

		public static Vector2 Abs(this Vector2 v) {
			return new Vector2(v.x.Abs(), v.y.Abs());
		}

		public static Vector2 Sign(this Vector2 v) {
			return new Vector2((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f);
		}

		public static Vector2 Inverse(this Vector2 v) {
			return new Vector2(1f / v.x, 1f / v.y);
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

		public static Vector2 Step(this Vector2 i, Vector2 edge) {
			return new Vector2((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f);
		}

		public static Vector2 Clamp(this Vector2 v, float min, float max) {
			return new Vector2(v.x.Clamp(min, max), v.y.Clamp(min, max));
		}

		public static Vector2 Clamp(this Vector2 v, Rect rect) {
			return new Vector2(v.x.Clamp(rect.min.x, rect.max.x), v.y.Clamp(rect.min.y, rect.max.y));
		}

		public static Vector2 ClampMagnitude(this Vector2 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector2 Remap(this Vector2 v, float from1, float to1, float from2, float to2) {
			return new Vector2(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2));
		}

		public static Vector2 Remap(this Vector2 v, Vector2 from1, Vector2 to1, Vector2 from2, Vector2 to2) {
			return new Vector2(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y));
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

		public static int Dot(this Vector2Int a, Vector2Int b) {
			return a.x * b.x + b.y * b.y;
		}

		public static Vector2Int Max(this Vector2Int a, Vector2Int b) {
			return new Vector2Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2Int Min(this Vector2Int a, Vector2Int b) {
			return new Vector2Int(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
		}

		public static Vector2Int Abs(this Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y);
		}

		public static Vector2Int Sign(this Vector2Int v) {
			return new Vector2Int((v.x < 0) ? -1 : 1, (v.y < 0) ? -1 : 1);
		}

		public static Vector2Int Clamp(this Vector2Int v, int min, int max) {
			return new Vector2Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y));
		}

		public static Vector2Int Clamp(this Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x > rect.max.x) ? rect.max.x : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y > rect.max.y) ? rect.max.y : v.y));
		}

		public static Vector2Int ClampMaxExcluded(this Vector2Int v, RectInt rect) {
			return new Vector2Int((v.x < rect.min.x) ? rect.min.x : ((v.x >= rect.max.x) ? rect.max.x - 1 : v.x), (v.y < rect.min.y) ? rect.min.y : ((v.y >= rect.max.y) ? rect.max.y - 1 : v.y));
		}

		public static Vector2Int Step(this Vector2Int i, Vector2Int edge) {
			return new Vector2Int((i.x < edge.x) ? 0 : 1, (i.y < edge.y) ? 0 : 1);
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

		public static float Dot(this Vector3 a, Vector3 b) {
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public static Vector3 Cross(this Vector3 l, Vector3 r) {
			return new Vector3(r.y * l.z - r.z * l.y, r.z * l.x - r.x * l.z, r.x * l.y - r.y * l.x);
		}

		public static Vector3 Max(this Vector3 a, Vector3 b) {
			return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3 Min(this Vector3 a, Vector3 b) {
			return new Vector3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
		}

		public static Vector3 Abs(this Vector3 v) {
			return new Vector3(v.x.Abs(), v.y.Abs(), v.z.Abs());
		}

		public static Vector3 Sign(this Vector3 v) {
			return new Vector3((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f, (v.z < 0f) ? -1f : 1f);
		}

		public static Vector3 Inverse(this Vector3 v) {
			return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
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

		public static Vector3 Step(this Vector3 i, Vector3 edge) {
			return new Vector3((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f, (i.z < edge.z) ? 0f : 1f);
		}

		public static Vector3 Clamp(this Vector3 v, float min, float max) {
			return new Vector3(v.x.Clamp(min, max), v.y.Clamp(min, max), v.z.Clamp(min, max));
		}

		public static Vector3 Clamp(Vector3 v, Bounds bounds) {
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			return new Vector3(v.x.Clamp(min.x, max.x), v.y.Clamp(min.y, max.y), v.z.Clamp(min.z, max.z));
		}

		public static Vector3 ClampMagnitude(this Vector3 v, float minMag, float maxMag) {
			float mag = v.magnitude;
			return (mag > maxMag) ? v.normalized * maxMag : ((mag < minMag) ? v.normalized * minMag : v);
		}

		public static Vector3 Remap(this Vector3 v, float from1, float to1, float from2, float to2) {
			return new Vector3(v.x.Remap(from1, to1, from2, to2), v.y.Remap(from1, to1, from2, to2), v.z.Remap(from1, to1, from2, to2));
		}

		public static Vector3 Remap(this Vector3 v, Vector3 from1, Vector3 to1, Vector3 from2, Vector3 to2) {
			return new Vector3(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y), v.z.Remap(from1.z, to1.z, from2.z, to2.z));
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

		public static int Dot(this Vector3Int a, Vector3Int b) {
			return a.x * b.x + b.y * b.y + a.z * b.z;
		}

		public static Vector3Int Max(this Vector3Int a, Vector3Int b) {
			return new Vector3Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		}

		public static Vector3Int Min(this Vector3Int a, Vector3Int b) {
			return new Vector3Int(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
		}

		public static Vector3Int Abs(this Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -v.x : v.x, (v.y < 0) ? -v.y : v.y, (v.z < 0) ? -v.z : v.z);
		}

		public static Vector3Int Sign(this Vector3Int v) {
			return new Vector3Int((v.x < 0) ? -1 : 1, (v.y < 0) ? -1 : 1, (v.z < 0) ? -1 : 1);
		}

		public static Vector3Int Clamp(this Vector3Int v, int min, int max) {
			return new Vector3Int((v.x < min) ? min : ((v.x > max) ? max : v.x), (v.y < min) ? min : ((v.y > max) ? max : v.y), (v.z < min) ? min : ((v.z > max) ? max : v.z));
		}

		public static Vector3Int Clamp(this Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x > box.max.x) ? box.max.x : v.x), (v.y < box.min.y) ? box.min.y : ((v.y > box.max.y) ? box.max.y : v.y), (v.z < box.min.z) ? box.min.z : ((v.z > box.max.z) ? box.max.z : v.z));
		}

		public static Vector3Int ClampMaxExcluded(this Vector3Int v, BoundsInt box) {
			return new Vector3Int((v.x < box.min.x) ? box.min.x : ((v.x >= box.max.x) ? box.max.x - 1 : v.x), (v.y < box.min.y) ? box.min.y : ((v.y >= box.max.y) ? box.max.y - 1 : v.y), (v.z < box.min.z) ? box.min.z : ((v.z >= box.max.z) ? box.max.z - 1 : v.z));
		}

		public static Vector3Int Step(this Vector3Int i, Vector3Int edge) {
			return new Vector3Int((i.x < edge.x) ? 0 : 1, (i.y < edge.y) ? 0 : 1, (i.z < edge.z) ? 0 : 1);
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

		public static float Dot(this Vector4 a, Vector4 b) {
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		public static Vector4 Max(this Vector4 a, Vector4 b) {
			return new Vector4(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z), Mathf.Max(a.w, b.w));
		}

		public static Vector4 Min(this Vector4 a, Vector4 b) {
			return new Vector4(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z), Mathf.Min(a.w, b.w));
		}

		public static Vector4 Abs(this Vector4 v) {
			return new Vector4(v.x.Abs(), v.y.Abs(), v.z.Abs(), v.w.Abs());
		}

		public static Vector4 Sign(this Vector4 v) {
			return new Vector4((v.x < 0f) ? -1f : 1f, (v.y < 0f) ? -1f : 1f, (v.z < 0f) ? -1f : 1f, (v.w < 0f) ? -1f : 1f);
		}

		public static Vector4 Inverse(this Vector4 v) {
			return new Vector4(1f / v.x, 1f / v.y, 1f / v.z, 1f / v.w);
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

		public static Vector4 Step(this Vector4 i, Vector4 edge) {
			return new Vector4((i.x < edge.x) ? 0f : 1f, (i.y < edge.y) ? 0f : 1f, (i.z < edge.z) ? 0f : 1f, (i.z < edge.w) ? 0f : 1f);
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

		public static Vector4 Remap(this Vector4 v, Vector4 from1, Vector4 to1, Vector4 from2, Vector4 to2) {
			return new Vector4(v.x.Remap(from1.x, to1.x, from2.x, to2.x), v.y.Remap(from1.y, to1.y, from2.y, to2.y), v.z.Remap(from1.z, to1.z, from2.z, to2.z), v.w.Remap(from1.w, to1.w, from2.w, to2.w));
		}

		public static Vector4 RemapMagnitude(this Vector4 v, float from1, float to1, float from2, float to2) {
			float mag = v.magnitude.Remap(from1, to1, from2, to2);
			return v.normalized * mag;
		}

		public static Vector3 ToVec3(this Vector4 v) {
			return new Vector3(v.x, v.y, v.z);
		}

		//// Rect \\\\
		public static float SignedDistance(this Rect r, Vector2 p) {
			p = p - r.position;
			Vector2 q = p.Abs() - r.size * 0.5f;
			return q.Max(Vector2.zero).magnitude + Mathf.Min(q.MaxComponent(), 0.0f);
		}

		public static Rect Floor(this Rect r) {
			return new Rect(r.position.Floor(), r.size.Floor());
		}

		public static RectInt FloorToInt(this Rect r) {
			return new RectInt(r.position.FloorToInt(), r.size.FloorToInt());
		}

		public static Rect Ceil(this Rect r) {
			return new Rect(r.position.Ceil(), r.size.Ceil());
		}

		public static RectInt CeilToInt(this Rect r) {
			return new RectInt(r.position.CeilToInt(), r.size.CeilToInt());
		}

		public static Rect Round(this Rect r) {
			return new Rect(r.position.Round(), r.size.Round());
		}

		public static RectInt RoundToInt(this Rect r) {
			return new RectInt(r.position.RoundToInt(), r.size.RoundToInt());
		}

		public static RectInt ToIntConservative(this Rect r) {
			return new RectInt(r.position.FloorToInt(), r.size.CeilToInt());
		}

		public static Rect Intersect(this Rect r, Rect b) {
			Vector2 nMin = Vector2.Max(r.min, b.min);
			Vector2 nMax = Vector2.Min(r.max, b.max);
			return Rect.MinMaxRect(nMin.x, nMin.y, nMax.x, nMax.y);
		}

		public static Rect Bounding(this Rect r, Rect b) {
			Vector2 nMin = Vector2.Min(r.min, b.min);
			Vector2 nMax = Vector2.Max(r.max, b.max);
			return Rect.MinMaxRect(nMin.x, nMin.y, nMax.x, nMax.y);
		}

		public static Vector2 ToNormalizedPoint(this Rect r, Vector2 p) {
			return Rect.PointToNormalized(r, p);
		}

		public static Vector2 FromNormalizedPoint(this Rect r, Vector2 p) {
			return Rect.NormalizedToPoint(r, p);
		}

		//// RectInt \\\\
		public static Rect Float(this RectInt r) {
			return new Rect(r.position.Float(), r.size.Float());
		}

		public static RectInt Intersect(this RectInt r, RectInt b) {
			Vector2Int nMin = Vector2Int.Max(r.min, b.min);
			Vector2Int nMax = Vector2Int.Min(r.max, b.max);
			return new RectInt(nMin, nMax - nMin);
		}

		public static RectInt Bounding(this RectInt r, RectInt b) {
			Vector2Int nMin = Vector2Int.Min(r.min, b.min);
			Vector2Int nMax = Vector2Int.Max(r.max, b.max);
			return new RectInt(nMin, nMax - nMin);
		}

	}
}
