using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;

namespace Xenon.Guid {
	public class GuidManager : EditorWindow {

		private static readonly string winTitle = "Guid Manager";
		private static readonly Vector2 winMinSize = new Vector2(300f, 300f);
		private static readonly Vector2Int maxTextureSize = new Vector2Int(8192, 8192);

		private static readonly List<int> metaExtHashes = new List<int>(new int[] {
			".meta".GetHashCode(),
			".unity".GetHashCode()
		});
		private static readonly Regex guidRegex = new Regex(@"\s+([a-f0-9]{32}),?\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex guidFieldRegex = new Regex(@"\s+guid:\s+([a-f0-9]{32})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		[MenuItem("Window/General/Guid Manager", priority = 21)]
		public static void ShowWindow() {
			GuidManager win = GetWindow<GuidManager>();
			if (win == null) {
				Debug.LogWarning("Could not create Window!");
				return;
			}
			win.titleContent = new GUIContent(winTitle);
			win.minSize = winMinSize;
		}


		private string targetGuidStr;
		private GUID targetGuid;

		private void OnGUI() {
			targetGuidStr = EditorGUILayout.TextField("Target GUID", targetGuidStr);
			if (!GUID.TryParse(targetGuidStr, out targetGuid)) {
				EditorGUILayout.HelpBox("Invalid GUID", MessageType.Warning);
			}
			if (GUILayout.Button("Select")) {
				SelectAsset();
			}
			if (GUILayout.Button("Search References")) {
				List<GUID> referencing = SearchGUID(targetGuid);
				Object[] refObjects = new Object[referencing.Count];
				Debug.Log("Referencing:");
				for (int i = 0; i < referencing.Count; i++) {
					Debug.Log(referencing[i]);
					refObjects[i] = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(referencing[i]));
				}
				Selection.objects = refObjects;
			}
		}

		private void SelectAsset() {
			string assetPath = AssetDatabase.GUIDToAssetPath(targetGuid);
			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
		}

		private List<GUID> SearchGUID(GUID guid) {
			string guidStr = guid.ToString();
			string path = "Assets/";
			List<GUID> referencing = new List<GUID>();
			
			foreach (FileInfo metaFile in TraverseAllMetaFiles(path)) {
				FileStream stream = metaFile.OpenRead();
				StreamReader reader = new StreamReader(stream);
				string fileContent = reader.ReadToEnd();
				MatchCollection matches = guidRegex.Matches(fileContent);
				foreach (Match match in matches) {
					if (match.Groups[1].Value.Equals(guidStr, System.StringComparison.InvariantCultureIgnoreCase)) {
						referencing.Add(GetGUID(metaFile, fileContent));
						break;
					}
				}
			}

			return referencing;
		}

		private GUID GetGUID(FileInfo file, string fileContent) {
			GUID guid;
			if (!file.Extension.Equals(".meta", System.StringComparison.InvariantCultureIgnoreCase)) {
				// the extension is not .meta, we need to open and read the corresponding .meta file to get the GUID
				FileStream stream = File.OpenRead(file.FullName + ".meta");
				StreamReader reader = new StreamReader(stream);
				fileContent = reader.ReadToEnd();
			}
			MatchCollection matches = guidFieldRegex.Matches(fileContent);
			foreach (Match match in matches) {
				if (GUID.TryParse(match.Groups[1].Value, out guid)) {
					return guid;
				}
			}
			return default;
		}

		private IEnumerable<FileInfo> TraverseAllMetaFiles(string root) {
			Stack<DirectoryInfo> directoryStack = new Stack<DirectoryInfo>();
			directoryStack.Push(new DirectoryInfo(root));
			while (directoryStack.Count > 0) {
				DirectoryInfo dir = directoryStack.Pop();
				try {
					foreach (DirectoryInfo subDir in dir.GetDirectories()) {
						directoryStack.Push(subDir);
					}
				} catch (System.UnauthorizedAccessException) {
					continue;
				}
				foreach (FileInfo file in dir.GetFiles()) {
					int extHash = file.Extension.GetHashCode();
					if (metaExtHashes.Contains(extHash)) {
						yield return file;
					}
					//if (file.Extension.Equals(".meta", System.StringComparison.InvariantCultureIgnoreCase)) {
					//	yield return file;
					//}
				}
			}
		}

	}
}
