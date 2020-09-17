using Unity.Burst;
using Unity.Mathematics;

using UnityEngine;

namespace Xenon.Burst {
	[BurstCompile]
	public static class RandomExt {

		public static float2 InCircle(this Unity.Mathematics.Random random, float radius) {
			float a = random.NextFloat(0f, maths.TWO_PI);
			float r = radius * math.sqrt(random.NextFloat(0f, 1f));
			return math.float2(r * math.cos(a), r * math.sin(a));
		}

		public static float3 InSphere(this Unity.Mathematics.Random random, float radius) {
			float u = random.NextFloat(-1f, 1f);
			float phi = random.NextFloat(0f, maths.TWO_PI);
			float m = math.sqrt(1.0f - u * u);
			float r = radius * maths.cbrt(random.NextFloat(0f, 1f));
			return math.float3(math.cos(phi) * m * r, math.sin(phi) * m * r, u);
		}

		public static float2 OnCircle(this Unity.Mathematics.Random random, float radius) {
			float a = random.NextFloat(0f, maths.TWO_PI);
			return math.float2(radius * math.cos(a), radius * math.sin(a));
		}

		public static float3 OnSphere(this Unity.Mathematics.Random random, float radius) {
			float u = random.NextFloat(-1f, 1f);
			float phi = random.NextFloat(0f, maths.TWO_PI);
			float m = math.sqrt(1.0f - u * u);
			return math.float3(math.cos(phi) * m * radius, math.sin(phi) * m * radius, u);
		}

		public static Color ColorHSV(this Unity.Mathematics.Random random, float hMin = 0.0f, float hMax = 360.0f, float sMin = 0.5f, float sMax = 1.0f, float vMin = 0.5f, float vMax = 1.0f, float aMin = 1.0f, float aMax = 1.0f) {
			Color c = Color.HSVToRGB(random.NextFloat(hMin, hMax), random.NextFloat(sMin, sMax), random.NextFloat(vMin, vMax));
			c.a = random.NextFloat(aMin, aMax);
			return c;
		}

	}

	public class Rand {

		private Xoroshiro128p rng;

		public Rand(ulong seed) {
			rng = new Xoroshiro128p(seed);
		}

		public ulong ULong() {
			return rng.NextULong();
		}

		public ulong ULong(ulong max) {
			return (ulong) (rng.NextULong() * (max / (float) ulong.MaxValue));
		}

		public ulong ULong(ulong min, ulong max) {
			return (ulong) (rng.NextULong() * ((max - min) / (float) ulong.MaxValue) + min);
		}

		public long Long() {
			return rng.NextLong();
		}

		public long Long(long max) {
			return (long) (rng.NextULong() * (max / (float) ulong.MaxValue));
		}

		public long Long(long min, long max) {
			return (long) (rng.NextULong() * ((max - min) / (float) ulong.MaxValue) + min);
		}

		public uint UInt() {
			return rng.NextUInt();
		}

		public uint UInt(uint max) {
			return (uint) (rng.NextUInt() * (max / (float) uint.MaxValue));
		}

		public uint UInt(uint min, uint max) {
			return (uint) (rng.NextUInt() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public int Int() {
			return rng.NextInt();
		}

		public int Int(int max) {
			return (int) (rng.NextUInt() * (max / (float) uint.MaxValue));
		}

		public int Int(int min, int max) {
			return (int) (rng.NextUInt() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public Vector2Int Vector2Int() {
			return new Vector2Int(Int(), Int());
		}

		public Vector2Int Vector2Int(Vector2Int max) {
			return new Vector2Int(Int(max.x), Int(max.y));
		}

		public Vector2Int Vector2Int(Vector2Int min, Vector2Int max) {
			return new Vector2Int(Int(min.x, max.x), Int(min.y, max.y));
		}

		public Vector3Int Vector3Int() {
			return new Vector3Int(Int(), Int(), Int());
		}

		public Vector3Int Vector3Int(Vector3Int max) {
			return new Vector3Int(Int(max.x), Int(max.y), Int(max.z));
		}

		public Vector3Int Vector3Int(Vector3Int min, Vector3Int max) {
			return new Vector3Int(Int(min.x, max.x), Int(min.y, max.y), Int(min.z, max.z));
		}

		public ushort UShort() {
			return rng.NextUShort();
		}

		public ushort UShort(ushort max) {
			return (ushort) (rng.NextByte() * (max / (float) uint.MaxValue));
		}

		public ushort UShort(ushort min, ushort max) {
			return (ushort) (rng.NextByte() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public short Short() {
			return rng.NextShort();
		}

		public short Short(short max) {
			return (short) (rng.NextByte() * (max / (float) uint.MaxValue));
		}

		public short Short(short min, short max) {
			return (short) (rng.NextByte() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public byte Byte() {
			return rng.NextByte();
		}

		public byte Byte(byte max) {
			return (byte) (rng.NextByte() * (max / (float) uint.MaxValue));
		}

		public byte Byte(byte min, byte max) {
			return (byte) (rng.NextByte() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public sbyte SByte() {
			return rng.NextSByte();
		}

		public sbyte SByte(byte max) {
			return (sbyte) (rng.NextSByte() * (max / (float) uint.MaxValue));
		}

		public sbyte SByte(byte min, byte max) {
			return (sbyte) (rng.NextSByte() * ((max - min) / (float) uint.MaxValue) + min);
		}

		public bool Bool() {
			return rng.NextBool();
		}

		public float Float() {
			return rng.NextFloat();
		}

		public float Float(float max) {
			return rng.NextUInt() * (max / (float) uint.MaxValue);
		}

		public float Float(float min, float max) {
			return rng.NextUInt() * ((max - min) / (float) uint.MaxValue) + min;
		}

		public Vector2 Vector2() {
			return new Vector2(Float(), Float());
		}

		public Vector2 Vector2(Vector2 max) {
			return new Vector2(Float(max.x), Float(max.y));
		}

		public Vector2 Vector2(Vector2 min, Vector2 max) {
			return new Vector2(Float(min.x, max.x), Float(min.y, max.y));
		}

		public Vector3 Vector3() {
			return new Vector3(Float(), Float(), Float());
		}

		public Vector3 Vector3(Vector3 max) {
			return new Vector3(Float(max.x), Float(max.y), Float(max.z));
		}

		public Vector3 Vector3(Vector3 min, Vector3 max) {
			return new Vector3(Float(min.x, max.x), Float(min.y, max.y), Float(min.z, max.z));
		}

		public Vector4 Vector4() {
			return new Vector4(Float(), Float(), Float(), Float());
		}

		public Vector4 Vector4(Vector4 max) {
			return new Vector4(Float(max.x), Float(max.y), Float(max.z), Float(max.w));
		}

		public Vector4 Vector4(Vector4 min, Vector4 max) {
			return new Vector4(Float(min.x, max.x), Float(min.y, max.y), Float(min.z, max.z), Float(min.w, max.w));
		}

		public double NextDouble() {
			return rng.NextDouble();
		}

		public double NextDouble(double max) {
			return rng.NextUInt() * (max / (double) uint.MaxValue);
		}

		public double NextDouble(double min, double max) {
			return rng.NextUInt() * ((max - min) / (double) uint.MaxValue) + min;
		}

		public float Uniform() {
			return rng.NextUInt() / (float) uint.MaxValue;
		}

		public float Range(float min, float max) {
			return Float(min, max);
		}

		public int Range(int min, int max) {
			return Mathf.FloorToInt(Range((float) min, (float) max));
		}

		public Vector2 InCircle(float radius) {
			float a = Uniform() * maths.TWO_PI;
			float r = radius * math.sqrt(Uniform());
			return new Vector2(r * math.cos(a), r * math.sin(a));
		}

		public Vector3 InSphere(float radius) {
			float u = 2.0f * Uniform() - 1.0f;
			float phi = maths.TWO_PI * Uniform();
			float m = math.sqrt(1.0f - u * u);
			float r = radius * maths.cbrt(Uniform());
			return new Vector3(math.cos(phi) * m * r, math.sin(phi) * m * r, u);
		}

		public Vector2 OnCircle(float radius) {
			float a = Uniform() * maths.TWO_PI;
			return new Vector2(radius * math.cos(a), radius * math.sin(a));
		}

		public Vector3 OnSphere(float radius) {
			float u = 2.0f * Uniform() - 1.0f;
			float phi = maths.TWO_PI * Uniform();
			float m = math.sqrt(1.0f - u * u);
			return new Vector3(math.cos(phi) * m * radius, math.sin(phi) * m * radius, u);
		}

		public Color ColorHSV(float hMin = 0.0f, float hMax = 360.0f, float sMin = 0.5f, float sMax = 1.0f, float vMin = 0.5f, float vMax = 1.0f, float aMin = 1.0f, float aMax = 1.0f) {
			Color c = Color.HSVToRGB(Range(hMin, hMax), Range(sMin, sMax), Range(vMin, vMax));
			c.a = Range(aMin, aMax);
			return c;
		}

	}
}
