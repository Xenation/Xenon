/*  Written in 2016-2018 by David Blackman and Sebastiano Vigna (vigna@acm.org)
 * To the extent possible under law, the author has dedicated all copyright
 * and related and neighboring rights to this software to the public domain
 * worldwide. This software is distributed without any warranty.
 
 * See <http://creativecommons.org/publicdomain/zero/1.0/>. */

using Unity.Burst;
using Unity.Burst.Intrinsics;

namespace Xenon.Burst {
	[BurstCompile]
	public class Xoroshiro128p {

		private const int a = 24, b = 16, c = 37;

		private static ulong Splitmix64(ulong x) {
			ulong z = (x += 0x9E3779B97F4A7C15ul);
			z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9ul;
			z = (z ^ (z >> 27)) * 0x94D049BB133111EBul;
			return z ^ (z >> 31);
		}

		private static ulong Rotate(ulong x, int k) {
			return (x << k) | (x >> (64 - k));
		}

		private v128 state;

		public Xoroshiro128p() : this((ulong) System.DateTime.Now.ToBinary()) {}
		public Xoroshiro128p(ulong seed) : this(new v128(Splitmix64(seed), Splitmix64(seed))) {}
		public Xoroshiro128p(v128 state) {
			this.state = ((state.ULong0 | state.ULong1) != 0) ? state : new v128(0ul);
		}

		public Xoroshiro128p(Xoroshiro128p other) {
			state = other.state;
		}

		public ulong Next() {
			ulong s0 = state.ULong0;
			ulong s1 = state.ULong1;
			ulong res = s0 + s1;

			s1 ^= s0;
			state.ULong0 = Rotate(s0, a) ^ s1 ^ (s1 << b);
			state.ULong1 = Rotate(s1, c);

			return res;
		}

		public ulong NextULong() {
			return Next();
		}

		public long NextLong() {
			return (long) Next();
		}

		public uint NextUInt() {
			return (uint) (Next() >> 32);
		}

		public int NextInt() {
			return (int) (Next() >> 32);
		}

		public ushort NextUShort() {
			return (ushort) (Next() >> 48);
		}

		public short NextShort() {
			return (short) (Next() >> 48);
		}

		public byte NextByte() {
			return (byte) (Next() >> 56);
		}

		public sbyte NextSByte() {
			return (sbyte) (Next() >> 56);
		}

		public bool NextBool() {
			return NextLong() < 0L;
		}

		public float NextFloat() {
			return (Next() & ((1L << 24) - 1)) * (1.0f / (1L << 24));
		}

		public double NextDouble() {
			return (Next() & ((1L << 53) - 1)) * (1.0 / (1L << 53));
		}

		// TODO Implement Jump and LongJump

	}
}
