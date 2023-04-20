using System;
using System.Reflection;

using UnityEditor.PackageManager;

namespace Xenon.Console.Example {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ConsoleCommandBAttribute : Attribute {
		public ConsoleCommandBAttribute(string name) {}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ConsoleArgumentParserAttribute : Attribute {
		public ConsoleArgumentParserAttribute() {}
	}

	public interface IConsoleArgument<T> {
		object TryGet();
	}

	public class Actor {

		[ConsoleArgumentParser]
		public static Actor TryGetActor(string name) {
			return null;
		}

	}

	public class ConsoleUsageExample {

		// With title
		[ConsoleCommandB("spawn")]
		public static void Spawn(Actor actor) {
			
		}

		private static void RndTests() {
			
			
		}

		private static void Exec() {
			
			MethodInfo parseMethod = GetParsingMethod<Actor>();
		}

		private static MethodInfo GetParsingMethod<T>() {
			foreach (MethodInfo method in typeof(T).GetMethods(BindingFlags.Static | BindingFlags.Public)) {
				ConsoleArgumentParserAttribute parserAttr = method.GetCustomAttribute<ConsoleArgumentParserAttribute>();
				if (parserAttr != null) {
					return method;
				}
			}
			return null;
		}

		// Tries to find a casting method to parse an arg into the type the method uses
		private static T TryCast<T>(string str) where T : class {
			MethodInfo castMethod = typeof(T).GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
			if (castMethod == null) return null;
			return (T) castMethod.Invoke(null, new object[] { str });
		}

	}
}
