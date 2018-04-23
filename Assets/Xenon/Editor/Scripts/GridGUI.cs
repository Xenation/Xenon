using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xenon {
	public class GridCell {

		public GUIContent content;
		public GUIStyle style;

		public GridCell(GUIContent content, GUIStyle style) {
			this.content = content;
			this.style = style;
		}

	}

	public class GridGUI {

		private struct GridPos {

			public int x, y;

			public GridPos(int x, int y) {
				this.x = x;
				this.y = y;
			}

		}

		private GUIStyle headerStyle;
		private Color headerBG = new Color(.7f, .7f, .7f);
		private Color[] linesColors = { new Color(1f, 1f, 1f), new Color(.8f, .8f, .8f)}; // TODO use editor pref colors

		private List<GridCell> header;
		private Dictionary<GridPos, GridCell> grid;

		private GridPos maxPos;
		
		public string this[int rowIndex, int colIndex] {
			get {
				return GetCellString(rowIndex, colIndex);
			}
			set {
				SetCell(rowIndex, colIndex, value);
			}
		}

		public GridGUI() {
			headerStyle = new GUIStyle(EditorStyles.boldLabel);
			headerStyle.alignment = TextAnchor.LowerCenter;
			grid = new Dictionary<GridPos, GridCell>();
			maxPos = new GridPos(0, 0);
		}

		public void SetHeader(params string[] titles) {
			header = new List<GridCell>();
			for (int i = 0; i < titles.Length; i++) {
				header.Add(new GridCell(new GUIContent(titles[i]), headerStyle));
			}
			maxPos = new GridPos(titles.Length - 1, 0);
		}

		public void SetCell(int rowIndex, int colIndex, string str) {
			GridCell cell;
			GridPos pos = new GridPos(colIndex, rowIndex);
			if (grid.TryGetValue(pos, out cell)) {
				cell.content.text = str;
			} else {
				cell = new GridCell(new GUIContent(str), EditorStyles.label);
				grid.Add(pos, cell);
				if (pos.x > maxPos.x) {
					maxPos.x = pos.x;
				}
				if (pos.y > maxPos.y) {
					maxPos.y = pos.y;
				}
			}
		}

		public string GetCellString(int rowIndex, int colIndex) {
			GridCell cell;
			GridPos pos = new GridPos(colIndex, rowIndex);
			if (grid.TryGetValue(pos, out cell)) {
				return cell.content.text;
			}
			return "";
		}

		public void Clear() {
			grid.Clear();
			maxPos = new GridPos(header.Count - 1, 0);
		}

		public void DisplayGUI(float width) {
			float cellWidth = width / header.Count;
			float cellHeight = EditorGUIUtility.singleLineHeight;
			Rect startRect = GUILayoutUtility.GetRect(width, (maxPos.y + 2) * cellHeight);
			Rect localRect = startRect;
			// Header
			localRect.width = cellWidth;
			localRect.height = cellHeight;
			foreach (GridCell cell in header) {
				GUI.DrawTexture(localRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, headerBG, 0, 0);
				GUI.Label(localRect, cell.content, cell.style);
				//GUI.Box(localRect, cell.content, cell.style);
				localRect.position += new Vector2(cellWidth, 0f);
			}
			localRect.position = new Vector2(startRect.position.x, localRect.y + cellHeight);
			// Grid
			GridPos curPos;
			GridCell curCell;
			for (int y = 0; y <= maxPos.y; y++) {
				for (int x = 0; x <= maxPos.x; x++) {
					curPos = new GridPos(x, y);
					if (grid.TryGetValue(curPos, out curCell)) {
						GUI.DrawTexture(localRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, linesColors[y % linesColors.Length], 0, 0);
						GUI.Label(localRect, curCell.content, curCell.style);
					}
					localRect.position += new Vector2(cellWidth, 0f);
				}
				localRect.position = new Vector2(startRect.position.x, localRect.y + cellHeight);
			}
		}

	}
}
