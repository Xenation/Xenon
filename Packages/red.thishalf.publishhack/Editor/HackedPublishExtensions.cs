using System.Reflection.Emit;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;

namespace PublishHack {
	// Removed for simplicity
	//public interface IHackedPackage {
	//	string uniqueId { get; }
	//	string name { get; }
	//	IEnumerable<IHackedPackageVersion> versions { get; }
	//}

	/// <summary>An interface that mirrors IPackageVersion from UnityEditor.PackageManagerUIModule</summary>
	public interface IHackedPackageVersion {
		string name { get; }
		string displayName { get; }
		string versionString { get; }
		string uniqueId { get; }
		string packageUniqueId { get; }
		//IHackedPackage package { get; }
		UnityEditor.PackageManager.PackageInfo packageInfo { get; }
		bool isInstalled { get; }
	}

	/// <summary>A class to contains a package's information</summary>
	/// <remarks>You should not need to use this direcly, see <see cref="IHackedPackageVersion"/>.</remarks>
	public class HackedPackageVersion : IHackedPackageVersion {
		public string name { get; set; }
		public string displayName { get; set; }
		public string versionString { get; set; }
		public string uniqueId { get; set; }
		public string packageUniqueId { get; set; }
		//public IHackedPackage package { get; set; }
		public UnityEditor.PackageManager.PackageInfo packageInfo { get; set; }
		public bool isInstalled { get; set; }
	}

	/// <summary>The interface to implement to register a publish extension through <see cref="HackedPublishExtensions"/></summary>
	public interface IHackedPublishExtension {
		/// <summary>The text to display in the new publish button</summary>
		string name { get; }
		/// <summary>This method will be called when the publish button is pressed</summary>
		void OnPublish(IHackedPackageVersion packageVersion);
	}

	/// <summary>Static type to register new Publish Extensions</summary>
	public static class HackedPublishExtensions {

		private static bool isLoaded = false;

		private static Assembly pmDevAssembly;
		private static Assembly pmUiAssembly;

		private static Type packageVersionInterface = null;
		private static Type publishExtensionInterface = null;
		private static Type pmDevExtensionsType = null;

		private static Type patchType = null;

		/// <summary>Registers the provided publish extension to the publish button</summary>
		public static void RegisterPublishExtension(IHackedPublishExtension ext) {
			if (!isLoaded) {
				SetupAll();
			}
			InjectPublishExtension(ext);
		}

		private static void SetupAll() {
			RetrievePackageManagerTypes();
			CreatePatchDynamicType();
			isLoaded = true;
		}

		private static void RetrievePackageManagerTypes() {
			GetPackageManagerAssemblies(out pmDevAssembly, out pmUiAssembly);

			// Find the IPackageVersion interface
			foreach (Type type in pmUiAssembly.GetTypes()) {
				if (!type.IsInterface) continue; // Ignore Non interfaces

				if (type.Name == "IPackageVersion") {
					packageVersionInterface = type;
					//Debug.Log($"Retrieved {type}");
					break;
				}
			}

			// Find the IPublishExtension interface and extensions manager
			foreach (Type type in pmDevAssembly.GetTypes()) {
				if (type.IsInterface && type.Name == "IPublishExtension") {
					publishExtensionInterface = type;
					//Debug.Log($"Retrieved {type}");
				} else if (type.IsAbstract && type.IsSealed && type.Name == "PackageManagerDevelopExtensions") {
					pmDevExtensionsType = type;
					//Debug.Log($"Retrieved {type}");
				}
				if (publishExtensionInterface != null && pmDevExtensionsType != null) {
					break;
				}
			}
		}

		private static void GetPackageManagerAssemblies(out Assembly developAsm, out Assembly uiAsm) {
			developAsm = null;
			uiAsm = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies) {
				if (assembly.GetName().Name == "Unity.PackageManagerUI.Develop.Editor") {
					developAsm = assembly;
				} else if (assembly.GetName().Name == "UnityEditor.PackageManagerUIModule") {
					uiAsm = assembly;
				}
				if (developAsm != null && uiAsm != null) {
					return;
				}
			}
		}

		private static void CreatePatchDynamicType() {
			// Build the "patch" type
			AssemblyName patchAssemblyName = new AssemblyName("Xenon.DynPatch");
			AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(patchAssemblyName, AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(patchAssemblyName.Name, patchAssemblyName.Name + ".dll", false);
			TypeAttributes typeAttr = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout;
			TypeBuilder typeBuilder = modBuilder.DefineType("DynPatcher", typeAttr, typeof(object), new Type[] { publishExtensionInterface });

			// hacked interface field for interop
			FieldAttributes hackedInterfAttr = FieldAttributes.Private;
			FieldBuilder hackedInterfFieldBuilder = typeBuilder.DefineField("hackedInterface", typeof(IHackedPublishExtension), hackedInterfAttr);

			// Constructor
			typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(IHackedPublishExtension) });
			ILGenerator constructorIL = constructorBuilder.GetILGenerator();
			constructorIL.Emit(OpCodes.Ldarg_0);
			constructorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
			constructorIL.Emit(OpCodes.Nop);
			constructorIL.Emit(OpCodes.Nop);
			constructorIL.Emit(OpCodes.Ldarg_0);
			constructorIL.Emit(OpCodes.Ldarg_1);
			constructorIL.Emit(OpCodes.Stfld, hackedInterfFieldBuilder);
			constructorIL.Emit(OpCodes.Ret);

			// name property from interface
			PropertyBuilder namePropBuilder = typeBuilder.DefineProperty("name", PropertyAttributes.None, typeof(string), null);
			MethodAttributes namePropGetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.Final;
			MethodBuilder namePropGetBuilder = typeBuilder.DefineMethod("get_name", namePropGetAttr, typeof(string), Type.EmptyTypes);
			ILGenerator namePropGetIL = namePropGetBuilder.GetILGenerator();
			//namePropGetIL.Emit(OpCodes.Ldstr, "ON GET NAME");
			//namePropGetIL.Emit(OpCodes.Call, typeof(Debug).GetMethod("Log", new Type[] { typeof(object) }));
			namePropGetIL.Emit(OpCodes.Ldarg_0);
			namePropGetIL.Emit(OpCodes.Ldfld, hackedInterfFieldBuilder);
			namePropGetIL.Emit(OpCodes.Callvirt, typeof(IHackedPublishExtension).GetProperty("name").GetGetMethod());
			namePropGetIL.Emit(OpCodes.Ret);
			namePropBuilder.SetGetMethod(namePropGetBuilder);

			// OnPublish from interface
			MethodAttributes onPublishAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;
			MethodBuilder onPublishBuilder = typeBuilder.DefineMethod("OnPublish", onPublishAttr, typeof(void), new Type[] { packageVersionInterface });
			ILGenerator onPublishIL = onPublishBuilder.GetILGenerator();
			LocalBuilder hackedVersionBuilder = onPublishIL.DeclareLocal(typeof(HackedPackageVersion));
			//onPublishIL.Emit(OpCodes.Nop);
			//onPublishIL.Emit(OpCodes.Ldstr, "ON PUBLISH!");
			//onPublishIL.Emit(OpCodes.Call, typeof(Debug).GetMethod("Log", new Type[] { typeof(object) }));
			onPublishIL.Emit(OpCodes.Nop);

			onPublishIL.Emit(OpCodes.Newobj, typeof(HackedPackageVersion).GetConstructor(Type.EmptyTypes));
			onPublishIL.Emit(OpCodes.Stloc_0);

			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "name");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "displayName");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "versionString");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "uniqueId");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "packageUniqueId");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "packageInfo");
			CopyToLocalStructReflect(onPublishIL, hackedVersionBuilder, "isInstalled");

			onPublishIL.Emit(OpCodes.Ldarg_0);
			onPublishIL.Emit(OpCodes.Ldfld, hackedInterfFieldBuilder);
			onPublishIL.Emit(OpCodes.Ldloc_0);
			onPublishIL.Emit(OpCodes.Box, typeof(HackedPackageVersion));
			onPublishIL.Emit(OpCodes.Callvirt, typeof(IHackedPublishExtension).GetMethod("OnPublish"));
			onPublishIL.Emit(OpCodes.Nop);
			onPublishIL.Emit(OpCodes.Ret);

			// Create Patch Type
			patchType = typeBuilder.CreateType();

			// Save the assembly for debugging
			//asmBuilder.Save(asmBuilder.GetName().Name + ".dll");
		}

		private static void CopyToLocalStructReflect(ILGenerator il, LocalBuilder local, string propertyName) {
			il.Emit(OpCodes.Ldstr, propertyName);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Call, typeof(HackedPublishExtensions).GetMethod("CopyPrivateProperty", BindingFlags.Public | BindingFlags.Static));
			il.Emit(OpCodes.Nop);

		}

		/// <summary>DO NOT USE, MEANT FOR USE BY THE GENERATED IL CODE IN THE DYNAMIC TYPE</summary>
		public static void CopyPrivateProperty(string propertyName, object source, HackedPackageVersion destination) {
			MethodInfo getterInfo = packageVersionInterface.GetProperty(propertyName).GetGetMethod();
			MethodInfo setterInfo = typeof(HackedPackageVersion).GetProperty(propertyName).GetSetMethod();
			object val = getterInfo.Invoke(source, null);
			setterInfo.Invoke(destination, new object[] { val });
		}

		private static void InjectPublishExtension(IHackedPublishExtension ext) {
			// Create patch instance
			object patch = Activator.CreateInstance(patchType, ext);

			// Inject Patch object
			pmDevExtensionsType.GetMethod("RegisterPublishExtension", new Type[] { publishExtensionInterface }).Invoke(null, new object[] { patch });
			Debug.Log($"Injected {ext.GetType().Name} through patch object!");
		}

	}
}
