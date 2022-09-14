# Publish Hack

The purpose of this package is to provide an API to create your own publish extensions for package development.
Currently Unity's "Package Development" package provides some utilities to ease package development but few are exposed for extension. This is the case for the publish button, although everything is already implemented for it to be extensible, all of it is under `internal` visibility.<br>
Now you can use the `HackedPublishExtensions` type to add your own publish extensions with your code.


## Usage Example

```CSharp
using UnityEngine;
using UnityEditor;
using PublishHack;
public class CustomPublishExtension : IHackedPublishExtension {

	[InitializeOnLoadMethod]
	private static void RegisterExtension() {
		HackedPublishExtensions.RegisterPublishExtension(new CustomPublishExtension());
	}

	public string name => "Custom Publish!";

	public void OnPublish(IHackedPackageVersion packageVersion) {
		Debug.Log($"Yay! Custom Publish Triggered on: {packageVersion.displayName} (v{packageVersion.versionString})");
	}
}
```


## How does it work?

Since the code of the Unity package does not expose anything, a few "hacks" were required to make this possible (hence the "Publish Hack" name). This is an overview of the step required to make this work.

#### Types Fetch

For the next steps we will need references to certain non visible types. To get these references first we find the relevant assemblies among the currently loaded assemblies (`Unity.PackageManagerUI.Develop.Editor` and `UnityEditor.PackageManagerUIModule`).
Then we search in the assemblies the types `IPackageVersion`, `IPublishExtension` and `PackageManagerDevelopExtensions`.
`IPackageVersion` is an interface to retrieve values describing the package (such as `name`, `version` and `packageInfo`).
`IPublishExtension` is the interface type needed for the publish extension callback, it has just a `name` property and `OnPublish` method.
`PackageManagerDevelopExtensions` is the type used to register new publish extensions.

#### IL Code Generation

Since to add a publish extension we need an object of type `IPublishExtension` which is not visible, we need to generate this type at runtine (we will call it patch type). The patch type inherits the `IPublishExtension`, it has a field of type `IHackedPublishExtension`. Its `name` property just returns the name from its `IHackedPublishExtension` field. However its implementation of `OnPublish` needs to copy all the values from the `IPackageVersion` object to a new `HackedPackageVersion` object. Once the copy is done it simply calls `OnPublish` on its `IHackedPublishExtension` field.
This is all done using IL code to bypass the visibility restrictions.

#### Injection

When a new publish extension is registered we simply use reflection to create an instance of our patch type. We can then retrieve the `RegisterPublishExtension` method on `PackageManagerDevelopExtensions` and call it using reflection (passing the patch type instance).

