using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public static class EdGUIPlus {

		public static T EnumButtonsField<T>(string label, T enVal, params string[] enLabels) {
			Type enType = enVal.GetType();
			Array enArray = Enum.GetValues(enType);
			List<T> values = new List<T>((T[]) enArray);

			return values[EnumToggles(label, enLabels, values.IndexOf(enVal))];
		}

		public static T EnumButtonsField<T>(string label, T enVal) {
			Type enType = enVal.GetType();
			Array enArray = Enum.GetValues(enType);
			List<T> values = new List<T>((T[]) enArray);
			string[] enLabels = new string[values.Count];
			for (int i = 0; i < values.Count; i++) {
				enLabels[i] = values[i].ToString();
			}

			return values[EnumToggles(label, enLabels, values.IndexOf(enVal))];
		}

		private static int EnumToggles(string mainLabel, string[] labels, int selectedIndex) {

			Rect totalRect = EditorGUILayout.GetControlRect();
			Rect fieldRect;
			if (mainLabel == null || mainLabel == "") {
				fieldRect = EditorGUI.IndentedRect(totalRect);
			} else {
				fieldRect = EditorGUI.PrefixLabel(totalRect, new GUIContent(mainLabel));
			}
			float btnWidth = fieldRect.width / labels.Length;
			Rect curRect = new Rect(fieldRect.x, fieldRect.y, btnWidth, fieldRect.height);

			if (labels.Length < 2) { // Only One Button
				GUI.Toggle(curRect, true, labels[0], EditorStyles.miniButton);
			} else { // Normal
				if (GUI.Toggle(curRect, (selectedIndex == 0), labels[0], EditorStyles.miniButtonLeft)) {
					selectedIndex = 0;
				}
				curRect.x += btnWidth;
				for (int i = 1; i < labels.Length - 1; i++) {
					if (GUI.Toggle(curRect, (selectedIndex == i), labels[i], EditorStyles.miniButtonMid)) {
						selectedIndex = i;
					}
					curRect.x += btnWidth;
				}
				if (GUI.Toggle(curRect, (selectedIndex == labels.Length - 1), labels[labels.Length - 1], EditorStyles.miniButtonRight)) {
					selectedIndex = labels.Length - 1;
				}
			}

			return selectedIndex;
		}

		public static bool Button(string text) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), text);
		}
		public static bool Button(GUIContent content) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), content);
		}
		public static bool Button(Texture texture) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), texture);
		}
		public static bool Button(string text, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), text, style);
		}
		public static bool Button(GUIContent content, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), content, style);
		}
		public static bool Button(Texture texture, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 18f)), texture, style);
		}

		public static bool InlineButton(string text) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), text);
		}
		public static bool InlineButton(GUIContent content) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), content);
		}
		public static bool InlineButton(Texture texture) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), texture);
		}
		public static bool InlineButton(string text, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), text, style);
		}
		public static bool InlineButton(GUIContent content, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), content, style);
		}
		public static bool InlineButton(Texture texture, GUIStyle style) {
			return GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false)), texture, style);
		}

		public static int IndentToPixels(int indent) { // Fix for GUILayout not using indents
			//return (indent == 0) ? 4 : indent * 19 - (indent - 1) * 4; // Weird but works
			return 4 + indent * 15;
		}

	}
}
