using Unity.Burst;
using Unity.Mathematics;

namespace Xenon.Burst {
	[BurstCompile]
	public static class maths {

		public const float PI = 3.1415926535897932384626433832795f;
		public const float TWO_PI = 6.283185307179586476925286766559f;
		public const float HALF_PI = 1.5707963267948966192313216916398f;
		public const float QUARTER_PI = 0.78539816339744830961566084581988f;
		public const float E = 2.7182818284590452353602874713527f;
		public const float DEG2RAD = 0.01745329251994329576923690768489f;
		public const float RAD2DEG = 57.295779513082320876798154814105f;

		public static float cbrt(float x) {
			return math.pow(x, 0.33333333333f);
		}

	}
}
