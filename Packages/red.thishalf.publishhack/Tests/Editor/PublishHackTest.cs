using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Reflection;
using System;

namespace PublishHack.Tests {

	public class TestPublishExtension : IHackedPublishExtension {

		[InitializeOnLoadMethod]
		private static void RegisterExtension() {
			HackedPublishExtensions.RegisterPublishExtension(new TestPublishExtension());
		}

		public string name => "TEST";

		public void OnPublish(IHackedPackageVersion packageVersion) {
			Debug.Log($"TEST Publish");
		}
	}

	class PublishHackTest {

		[Test]
		public void TestRegister() {
			Assembly upmDevAssembly = FindUPMDevelopAssembly();
			Assert.IsNotNull(upmDevAssembly, "Could not find UPM Develop Assembly!");

			Type pmDevExtensionsType = FindPublishExtensionsType(upmDevAssembly);
			Assert.IsNotNull(pmDevExtensionsType, "Could not find UPM Publish Extensions Type!");

			int beforeCount = RetrieveExtListCount(pmDevExtensionsType);
			// Unfortunately there is no Unregister for publish extensions so testing causes a dummy extension to be added and stays.
			HackedPublishExtensions.RegisterPublishExtension(new TestPublishExtension());
			int afterCount = RetrieveExtListCount(pmDevExtensionsType);

			Assert.AreEqual(beforeCount + 1, afterCount, "Publish Extensions Count not Incremented!");
		}

		private int RetrieveExtListCount(Type pmDevExtensionsType) {
			PropertyInfo extListProp = pmDevExtensionsType.GetProperty("publishExtensions", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.IsNotNull(extListProp);
			MethodInfo extListGet = extListProp.GetMethod;
			Assert.IsNotNull(extListGet);
			object extList = extListGet.Invoke(null, new object[0]);
			Type extListType = extList.GetType();
			Assert.IsNotNull(extListType);
			PropertyInfo extListCountProp = extListType.GetProperty("Count");
			Assert.IsNotNull(extListCountProp);
			MethodInfo extListCountGet = extListCountProp.GetMethod;
			Assert.IsNotNull(extListCountGet);

			return (int) extListCountGet.Invoke(extList, new object[0]);
		}

		private Assembly FindUPMDevelopAssembly() {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies) {
				if (assembly.GetName().Name == "Unity.PackageManagerUI.Develop.Editor") {
					return assembly;
				}
			}
			return null;
		}

		private Type FindPublishExtensionsType(Assembly assembly) {
			foreach (Type type in assembly.GetTypes()) {
				if (type.IsAbstract && type.IsSealed && type.Name == "PackageManagerDevelopExtensions") {
					return type;
				}
			}
			return null;
		}

	}
}