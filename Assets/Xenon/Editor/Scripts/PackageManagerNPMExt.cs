using UnityEditor;
using PublishHack;

namespace Xenon.Editor {
	public class NPMPublishExt : IHackedPublishExtension {

		[InitializeOnLoadMethod]
		private static void RegisterExtension() {
			HackedPublishExtensions.RegisterPublishExtension(new NPMPublishExt());
		}

		public string name => "Publish to npm";

		public void OnPublish(IHackedPackageVersion packageVersion) {
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			//startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			UnityEditor.PackageManager.PackageInfo packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath($"Packages/{packageVersion.name}");
			startInfo.Arguments = $"/k npm publish {packageInfo.resolvedPath} --registry https://npm.thishalf.red";
			process.StartInfo = startInfo;
			process.Start();
		}
	}
}
