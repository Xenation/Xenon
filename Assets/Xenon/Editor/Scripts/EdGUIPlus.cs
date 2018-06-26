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

		public static int IndentToPixels(int indent) { // Fix for GUILayout not using indents
			//return (indent == 0) ? 4 : indent * 19 - (indent - 1) * 4; // Weird but works
			return 4 + indent * 15;
		}

	}
}
