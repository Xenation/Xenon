using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public class ConditionalSymbolsFixPostprocessor : AssetPostprocessor {

		private static readonly List<string> excludedSymbols = new List<string>(new string[] {
			"DEBUG"
		});

		private static readonly List<string> addedSymbols = new List<string>(new string[] {
			"RELEASE"
		});

		private static Regex debugGroupRegex = new Regex(@"<PropertyGroup Condition=.*Debug.*>[\s\S]*?<DefineConstants>(.*)</DefineConstants>[\s\S]*?</PropertyGroup>", RegexOptions.Multiline | RegexOptions.Compiled);
		private static Regex releaseGroupRegex = new Regex(@"<PropertyGroup Condition=.*Release.*>[\s\S]*?(</PropertyGroup>)", RegexOptions.Multiline | RegexOptions.Compiled);

		// TODO Check for existing DefineConstants tag in Release config PropertyGroup as it may get added/fixed eventually
		public static string OnGeneratedCSProject(string path, string content) {
			// Search for the Debug symbols using regex
			Match debugGroupMatch = debugGroupRegex.Match(content);
			if (!debugGroupMatch.Success) {
				Debug.LogWarning($"Could not find any PropertyGroup with Debug Condition! (kept csproj file unmodified)\n{path}");
				return content;
			}

			// Fetch symbols into a list
			string debugSymbolsStr = debugGroupMatch.Groups[1].Captures[0].Value;
			List<string> debugSymbols = new List<string>(debugSymbolsStr.Split(';'));
			// Remove all symbols that shouldn't be in Release Configuration
			for (int i = 0; i < debugSymbols.Count; i++) {
				if (excludedSymbols.Contains(debugSymbols[i])) {
					debugSymbols.RemoveAt(i);
					i--;
				}
			}
			// Add all the Release only symbols
			debugSymbols.AddRange(addedSymbols);
			// Reform a string for all the symbols left
			string releaseSymbolsStr = string.Join(';', debugSymbols);

			// Search for the Release group to insert the symbols
			Match releaseGroupMatch = releaseGroupRegex.Match(content);
			if (!releaseGroupMatch.Success) {
				Debug.LogWarning($"Could not find any PropertyGroup with Release Condition! (kept csproj file unmodified)\n{path}");
				return content;
			}

			// Insert new DefineConstants block
			string finalContent = content.Insert(releaseGroupMatch.Groups[1].Captures[0].Index, $"  <DefineConstants>{releaseSymbolsStr}</DefineConstants>\n");

			return finalContent;
		}

	}
}
