using System;

namespace Xenon.Console {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ConsoleArgumentParserAttribute : Attribute {}
}
