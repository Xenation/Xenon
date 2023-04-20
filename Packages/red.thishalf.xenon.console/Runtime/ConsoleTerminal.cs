using UnityEngine;
using UnityEngine.UI;

namespace Xenon.Console {
	public class ConsoleTerminal : MonoBehaviour {

		internal static ConsoleTerminal GetOrCreateTerminal() {
			ConsoleTerminal terminal = GameObject.FindObjectOfType<ConsoleTerminal>();
			if (terminal != null) {
				return terminal;
			}

			GameObject canvasObj = new GameObject("Console Canvas");
			//canvasObj.hideFlags = HideFlags.HideAndDontSave;
			Canvas canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;

			GameObject bgObj = new GameObject("TermBG");
			bgObj.transform.parent = canvasObj.transform;
			Image bg = bgObj.AddComponent<Image>();
			bg.color = new Color(0f, 0f, 0f, 0.5f);
			RectTransform bgTransform = bgObj.GetComponent<RectTransform>();
			bgTransform.anchorMin = new Vector2(0.2f, 1f);
			bgTransform.anchorMax = new Vector2(0.8f, 1f);
			bgTransform.pivot = new Vector2(0.5f, 1f);
			bgTransform.offsetMin = new Vector2(0f, -100f);
			bgTransform.offsetMax = new Vector2(0f, 0f);

			terminal = canvasObj.AddComponent<ConsoleTerminal>();
			return terminal;
		}



		public void Write(string str) {

		}

	}
}
