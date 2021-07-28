using System.Collections.Generic;

using UnityEngine;

namespace Xenon {
	public static class Log {

		public enum Severity : byte {
			INF = 0b0001,
			WAR = 0b0010,
			ERR = 0b0100,
			ASS = 0b1000
		}

		private struct Category {
			public byte bit;
			public int nameHash;
			public string name;
		}

		private struct Message {
			public Severity severity;
			public ulong categories;
			public string content;
			public System.DateTime time;

			public override string ToString() {
				return ToString(true);
			}

			public string ToString(bool includeTime) {
				return ((includeTime) ? $"[{time.Hour.ToString("00")}:{time.Minute.ToString("00")}:{time.Second.ToString("00")}] " : "") + $"[{severity}] ({GetCategoryNamesString(categories)}): {content}";
			}
		}

		private static Category[] categories = new Category[sizeof(ulong) * 8];
		private static Queue<Message> messages = new Queue<Message>();

		static Log() {
			RegisterCategory("LOG");
		}

		public static void Append(Severity severity, ulong categories, string msg) {
			Message message = new Message() {
				severity = severity,
				categories = categories,
				content = msg,
				time = System.DateTime.Now
			};
			messages.Enqueue(message);
			UnityDebug(message);
		}

		public static void Info(ulong categories, string msg) {
			Append(Severity.INF, categories, msg);
		}

		public static void Info(string msg, params string[] categories) {
			Append(Severity.INF, Categories(categories), msg);
		}

		public static void Warn(ulong categories, string msg) {
			Append(Severity.WAR, categories, msg);
		}

		public static void Warn(string msg, params string[] categories) {
			Append(Severity.WAR, Categories(categories), msg);
		}

		public static void Error(ulong categories, string msg) {
			Append(Severity.ERR, categories, msg);
		}

		public static void Error(string msg, params string[] categories) {
			Append(Severity.ERR, Categories(categories), msg);
		}

		public static void Assert(bool condition, ulong categories, string msg) {
			if (condition) return;
			Append(Severity.ASS, categories, msg);
			Debug.Break();
		}

		public static void Assert(bool condition, string msg, params string[] categories) {
			if (condition) return;
			Append(Severity.ASS, Categories(categories), msg);
			Debug.Break();
		}

		public static ulong Categories(params string[] categoryNames) {
			ulong bitField = 0;
			foreach (string categoryName in categoryNames) {
				sbyte bit = CategoryBit(categoryName);
				if (bit < 0) { // Category does not exist yet, create it
					bit = (sbyte) RegisterCategory(categoryName);
				}
				bitField |= (ulong) 1 << bit;
			}
			return bitField;
		}

		public static ulong Cat(params string[] categoryNames) {
			return Categories(categoryNames);
		}

		private static sbyte CategoryBit(string name) {
			int nameHash = name.GetHashCode();
			for (int i = 0; i < categories.Length; i++) {
				ref readonly Category category = ref categories[i];
				if (category.nameHash == nameHash) {
					return (sbyte) category.bit;
				}
			}
			return -1;
		}

		private static uint RegisterCategory(string name) {
			uint bit = FindFreeCategoryIndex();
			Assert(bit < categories.Length, "Too many categories!", "LOG");
			ref Category category = ref categories[bit];
			category = new Category() {
				bit = (byte) bit,
				name = name,
				nameHash = name.GetHashCode()
			};
			return bit;
		}

		private static uint FindFreeCategoryIndex() {
			for (uint bit = 0; bit < categories.Length; bit++) {
				ref readonly Category category = ref categories[bit];
				if (category.nameHash == 0) return bit;
			}
			return uint.MaxValue;
		}

		public static string[] GetCategoryNames(ulong categoryBits) {
			uint nameCount = 0;
			for (int bitIndex = 0; bitIndex < sizeof(ulong) * 8; bitIndex++) {
				if ((categoryBits & (1ul << bitIndex)) != 0ul) {
					nameCount++;
				}
			}
			string[] names = new string[nameCount];
			uint nameIndex = 0;
			for (int bitIndex = 0; bitIndex < sizeof(ulong) * 8; bitIndex++) {
				if ((categoryBits & (1ul << bitIndex)) != 0ul) {
					ref readonly Category category = ref categories[bitIndex];
					names[nameIndex++] = category.name;
				}
			}
			return names;
		}

		public static string GetCategoryNamesString(ulong categoryBits) {
			if (categoryBits == 0) return "";
			string names = "";
			for (int bitIndex = 1; bitIndex < sizeof(ulong) * 8; bitIndex++) {
				if ((categoryBits & (1ul << bitIndex)) != 0ul) {
					ref readonly Category category = ref categories[bitIndex];
					names += $"{category.name}/";
				}
			}
			return names.Remove(names.Length - 1);
		}

		private static void UnityDebug(Message msg) {
			switch (msg.severity) {
				case Severity.INF:
					Debug.Log(msg.ToString(false));
					break;
				case Severity.WAR:
					Debug.LogWarning(msg.ToString(false));
					break;
				case Severity.ERR:
					Debug.LogError(msg.ToString(false));
					break;
				case Severity.ASS:
					Debug.Assert(false, msg.ToString(false));
					break;
			}
		}

	}
}
