using System;
using UnityEditor;
using UnityEngine;

namespace Xenon.Editor {
	public static class EdGUIPlus {

		public static Enum EnumButtonsField(string label, Enum enVal, params string[] enLabels) {
			Type enType = enVal.GetType();
			Enum[] values = (Enum[]) Enum.GetValues(enType);

			return values[EnumToggles(enLabels)];
		}

		public static Enum EnumButtonsField(string label, Enum enVal) {
			Type enType = enVal.GetType();
			Enum[] values = (Enum[]) Enum.GetValues(enType);
			string[] enLabels = new string[values.Length];
			for (int i = 0; i < values.Length; i++) {
				enLabels[i] = values[i].ToString();
			}

			return values[EnumToggles(enLabels)];
		}

		private static int EnumToggles(string[] labels) {
			bool togg = false;
			int toggIndex = 0;
			EditorGUILayout.BeginHorizontal();
			if (labels.Length < 2) {
				GUILayout.Toggle(togg, labels[0], EditorStyles.miniButton);
			} else {
				GUILayout.Toggle(togg, labels[0], EditorStyles.miniButtonLeft);
				for (int i = 1; i < labels.Length - 1; i++) {
					GUILayout.Toggle(togg, labels[i], EditorStyles.miniButtonMid);
					if (togg) {
						toggIndex = i;
					}
				}
				GUILayout.Toggle(togg, labels[labels.Length - 1], EditorStyles.miniButtonRight);
				if (togg) {
					toggIndex = labels.Length - 1;
				}
			}
			EditorGUILayout.EndHorizontal();

			return toggIndex;
		}

	}
}
