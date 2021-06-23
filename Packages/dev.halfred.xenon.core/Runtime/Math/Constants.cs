using UnityEngine;

namespace Xenon {
	public static class ConstFloat {
		public const float PI = 3.1415926535897932384626433832795f;
		public const float TWO_PI = 6.283185307179586476925286766559f;
		public const float HALF_PI = 1.5707963267948966192313216916398f;
		public const float QUARTER_PI = 0.78539816339744830961566084581988f;
		public const float E = 2.7182818284590452353602874713527f;
		public const float DEG2RAD = 0.01745329251994329576923690768489f;
		public const float RAD2DEG = 57.295779513082320876798154814105f;
	}

	public static class ConstVector2Int {
		public static readonly Vector2Int zero = new Vector2Int(0, 0);
		public static readonly Vector2Int one = new Vector2Int(1, 1);
		public static readonly Vector2Int right = new Vector2Int(1, 0);
		public static readonly Vector2Int left = new Vector2Int(-1, 0);
		public static readonly Vector2Int up = new Vector2Int(0, 1);
		public static readonly Vector2Int down = new Vector2Int(0, -1);
		public static readonly Vector2Int max = new Vector2Int(int.MaxValue, int.MaxValue);
		public static readonly Vector2Int min = new Vector2Int(int.MinValue, int.MinValue);
	}

	public static class ConstVector3Int {
		public static readonly Vector3Int zero = new Vector3Int(0, 0, 0);
		public static readonly Vector3Int one = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int right = new Vector3Int(1, 0, 0);
		public static readonly Vector3Int left = new Vector3Int(-1, 0, 0);
		public static readonly Vector3Int up = new Vector3Int(0, 1, 0);
		public static readonly Vector3Int down = new Vector3Int(0, -1, 0);
		public static readonly Vector3Int forward = new Vector3Int(0, 0, 1);
		public static readonly Vector3Int backward = new Vector3Int(0, 0, -1);
		public static readonly Vector3Int max = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
		public static readonly Vector3Int min = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
	}

	public static class ConstVector2 {
		public static readonly Vector2 zero = new Vector2(0, 0);
		public static readonly Vector2 one = new Vector2(1, 1);
		public static readonly Vector2 right = new Vector2(1, 0);
		public static readonly Vector2 left = new Vector2(-1, 0);
		public static readonly Vector2 up = new Vector2(0, 1);
		public static readonly Vector2 down = new Vector2(0, -1);
		public static readonly Vector2 positiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
		public static readonly Vector2 negativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		public static readonly Vector2 nan = new Vector2(float.NaN, float.NaN);
	}

	public static class ConstVector3 {
		public static readonly Vector3 zero = new Vector3(0, 0, 0);
		public static readonly Vector3 one = new Vector3(1, 1, 1);
		public static readonly Vector3 right = new Vector3(1, 0, 0);
		public static readonly Vector3 left = new Vector3(-1, 0, 0);
		public static readonly Vector3 up = new Vector3(0, 1, 0);
		public static readonly Vector3 down = new Vector3(0, -1, 0);
		public static readonly Vector3 forward = new Vector3(0, 0, 1);
		public static readonly Vector3 backward = new Vector3(0, 0, -1);
		public static readonly Vector3 positiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		public static readonly Vector3 negativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
		public static readonly Vector3 nan = new Vector3(float.NaN, float.NaN, float.NaN);
	}

	public static class ConstVector4 {
		public static readonly Vector4 zero = new Vector4(0, 0, 0, 0);
		public static readonly Vector4 zeroPoint = new Vector4(0, 0, 0, 1);
		public static readonly Vector4 one = new Vector4(1, 1, 1, 1);
		public static readonly Vector4 oneDir = new Vector4(1, 1, 1, 0);
		public static readonly Vector4 right = new Vector4(1, 0, 0, 0);
		public static readonly Vector4 left = new Vector4(-1, 0, 0, 0);
		public static readonly Vector4 up = new Vector4(0, 1, 0, 0);
		public static readonly Vector4 down = new Vector4(0, -1, 0, 0);
		public static readonly Vector4 forward = new Vector4(0, 0, 1, 0);
		public static readonly Vector4 backward = new Vector4(0, 0, -1, 0);
		public static readonly Vector4 positiveInfinity = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		public static readonly Vector4 negativeInfinity = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
		public static readonly Vector4 positiveInfinityPoint = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, 1f);
		public static readonly Vector4 negativeInfinityPoint = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, 1);
		public static readonly Vector4 positiveInfinityDir = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, 0);
		public static readonly Vector4 negativeInfinityDir = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, 0);
		public static readonly Vector4 nan = new Vector4(float.NaN, float.NaN, float.NaN, float.NaN);
	}

	public static class ConstColor {
		public static readonly Color clear = new Color(0, 0, 0, 0);
		public static readonly Color white = new Color(1, 1, 1, 1);
		public static readonly Color black = new Color(0, 0, 0, 1);
		public static readonly Color lightGray = new Color(.666f, .666f, .666f, 1);
		public static readonly Color darkGray = new Color(.333f, .333f, .333f, 1);
		public static readonly Color blue = new Color(0, 0, 1, 1);
		public static readonly Color green = new Color(0, 1, 0, 1);
		public static readonly Color cyan = new Color(0, 1, 1, 1);
		public static readonly Color red = new Color(1, 0, 0, 1);
		public static readonly Color magenta = new Color(1, 0, 1, 1);
		public static readonly Color yellow = new Color(1, 1, 0, 1);
		public static readonly Color orange = new Color(1, .5f, 0, 1);
		public static readonly Color lime = new Color(.5f, 1, 0, 1);
		public static readonly Color turquoise = new Color(0, 1, .5f, 1);
		public static readonly Color sky = new Color(0, .5f, 1, 1);
		public static readonly Color purple = new Color(.5f, 0, 1, 1);
		public static readonly Color pink = new Color(1, 0, .5f, 1);
	}
}
