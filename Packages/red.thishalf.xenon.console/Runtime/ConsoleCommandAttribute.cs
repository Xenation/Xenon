using System;

namespace Xenon.Console {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class ConsoleCommandAttribute : Attribute {

		public string name;

		public ConsoleCommandAttribute(string name) {
			this.name = name;
		}

	}
}
