using System.Collections.Generic;
using System.Reflection;

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

		public struct Message {
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

		public delegate void MessageEvent(Message message);
		public static event MessageEvent onMessageAppended;

		public static ulong appendCategoriesFilter = ~0ul;

		private static uint currentCategoriesCount = 0;
		private static Category[] categories = new Category[sizeof(ulong) * 8];
		private static History<Message> messages = new History<Message>(1000);
		private static Dictionary<System.Type, ulong> typeMarkers = new Dictionary<System.Type, ulong>();
		private static Dictionary<MethodBase, ulong> methodMarkers = new Dictionary<MethodBase, ulong>();

		static Log() {
			Marker("LOG");
		}

		public static ulong Marker(params string[] categories) {
			System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
			System.Diagnostics.StackFrame callFrame = trace.GetFrame(1);
			MethodBase method = callFrame.GetMethod();
			System.Type type = method.DeclaringType;
			ulong categoryBits = Categories(categories);
			if (method == type.TypeInitializer) { // The caller is a static initializer
				typeMarkers[type] = categoryBits;
			} else { // The caller is a classic method
				methodMarkers[method] = categoryBits;
			}
			return categoryBits;
		}

		private static void Append(Severity severity, string msg) {
			ulong categories = FindMarkedCategories();
			Append(severity, categories, msg);
		}

		private static void Append(Severity severity, ulong categories, string msg) {
			if ((categories & appendCategoriesFilter) == 0) return;
			Message message = new Message() {
				severity = severity,
				categories = categories,
				content = msg,
				time = System.DateTime.Now
			};
			messages.Add(ref message);
			onMessageAppended?.Invoke(message);
		}

		private static ulong FindMarkedCategories() {
			System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
			System.Diagnostics.StackFrame callFrame = trace.GetFrame(3);
			MethodBase method = callFrame.GetMethod();
			System.Type type = method.DeclaringType;
			ulong typeCategories;
			typeMarkers.TryGetValue(type, out typeCategories);
			ulong methodCategories;
			methodMarkers.TryGetValue(method, out methodCategories);
			return typeCategories | methodCategories;
		}

		#region Info
		public static void Info(ulong categories, string msg) {
			Append(Severity.INF, categories, msg);
		}

		public static void Info(string msg, params string[] categories) {
			Append(Severity.INF, Categories(categories), msg);
		}

		public static void Info(string msg) {
			Append(Severity.INF, msg);
		}

		public static void Info(ulong categories, object msg) {
			Append(Severity.INF, categories, msg.ToString());
		}

		public static void Info(object msg, params string[] categories) {
			Append(Severity.INF, Categories(categories), msg.ToString());
		}

		public static void Info(object msg) {
			Append(Severity.INF, msg.ToString());
		}
		#endregion

		#region Warn
		public static void Warn(ulong categories, string msg) {
			Append(Severity.WAR, categories, msg);
		}

		public static void Warn(string msg, params string[] categories) {
			Append(Severity.WAR, Categories(categories), msg);
		}

		public static void Warn(string msg) {
			Append(Severity.WAR, msg);
		}

		public static void Warn(ulong categories, object msg) {
			Append(Severity.WAR, categories, msg.ToString());
		}

		public static void Warn(object msg, params string[] categories) {
			Append(Severity.WAR, Categories(categories), msg.ToString());
		}

		public static void Warn(object msg) {
			Append(Severity.WAR, msg.ToString());
		}
		#endregion

		#region Error
		public static void Error(ulong categories, string msg) {
			Append(Severity.ERR, categories, msg);
		}

		public static void Error(string msg, params string[] categories) {
			Append(Severity.ERR, Categories(categories), msg);
		}

		public static void Error(string msg) {
			Append(Severity.ERR, msg);
		}

		public static void Error(ulong categories, object msg) {
			Append(Severity.ERR, categories, msg.ToString());
		}

		public static void Error(object msg, params string[] categories) {
			Append(Severity.ERR, Categories(categories), msg.ToString());
		}

		public static void Error(object msg) {
			Append(Severity.ERR, msg.ToString());
		}
		#endregion

		#region Assert
		public static void Assert(bool condition, ulong categories, string msg) {
			if (condition) return;
			Append(Severity.ASS, categories, msg);
		}

		public static void Assert(bool condition, string msg, params string[] categories) {
			if (condition) return;
			Append(Severity.ASS, Categories(categories), msg);
		}

		public static void Assert(bool condition, string msg) {
			if (condition) return;
			Append(Severity.ASS, msg);
		}

		public static void Assert(bool condition, ulong categories, object msg) {
			if (condition) return;
			Append(Severity.ASS, categories, msg.ToString());
		}

		public static void Assert(bool condition, object msg, params string[] categories) {
			if (condition) return;
			Append(Severity.ASS, Categories(categories), msg.ToString());
		}

		public static void Assert(bool condition, object msg) {
			if (condition) return;
			Append(Severity.ASS, msg.ToString());
		}
		#endregion

		#region Fallback
		public static T Fallback<T>(bool condition, T current, T fallback, ulong categories, string msg) {
			if (condition) return current;
			Append(Severity.ASS, categories, msg);
			return fallback;
		}

		public static T Fallback<T>(bool condition, T current, T fallback, string msg, params string[] categories) {
			if (condition) return current;
			Append(Severity.ASS, Categories(categories), msg);
			return fallback;
		}

		public static T Fallback<T>(bool condition, T current, T fallback, string msg) {
			if (condition) return current;
			Append(Severity.ASS, msg);
			return fallback;
		}

		public static T Fallback<T>(bool condition, T current, T fallback, ulong categories, object msg) {
			if (condition) return current;
			Append(Severity.ASS, categories, msg.ToString());
			return fallback;
		}

		public static T Fallback<T>(bool condition, T current, T fallback, object msg, params string[] categories) {
			if (condition) return current;
			Append(Severity.ASS, Categories(categories), msg.ToString());
			return fallback;
		}

		public static T Fallback<T>(bool condition, T current, T fallback, object msg) {
			if (condition) return current;
			Append(Severity.ASS, msg.ToString());
			return fallback;
		}
		#endregion

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
			uint bit = currentCategoriesCount++;
			Assert(bit < categories.Length, "Too many categories!", "LOG");
			ref Category category = ref categories[bit];
			category = new Category() {
				bit = (byte) bit,
				name = name,
				nameHash = name.GetHashCode()
			};
			return bit;
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

		public static void Clear() {
			messages.Clear();
		}

	}

	public interface LogOutput {
		public void ProcessMessage(Log.Message message);
	}
}
