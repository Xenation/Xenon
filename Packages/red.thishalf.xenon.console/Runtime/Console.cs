using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Xenon.Console {
	public static class Console {

		#region Types
		private class Command {
			public string name;
			public List<CommandOverload> overloads;

			public Command(ConsoleCommandAttribute attrib) {
				name = attrib.name;
			}

			public void AddOverload(MethodInfo method) {
				CommandOverload overload = CommandOverload.FromMethod(method);
				if (overload != null) {
					overloads.Add(overload);
				}
			}

			public bool Execute(string[] args) {
				foreach (CommandOverload overload in overloads) {
					object[] parsedArgs = overload.TryParse(args);
					if (parsedArgs != null) {
						// We found a valid overload, call it
						overload.method.Invoke(null, parsedArgs);
						return true;
					}
				}
				return false;
			}

		}

		private class CommandOverload {

			public static CommandOverload FromMethod(MethodInfo method) {
				CommandOverload overload = new CommandOverload(method);
				if (!overload.FetchArgTypes()) {
					return null;
				}
				return overload;
			}

			public MethodInfo method;
			public ParameterInfo[] methodParams;
			public CommandArgumentType[] argTypes;

			private CommandOverload(MethodInfo method) {
				this.method = method;
				methodParams = method.GetParameters();
			}

			private bool FetchArgTypes() {
				argTypes = new CommandArgumentType[methodParams.Length];
				foreach (ParameterInfo param in methodParams) {
					CommandArgumentType cmdArgType;
					if (!knownArgTypes.TryGetValue(param.ParameterType, out cmdArgType)) {
						// The Argument Type has no known parser, thus this overload is invalid
						return false;
					}
				}
				return true;
			}

			public object[] TryParse(string[] args) {
				if (args.Length > argTypes.Length) {
					// Too many input args
					return null;
				}

				object[] parsedArgs = new object[argTypes.Length];

				int argIndex;
				for (argIndex = 0; argIndex < args.Length; argIndex++) {
					object parsedArg = argTypes[argIndex].TryParse(args[argIndex]);
					if (parsedArg == null) {
						// ARG CAN'T BE PARSED
						return null;
					}
					parsedArgs[argIndex] = parsedArg;
				}

				if (argIndex != argTypes.Length) {
					// Less arguments than needed were provided
					// Try to fill the remaining with defaults
					for (; argIndex < argTypes.Length; argIndex++) {
						if (!methodParams[argIndex].HasDefaultValue) {
							// param has no default, can't make it up
							return null;
						}
						parsedArgs[argIndex] = methodParams[argIndex].DefaultValue;
					}
				}

				return parsedArgs;
			}
		}

		private class CommandArgumentType {
			public Type type;

			public CommandArgumentType(Type type) {
				this.type = type;
			}

			public virtual object TryParse(string arg) {
				return null;
			}
		}

		private abstract class CommandNativeArgumentType<T> : CommandArgumentType {

			public CommandNativeArgumentType() : base(typeof(T)) {}

			public override object TryParse(string arg) {
				T parsed;
				if (!NativeTryParse(arg, out parsed)) {
					return null;
				}
				return parsed;
			}

			public abstract bool NativeTryParse(string str, out T parsed);

		}

		private class CommandBoolArgumentType : CommandNativeArgumentType<bool> {
			public override bool NativeTryParse(string str, out bool parsed) {
				return bool.TryParse(str, out parsed);
			}
		}
		private class CommandByteArgumentType : CommandNativeArgumentType<byte> {
			public override bool NativeTryParse(string str, out byte parsed) {
				return byte.TryParse(str, out parsed);
			}
		}
		private class CommandSByteArgumentType : CommandNativeArgumentType<sbyte> {
			public override bool NativeTryParse(string str, out sbyte parsed) {
				return sbyte.TryParse(str, out parsed);
			}
		}
		private class CommandUShortArgumentType : CommandNativeArgumentType<ushort> {
			public override bool NativeTryParse(string str, out ushort parsed) {
				return ushort.TryParse(str, out parsed);
			}
		}
		private class CommandShortArgumentType : CommandNativeArgumentType<short> {
			public override bool NativeTryParse(string str, out short parsed) {
				return short.TryParse(str, out parsed);
			}
		}
		private class CommandUIntArgumentType : CommandNativeArgumentType<uint> {
			public override bool NativeTryParse(string str, out uint parsed) {
				return uint.TryParse(str, out parsed);
			}
		}
		private class CommandIntArgumentType : CommandNativeArgumentType<int> {
			public override bool NativeTryParse(string str, out int parsed) {
				return int.TryParse(str, out parsed);
			}
		}
		private class CommandULongArgumentType : CommandNativeArgumentType<ulong> {
			public override bool NativeTryParse(string str, out ulong parsed) {
				return ulong.TryParse(str, out parsed);
			}
		}
		private class CommandLongArgumentType : CommandNativeArgumentType<long> {
			public override bool NativeTryParse(string str, out long parsed) {
				return long.TryParse(str, out parsed);
			}
		}
		private class CommandFloatArgumentType : CommandNativeArgumentType<float> {
			public override bool NativeTryParse(string str, out float parsed) {
				return float.TryParse(str, out parsed);
			}
		}
		private class CommandDoubleArgumentType : CommandNativeArgumentType<double> {
			public override bool NativeTryParse(string str, out double parsed) {
				return double.TryParse(str, out parsed);
			}
		}

		private class CommandCustomArgumentType : CommandArgumentType {
			public MethodInfo parser;

			public CommandCustomArgumentType(MethodInfo parser) : base(parser.ReturnType) {
				this.parser = parser;
			}

			public override object TryParse(string arg) {
				return parser.Invoke(null, new object[] { arg });
			}

		}
		#endregion



		private static Dictionary<Type, CommandArgumentType> knownArgTypes = new Dictionary<Type, CommandArgumentType>();
		private static Dictionary<string, Command> commands = new Dictionary<string, Command>();
		private static ConsoleTerminal terminal;

		static Console() {
			RegisterNativeArgTypes();
			FetchAllCommands();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void IntializeTerminal() {
			Debug.Log($"CONSOLE INIT TERM");
			terminal = ConsoleTerminal.GetOrCreateTerminal();
		}

		public static void Write(string str) {
			terminal.Write(str);
		}

		public static void ExecuteCommand(string command) {
			command = command.Trim();
			List<string> args = new List<string>(command.Split(' '));
			if (args == null || args.Count == 0) {
				return;
			}
			string cmdName = args[0];
			Command cmd = FindCommand(cmdName);
			args.RemoveAt(0);
			if (!cmd.Execute(args.ToArray())) {
				Write("Could not parse the command!");
			}
		}

		private static Command FindCommand(string name) {
			return null;
		}

		private static void RegisterNativeArgTypes() {
			knownArgTypes.Add(typeof(bool), new CommandBoolArgumentType());
			knownArgTypes.Add(typeof(byte), new CommandByteArgumentType());
			knownArgTypes.Add(typeof(sbyte), new CommandSByteArgumentType());
			knownArgTypes.Add(typeof(ushort), new CommandUShortArgumentType());
			knownArgTypes.Add(typeof(short), new CommandShortArgumentType());
			knownArgTypes.Add(typeof(uint), new CommandUIntArgumentType());
			knownArgTypes.Add(typeof(int), new CommandIntArgumentType());
			knownArgTypes.Add(typeof(ulong), new CommandULongArgumentType());
			knownArgTypes.Add(typeof(long), new CommandLongArgumentType());
			knownArgTypes.Add(typeof(float), new CommandFloatArgumentType());
			knownArgTypes.Add(typeof(double), new CommandDoubleArgumentType());
		}

		private static void FetchAllCommands() {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies) {
				Type[] types = assembly.GetTypes();
				foreach (Type type in types) {
					MethodInfo[] staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
					foreach (MethodInfo method in staticMethods) {
						IEnumerable<Attribute> attributes = method.GetCustomAttributes();
						foreach (Attribute attribute in attributes) {
							switch (attribute) {
								case ConsoleCommandAttribute cmd:
									Command command;
									if (!commands.TryGetValue(cmd.name, out command)) {
										// Command does not exist yet create it
										command = new Command(cmd);
										command.AddOverload(method);
										commands.Add(cmd.name, command);
									} else {
										// Command already has at least one overload add a new one
										command.AddOverload(method);
									}
									break;
								case ConsoleArgumentParserAttribute parser:
									if (knownArgTypes.ContainsKey(method.ReturnType)) {
										Debug.LogWarning($"Found a duplicate console arg parser for type {method.ReturnType}, ignoring!");
										break;
									}
									knownArgTypes.Add(method.ReturnType, new CommandCustomArgumentType(method));
									break;
							}
						}
					}
				}
			}
		}

	}
}
